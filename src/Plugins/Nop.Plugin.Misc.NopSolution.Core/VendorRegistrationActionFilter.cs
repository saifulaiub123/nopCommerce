using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Vendors;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.VendorRegistration.Models;
using Nop.Plugin.Misc.VendorRegistration.Services;
using Nop.Services.Cms;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.VendorRegistration;
public class VendorRegistrationActionFilter : ActionFilterAttribute
{
    protected readonly IVendorRegistrationService _vendorRegistrationService;
    protected readonly IWidgetPluginManager _widgetPluginManager;

    public VendorRegistrationActionFilter(
        IVendorRegistrationService vendorRegistrationService, 
        IWidgetPluginManager widgetPluginManager)
    {
        _vendorRegistrationService = vendorRegistrationService;
        _widgetPluginManager = widgetPluginManager;
    }


    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Result == null)
            await next();
        if (!await _widgetPluginManager.IsPluginActiveAsync(VendorRegistrationDefaults.SystemName))
            await next();

        await HandleVendorRegistrationForm(context);
    }
    private async Task HandleVendorRegistrationForm(ActionExecutingContext context)
    {
        //if (!_vendorSettings.AllowCustomersToApplyForVendorAccount)
        //    return RedirectToRoute("Homepage");

        //if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
        //    return Challenge();
        
        var controller = context.RouteData.Values["controller"].ToString() == "Customer";
        var action = context.RouteData.Values["action"].ToString() == "Register";
        var methodType = context.HttpContext.Request.Method == "POST";

        if (controller && action && methodType)
        {
            var session = context.HttpContext.Session;
            var customer = await session.GetAsync<Customer>(VendorRegistrationDefaults.CustomerAddedSuccessSessionKey);
            if (customer != null && string.IsNullOrEmpty(customer.Email))
                return;

            var accountType = "V";
            if (!string.IsNullOrEmpty(accountType) && accountType.Equals("V"))
            {
                var vendor = new ApplyVendorModel()
                {
                    Name = context.HttpContext.Request.Form["VendorName"].ToString(),
                    Description = context.HttpContext.Request.Form["VendorDescription"]
                };
                await _vendorRegistrationService.ProcessVendorRegistration(vendor, customer);
            }

            await session.RemoveAsync(VendorRegistrationDefaults.CustomerAddedSuccessSessionKey);
        }
    }
}
