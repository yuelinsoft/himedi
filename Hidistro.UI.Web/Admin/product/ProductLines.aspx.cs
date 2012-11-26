using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductLines)]
    public partial class ProductLines : AdminPage
    {
        private void BindData()
        {
            grdProductLine.DataSource = ProductLineHelper.GetProductLines();
            grdProductLine.DataBind();
        }

        private void grdProductLine_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int lineId = (int)grdProductLine.DataKeys[e.RowIndex].Value;
            if (ProductLineHelper.DeleteProductLine(lineId))
            {
                BindData();
                ShowMsg("成功删除了已选定的产品线", true);
            }
            else
            {
                ShowMsg("不能删除有商品的产品线或最后一个产品线", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdProductLine.RowDeleting += new GridViewDeleteEventHandler(grdProductLine_RowDeleting);
            if (!base.IsPostBack)
            {
                BindData();
            }
        }
    }
}

