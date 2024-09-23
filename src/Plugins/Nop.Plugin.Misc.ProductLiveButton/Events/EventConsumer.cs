﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.ProductLiveButton.Models;
using Nop.Plugin.Misc.ProductLiveButton.Services;
using Nop.Services.Authentication;
using Nop.Services.Catalog;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Customer;

namespace Nop.Plugin.Misc.ProductLiveButton.Events;
public class EventConsumer :
    IConsumer<ModelPreparedEvent<BaseNopModel>>,
    IConsumer<ModelReceivedEvent<BaseNopModel>>,
    IConsumer<EntityInsertedEvent<Product>>,
    IConsumer<EntityUpdatedEvent<Product>>,
    IConsumer<EntityDeletedEvent<Product>>
{

    #region Fields

    protected readonly IProductDemoService _productDemoService;
    private readonly IPermissionService _permissionService;
    protected readonly IHttpContextAccessor _httpContextAccessor;

    #endregion

    #region Ctor

    public EventConsumer(
        IProductDemoService productDemoService,
        IPermissionService permissionService,
        IHttpContextAccessor httpContextAccessor)
    {
        _productDemoService = productDemoService;
        _permissionService = permissionService;
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    public async Task HandleEventAsync(EntityInsertedEvent<Product> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;

        var session = _httpContextAccessor.HttpContext?.Session;
        await session.SetAsync(ProductLiveButtonDefaults.ProductAddOrUpdateSuccessSessionKey, eventMessage.Entity);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<Product> eventMessage)
    {
        if (eventMessage.Entity is null)
            return;

        var session = _httpContextAccessor.HttpContext?.Session;
        await session.SetAsync<Product>(String.Format(ProductLiveButtonDefaults.ProductAddOrUpdateSuccessSessionKey), eventMessage.Entity);
    }

    public async Task HandleEventAsync(EntityDeletedEvent<Product> eventMessage)
    {
        throw new NotImplementedException();
    }
}
