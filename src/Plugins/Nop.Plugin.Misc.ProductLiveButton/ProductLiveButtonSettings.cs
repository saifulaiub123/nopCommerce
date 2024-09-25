using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.ProductLiveButton;

public class ProductLiveButtonSettings : ISettings
{
    /// <summary>
    /// Gets or sets button title
    /// </summary>
    public string ButtonTitle { get; set; }
    public string ButtonBackgroundColor { get; set; }
    public string ButtonTextColor { get; set; }
    public bool ShowInProductBox { get; set; }
    public string CustomCss { get; set; }
}