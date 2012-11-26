using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ExpressTemplates)]
    public partial class AddExpressTemplate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

