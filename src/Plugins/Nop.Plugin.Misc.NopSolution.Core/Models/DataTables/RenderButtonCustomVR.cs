
using Nop.Web.Framework.Models.DataTables;

namespace Nop.Plugin.Misc.NopHunter.VendorRegistration.Models.DataTables;
public class RenderButtonCustomVR : RenderButtonCustom
{
    public RenderButtonCustomVR(string className, string title) : base(className, title)
    {
    }

    public ButtonDisable ButtonDisable { get; set; }
}
public class ButtonDisable
{
    public string ColumnName { get; set; }
    public string Value { get; set; }
}
