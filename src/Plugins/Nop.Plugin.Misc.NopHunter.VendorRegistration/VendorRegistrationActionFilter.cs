using Microsoft.AspNetCore.Mvc.Filters;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Vendors;
using Nop.Core.Http.Extensions;
using Nop.Plugin.Misc.VendorRegistration.Models;
using Nop.Plugin.Misc.VendorRegistration.Services;
using Nop.Web.Models.Catalog;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.VendorRegistration;
public class VendorRegistrationActionFilter : ActionFilterAttribute
{
    protected readonly IVendorRegistrationService _vendorRegistrationService;

    public VendorRegistrationActionFilter(IVendorRegistrationService vendorRegistrationService)
    {
        _vendorRegistrationService = vendorRegistrationService;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Result == null)
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

            var accountType = context.HttpContext.Request.Form["AccountType"].ToString();
            if (!string.IsNullOrEmpty(accountType) && accountType.Equals("V"))
            {
                await _vendorRegistrationService.ProcessVendorRegistration(new ApplyVendorModel(), customer);
            }

            await session.RemoveAsync(VendorRegistrationDefaults.CustomerAddedSuccessSessionKey);
        }
    }
}
