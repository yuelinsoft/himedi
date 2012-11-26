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
    [PrivilegeCheck(Privilege.PaymentModes)]
    public partial class PaymentTypes : AdminPage
    {

        private void BindData()
        {
            grdPaymentMode.DataSource = SalesHelper.GetPaymentModes();
            grdPaymentMode.DataBind();
        }

        private void grdPaymentMode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Sort")
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                int modeId = (int)grdPaymentMode.DataKeys[rowIndex].Value;
                int displaySequence = Convert.ToInt32((grdPaymentMode.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text);
                int replaceModeId = 0;
                int replaceDisplaySequence = 0;
                if (e.CommandName == "Fall")
                {
                    if (rowIndex < (grdPaymentMode.Rows.Count - 1))
                    {
                        replaceModeId = (int)grdPaymentMode.DataKeys[rowIndex + 1].Value;
                        replaceDisplaySequence = Convert.ToInt32((grdPaymentMode.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text);
                    }
                }
                else if ((e.CommandName == "Rise") && (rowIndex > 0))
                {
                    replaceModeId = (int)grdPaymentMode.DataKeys[rowIndex - 1].Value;
                    replaceDisplaySequence = Convert.ToInt32((grdPaymentMode.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text);
                }
                if ((replaceModeId > 0) && (replaceDisplaySequence > 0))
                {
                    SalesHelper.SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
                    BindData();
                }
            }
        }

        private void grdPaymentMode_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SalesHelper.DeletePaymentMode((int)grdPaymentMode.DataKeys[e.RowIndex].Value))
            {
                BindData();
                ShowMsg("成功删除了一个支付方式", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdPaymentMode.RowDeleting += new GridViewDeleteEventHandler(grdPaymentMode_RowDeleting);
            grdPaymentMode.RowCommand += new GridViewCommandEventHandler(grdPaymentMode_RowCommand);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
    }
}

