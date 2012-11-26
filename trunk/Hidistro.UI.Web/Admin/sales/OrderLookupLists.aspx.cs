using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.OrderLookup)]
    public partial class OrderLookupLists : AdminPage
    {

        private void BindData()
        {
            grdOrderLookupList.DataSource = OrderHelper.GetOrderLookupLists();
            grdOrderLookupList.DataBind();
        }

        private void grdOrderLookupList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (OrderHelper.DeleteOrderLookupList((int)grdOrderLookupList.DataKeys[e.RowIndex].Value))
            {
                BindData();
                ShowMsg("成功删除了选择的订单可选项", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdOrderLookupList.RowDeleting += new GridViewDeleteEventHandler(grdOrderLookupList_RowDeleting);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
    }
}

