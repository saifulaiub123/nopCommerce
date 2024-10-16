using Nop.Core.Domain.Vendors;
using Nop.Core;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
public interface IVendorServiceCustom
{
    Task<IPagedList<Vendor>> GetAllVendorsAsync(VendorSearchModelCustom vendorSearchModel);
}
