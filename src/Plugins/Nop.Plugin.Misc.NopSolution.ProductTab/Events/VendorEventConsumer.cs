using Microsoft.AspNetCore.Http;
using Nop.Core.Events;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Core.Domain.Vendors;
using Nop.Core.Http.Extensions;
using Nop.Services.Cms;

namespace Nop.Plugin.Misc.NopSolution.ProductTab.Events;
public class VendorEventConsumer :
    IConsumer<EntityInsertedEvent<Vendor>>
{

    #region Fields

    private readonly IPermissionService _permissionService;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IWidgetPluginManager _widgetPluginManager;

    #endregion

    #region Ctor

    public VendorEventConsumer(
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IWidgetPluginManager widgetPluginManager)
    {
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
        _widgetPluginManager = widgetPluginManager;
    }

    #endregion

    public async Task HandleEventAsync(EntityInsertedEvent<Vendor> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;
        
        if (!await _widgetPluginManager.IsPluginActiveAsync(ProductTabDefaults.SystemName))
            return;

        var session = _httpContextAccessor.HttpContext?.Session;
        await session.SetAsync(ProductTabDefaults.CustomerAddedSuccessSessionKey, eventMessage.Entity);
    }
}

