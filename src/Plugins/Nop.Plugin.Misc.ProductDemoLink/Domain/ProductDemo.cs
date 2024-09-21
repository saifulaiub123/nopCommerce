using Nop.Core;

namespace Nop.Plugin.Misc.ProductDemoLink.Domain;
public class ProductDemo : BaseEntity
{
    public int ProductId { get; set; }
    public string DemoLink { get; set; }
    public bool ShowInProductPictureBottom { get; set; }
}
