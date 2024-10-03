﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Misc.VendorRegistration.Models;
using Nop.Plugin.Misc.VendorRegistration.Services;
using Nop.Services.Configuration;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.VendorRegistration.Components;
public class RegisterPageViewComponent : NopViewComponent
{
    protected readonly IVendorRegistrationService _productDemoService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IVendorModelFactory _vendorModelFactory;

    public RegisterPageViewComponent(
        IVendorRegistrationService productDemoService,
        ISettingService settingService,
        IStoreContext storeContext,
        IVendorModelFactory vendorModelFactory)
    {
        _productDemoService = productDemoService;
        _settingService = settingService;
        _storeContext = storeContext;
        _vendorModelFactory = vendorModelFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new ApplyVendorModel();
        model = await _vendorModelFactory.PrepareApplyVendorModelAsync(model, true, false, null);
        return View("~/Plugins/Misc.NopHunter.VendorRegistration/Views/RegistrationPageView.cshtml",model);
    }

    //public class ProductDemoEventConsumer : IConsumer<AdminProductDetailsCreated>
    //{
    //    public void HandleEvent(AdminProductDetailsCreated eventMessage)
    //    {
    //        // Insert the product demo view component into the product creation page
    //        var component = new TagBuilder("div");
    //        component.InnerHtml.AppendHtml(eventMessage.ViewComponentHelper.RenderComponentAsync("ProductDemo", new { productId = eventMessage.ProductId }).Result);
    //        eventMessage.BlocksToRender.Add(component);
    //    }
    //}


}
