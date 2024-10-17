using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Vendors;
using Nop.Core;
using Nop.Data;
using Nop.Core.Domain.Customers;
using Nop.Services.Html;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Services.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
public class VendorServiceCustom: IVendorServiceCustom
{
    #region Fields
    protected readonly IRepository<Vendor> _vendorRepository;

    #endregion

    #region Ctor

    public VendorServiceCustom(IRepository<Vendor> vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    #endregion

    #region Method

    public virtual async Task<IPagedList<Vendor>> GetAllVendorsAsync(VendorSearchModelCustom vendorSearchModel)
    {
        var vendors = await _vendorRepository.GetAllPagedAsync(query =>
        {
            if (!string.IsNullOrWhiteSpace(vendorSearchModel.SearchName))
                query = query.Where(v => v.Name.Contains(vendorSearchModel.SearchName));

            if (!string.IsNullOrWhiteSpace(vendorSearchModel.SearchEmail))
                query = query.Where(v => v.Email.Contains(vendorSearchModel.SearchEmail));
            if(vendorSearchModel.SearchIsActive.HasValue)
                query = query.Where(v => v.Active == vendorSearchModel.SearchIsActive.Value);

            query = query.Where(v => !v.Deleted);
            query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Name).ThenBy(v => v.Email);

            return query;
        }, vendorSearchModel.Page - 1, vendorSearchModel.PageSize, false, false);

        return vendors;
    }
    public virtual async Task<IList<Vendor>> GetAllVendorsByIds(List<int> ids)
    {
        var vendors = await _vendorRepository.GetByIdsAsync(ids, null, false);
        return vendors;
    }
    public async Task UpdateVendors(List<Vendor> vendors)
    {
        await _vendorRepository.UpdateAsync(vendors, false);
    }

    #endregion
}
