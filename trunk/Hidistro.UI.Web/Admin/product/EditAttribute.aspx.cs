using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.product.ascx;
using System;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.EditProductType)]
    public partial class EditAttribute : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

