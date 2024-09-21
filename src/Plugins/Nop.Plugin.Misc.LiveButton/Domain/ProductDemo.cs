using Nop.Core;

namespace Nop.Plugin.Misc.LiveButton.Domain;
public class ProductDemo : BaseEntity
{
    public int ProductId { get; set; }
    public string DemoLink { get; set; }
    public bool ShowInProductPictureBottom { get; set; }
}
