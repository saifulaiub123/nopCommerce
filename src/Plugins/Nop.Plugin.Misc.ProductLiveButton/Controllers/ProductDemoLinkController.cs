﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Plugin.Misc.ProductLiveButton.Domain;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Localization;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.ProductLiveButton.Controllers;
public class ProductDemoLinkController : BasePluginController
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly ProductDemoService _productDemoService;

    #endregion

    #region Ctor

    public ProductDemoLinkController(
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        IStoreContext storeContext,
        ProductDemoService productDemoService)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
        _productDemoService = productDemoService;
    }

    #endregion

    #region Methods

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public virtual async Task<IActionResult> AddProductDemo(ProductDemoModel model)
    {
        if (!ModelState.IsValid)
        {
            return ErrorJson(ModelState.SerializeErrors());
        }
        await _productDemoService.AddOrUpdateAsync(model);

        return Json(new { Result = true });
    }

    #endregion
}
