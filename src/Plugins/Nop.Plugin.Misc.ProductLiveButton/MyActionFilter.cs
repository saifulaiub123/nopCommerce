using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core.Domain.Catalog;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.ProductLiveButton.Controllers;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;
using Nop.Services.Authentication;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.ProductLiveButton;
public class MyActionFilter : ActionFilterAttribute
{
    protected readonly IProductDemoService _productDemoService;

    public MyActionFilter(IProductDemoService productDemoService)
    {
        _productDemoService = productDemoService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var t = context;
        if (context.Result == null)
            await next();

        await HandleProductDemoLinkForm(context);
    }
    private async Task HandleProductDemoLinkForm(ActionExecutingContext context)
    {
        var controller = context.RouteData.Values["controller"].ToString();
        var action = context.RouteData.Values["action"].ToString();
        var methodType = context.HttpContext.Request.Method;

        if (controller == "Product" && action == "Edit" && methodType == "POST")
        {
            var session = context.HttpContext.Session;
            var entity = await session.GetAsync<Product>(ProductLiveButtonDefaults.ProductAddOrUpdateSuccessSessionKey);

            var productDemoModel = new ProductDemoModel()
            {
                ProductId = entity.Id,
                DemoLink = context.HttpContext.Request.Form[nameof(ProductDemoModel.DemoLink).ToLower()].ToString(),
                ShowInProductPictureBottom = Convert.ToBoolean(context.HttpContext.Request.Form[nameof(ProductDemoModel.ShowInProductPictureBottom).ToLower()].ToString().Split(',')[0])
            };
            
            await _productDemoService.AddOrUpdateAsync(productDemoModel);
            await session.RemoveAsync(ProductLiveButtonDefaults.ProductAddOrUpdateSuccessSessionKey);
        }
    }
}
