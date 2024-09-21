using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.Misc.ReplaceHomePage.Infrastructure
{
    public class AddToCartViewEngine : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.AreaName == null && context.ViewName == "_AddToCart")
            {
                viewLocations = new[] { "~/Plugins/Misc.ReplaceHomePage/Views/Product/_AddToCart.cshtml" }.Concat(viewLocations);
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
