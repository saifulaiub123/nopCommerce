using Nop.Core.Domain.Vendors;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Web.Framework.Models.Extensions;
using Nop.Services.Vendors;
using Nop.Core.Domain.Directory;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Helpers;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Factory;
public class VendorModelFactoryCustom: VendorModelFactory, IVendorModelFactoryCustom
{
    #region Fields

    protected readonly CurrencySettings _currencySettings;
    protected readonly ICurrencyService _currencyService;
    protected readonly IAddressModelFactory _addressModelFactory;
    protected readonly IAddressService _addressService;
    protected readonly IAttributeParser<VendorAttribute, VendorAttributeValue> _vendorAttributeParser;
    protected readonly IAttributeService<VendorAttribute, VendorAttributeValue> _vendorAttributeService;
    protected readonly ICustomerService _customerService;
    protected readonly IDateTimeHelper _dateTimeHelper;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IVendorService _vendorService;
    protected readonly VendorSettings _vendorSettings;

    protected readonly IVendorServiceCustom _vendorServiceCustom;


    #endregion

    #region Ctor

    public VendorModelFactoryCustom
        (
            CurrencySettings currencySettings,
            ICurrencyService currencyService,
            IAddressModelFactory addressModelFactory,
            IAddressService addressService,
            IAttributeParser<VendorAttribute, VendorAttributeValue> vendorAttributeParser,
            IAttributeService<VendorAttribute, VendorAttributeValue> vendorAttributeService,
            ICustomerService customerService,
            IDateTimeHelper dateTimeHelper,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            ILocalizedModelFactory localizedModelFactory,
            IUrlRecordService urlRecordService,
            IVendorService vendorService,
            VendorSettings vendorSettings,
            IVendorServiceCustom vendorServiceCustom
        )
        : base
        (
            currencySettings,
            currencyService,
            addressModelFactory,
            addressService,
            vendorAttributeParser,
            vendorAttributeService,
            customerService,
            dateTimeHelper,
            genericAttributeService,
            localizationService,
            localizedModelFactory,
            urlRecordService,
            vendorService,
            vendorSettings
        )
    {
        _currencySettings = currencySettings;
        _currencyService = currencyService;
        _addressModelFactory = addressModelFactory;
        _addressService = addressService;
        _vendorAttributeParser = vendorAttributeParser;
        _vendorAttributeService = vendorAttributeService;
        _customerService = customerService;
        _dateTimeHelper = dateTimeHelper;
        _genericAttributeService = genericAttributeService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _urlRecordService = urlRecordService;
        _vendorService = vendorService;
        _vendorSettings = vendorSettings;
        _vendorServiceCustom = vendorServiceCustom;
    }

    public virtual async Task<VendorListModel> PrepareVendorListModelAsync(VendorSearchModelCustom searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get vendors
        var vendors = await _vendorServiceCustom.GetAllVendorsAsync(searchModel);

        //prepare list model
        var model = await new VendorListModel().PrepareToGridAsync(searchModel, vendors, () =>
        {
            //fill in model values from the entity
            return vendors.SelectAwait(async vendor =>
            {
                var vendorModel = vendor.ToModel<VendorModel>();

                vendorModel.SeName = await _urlRecordService.GetSeNameAsync(vendor, 0, true, false);

                return vendorModel;
            });
        });

        return model;
    }

    public async Task<VendorSearchModelCustom> PrepareVendorSearchModelAsync(VendorSearchModelCustom searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        searchModel.AvailableActiveValues = new List<SelectListItem> {
            new(await _localizationService.GetResourceAsync("Admin.Common.All"), string.Empty),
            new(await _localizationService.GetResourceAsync("Admin.Common.Yes"), true.ToString(), true),
            new(await _localizationService.GetResourceAsync("Admin.Common.No"), false.ToString())
        };
        searchModel.SetGridPageSize();

        return searchModel;
    }

    #endregion
}
