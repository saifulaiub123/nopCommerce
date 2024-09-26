using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.ProductLiveButton.Components;
public class HomePageLiveButtonViewComponent : NopViewComponent
{
    protected readonly IProductDemoService _productDemoService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;

    public HomePageLiveButtonViewComponent(
        IProductDemoService productDemoService,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _productDemoService = productDemoService;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var productDemoModel = new ProductDemoModel();
        var t = await _productDemoService.GetByProductIds(new List<int>() { 1, 18 });

        var product = additionalData as ProductOverviewModel;
        //if (product is not null)
        //{

        //}
        //if (productDemoModel is null)
        //    return Content(string.Empty);

        //var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        //var settings = await _settingService.LoadSettingAsync<ProductLiveButtonSettings>(storeScope);

        //var publicInfoProductPageModel = new PublicInfoProductPageModel()
        //{
        //    ProductDemoModel = productDemoModel,
        //    Settings = settings
        //};

        return View("~/Plugins/Misc.ProductLiveButton/Views/PublicInfoHomePage.cshtml");
    }

    //public class ProductDemoEventConsumer : IConsumer<AdminProductDetailsCreated>
    //{
    //    public void HandleEvent(AdminProductDetailsCreated eventMessage)
    //    {
    //        // Insert the product demo view component into the product creation page
    //        var component = new TagBuilder("div");
    //        component.InnerHtml.AppendHtml(eventMessage.ViewComponentHelper.RenderComponentAsync("ProductDemo", new { productId = eventMessage.ProductId }).Result);
    //        eventMessage.BlocksToRender.Add(component);
    //    }
    //}


}
