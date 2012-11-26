using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.AddProductType)]
    public partial class AddProductTypeFinish : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

