using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.VendorRegistration.Models;

public record ConfigurationModel : BaseNopModel
{
    public int ActiveStoreScopeConfiguration { get; set; }

}