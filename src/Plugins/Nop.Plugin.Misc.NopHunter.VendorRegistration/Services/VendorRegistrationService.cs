using Nop.Core.Caching;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.VendorRegistration.Domain;
using Nop.Plugin.Misc.VendorRegistration.Models;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.VendorRegistration.Infrastructure.Mapper;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Vendors;
using Nop.Core.Domain.Security;
using DocumentFormat.OpenXml.EMMA;
using Nop.Core.Domain.Localization;
using Nop.Services.Vendors;
using System.Net;
using Nop.Services.Attributes;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Html;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Seo;
using Nop.Web.Factories;
using Nop.Web.Models.Vendors;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;

namespace Nop.Plugin.Misc.VendorRegistration.Services;
public class VendorRegistrationService : IVendorRegistrationService
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
    protected readonly IVendorWorkflowMessageService _vendorWorkflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly VendorSettings _vendorSettings;
    private static readonly char[] _separator = [','];

    #endregion

    #region Ctr
    public VendorRegistrationService(
        CaptchaSettings captchaSettings,
        IAttributeParser<VendorAttribute,
            VendorAttributeValue> vendorAttributeParser,
        IAttributeService<VendorAttribute,
            VendorAttributeValue> vendorAttributeService,
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
        IVendorWorkflowMessageService vendorWorkflowMessageService)
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
        _vendorWorkflowMessageService = vendorWorkflowMessageService;
    }
    #endregion

    #region Utilities

    /// <summary>
    /// Insert the configuration
    /// </summary>
    /// <param name="configuration">Configuration</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    //protected async Task InsertConfigurationAsync(GoogleAuthenticatorRecord configuration)
    //{
    //    ArgumentNullException.ThrowIfNull(configuration);

    //    await _repository.InsertAsync(configuration);
    //    await _staticCacheManager.RemoveByPrefixAsync(GoogleAuthenticatorDefaults.PrefixCacheKey);
    //}

    /// <summary>
    /// Update the configuration
    /// </summary>
    /// <param name="configuration">Configuration</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    //protected async Task UpdateConfigurationAsync(GoogleAuthenticatorRecord configuration)
    //{
    //    ArgumentNullException.ThrowIfNull(configuration);

    //    await _repository.UpdateAsync(configuration);
    //    await _staticCacheManager.RemoveByPrefixAsync(GoogleAuthenticatorDefaults.PrefixCacheKey);
    //}

    /// <summary>
    /// Delete the configuration
    /// </summary>
    /// <param name="configuration">Configuration</param>
    //internal async Task DeleteConfigurationAsync(GoogleAuthenticatorRecord configuration)
    //{
    //    ArgumentNullException.ThrowIfNull(configuration);

    //    await _repository.DeleteAsync(configuration);
    //    await _staticCacheManager.RemoveByPrefixAsync(GoogleAuthenticatorDefaults.PrefixCacheKey);
    //}

    /// <summary>
    /// Get a configuration by the identifier
    /// </summary>
    /// <param name="configurationId">Configuration identifier</param>
    /// <returns>Configuration</returns>
    //internal async Task<GoogleAuthenticatorRecord> GetConfigurationByIdAsync(int configurationId)
    //{
    //    if (configurationId == 0)
    //        return null;

    //    return await _staticCacheManager.GetAsync(_staticCacheManager.PrepareKeyForDefaultCache(GoogleAuthenticatorDefaults.ConfigurationCacheKey, configurationId), async () =>
    //        await _repository.GetByIdAsync(configurationId));
    //}

    //internal GoogleAuthenticatorRecord GetConfigurationByCustomerEmail(string email)
    //{
    //    if (string.IsNullOrEmpty(email))
    //        return null;

    //    var query = _repository.Table;
    //    return query.FirstOrDefault(record => record.Customer == email);
    //}

    #endregion

    #region Methods

    /// <summary>
    /// Get configurations
    /// </summary>
    /// <param name="productId">ProductId</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the paged list of configurations
    /// </returns>
    public async Task<ApplyVendorModel> ProcessVendorRegistration(ApplyVendorModel model,Customer customer)
    {
        if (!_vendorSettings.AllowCustomersToApplyForVendorAccount)
        {
            model.Result = await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Submitted");
            return model;
        }
            
        if (!await _customerService.IsRegisteredAsync(customer))
        {
            model.Result = await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Submitted");
            return model;
        }

        if (await _customerService.IsAdminAsync(customer))
        {
            model.Result = await _localizationService.GetResourceAsync("Vendors.ApplyAccount.IsAdmin");
            return model;
        }

        //if (_captchaSettings.Enabled && _captchaSettings.ShowOnApplyVendorPage && !captchaValid)
        //{
        //    ModelState.AddModelError("", await _localizationService.GetResourceAsync("Common.WrongCaptchaMessage"));
        //}
        var pictureId = 0;

        //if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
        //{
        //    try
        //    {
        //        var contentType = uploadedFile.ContentType.ToLowerInvariant();

        //        if (!contentType.StartsWith("image/") || contentType.StartsWith("image/svg"))
        //            ModelState.AddModelError("", await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Picture.ErrorMessage"));
        //        else
        //        {
        //            var vendorPictureBinary = await _downloadService.GetDownloadBitsAsync(uploadedFile);
        //            var picture = await _pictureService.InsertPictureAsync(vendorPictureBinary, contentType, null);

        //            if (picture != null)
        //                pictureId = picture.Id;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        ModelState.AddModelError("", await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Picture.ErrorMessage"));
        //    }
        //}


        var description = _htmlFormatter.FormatText(model.Description, false, false, true, false, false, false);
        //disabled by default
        var vendor = new Vendor
        {
            Name = model.Name,
            Email = customer.Email,
            PageSize = 6,
            AllowCustomersToSelectPageSize = true,
            PageSizeOptions = _vendorSettings.DefaultVendorPageSizeOptions,
            PictureId = pictureId,
            Description = WebUtility.HtmlEncode(description)
        };
        await _vendorService.InsertVendorAsync(vendor);
        //search engine name (the same as vendor name)
        var seName = await _urlRecordService.ValidateSeNameAsync(vendor, vendor.Name, vendor.Name, true);
        await _urlRecordService.SaveSlugAsync(vendor, seName, 0);

        customer.VendorId = vendor.Id;
        await _customerService.UpdateCustomerAsync(customer);

        //notify store owner here (email)
        await _workflowMessageService.SendNewVendorAccountApplyStoreOwnerNotificationAsync(customer, vendor, _localizationSettings.DefaultAdminLanguageId);
        await _vendorWorkflowMessageService.SendVendorAccountCreationNotificationToVendor(customer, vendor, _localizationSettings.DefaultAdminLanguageId);
        model.DisableFormInput = true;
        model.Result = await _localizationService.GetResourceAsync("Vendors.ApplyAccount.Submitted");

        return model;
    }
    
    #endregion

    #region Properties
    #endregion
}
