using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Plugin.Misc.ProductLiveButton;
using Nop.Plugin.Misc.ProductLiveButton.Component;
using Nop.Plugin.Misc.ProductLiveButton.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.ProductLiveButton;

/// <summary>
/// Google Analytics plugin
/// </summary>
public class ProductLiveButtonPlugin : BasePlugin, IWidgetPlugin
{
    #region Fields

    protected readonly IActionContextAccessor _actionContextAccessor;
    protected readonly ILocalizationService _localizationService;
    protected readonly IWebHelper _webHelper;
    protected readonly ISettingService _settingService;
    protected readonly IUrlHelperFactory _urlHelperFactory;
    protected readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public ProductLiveButtonPlugin(IActionContextAccessor actionContextAccessor,
        ILocalizationService localizationService,
        IWebHelper webHelper,
        ISettingService settingService,
        IUrlHelperFactory urlHelperFactory,
        WidgetSettings widgetSettings)
    {
        _actionContextAccessor = actionContextAccessor;
        _localizationService = localizationService;
        _webHelper = webHelper;
        _settingService = settingService;
        _urlHelperFactory = urlHelperFactory;
        _widgetSettings = widgetSettings;
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
            AdminWidgetZones.ProductDetailsBlock,
            PublicWidgetZones.ProductDetailsEssentialTop
        });
    }

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(ProductLiveButtonDefaults.ConfigurationRouteName);
    }

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        ArgumentNullException.ThrowIfNull(widgetZone);

        if (widgetZone.Equals(AdminWidgetZones.ProductDetailsBlock))
            return typeof(AdminProductLiveButtonDataViewComponent);
        if (widgetZone.Equals(PublicWidgetZones.ProductDetailsEssentialTop))
            return typeof(PublicProductPageLiveButtonViewComponent);

        return null;
    }

    /// <summary>
    /// Install plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
    //    var settings = new GoogleAnalyticsSettings
    //    {
    //        GoogleId = "G-XXXXXXXXXX",
    //        TrackingScript = @"<!-- Global site tag (gtag.js) - Google Analytics -->
    //            <script async src='https://www.googletagmanager.com/gtag/js?id={GOOGLEID}'></script>
    //            <script>
    //              window.dataLayer = window.dataLayer || [];
    //              function gtag(){dataLayer.push(arguments);}
    //              gtag('js', new Date());

    //              gtag('config', '{GOOGLEID}');
    //              {CUSTOMER_TRACKING}
    //            </script>"
    //    };
    //    await _settingService.SaveSettingAsync(settings);

        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(ProductLiveButtonDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(ProductLiveButtonDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Misc.ProductLiveButton.Title"] = "Product demo link",
            ["Plugins.Misc.ProductLiveButton.DemoLink"] = "Link",
            ["Plugins.Misc.ProductLiveButton.ShowInProductPictureBottom"] = "Show link on Picture",
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        //settings
        if (_widgetSettings.ActiveWidgetSystemNames.Contains(ProductLiveButtonDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(ProductLiveButtonDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }
        await _settingService.DeleteSettingAsync<ProductLiveButtonSettings>();

        //locales
        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.ProductLiveButton");

        await base.UninstallAsync();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;

    #endregion
}