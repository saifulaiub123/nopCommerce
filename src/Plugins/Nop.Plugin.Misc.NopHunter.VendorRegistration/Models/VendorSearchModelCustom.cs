using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
public record VendorSearchModelCustom : VendorSearchModel
{
    public VendorSearchModelCustom()
    {
        AvailableActiveValues = new List<SelectListItem>();
    }
    [NopResourceDisplayName("Admin.Vendors.List.SearchIsActive")]
    public bool? SearchIsActive { get; set; } = false;
    public IList<SelectListItem> AvailableActiveValues { get; set; }
}
