using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.NopSolution.ProductTab.Models;
public partial record ProductTabModel: BaseNopModel
{
    public IList<SelectListItem> AvailablePositions { get; set; }
    public IList<SelectListItem> AvailableCategories { get; set; }
    public IList<SelectListItem> AvailableOrderBy { get; set; }
}
