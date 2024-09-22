using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.ProductDemoLink.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.ProductDemoLink.Component;
public class ProductDemoLinkViewComponent : NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new ProductDemoModel();
        return View("~/Plugins/Misc.ProductDemoLink/Views/CreateOrUpdate.cshtml", model);
    }
}
