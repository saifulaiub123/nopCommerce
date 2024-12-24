using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.NopSolution.ProductTab;
using Nop.Plugin.Misc.NopSolution.ProductTab.Models;
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
    protected readonly ICustomerActivityService _customerActivityService;
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

        ICustomerActivityService customerActivityService,
        LocalizationSettings localizationSettings)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _permissionService = permissionService;
        _settingService = settingService;
        _storeContext = storeContext;
        _vendorModelFactory = vendorModelFactory;

        _customerActivityService = customerActivityService;
        _localizationSettings = localizationSettings;
    }

    #endregion

    #region Methods
    public async Task<IActionResult> Configure()
    {
        //load settings for a chosen store scope
        var model = new ProductTabModel();
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<ProductTabSettings>(storeScope);

        return View("~/Plugins/Misc.NopSolution.ProductTab/Views/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var settings = await _settingService.LoadSettingAsync<ProductTabSettings>(storeScope);

        
       

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
    #endregion
}
