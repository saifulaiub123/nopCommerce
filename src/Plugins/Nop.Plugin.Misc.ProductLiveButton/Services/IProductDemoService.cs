
using Nop.Data;
using Nop.Plugin.Misc.ProductLiveButton.Models;

namespace Nop.Plugin.Misc.ProductLiveButton.Services;
public interface IProductDemoService
{
    Task<ProductDemoModel> GetByProductId(int productId);
    Task<List<ProductDemoModel>> GetByProductIds(List<int> productIds);

    /// <summary>
    /// Add configuration of GoogleAuthenticator
    /// </summary>
    /// <param name="customerEmail">Customer email</param>
    /// <param name="key">Secret key</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task AddOrUpdateAsync(ProductDemoModel model);

    Task UpdateAsync(ProductDemoModel model);

    Task DeleteAsync(int? productId);
}
