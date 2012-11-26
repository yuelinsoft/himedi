using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Summary)]
    public partial class Default : AdminPage
    {
        private void BindOrders()
        {
            int num;
            DataTable recentlyOrders = OrderHelper.GetRecentlyOrders(out num);
            lblOrderNumbers.Text = recentlyOrders.Rows.Count.ToString();
            hpksendOrder.Text = num.ToString();
            hpksendOrder.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?OrderStatus=2";
            allorders.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx";
            grdOrders.DataSource = recentlyOrders;
            grdOrders.DataBind();
        }

        private void BindPurchaseOrders()
        {
            int num;
            DataTable recentlyPurchaseOrders = SalesHelper.GetRecentlyPurchaseOrders(out num);
            lblPurchaseOrderNumbers.Text = recentlyPurchaseOrders.Rows.Count.ToString();
            hpksendPurchaseOrder.Text = num.ToString();
            allPurchaseOrder.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/ManagePurchaseOrder.aspx";
            hpksendPurchaseOrder.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/ManagePurchaseOrder.aspx?PurchaseStatus=2";
            grdPurchaseOrders.DataSource = recentlyPurchaseOrders;
            grdPurchaseOrders.DataBind();
        }

        private void BindStatistics(AdminStatisticsInfo statistics)
        {
            IUser user = Users.GetUser(HiContext.Current.User.UserId);
            ltrAdminName.Text = user.Username;
            lblTime.Time = user.LastLoginDate;
            ltrWaitSendOrdersNumber.Text = (statistics.OrderNumbWaitConsignment > 0) ? statistics.OrderNumbWaitConsignment.ToString() : string.Empty;
            ltrWaitSendPurchaseOrdersNumber.Text = (statistics.PurchaseOrderNumbWaitConsignment > 0) ? statistics.PurchaseOrderNumbWaitConsignment.ToString() : string.Empty;
            hpkLiuYan.Text = (statistics.LeaveComments > 0) ? statistics.LeaveComments.ToString() : "0";
            hpkZiXun.Text = (statistics.ProductConsultations > 0) ? statistics.ProductConsultations.ToString() : "0";
            hpkMessages.Text = (statistics.Messages > 0) ? statistics.Messages.ToString() : "0";
            hpkLiuYan.NavigateUrl = Globals.ApplicationPath + "/Admin/comment/ManageLeaveComments.aspx?MessageStatus=3";
            hpkZiXun.NavigateUrl = Globals.ApplicationPath + "/Admin/comment/ProductConsultations.aspx";
            hpkMessages.NavigateUrl = Globals.ApplicationPath + "/Admin/comment/ReceivedMessages.aspx?IsRead=0";
            lblTodayOrderAmout.Text = (statistics.OrderPriceToday > 0M) ? Globals.FormatMoney(statistics.OrderPriceToday) : string.Empty;
            lblTodaySalesProfile.Text = (statistics.ProfitTotal != 0M) ? Globals.FormatMoney(statistics.ProfitTotal) : string.Empty;
            ltrTodayAddMemberNumber.Text = (statistics.UserNewAddToday > 0) ? statistics.UserNewAddToday.ToString() : string.Empty;
            ltrTodayAddDistroNumber.Text = (statistics.DistroButorsNewAddToday > 0) ? statistics.DistroButorsNewAddToday.ToString() : string.Empty;
            lblMembersBalanceTotal.Text = Globals.FormatMoney(statistics.MembersBalance);
            lblDistrosBalanceTotal.Text = Globals.FormatMoney(statistics.DistrosBalance);
            lblAllBalanceTotal.Text = Globals.FormatMoney(statistics.BalanceTotal);
            lblProductCountTotal.Text = (statistics.ProductAlert > 0) ? ("<a href='" + Globals.ApplicationPath + "/Admin/product/ProductAlert.aspx'>" + statistics.ProductAlert.ToString() + "</a>") : "0";
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            decimal num;
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(lblPurchaseOrderId.Value);
            int length = 0;
            if (txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") > 0)
            {
                length = txtPurchaseOrderDiscount.Text.Trim().Substring(txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") + 1).Length;
            }
            if (!(decimal.TryParse(txtPurchaseOrderDiscount.Text.Trim(), out num) && (length <= 2)))
            {
                ShowMsg("折扣只能是数值，且不能超过2位小数", false);
            }
            else
            {
                purchaseOrder.AdjustedDiscount += num;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<PurchaseOrderInfo>(purchaseOrder, new string[] { "ValPurchaseOrder" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                        ShowMsg(msg, false);
                        return;
                    }
                }
                if (purchaseOrder.GetPurchaseTotal() >= 0M)
                {
                    if (SalesHelper.UpdatePurchaseOrderAmount(purchaseOrder))
                    {
                        BindPurchaseOrders();
                        ShowMsg("修改成功", true);
                    }
                    else
                    {
                        ShowMsg("修改失败", false);
                    }
                }
                else
                {
                    ShowMsg("折扣值不能使得采购单总金额为负", false);
                }
            }
        }

        protected void grdOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                OrderStatus status = (OrderStatus)DataBinder.Eval(e.Row.DataItem, "OrderStatus");
                HyperLink link = (HyperLink)e.Row.FindControl("lkbtnEditPrice");
                HyperLink link2 = (HyperLink)e.Row.FindControl("lkbtnSendGoods");
                switch (status)
                {
                    case OrderStatus.WaitBuyerPay:
                        link.Visible = true;
                        link.Text = link.Text + "<br />";
                        break;

                    case OrderStatus.BuyerAlreadyPaid:
                        {
                            int num = (int)DataBinder.Eval(e.Row.DataItem, "GroupBuyId");
                            if (num <= 0)
                            {
                                link2.Visible = true;
                                link2.Text = link2.Text + "<br />";
                                break;
                            }
                            GroupBuyStatus status2 = (GroupBuyStatus)DataBinder.Eval(e.Row.DataItem, "GroupBuyStatus");
                            if (status2 == GroupBuyStatus.Success)
                            {
                                link2.Visible = true;
                                link2.Text = link2.Text + "<br />";
                            }
                            break;
                        }
                }
            }
        }

        private void grdPurchaseOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink link = (HyperLink)e.Row.FindControl("lkbtnSendGoods");
                HtmlGenericControl control = (HtmlGenericControl)e.Row.FindControl("lkbtnEditPurchaseOrder");
                Label label = (Label)e.Row.FindControl("lblDistroStatus");
                switch (((OrderStatus)DataBinder.Eval(e.Row.DataItem, "PurchaseStatus")))
                {
                    case OrderStatus.WaitBuyerPay:
                        control.Visible = true;
                        control.InnerHtml = control.InnerHtml + "<br />";
                        label.Text = "还未付款";
                        break;

                    case OrderStatus.BuyerAlreadyPaid:
                        link.Visible = true;
                        link.Text = link.Text + "<br />";
                        label.Text = "已经付款";
                        break;

                    case OrderStatus.Closed:
                        label.Text = "已取消该采购单";
                        break;

                    case OrderStatus.Finished:
                        label.Text = "已经确认收货";
                        break;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdOrders.RowDataBound += new GridViewRowEventHandler(grdOrders_RowDataBound);
            grdPurchaseOrders.RowDataBound += new GridViewRowEventHandler(grdPurchaseOrders_RowDataBound);
            btnEditOrder.Click += new EventHandler(btnEditOrder_Click);
            if (!Page.IsPostBack)
            {
                int num;
                if (int.TryParse(base.Request.QueryString["Status"], out num))
                {
                    hidStatus.Value = num.ToString();
                }
                AdminStatisticsInfo statistics = SalesHelper.GetStatistics();
                BindStatistics(statistics);
                BindPurchaseOrders();
                BindOrders();
            }
            if ((Page.Request["action"] == "EditPurchaseOrder") && (Page.Request["PurchaseId"] != null))
            {
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(Page.Request["PurchaseId"].ToString());
                StringBuilder builder = new StringBuilder();
                if (purchaseOrder != null)
                {
                    builder.Append("{");
                    builder.Append("\"PurchaseTotal\":\"" + purchaseOrder.GetPurchaseTotal().ToString("F", CultureInfo.InvariantCulture) + "\"");
                    builder.Append("}");
                    Page.Response.ContentType = "text/plain";
                    base.Response.Write(builder.ToString());
                    base.Response.End();
                }
            }
        }
    }
}

