using Nop.Core.Domain.Customers;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.VendorRegistration.Services;
public interface IVendorRegistrationService
{
    Task<ApplyVendorModel> ProcessVendorRegistration(ApplyVendorModel model, Customer customer);
}
