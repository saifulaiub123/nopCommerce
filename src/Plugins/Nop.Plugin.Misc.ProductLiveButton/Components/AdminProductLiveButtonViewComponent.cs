using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.ProductLiveButton.Component;
public class AdminProductLiveButtonViewComponent : NopViewComponent
{
    #region Fields
    protected readonly IProductDemoService _productDemoService;

    #endregion

    #region Ctor
    public AdminProductLiveButtonViewComponent(
        IProductDemoService productDemoService)
    {
        _productDemoService = productDemoService;
    }

    #endregion


    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var productDemoModel = new ProductDemoModel();

        var product = additionalData as ProductModel;
        if (product is not null)
        {
            productDemoModel = await _productDemoService.GetByProductId(product.Id);
            if (productDemoModel is null)
            {
                productDemoModel = new ProductDemoModel()
                {
                    ProductId = product.Id,
                };
            }
        }
        else
        {
            productDemoModel = new ProductDemoModel();
        }
            
        return View("~/Plugins/Misc.ProductLiveButton/Views/CreateOrUpdate.cshtml", productDemoModel);
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
