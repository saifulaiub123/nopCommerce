using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.ProductLiveButton.Models;

public record ConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Misc.ProductLiveButton.Field.ButtonTitle")]
    public string ButtonTitle { get; set; }
    [NopResourceDisplayName("Plugins.Misc.ProductLiveButton.Field.ButtonBackgroundColor")]
    public string ButtonBackgroundColor { get; set; }
    [NopResourceDisplayName("Plugins.Misc.ProductLiveButton.Field.ButtonTextColor")]
    public string ButtonTextColor { get; set; }
    [NopResourceDisplayName("Plugins.Misc.ProductLiveButton.Field.ShowInProductBox")]
    public bool ShowInProductBox { get; set; }
    [NopResourceDisplayName("Plugins.Misc.ProductLiveButton.Field.CustomCss")]
    public string CustomCss { get; set; }
    public int ActiveStoreScopeConfiguration { get; set; }

}