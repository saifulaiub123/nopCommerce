using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Models;
public class ActivateVendorModel
{
    public string SelectedIds { get; set; }
    public bool IsSendEmail { get; set; }
}
