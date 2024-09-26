using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Misc.ProductLiveButton.Controllers;
public class ProductLiveButtonPublicController : BasePluginController
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly ICommonModelFactory _commonModelFactory;
    #endregion

    #region Ctor

    public ProductLiveButtonPublicController(
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        IStoreContext storeContext,
        ICommonModelFactory commonModelFactory)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
        _commonModelFactory = commonModelFactory;
    }

    #endregion

    #region Methods

    public async Task<IActionResult> PreviewDemo(string link)
    {
        var logoModel = await _commonModelFactory.PrepareLogoModelAsync();

        var previewDemoModel = new PreviewDemoModel()
        {
            LogoModel = logoModel,
            DemoLink = link
        };
        return View("~/Plugins/Misc.ProductLiveButton/Views/PreviewDemo.cshtml", previewDemoModel);
    }
    #endregion
}

