using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.NopHunter.LiveChat.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.NopHunter.LiveChat.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class LiveChatAdminController : BasePluginController
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingService _settingService;
    private readonly LiveChatSettings _liveChatSettings;

    #endregion

    #region Ctor

    public LiveChatAdminController(ILocalizationService localizationService,
        INotificationService notificationService,
        ISettingService settingService,
        LiveChatSettings liveChatSettings)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _settingService = settingService;
        _liveChatSettings = liveChatSettings;
    }

    #endregion

    #region Utilities
    #endregion

    #region Methods

    public async Task<IActionResult> Configure()
    {
        var model = new ConfigurationModel
        {
            Script = _liveChatSettings.Script,
        };

        return View(model);
    }

    [HttpPost, ActionName("Configure")]
    [FormValueRequired("save")]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        _liveChatSettings.Script = model.Script;

        await _settingService.SaveSettingAsync(_liveChatSettings);
        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugins.Misc.NopHunter.Common.UpdateSuccess"));
        return await Configure();
    }
    #endregion
}