
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.NopHunter.LiveChat.Models;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Misc.NopHunter.LiveChat.Components;

/// <summary>
/// Represents view component to embed tracking script on pages
/// </summary>
public class WidgetsLiveChatViewComponent : NopViewComponent
{
    #region Fields
    private readonly LiveChatSettings _liveChatSettings;

    #endregion

    #region Ctor

    public WidgetsLiveChatViewComponent(LiveChatSettings liveChatSettings)
    {
        _liveChatSettings = liveChatSettings;
    }

    #endregion

    #region Utilities

    #endregion

    #region Methods

    /// <summary>
    /// Invoke view component
    /// </summary>
    /// <param name="widgetZone">Widget zone name</param>
    /// <param name="additionalData">Additional data</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the view component result
    /// </returns>
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        //ensure tracking is enabled
        var model = new ConfigurationModel()
        {
            Script = _liveChatSettings.Script
        };
        return View("~/Plugins/Misc.NopHunter.LiveChat/Views/Script.cshtml", model);
    }

    #endregion
}