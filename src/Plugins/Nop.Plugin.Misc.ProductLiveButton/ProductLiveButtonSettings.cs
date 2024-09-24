using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.ProductLiveButton;

public class ProductLiveButtonSettings : ISettings
{
    /// <summary>
    /// Gets or sets button title
    /// </summary>
    public string ButtonTitle { get; set; }
    public string ShowInProductPictureBottom { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include customer identifier to script
    /// </summary>
    public bool IncludeCustomerId { get; set; }
}