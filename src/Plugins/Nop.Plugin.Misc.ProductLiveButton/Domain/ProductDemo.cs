using System.ComponentModel.DataAnnotations;
using Nop.Core;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.ProductLiveButton.Domain;
public class ProductDemo : BaseEntity
{
    public int ProductId { get; set; }
    public string DemoLink { get; set; }
    //public bool ShowInProductPictureBottom { get; set; }
}
