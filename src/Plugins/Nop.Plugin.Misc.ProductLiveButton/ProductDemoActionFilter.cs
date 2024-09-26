using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core.Domain.Catalog;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;

namespace Nop.Plugin.Misc.ProductLiveButton;
public class ProductDemoActionFilter : ActionFilterAttribute
{
    protected readonly IProductDemoService _productDemoService;

    public ProductDemoActionFilter(IProductDemoService productDemoService)
    {
        _productDemoService = productDemoService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Result == null)
            await next();

        await HandleProductDemoLinkForm(context);
    }
    private async Task HandleProductDemoLinkForm(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"].ToString() == "Product";
        var action = context.RouteData.Values["action"].ToString() == "Create" || context.RouteData.Values["action"].ToString() == "Edit";
        var methodType = context.HttpContext.Request.Method == "POST";

        if (controller && action && methodType)
        {
            var session = context.HttpContext.Session;
            var entity = await session.GetAsync<Product>(ProductLiveButtonDefaults.ProductAddOrUpdateSuccessSessionKey);

            var productDemoModel = new ProductDemoModel()
            {
                ProductId = entity.Id,
                DemoLink = context.HttpContext.Request.Form[nameof(ProductDemoModel.DemoLink).ToLower()].ToString(),
            };
            
            await _productDemoService.AddOrUpdateAsync(productDemoModel);
            await session.RemoveAsync(ProductLiveButtonDefaults.ProductAddOrUpdateSuccessSessionKey);
        }
    }
}
