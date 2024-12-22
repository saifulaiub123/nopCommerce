using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Controllers;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Factory;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Infrastructure;
using Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
using Nop.Plugin.Misc.VendorRegistration.Services;
using Nop.Services.Media;
using Nop.Web.Controllers;

namespace Nop.Plugin.Misc.VendorRegistration.Infrastructure;

/// <summary>
/// Represents object for the configuring services on application startup
/// </summary>
public class NopStartup : INopStartup
{
    /// <summary>
    /// Add and configure any of the middleware
    /// </summary>
    /// <param name="services">Collection of service descriptors</param>
    /// <param name="configuration">Configuration of the application</param>
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IVendorRegistrationService, VendorRegistrationService>();
        services.AddScoped<IVendorWorkflowMessageService, VendorWorkflowMessageService>();
        services.AddScoped<IVendorServiceCustom, VendorServiceCustom>();
        services.AddScoped<IVendorModelFactoryCustom, VendorModelFactoryCustom>();
        services.AddScoped<VendorController, VendorCustomController>();
        services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add<VendorRegistrationActionFilter>();
        });
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationExpanders.Add(new ViewEngine());
        });

    }

    /// <summary>
    /// Configure the using of added middleware
    /// </summary>
    /// <param name="application">Builder for configuring an application's request pipeline</param>
    public void Configure(IApplicationBuilder application)
    {
        
    }

    /// <summary>
    /// Gets order of this startup configuration implementation
    /// </summary>
    public int Order => 3000;
}