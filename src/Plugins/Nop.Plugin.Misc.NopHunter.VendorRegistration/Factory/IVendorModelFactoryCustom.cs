using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Web.Areas.Admin.Models.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Factory;
public interface IVendorModelFactoryCustom
{
    Task<VendorListModel> PrepareVendorListModelAsync(VendorSearchModelCustom searchModel);
}
