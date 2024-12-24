using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Web.Framework;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Infrastructure;
public class ViewEngine : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (context.AreaName == null && context.ViewName == "ApplyVendor")
        {
            viewLocations = new[] { "~/Plugins/Misc.NopHunter.VendorRegistration/Views/Vendor/ApplyVendor.cshtml" }.Concat(viewLocations);
        }
        else if (context.AreaName == null && context.ViewName == "_CustomerForm")
        {
            viewLocations = new[] { "~/Plugins/Misc.NopHunter.VendorRegistration/Views/Vendor/_CustomerForm.cshtml" }.Concat(viewLocations);
        }
        else if (context.AreaName == null && context.ViewName == "_VendorForm")
        {
            viewLocations = new[] { "~/Plugins/Misc.NopHunter.VendorRegistration/Views/Vendor/_VendorForm.cshtml" }.Concat(viewLocations);
        }
        //else if (context.AreaName == AreaNames.ADMIN && context.ViewName == "_Table.Definition")
        //{
        //    viewLocations = new[] { "~/Plugins/Misc.NopHunter.VendorRegistration/Areas/Admin/Views/Shared/_Table.Definition.cshtml" }.Concat(viewLocations);
        //}
        return viewLocations;
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }
}

