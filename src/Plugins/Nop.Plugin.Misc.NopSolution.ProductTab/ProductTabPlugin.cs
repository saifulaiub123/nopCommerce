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
using Nop.Plugin.Misc.NopSolution.ProductTab.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Services.Common;

namespace Nop.Plugin.Misc.NopSolution.ProductTab;

/// <summary>
/// Google Analytics plugin
/// </summary>
public class ProductTabPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
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

    public ProductTabPlugin(IActionContextAccessor actionContextAccessor,
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
            PublicWidgetZones.HomepageBeforeBestSellers
        });
    }

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(ProductTabDefaults.ConfigurationRouteName);
    }

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        ArgumentNullException.ThrowIfNull(widgetZone);

        if (widgetZone.Equals(PublicWidgetZones.HomepageBeforeBestSellers))
            return typeof(ProductTabViewComponent);

        return null;
    }

    /// <summary>
    /// Install plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        try
        {
            
            var settings = new ProductTabSettings();
            await _settingService.SaveSettingAsync(settings);

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(ProductTabDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(ProductTabDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            //await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            //{
            //    //common
            //    ["Plugins.Misc.NopSolution.ProductTab.Common.Success"] = "Success",
            //    ["Plugins.Misc.NopSolution.ProductTab.Common.Error"] = "Error",
            //    ["Plugins.Misc.NopSolution.ProductTab.Common.ErrorMessage"] = "Something went wrong",
            //    ["Plugins.Misc.NopSolution.ProductTab.Common.UpdateSuccess"] = "Successfully updated",

            //    //admin 
            //    ["Plugins.Misc.NopSolution.ProductTab.Field.EditVendorInfoTitle"] = "Update vendor",
            //    ["Plugins.Misc.NopSolution.ProductTab.Field.EditEmailTemplateTitle"] = "Update email template for vendor",

            //    //public
            //    ["Plugins.Misc.NopSolution.ProductTab.FieldSet.VendorDetails"] = "Vendor details",
            //    ["Plugins.Misc.NopSolution.ProductTab.FieldSet.PersonalDetails"] = "Personal details",
            //    ["Plugins.Misc.NopSolution.ProductTab.FieldSet.Password"] = "Password",
            //    ["Plugins.Misc.NopSolution.ProductTab.Field.Button.Apply"] = "Apply",
            //    ["Plugins.Misc.NopSolution.ProductTab.Field.AccountType"] = "Account type",
            //    ["Plugins.Misc.NopSolution.ProductTab.Field.AccountType.Customer"] = "Customer",
            //    ["Plugins.Misc.NopSolution.ProductTab.Field.AccountType.Vendor"] = "Vendor",

            //    ["Plugins.Misc.NopSolution.ProductTab.Admin.Vendors.List.SearchIsActive"] = "Is Active",
            //    ["Plugins.Misc.NopSolution.ProductTab.Admin.Vendors.List.Button.Activate"] = "Activate",
            //    ["Plugins.Misc.NopSolution.ProductTab.Admin.Vendors.List.Button.ActivateAndSendEmail"] = "Activate & Send Mail",

            //    ["Plugins.Misc.NopSolution.ProductTab.Admin.Vendors.NoVendorSelected"] = "No vendors selected",
                
            //});

            
        }
        catch (Exception)
        {
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
        if (_widgetSettings.ActiveWidgetSystemNames.Contains(ProductTabDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(ProductTabDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }
        await _settingService.DeleteSettingAsync<ProductTabSettings>();

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.NopSolution.ProductTab");

        //Message Template
        var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(ProductTabDefaults.NEW_VENDOR_ACCOUNT_APPLY_STORE_VENDOR_NOTIFICATION);
        if(messageTemplates.Any())
            await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplates.First());

        messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(ProductTabDefaults.VENDOR_ACCOUNT_ACTIVATION_NOTIFICATION);
        if(messageTemplates.Any())
            await _messageTemplateService.DeleteMessageTemplateAsync(messageTemplates.First());
        
        await base.UninstallAsync();
    }

    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        //var current = decimal.TryParse(currentVersion, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : 1.00M;

        //if (currentVersion == "4.80.2" && targetVersion == "4.80.3")
        //{
        //    var settings = new ProductTabSettings();
            
        //    await _settingService.SaveSettingAsync(settings);

        //    if (!_widgetSettings.ActiveWidgetSystemNames.Contains(ProductTabDefaults.SystemName))
        //    {
        //        _widgetSettings.ActiveWidgetSystemNames.Add(ProductTabDefaults.SystemName);
        //        await _settingService.SaveSettingAsync(_widgetSettings);
        //    }

        //    await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        //    {
        //        ["Plugins.Misc.VendorRegistration.Field.ButtonTitle"] = "Title",
        //        ["Plugins.Misc.VendorRegistration.Field.ButtonBackgroundColor"] = "Background Color",
        //        ["Plugins.Misc.VendorRegistration.Field.ButtonTextColor"] = "Text Color",
        //        ["Plugins.Misc.VendorRegistration.Field.ShowInProductBox"] = "Show button in product Box",
        //        ["Plugins.Misc.VendorRegistration.Field.CustomCss"] = "Custom Css",
        //    });
        //}
    }
    #endregion 

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;

    #endregion
}