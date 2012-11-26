using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.product.ascx;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    /// <summary>
    /// 添加产品类型
    /// </summary>
    [PrivilegeCheck(Privilege.AddProductType)]
    public partial class AddAttribute : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.GetAdminAbsolutePath("/product/AddSpecification.aspx?typeId=" + Page.Request.QueryString["typeId"]), true);
        }


    }
}

