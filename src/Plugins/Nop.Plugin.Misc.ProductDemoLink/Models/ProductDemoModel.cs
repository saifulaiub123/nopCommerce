
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.ProductDemoLink.Models;
public record ProductDemoModel : BaseNopEntityModel
{
    public int ProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductDemoLink.DemoLink")]
    [UIHint("Show demo button in picture")]
    public string DemoLink { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductDemoLink.ShowInProductPictureBottom")]
    [UIHint("Show demo button in picture")]
    public bool ShowInProductPictureBottom { get; set; }
}
