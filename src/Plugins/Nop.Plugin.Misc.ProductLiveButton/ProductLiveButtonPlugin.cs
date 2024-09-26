using System.Globalization;
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
            return typeof(AdminProductLiveButtonViewComponent);
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
        var settings = new ProductLiveButtonSettings()
        {
            ButtonTitle = "Preview",
            ButtonBackgroundColor = "#27c3e2",
            ButtonTextColor = "#fafbfc",
            ShowInProductBox = true,
            CustomCss = ""
        };
        await _settingService.SaveSettingAsync(settings);

        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(ProductLiveButtonDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(ProductLiveButtonDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Misc.ProductLiveButton.SectionTitle"] = "Product Live Button",
            ["Plugins.Misc.ProductLiveButton.Field.DemoLink"] = "Link",

            ["Plugins.Misc.ProductLiveButton.Field.ButtonTitle"] = "Title",
            ["Plugins.Misc.ProductLiveButton.Field.ButtonBackgroundColor"] = "Background Color",
            ["Plugins.Misc.ProductLiveButton.Field.ButtonTextColor"] = "Text Color",
            ["Plugins.Misc.ProductLiveButton.Field.ShowInProductBox"] = "Show button in product Box",
            ["Plugins.Misc.ProductLiveButton.Field.CustomCss"] = "Custom Css",
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

    public override async Task UpdateAsync(string currentVersion, string targetVersion)
    {
        var current = decimal.TryParse(currentVersion, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : 1.00M;

        if (currentVersion == "4.80.2" && targetVersion == "4.80.3")
        {
            var settings = new ProductLiveButtonSettings()
            {
                ButtonTitle = "Preview",
                ButtonBackgroundColor = "#27c3e2",
                ButtonTextColor = "#fafbfc",
                ShowInProductBox = true,
                CustomCss = ""
            };
            await _settingService.SaveSettingAsync(settings);

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(ProductLiveButtonDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(ProductLiveButtonDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Misc.ProductLiveButton.Field.ButtonTitle"] = "Title",
                ["Plugins.Misc.ProductLiveButton.Field.ButtonBackgroundColor"] = "Background Color",
                ["Plugins.Misc.ProductLiveButton.Field.ButtonTextColor"] = "Text Color",
                ["Plugins.Misc.ProductLiveButton.Field.ShowInProductBox"] = "Show button in product Box",
                ["Plugins.Misc.ProductLiveButton.Field.CustomCss"] = "Custom Css",
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