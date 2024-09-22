using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Misc.ProductLiveButton.Component;
public class ProductLiveButtonViewComponent : NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new ProductDemoModel();
        return View("~/Plugins/Misc.ProductLiveButton/Views/CreateOrUpdate.cshtml", model);
    }
}
