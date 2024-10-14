using Nop.Web.Areas.Admin.Models.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
public record VendorSearchModelCustom : VendorSearchModel
{
    public bool? Active { get; set; }
}
