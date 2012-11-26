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
    [PrivilegeCheck(Privilege.ShippingModes)]
    public partial class ShippingTypes : AdminPage
    {

        public void BindData()
        {
            grdShippingModes.DataSource = SalesHelper.GetShippingModes();
            grdShippingModes.DataBind();
        }

        private void grdShippingModes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                int modeId = (int)grdShippingModes.DataKeys[rowIndex].Value;
                int displaySequence = int.Parse((grdShippingModes.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceModeId = 0;
                int replaceDisplaySequence = 0;
                string commandName = e.CommandName;
                if (commandName != null)
                {
                    if (commandName != "Fall")
                    {
                        if ((commandName == "Rise") && (rowIndex > 0))
                        {
                            replaceModeId = (int)grdShippingModes.DataKeys[rowIndex - 1].Value;
                            replaceDisplaySequence = int.Parse((grdShippingModes.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                        }
                    }
                    else if (rowIndex < (grdShippingModes.Rows.Count - 1))
                    {
                        replaceModeId = (int)grdShippingModes.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = int.Parse((grdShippingModes.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                if ((replaceModeId > 0) && (replaceDisplaySequence > 0))
                {
                    SalesHelper.SwapShippingModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
                    BindData();
                }
            }
        }

        private void grdShippingModes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SalesHelper.DeleteShippingMode((int)grdShippingModes.DataKeys[e.RowIndex].Value))
            {
                BindData();
                ShowMsg("删除成功", true);
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdShippingModes.RowCommand += new GridViewCommandEventHandler(grdShippingModes_RowCommand);
            grdShippingModes.RowDeleting += new GridViewDeleteEventHandler(grdShippingModes_RowDeleting);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
    }
}

