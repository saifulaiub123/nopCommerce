using Nop.Core.Domain.Vendors;
using Nop.Core;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Services.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
public interface IVendorServiceCustom
{
    Task<IPagedList<Vendor>> GetAllVendorsAsync(VendorSearchModelCustom vendorSearchModel);
    Task<IList<Vendor>> GetAllVendorsByIds(List<int> ids);
    Task UpdateVendors(List<Vendor> vendors);
}
