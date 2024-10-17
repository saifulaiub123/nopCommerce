using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Factory;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Plugin.Misc.VendorRegistration.Domain;
using Nop.Plugin.Misc.VendorRegistration.Models;
using Nop.Plugin.Misc.VendorRegistration.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Localization;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Misc.VendorRegistration.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class VendorRegistrationAdminController : BasePluginController
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPermissionService _permissionService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IVendorModelFactory _vendorModelFactory;

    protected readonly IVendorModelFactoryCustom _vendorModelFactoryCustom;


    #endregion

    #region Ctor

    public VendorRegistrationAdminController(
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        IStoreContext storeContext,
        IVendorModelFactory vendorModelFactory,

        IVendorModelFactoryCustom vendorModelFactoryCustom)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
        _vendorModelFactory = vendorModelFactory;

        _vendorModelFactoryCustom = vendorModelFactoryCustom;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure()
    {
        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<VendorRegistrationSettings>(storeScope);

        //var model = new ConfigurationModel();
        var model = await _vendorModelFactoryCustom.PrepareVendorSearchModelAsync(new VendorSearchModelCustom());

        return View("~/Plugins/Misc.NopHunter.VendorRegistration/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<VendorRegistrationSettings>(storeScope);

        
       

        /* We do not clear cache after each setting update.
         * This behavior can increase performance because cached settings will not be cleared 
         * and loaded from database after each update */
        //await _settingService.SaveSettingAsync(settings, x => x.ButtonTitle, storeScope, false);
        //await _settingService.SaveSettingAsync(settings, x => x.ButtonBackgroundColor, storeScope, false);
        //await _settingService.SaveSettingAsync(settings, x => x.ButtonTextColor, storeScope, false);
        //await _settingService.SaveSettingAsync(settings, x => x.ShowInProductBox, storeScope, false);
        //await _settingService.SaveSettingAsync(settings, x => x.CustomCss, storeScope, false);

        //now clear settings cache
        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }


    [CheckPermission(StandardPermission.Customers.VENDORS_VIEW)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _vendorModelFactoryCustom.PrepareVendorSearchModelAsync(new VendorSearchModelCustom());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Customers.VENDORS_VIEW)]
    public virtual async Task<IActionResult> List(VendorSearchModelCustom searchModel)
    {
        //prepare model
        var model = await _vendorModelFactoryCustom.PrepareVendorListModelAsync(searchModel);

        return Json(model);
    }


    [HttpPost]
    //[CheckPermission(StandardPermission.Customers.VENDORS_CREATE_EDIT_DELETE)]
    public virtual async Task<IActionResult> ActivateVendor(string SelectedIds, bool IsSendEmail)
    {
        //var orders = new List<Vendor>();
        //if (selectedIds != null)
        //{
        //    var ids = selectedIds
        //        .Split(_separator, StringSplitOptions.RemoveEmptyEntries)
        //        .Select(x => Convert.ToInt32(x))
        //        .ToArray();
        //    orders.AddRange(await (await _orderService.GetOrdersByIdsAsync(ids))
        //        .WhereAwait(HasAccessToOrderAsync).ToListAsync());
        //}

        //try
        //{
        //    var xml = await _exportManager.ExportOrdersToXmlAsync(orders);
        //    return File(Encoding.UTF8.GetBytes(xml), MimeTypes.ApplicationXml, "orders.xml");
        //}
        //catch (Exception exc)
        //{
        //    await _notificationService.ErrorNotificationAsync(exc);
        //    return RedirectToAction("List");
        //}
        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Customers.Customers.SendEmail.Queued"));

        return Json(new
        {
            Success = true
        });
    }
    #endregion
}
