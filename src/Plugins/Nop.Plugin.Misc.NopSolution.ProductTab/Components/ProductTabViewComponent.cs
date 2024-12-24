using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.NopSolution.ProductTab.Models;
using Nop.Services.Configuration;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.NopSolution.ProductTab.Components;
public class ProductTabViewComponent : NopViewComponent
{
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IVendorModelFactory _vendorModelFactory;

    public ProductTabViewComponent(
        ISettingService settingService,
        IStoreContext storeContext,
        IVendorModelFactory vendorModelFactory)
    {
        _settingService = settingService;
        _storeContext = storeContext;
        _vendorModelFactory = vendorModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new ProductTabModel();
        //model = await _vendorModelFactory.PrepareApplyVendorModelAsync(model, true, false, null);
        return View("~/Plugins/Misc.NopSolution.ProductTab/Views/ProductTabPublic.cshtml", model);
    }
}
