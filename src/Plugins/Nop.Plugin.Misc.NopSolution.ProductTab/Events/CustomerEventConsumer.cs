using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Customers;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Services.Events;
using Nop.Services.Security;

namespace Nop.Plugin.Misc.VendorRegistration.Events;
public class CustomerEventConsumer :
    IConsumer<EntityInsertedEvent<Customer>>,
    IConsumer<EntityUpdatedEvent<Customer>>
{

    #region Fields

    private readonly IPermissionService _permissionService;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Ctor

    public CustomerEventConsumer(
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor)
    {
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    public async Task HandleEventAsync(EntityUpdatedEvent<Customer> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;

        var session = _httpContextAccessor.HttpContext?.Session;
        
    }
    public async Task HandleEventAsync(EntityInsertedEvent<Customer> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;

    }
}
