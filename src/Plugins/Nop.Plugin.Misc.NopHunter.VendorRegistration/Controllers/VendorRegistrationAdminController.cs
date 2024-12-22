using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Factory;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
using Nop.Plugin.Misc.VendorRegistration.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

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
    protected readonly IVendorServiceCustom _vendorServiceCustom;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly IVendorWorkflowMessageService _vendorWorkflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;


    private static readonly char[] _separator = [','];

    #endregion

    #region Ctor

    public VendorRegistrationAdminController(
        ILocalizationService localizationService,
        INotificationService notificationService,
        IPermissionService permissionService,
        ISettingService settingService,
        IStoreContext storeContext,
        IVendorModelFactory vendorModelFactory,

        IVendorModelFactoryCustom vendorModelFactoryCustom,
        IVendorServiceCustom vendorServiceCustom,
        ICustomerActivityService customerActivityService,
        IVendorWorkflowMessageService vendorWorkflowMessageService,
        LocalizationSettings localizationSettings)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
        _vendorModelFactory = vendorModelFactory;

        _vendorModelFactoryCustom = vendorModelFactoryCustom;
        _vendorServiceCustom = vendorServiceCustom;
        _customerActivityService = customerActivityService;
        _vendorWorkflowMessageService = vendorWorkflowMessageService;
        _localizationSettings = localizationSettings;
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
    public virtual async Task<IActionResult> ActivateVendor(string selectedIds, bool isSendEmail)
    {
        var vendors = new List<Vendor>();
        if (selectedIds != null)
        {
            var ids = selectedIds
                .Split(_separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x))
                .ToList();
            vendors.AddRange(await _vendorServiceCustom.GetAllVendorsByIds(ids));
        }
        vendors.ForEach(c => c.Active = true);
        await _vendorServiceCustom.UpdateVendors(vendors);
        await _customerActivityService.InsertActivityAsync("EditVendor", string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditVendor"), selectedIds));

        if(isSendEmail)
        {
            await _vendorWorkflowMessageService.SendVendorAccountActivationNotificationToVendors(vendors, _localizationSettings.DefaultAdminLanguageId);
        }
        return Json(new { Success = true });
    }
    #endregion
}
