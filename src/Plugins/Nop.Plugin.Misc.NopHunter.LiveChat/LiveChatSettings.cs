using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.NopHunter.LiveChat;

/// <summary>
/// Represents plugin settings
/// </summary>
public class LiveChatSettings : ISettings
{
    /// <summary>
    /// Gets or sets the API key
    /// </summary>
    public string Script { get; set; }

}