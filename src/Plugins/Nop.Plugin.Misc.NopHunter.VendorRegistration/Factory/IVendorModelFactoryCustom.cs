using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Factory;
public interface IVendorModelFactoryCustom : IVendorModelFactory
{
    Task<VendorListModel> PrepareVendorListModelAsync(VendorSearchModelCustom searchModel);
    Task<VendorSearchModelCustom> PrepareVendorSearchModelAsync(VendorSearchModelCustom searchModel);
}
