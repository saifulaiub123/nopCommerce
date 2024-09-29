

namespace Nop.Plugin.Misc.ProductLiveButton.Models;
public class PublicInfoProductPageModel
{
    public ProductDemoModel ProductDemoModel { get; set; }
    public ProductLiveButtonSettings Settings { get; set; }
}

public class PublicInfoHomeProductPageModel
{
    public List<ProductDemoModel> ProductDemoModels { get; set; }
    public ProductLiveButtonSettings Settings { get; set; }
}