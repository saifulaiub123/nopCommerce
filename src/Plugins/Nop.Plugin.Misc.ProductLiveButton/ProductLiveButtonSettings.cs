using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.ProductLiveButton;

public class ProductLiveButtonSettings : ISettings
{
    public string GoogleId { get; set; }
    public string ApiSecret { get; set; }
    public string TrackingScript { get; set; }
    public bool EnableEcommerce { get; set; }
    public bool UseSandbox { get; set; }
    public bool IncludingTax { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include customer identifier to script
    /// </summary>
    public bool IncludeCustomerId { get; set; }
}