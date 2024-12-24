using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Plugin.Misc.VendorRegistration.Services;
using Nop.Plugin.Misc.VendorRegistration;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Core.Domain.Vendors;
using Nop.Core.Http.Extensions;
using Nop.Services.Cms;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Events;
public class VendorEventConsumer :
    IConsumer<EntityInsertedEvent<Vendor>>
{

    #region Fields

    protected readonly IVendorRegistrationService _productDemoService;
    private readonly IPermissionService _permissionService;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IWidgetPluginManager _widgetPluginManager;

    #endregion

    #region Ctor

    public VendorEventConsumer(
        IVendorRegistrationService productDemoService,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor,
        IWidgetPluginManager widgetPluginManager)
    {
        _productDemoService = productDemoService;
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
        _widgetPluginManager = widgetPluginManager;
    }

    #endregion

    public async Task HandleEventAsync(EntityInsertedEvent<Vendor> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;
        
        if (!await _widgetPluginManager.IsPluginActiveAsync(VendorRegistrationDefaults.SystemName))
            return;

        var session = _httpContextAccessor.HttpContext?.Session;
        await session.SetAsync(VendorRegistrationDefaults.CustomerAddedSuccessSessionKey, eventMessage.Entity);
    }
}

