using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Plugin.Misc.ProductLiveButton.Domain;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Localization;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.ProductLiveButton.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class ProductLiveButtonAdminController : BasePluginController
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public ProductLiveButtonAdminController(
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure()
    {
        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<ProductLiveButtonSettings>(storeScope);

        var model = new ConfigurationModel
        {
            ButtonTitle = settings.ButtonTitle,
            ButtonBackgroundColor = settings.ButtonBackgroundColor,
            ButtonTextColor = settings.ButtonTextColor,
            ShowInProductBox = settings.ShowInProductBox,
            CustomCss = settings.CustomCss,
            ActiveStoreScopeConfiguration = storeScope
        };

        return View("~/Plugins/Misc.ProductLiveButton/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<ProductLiveButtonSettings>(storeScope);

        settings.ButtonTitle = model.ButtonTitle;
        settings.ButtonBackgroundColor = model.ButtonBackgroundColor;
        settings.ButtonTextColor = model.ButtonTextColor;
        settings.ShowInProductBox = model.ShowInProductBox;
        settings.CustomCss = model.CustomCss;
       

        /* We do not clear cache after each setting update.
         * This behavior can increase performance because cached settings will not be cleared 
         * and loaded from database after each update */
        await _settingService.SaveSettingAsync(settings, x => x.ButtonTitle, storeScope, false);
        await _settingService.SaveSettingAsync(settings, x => x.ButtonBackgroundColor, storeScope, false);
        await _settingService.SaveSettingAsync(settings, x => x.ButtonTextColor, storeScope, false);
        await _settingService.SaveSettingAsync(settings, x => x.ShowInProductBox, storeScope, false);
        await _settingService.SaveSettingAsync(settings, x => x.CustomCss, storeScope, false);

        //now clear settings cache
        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion
}
