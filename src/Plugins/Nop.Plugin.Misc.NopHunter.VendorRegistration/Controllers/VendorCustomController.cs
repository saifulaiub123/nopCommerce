using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Web.Controllers;
using Nop.Web.Factories;
using Nop.Web.Models.Customer;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Controllers;
public class VendorCustomController : VendorController
{
    #region Fields

    protected readonly CaptchaSettings _captchaSettings;
    protected readonly IAttributeParser<VendorAttribute, VendorAttributeValue> _vendorAttributeParser;
    protected readonly IAttributeService<VendorAttribute, VendorAttributeValue> _vendorAttributeService;
    protected readonly ICustomerService _customerService;
    protected readonly IDownloadService _downloadService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IHtmlFormatter _htmlFormatter;
    protected readonly ILocalizationService _localizationService;
    protected readonly IPictureService _pictureService;
    protected readonly IUrlRecordService _urlRecordService;
    protected readonly IVendorModelFactory _vendorModelFactory;
    protected readonly IVendorService _vendorService;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly VendorSettings _vendorSettings;
    private static readonly char[] _separator = [','];


    protected readonly ICustomerModelFactory _customerModelFactory;

    #endregion

    #region Ctor

    public VendorCustomController(CaptchaSettings captchaSettings,
        IAttributeParser<VendorAttribute, VendorAttributeValue> vendorAttributeParser,
        IAttributeService<VendorAttribute, VendorAttributeValue> vendorAttributeService,
        ICustomerService customerService,
        IDownloadService downloadService,
        IGenericAttributeService genericAttributeService,
        IHtmlFormatter htmlFormatter,
        ILocalizationService localizationService,
        IPictureService pictureService,
        IUrlRecordService urlRecordService,
        IVendorModelFactory vendorModelFactory,
        IVendorService vendorService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        VendorSettings vendorSettings,
        ICustomerModelFactory customerModelFactory)
        : base
        (
            captchaSettings,
            vendorAttributeParser,
            vendorAttributeService,
            customerService,
            downloadService,
            genericAttributeService,
            htmlFormatter,
            localizationService,
            pictureService,
            urlRecordService,
            vendorModelFactory,
            vendorService,
            workContext,
            workflowMessageService,
            localizationSettings,
            vendorSettings
            )
    {
        _captchaSettings = captchaSettings;
        _vendorAttributeParser = vendorAttributeParser;
        _vendorAttributeService = vendorAttributeService;
        _customerService = customerService;
        _downloadService = downloadService;
        _genericAttributeService = genericAttributeService;
        _htmlFormatter = htmlFormatter;
        _localizationService = localizationService;
        _pictureService = pictureService;
        _urlRecordService = urlRecordService;
        _vendorModelFactory = vendorModelFactory;
        _vendorService = vendorService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
        _vendorSettings = vendorSettings;
        _customerModelFactory = customerModelFactory;
    }

    #endregion

    #region Methods
    public override async Task<IActionResult> ApplyVendor()
    {
        if (!_vendorSettings.AllowCustomersToApplyForVendorAccount)
            return RedirectToRoute("Homepage");

        var model = new VendorRegistrationCustomModel();

        var vendorModel = new ApplyVendorModel();
        model.Vendor = await _vendorModelFactory.PrepareApplyVendorModelAsync(vendorModel, true, false, null);

        var registerModel = new RegisterModel();
        model.Customer = await _customerModelFactory.PrepareRegisterModelAsync(registerModel, false, setDefaultValues: true);

        return View(model);
    }
    #endregion
}
