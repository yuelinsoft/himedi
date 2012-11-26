using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Shippers)]
    public partial class Shippers : AdminPage
    {

        private void BindShippers()
        {
            grdShippers.DataSource = SalesHelper.GetShippers(false);
            grdShippers.DataBind();
        }

        private void grdShippers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int shipperId = (int)grdShippers.DataKeys[namingContainer.RowIndex].Value;
                if (!SalesHelper.GetShipper(shipperId).IsDefault)
                {
                    SalesHelper.SetDefalutShipper(shipperId);
                    BindShippers();
                }
            }
        }

        private void grdShippers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int shipperId = (int)grdShippers.DataKeys[e.RowIndex].Value;
            if (SalesHelper.DeleteShipper(shipperId))
            {
                BindShippers();
                ShowMsg("已经成功删除选择的发货信息", true);
            }
            else
            {
                ShowMsg("不能删除默认的发货信息", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdShippers.RowDeleting += new GridViewDeleteEventHandler(grdShippers_RowDeleting);
            grdShippers.RowCommand += new GridViewCommandEventHandler(grdShippers_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindShippers();
            }
        }
    }
}

