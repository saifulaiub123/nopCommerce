using Nop.Web.Models.Customer;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
public class VendorRegistrationCustomModel
{
    public ApplyVendorModel Vendor { get; set; }
    public RegisterModel Customer { get; set; }
}
