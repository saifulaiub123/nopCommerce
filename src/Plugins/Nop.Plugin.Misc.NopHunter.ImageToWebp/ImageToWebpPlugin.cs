using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Nop.Core;
using Nop.Plugin.Misc.LiveButton.Component;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.WebApi.Frontend;

/// <summary>
/// Represents the Web API frontend plugin
/// </summary>
public class ImageToWebpPlugin : BasePlugin, IMiscPlugin
{

    #region Ctor

    public ImageToWebpPlugin(){ }

    #endregion

    #region Methods


    public override async Task InstallAsync()
    {
        await base.InstallAsync();
    }
    
    public override async Task UninstallAsync()
    {
        
        await base.UninstallAsync();
    }

    #endregion
}