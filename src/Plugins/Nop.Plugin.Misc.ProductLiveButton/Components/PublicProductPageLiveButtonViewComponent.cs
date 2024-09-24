using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.ProductLiveButton.Components;
public class PublicProductPageLiveButtonViewComponent : NopViewComponent
{
    protected readonly IProductDemoService _productDemoService;

    public PublicProductPageLiveButtonViewComponent(
        IProductDemoService productDemoService)
    {
        _productDemoService = productDemoService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var productDemoModel = new ProductDemoModel();

        var product = additionalData as ProductDetailsModel;
        if (product is not null)
        {
            productDemoModel = await _productDemoService.GetByProductId(product.Id);
        }
        else
        {
            productDemoModel = new ProductDemoModel();
        }

        return View("~/Plugins/Misc.ProductLiveButton/Views/PublicInfoProductPage.cshtml", productDemoModel);
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