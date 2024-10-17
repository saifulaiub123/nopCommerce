using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Vendors;
using Nop.Web.Models.Vendors;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Services;
public interface IVendorWorkflowMessageService
{
    Task<IList<int>> SendVendorAccountCreationNotificationToVendor(Customer customer, Vendor vendor, int languageId);
    Task SendVendorAccountActivationNotificationToVendors(List<Vendor> vendors, int languageId);
}
