using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.LiveButton.Domain;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.LiveButton.Component;
public class ProductDemoViewComponent : NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new ProductDemo();
        return View("~/Plugins/Misc.LiveButton/Views/CreateOrUpdate.cshtml", model);
    }
}
