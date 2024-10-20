using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Media;
using Nop.Plugin.Misc.NopHunter.LiveChat.Components;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.NopHunter.LiveChat;

/// <summary>
/// Represents Omnisend plugin
/// </summary>
public class LiveChatPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
{
    #region Fields

    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingService;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public LiveChatPlugin(IActionContextAccessor actionContextAccessor,
        ILocalizationService localizationService,
        ISettingService settingService,
        IUrlHelperFactory urlHelperFactory,
        WidgetSettings widgetSettings)
    {
        _actionContextAccessor = actionContextAccessor;
        _localizationService = localizationService;
        _settingService = settingService;
        _urlHelperFactory = urlHelperFactory;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        if (_actionContextAccessor.ActionContext != null)
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext)
                .RouteUrl(LiveChatDefaults.ConfigurationRouteName);

        return base.GetConfigurationPageUrl();
    }

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone is null)
            throw new ArgumentNullException(nameof(widgetZone));

        if (widgetZone.Equals(PublicWidgetZones.HeaderAfter))
            return typeof(WidgetsLiveChatViewComponent);

        return null;
    }

    /// <summary>
    /// Gets widget zones where this widget should be rendered
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the widget zones
    /// </returns>
    public Task<IList<string>> GetWidgetZonesAsync()
    {

        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HeaderAfter });
    }

    /// <summary>
    /// Install plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        var settings = new LiveChatSettings
        {
            Script = ""
        };

        await _settingService.SaveSettingAsync(settings);

        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(LiveChatDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(LiveChatDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Misc.NopHunter.LiveChat.Script"] = "Live chat script",
            ["Plugins.Misc.NopHunter.Common.UpdateSuccess"] = "Successfully updated"
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall the plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        if (_widgetSettings.ActiveWidgetSystemNames.Contains(LiveChatDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(LiveChatDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }
        await _settingService.DeleteSettingAsync<LiveChatSettings>();

        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.NopHunter.LiveChat");

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