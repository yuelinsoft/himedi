using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class Default : DistributorPage
    {

        private void BindBusinessInformation(StatisticsInfo statisticsInfo)
        {
            ltrWaitSendOrdersNumber.Text = statisticsInfo.OrderNumbWaitConsignment.ToString();
            hpkZiXun.Text = statisticsInfo.ProductConsultations.ToString();
            hpkMessages.Text = statisticsInfo.Messages.ToString();
            hpkLiuYan.Text = statisticsInfo.LeaveComments.ToString();
            lblTodayOrderAmout.Text = Globals.FormatMoney(statisticsInfo.OrderPriceToday);
            lblTodaySalesProfile.Text = Globals.FormatMoney(statisticsInfo.OrderProfitToday);
            ltrTodayAddMemberNumber.Text = statisticsInfo.UserNewAddToday.ToString();
            lblMembersBalanceTotal.Text = Globals.FormatMoney(statisticsInfo.Balance);
            lblProductCountTotal.Text = (statisticsInfo.ProductAlert > 0) ? ("<a href='" + Globals.ApplicationPath + "/Shopadmin/product/MyProductAlert.aspx'>" + statisticsInfo.ProductAlert.ToString() + "</a>") : "0";
            ltrWaitSendPurchaseOrdersNumber.Text = statisticsInfo.PurchaseOrderNumbWaitConsignment.ToString();
            hpkLiuYan.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/ManageMyLeaveComments.aspx?MessageStatus=3";
            hpkZiXun.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/MyProductConsultations.aspx";
            hpkMessages.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/MyReceivedMessages.aspx?IsRead=0";
        }

        private void BindLabels()
        {
            Distributor distributor = SubsiteStoreHelper.GetDistributor();
            ltrAdminName.Text = distributor.Username;
            lblTime.Time = distributor.LastLoginDate;
            AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
            lblDistrosBalanceTotal.Text = (myAccountSummary.AccountAmount > 0M) ? Globals.FormatMoney(myAccountSummary.AccountAmount) : string.Empty;
        }

        private void BindOrders()
        {
            int num;
            DataTable recentlyOrders = SubsiteSalesHelper.GetRecentlyOrders(out num);
            lblOrderNumbers.Text = recentlyOrders.Rows.Count.ToString();
            hpksendOrder.Text = num.ToString();
            hpksendOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/Shopadmin/sales/ManageMyOrder.aspx?OrderStatus={0}", 2);
            allorders.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/ManageMyOrder.aspx";
            grdOrders.DataSource = recentlyOrders;
            grdOrders.DataBind();
        }

        private void BindPurchaseOrders()
        {
            int num;
            DataTable recentlyPurchaseOrders = SubsiteSalesHelper.GetRecentlyPurchaseOrders(out num);
            lblPurchaseOrderNumbers.Text = recentlyPurchaseOrders.Rows.Count.ToString();
            allPurchaseOrder.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyPurchaseOrder.aspx";
            allPurchaseOrder2.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx";
            grdPurchaseOrders.DataSource = recentlyPurchaseOrders;
            grdPurchaseOrders.DataBind();
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hidPurchaseOrderId.Value))
            {
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(hidPurchaseOrderId.Value);
                purchaseOrder.CloseReason = ddlCloseReason.SelectedValue;
                if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
                {
                    BindPurchaseOrders();
                    ShowMsg("取消采购成功", true);
                }
                else
                {
                    ShowMsg("取消采购失败", false);
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
                HtmlGenericControl control = (HtmlGenericControl)e.Row.FindControl("lkBtnCancelPurchaseOrder");
                HyperLink link2 = (HyperLink)e.Row.FindControl("lkbtnPay");
                OrderStatus status = (OrderStatus)DataBinder.Eval(e.Row.DataItem, "PurchaseStatus");
                string purchaseOrderId = (string)DataBinder.Eval(e.Row.DataItem, "PurchaseOrderId");
                PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
                if (!purchaseOrder.IsManualPurchaseOrder && (status == OrderStatus.SellerAlreadySent))
                {
                    OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(purchaseOrder.OrderId);
                    if ((orderInfo != null) && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid))
                    {
                        link.Visible = true;
                    }
                }
                if (status == OrderStatus.WaitBuyerPay)
                {
                    control.Visible = true;
                    control.InnerHtml = control.InnerHtml + "<br />";
                    link2.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdOrders.RowDataBound += new GridViewRowEventHandler(grdOrders_RowDataBound);
            grdPurchaseOrders.RowDataBound += new GridViewRowEventHandler(grdPurchaseOrders_RowDataBound);
            btnClosePurchaseOrder.Click += new EventHandler(btnClosePurchaseOrder_Click);
            if (!base.IsPostBack)
            {
                int num;
                if (int.TryParse(base.Request.QueryString["Status"], out num))
                {
                    hidStatus.Value = num.ToString();
                }
                BindLabels();
                StatisticsInfo statistics = SubsiteSalesHelper.GetStatistics();
                BindBusinessInformation(statistics);
                BindPurchaseOrders();
                BindOrders();
            }
        }
    }
}

