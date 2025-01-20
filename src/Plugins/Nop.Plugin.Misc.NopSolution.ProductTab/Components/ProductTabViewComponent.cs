using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Plugin.Misc.NopSolution.ProductTab.Models;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.NopSolution.ProductTab.Components;
public class ProductTabViewComponent : NopViewComponent
{
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly ICategoryService _categoryService;
    protected readonly ILocalizationService _localizationService;


    public ProductTabViewComponent(
        ISettingService settingService,
        IStoreContext storeContext,
        IVendorModelFactory vendorModelFactory,
        ICategoryService categoryService,
        ILocalizationService localizationService)
    {
        _settingService = settingService;
        _storeContext = storeContext;
        _categoryService = categoryService;
        _localizationService = localizationService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new ProductTabModel();

        var currentStore = await _storeContext.GetCurrentStoreAsync();
        var allCategories = await _categoryService.GetAllCategoriesAsync(storeId: currentStore.Id);

        if (allCategories.Any())
        {
            //first empty entry
            model.AvailableCategories.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Common.All")
            });
            //all other categories
            foreach (var c in allCategories)
            {
                model.AvailableCategories.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    //Selected = model.cid == c.Id
                });
            }
        }


        //model = await _vendorModelFactory.PrepareApplyVendorModelAsync(model, true, false, null);
        return View("~/Plugins/Misc.NopSolution.ProductTab/Views/ProductTabPublic.cshtml", model);
    }
}
