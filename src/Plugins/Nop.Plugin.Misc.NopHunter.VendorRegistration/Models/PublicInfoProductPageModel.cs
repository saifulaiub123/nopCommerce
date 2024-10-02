

namespace Nop.Plugin.Misc.VendorRegistration.Models;
public class PublicInfoProductPageModel
{
    public ProductDemoModel ProductDemoModel { get; set; }
    public VendorRegistrationSettings Settings { get; set; }
}

public class PublicInfoHomeProductPageModel
{
    public List<ProductDemoModel> ProductDemoModels { get; set; }
    public VendorRegistrationSettings Settings { get; set; }
}