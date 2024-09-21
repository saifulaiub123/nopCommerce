//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Routing;
//using Nop.Web.Framework;
//using Nop.Web.Framework.Mvc.Routing;
//using Nop.Web.Infrastructure;

//namespace Nop.Plugin.Misc.ReplaceHomePage.Infrastructure;
//public class RouteProvider : BaseRouteProvider, IRouteProvider
//{

    
//    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
//    {
//        var lang = GetLanguageRoutePattern();

//        endpointRouteBuilder.MapControllerRoute(name: "Nop.Plugin.Misc.ReplaceHomePage",
//            pattern: "product/test",
//            defaults: new { controller = "Home", action = "CustomIndex" },
//            new[] { "Nop.Plugin.Misc.ReplaceHomePage.Controllers" });
//    }

//    public int Priority
//    {
//        get
//        {
//            return int.MaxValue;
//        }
//    }

//}

