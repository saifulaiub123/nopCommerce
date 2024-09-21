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
public class LiveButtonPlugin : BasePlugin, IMiscPlugin, IWidgetPlugin
{
    #region Fields

    protected readonly IPermissionService _permissionService;
    protected readonly IWebHelper _webHelper;

    #endregion

    #region Ctor

    public LiveButtonPlugin(IPermissionService permissionService,
        IWebHelper webHelper)
    {
        _permissionService = permissionService;
        _webHelper = webHelper;
    }

    public bool HideInWidgetList => throw new NotImplementedException();

    #endregion

    #region Methods
    public override string GetConfigurationPageUrl()
    {
        //return $"{_webHelper.GetStoreLocation()}Admin/LiveButton/Views/Configure/Configure/Configure.cshtml";
        return $"{_webHelper.GetStoreLocation()}Admin/Brevo/Configure";
    }

    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone.Equals(AdminWidgetZones.ProductDetailsBlock))
            return typeof(ProductDemoViewComponent);

        return null;
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string>
            {
                AdminWidgetZones.ProductDetailsBlock,
            });
    }

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