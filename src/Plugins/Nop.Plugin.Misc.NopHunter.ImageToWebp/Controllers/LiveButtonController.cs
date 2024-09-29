using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.WebApi.Frontend.Controllers;

[AutoValidateAntiforgeryToken]
[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
public class LiveButtonController : BasePluginController
{
    #region Fields

    protected readonly IPermissionService _permissionService;

    #endregion

    #region Ctor 

    public LiveButtonController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_PLUGINS)]
    public virtual async Task<IActionResult> Configure()
    {
        return View("~/Plugins/Misc.ReplaceHomePage/Views/Configure/Configure.cshtml");
    }

    #endregion
}