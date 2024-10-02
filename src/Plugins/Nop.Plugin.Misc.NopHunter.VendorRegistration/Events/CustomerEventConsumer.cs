using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.VendorRegistration.Services;
using Nop.Services.Events;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.VendorRegistration.Events;
public class CustomerEventConsumer :
    IConsumer<EntityInsertedEvent<Customer>>
{

    #region Fields

    protected readonly IVendorRegistrationService _productDemoService;
    private readonly IPermissionService _permissionService;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Ctor

    public CustomerEventConsumer(
        IVendorRegistrationService productDemoService,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor)
    {
        _productDemoService = productDemoService;
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    public async Task HandleEventAsync(EntityInsertedEvent<Customer> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;

        var session = _httpContextAccessor.HttpContext?.Session;
        await session.SetAsync(VendorRegistrationDefaults.CustomerAddedSuccessSessionKey, eventMessage.Entity);
    }
}
