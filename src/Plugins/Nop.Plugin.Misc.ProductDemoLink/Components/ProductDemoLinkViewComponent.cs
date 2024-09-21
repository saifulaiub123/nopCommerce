using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.ProductDemoLink.Domain;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.ProductDemoLink.Component;
public class ProductDemoLinkViewComponent : NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new ProductDemo();
        return View("~/Plugins/Misc.ProductDemoLink/Views/CreateOrUpdate.cshtml", model);
    }
}
