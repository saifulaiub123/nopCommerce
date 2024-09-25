
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.ProductLiveButton.Models;
public record ProductDemoModel : BaseNopEntityModel
{
    public int ProductId { get; set; }

    [NopResourceDisplayName("Plugins.Misc.ProductLiveButton.Field.DemoLink")]
    [UIHint("Show demo button in picture")]
    public string DemoLink { get; set; }

    //[NopResourceDisplayName("Plugins.Misc.ProductLiveButton.ShowInProductPictureBottom")]
    //[UIHint("Show demo button in picture")]
    //public bool ShowInProductPictureBottom { get; set; }
}
