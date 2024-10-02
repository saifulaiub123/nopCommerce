using System.Globalization;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Messages;
using Nop.Data;
using Nop.Plugin.Misc.VendorRegistration;
using Nop.Plugin.Misc.VendorRegistration.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.VendorRegistration;

/// <summary>
/// Google Analytics plugin
/// </summary>
public class VendorRegistrationPlugin : BasePlugin, IWidgetPlugin
{
    #region Fields

    protected readonly IActionContextAccessor _actionContextAccessor;
    protected readonly ILocalizationService _localizationService;
    protected readonly IWebHelper _webHelper;
    protected readonly ISettingService _settingService;
    protected readonly IUrlHelperFactory _urlHelperFactory;
    protected readonly IMessageTemplateService _messageTemplateService;
    protected readonly WidgetSettings _widgetSettings;
    protected readonly IRepository<EmailAccount> _emailAccountRepository;
    protected readonly EmailAccountSettings _emailAccountSettings;


    #endregion

    #region Ctor

    public VendorRegistrationPlugin(IActionContextAccessor actionContextAccessor,
        ILocalizationService localizationService,
        IWebHelper webHelper,
        ISettingService settingService,
        IUrlHelperFactory urlHelperFactory,
        WidgetSettings widgetSettings,
        IMessageTemplateService messageTemplateService,
        IRepository<EmailAccount> emailAccountRepository,
        EmailAccountSettings emailAccountSettings)
    {
        _actionContextAccessor = actionContextAccessor;
        _localizationService = localizationService;
        _webHelper = webHelper;
        _settingService = settingService;
        _urlHelperFactory = urlHelperFactory;
        _widgetSettings = widgetSettings;
        _messageTemplateService = messageTemplateService;
        _emailAccountRepository = emailAccountRepository;
        _emailAccountSettings = emailAccountSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets widget zones where this widget should be rendered
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the widget zones
    /// </returns>
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
        {
            PublicWidgetZones.RegisterTop
        });
    }

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(VendorRegistrationDefaults.ConfigurationRouteName);
    }

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        ArgumentNullException.ThrowIfNull(widgetZone);

        if (widgetZone.Equals(PublicWidgetZones.RegisterTop))
            return typeof(RegisterPageViewComponent);

        return null;
    }

    /// <summary>
    /// Install plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            
            var settings = new VendorRegistrationSettings();
            await _settingService.SaveSettingAsync(settings);

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(VendorRegistrationDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(VendorRegistrationDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                //admin 
                ["Plugins.Misc.NopHunter.VendorRegistration.Field.EditVendorInfoTitle"] = "Update vendor",
                ["Plugins.Misc.NopHunter.VendorRegistration.Field.EditEmailTemplateTitle"] = "Update email template for vendor",

                //public
                ["Plugins.Misc.VendorRegistration.Field.AccountType"] = "Account type",
                ["Plugins.Misc.VendorRegistration.Field.Customer"] = "Customer",
                ["Plugins.Misc.VendorRegistration.Field.Vendor"] = "Vendor",
            });

            var emailAccountSettings = await _settingService.LoadSettingAsync<EmailAccountSettings>();

            await _messageTemplateService.InsertMessageTemplateAsync(new MessageTemplate()
            {
                Name = VendorRegistrationDefaults.NEW_VENDOR_ACCOUNT_APPLY_STORE_VENDOR_NOTIFICATION,
                Subject = "%Store.Name% - Vendor account is pending to Active",
                Body = $@"<p><a href=""%Store.URL%"">%Store.Name%</a> <br /><br />Your vendor account for <strong>%Store.Name% </strong>has been created with below details.</p>
                        <p><br />Vendor name: %Vendor.Name% <br />Vendor email: %Vendor.Email% <br /><br />Your account is currently in <strong>Pending </strong>state. Once it will approve by the admin you can access to the <strong>Admin Panel</strong></p>
                        <p>For any type of help email us at <a href=""mailto:contact@digitalmart.com"">contact@digitalmart.com</a>
                    </p>",
                IsActive = false,
                AttachedDownloadId = 0,
                EmailAccountId = emailAccountSettings.DefaultEmailAccountId,
                LimitedToStores = false,
                DelayBeforeSend = null,
                DelayPeriod = MessageDelayPeriod.Hours
            });
            await base.InstallAsync();
            transaction.Complete();
            
        }
        catch (Exception)
        {
            transaction.Dispose();
            throw;
        }
        
        

       
    }

    /// <summary>
    /// Uninstall plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        //settings
        if (_widgetSettings.ActiveWidgetSystemNames.Contains(VendorRegistrationDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(VendorRegistrationDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }
        await _settingService.DeleteSettingAsync<VendorRegistrationSettings>();

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.VendorRegistration");

        await base.UninstallAsync();
    }

    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        var current = decimal.TryParse(currentVersion, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : 1.00M;

        if (currentVersion == "4.80.2" && targetVersion == "4.80.3")
        {
            var settings = new VendorRegistrationSettings();
            
            await _settingService.SaveSettingAsync(settings);

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(VendorRegistrationDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(VendorRegistrationDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Misc.VendorRegistration.Field.ButtonTitle"] = "Title",
                ["Plugins.Misc.VendorRegistration.Field.ButtonBackgroundColor"] = "Background Color",
                ["Plugins.Misc.VendorRegistration.Field.ButtonTextColor"] = "Text Color",
                ["Plugins.Misc.VendorRegistration.Field.ShowInProductBox"] = "Show button in product Box",
                ["Plugins.Misc.VendorRegistration.Field.CustomCss"] = "Custom Css",
            });
        }
    }
    #endregion 

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;

    #endregion
}