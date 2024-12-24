using Nop.Web.Models.Customer;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.NopSolution.ProductTab.Models;
public class ProductTabCustomModel
{
    public ApplyVendorModel Vendor { get; set; }
    public RegisterModel Customer { get; set; }
}
