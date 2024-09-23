using Nop.Core.Caching;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.ProductLiveButton.Domain;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using static Nop.Services.Security.StandardPermission;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Misc.ProductLiveButton.Services;
public class ProductDemoService : IProductDemoService
{
    #region Fields

    protected readonly IRepository<ProductDemo> _productDemoRepository;
    protected readonly IRepository<Product> _productRepository;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly IWorkContext _workContext;
    //protected readonly GoogleAuthenticatorSettings _googleAuthenticatorSettings;
    //protected TwoFactorAuthenticator _twoFactorAuthenticator;

    #endregion

    #region Ctr

    public ProductDemoService(
        IRepository<ProductDemo> productDemoRepository,
        IStaticCacheManager staticCacheManager,
        IWorkContext workContext,
        IRepository<Product> productRepository)
    {
        _staticCacheManager = staticCacheManager;
        _workContext = workContext;
        _productDemoRepository = productDemoRepository;
        _productRepository = productRepository;
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
    public async Task<ProductDemoModel> GetByProductId(int productId)
    {
        var existingRecord = await _productDemoRepository.Table.
            FirstOrDefaultAsync(record => record.ProductId == productId);

        if(existingRecord is not null) return existingRecord.ToModel<ProductDemoModel>();
       
        return null;
    }

    /// <summary>
    /// Add configuration of GoogleAuthenticator
    /// </summary>
    /// <param name="customerEmail">Customer email</param>
    /// <param name="key">Secret key</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task AddOrUpdateAsync(ProductDemoModel model)
    {
        if (model.ProductId == 0)
            return;

        var existingRecord = _productDemoRepository.Table.
            FirstOrDefault(record => record.ProductId == model.ProductId);

        if (existingRecord is not null)
        {
            existingRecord.DemoLink = model.DemoLink;
            existingRecord.ShowInProductPictureBottom = model.ShowInProductPictureBottom;
            await _productDemoRepository.UpdateAsync(existingRecord, false);

        }
        else
        {
            var entity = model.ToEntity<ProductDemo>();
            await _productDemoRepository.InsertAsync(entity, false);
        }
       
        return;
        
    }
    public async Task UpdateAsync(ProductDemoModel model)
    {
        if (model.ProductId == 0)
            return;

        var existingRecord = _productDemoRepository.Table.
            FirstOrDefault(record => record.ProductId == model.ProductId);

        //ArgumentNullException.ThrowIfNull(existingRecord);

        existingRecord.DemoLink = model.DemoLink;
        existingRecord.ShowInProductPictureBottom = model.ShowInProductPictureBottom;

        await _productDemoRepository.UpdateAsync(existingRecord, false);

        return;

    }
    public async Task DeleteAsync(int? productId)
    {
        if (productId == 0 || productId == null)
            return;

        var existingRecord = _productDemoRepository.Table.
            FirstOrDefault(record => record.ProductId == productId);

        ArgumentNullException.ThrowIfNull(existingRecord);

        await _productDemoRepository.DeleteAsync(existingRecord, false);

        return;

    }
    #endregion

    #region Properties
    #endregion
}
