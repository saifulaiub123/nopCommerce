using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Vendors;
using Nop.Core;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Factories;
using Nop.Core.Domain.Common;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Factory;
public class VendorModelFactoryCustom: VendorModelFactory, IVendorModelFactoryCustom
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CommonSettings _commonSettings;
    protected readonly IAttributeParser<VendorAttribute, VendorAttributeValue> _vendorAttributeParser;
    protected readonly IAttributeService<VendorAttribute, VendorAttributeValue> _vendorAttributeService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPictureService _pictureService;
    protected readonly IWorkContext _workContext;
    protected readonly MediaSettings _mediaSettings;
    protected readonly VendorSettings _vendorSettings;

    #endregion

    #region Ctor

    public VendorModelFactoryCustom(
        CaptchaSettings captchaSettings,
        CommonSettings commonSettings,
        IAttributeParser<VendorAttribute, VendorAttributeValue> vendorAttributeParser,
        IAttributeService<VendorAttribute, VendorAttributeValue> vendorAttributeService,
        IGenericAttributeService genericAttributeService,
        ILocalizationService localizationService,
        IPictureService pictureService,
        IWorkContext workContext,
        MediaSettings mediaSettings,
        VendorSettings vendorSettings
        )
        :base
        (
            captchaSettings,
            commonSettings,
            vendorAttributeParser,
            vendorAttributeService,
            genericAttributeService,
            localizationService,
            pictureService,
            workContext,
            mediaSettings,
            vendorSettings
        )
        {
            _captchaSettings = captchaSettings;
            _commonSettings = commonSettings;
            _vendorAttributeParser = vendorAttributeParser;
            _vendorAttributeService = vendorAttributeService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _pictureService = pictureService;
            _workContext = workContext;
            _mediaSettings = mediaSettings;
            _vendorSettings = vendorSettings;
        }

    public virtual async Task<VendorListModel> PrepareVendorListModelAsync(VendorSearchModelCustom searchModel)
    {
        throw new NotImplementedException();
    }

    #endregion
}
