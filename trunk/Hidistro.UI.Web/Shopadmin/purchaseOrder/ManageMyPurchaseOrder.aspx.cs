using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ManageMyPurchaseOrder : DistributorPage
    {


        private void BindPurchaseOrders()
        {
            PurchaseOrderQuery purchaseOrderQuery = GetPurchaseOrderQuery();
            purchaseOrderQuery.SortOrder = SortAction.Desc;
            purchaseOrderQuery.SortBy = "PurchaseDate";
            purchaseOrderQuery.IsManualPurchaseOrder = false;
            DbQueryResult purchaseOrders = SubsiteSalesHelper.GetPurchaseOrders(purchaseOrderQuery);
            dlstPurchaseOrders.DataSource = purchaseOrders.Data;
            dlstPurchaseOrders.DataBind();
            pager.TotalRecords = purchaseOrders.TotalRecords;
            pager1.TotalRecords = purchaseOrders.TotalRecords;
            txtOrderId.Text = purchaseOrderQuery.OrderId;
            txtProductName.Text = purchaseOrderQuery.ProductName;
            txtPurchaseOrderId.Text = purchaseOrderQuery.PurchaseOrderId;
            calendarStartDate.SelectedDate = purchaseOrderQuery.StartDate;
            calendarEndDate.SelectedDate = purchaseOrderQuery.EndDate;
            lblStatus.Text = ((int)purchaseOrderQuery.PurchaseStatus).ToString();
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

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBinderPurchaseOrders(false);
        }

        private void dlstPurchaseOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(e.CommandArgument.ToString());
            if (((purchaseOrder != null) && (e.CommandName == "FINISH_TRADE")) && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
            {
                if (SubsiteSalesHelper.ConfirmPurchaseOrderFinish(purchaseOrder))
                {
                    BindPurchaseOrders();
                    ShowMsg("成功的完成了该采购单", true);
                }
                else
                {
                    ShowMsg("完成采购单失败", false);
                }
            }
        }

        private void dlstPurchaseOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HyperLink link = (HyperLink)e.Item.FindControl("lkbtnPay");
                HyperLink link2 = (HyperLink)e.Item.FindControl("lkbtnSendGoods");
                HtmlGenericControl control = (HtmlGenericControl)e.Item.FindControl("lkBtnCancelPurchaseOrder");
                ImageLinkButton button = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmPurchaseOrder");
                OrderStatus status = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "PurchaseStatus");
                string orderId = (string)DataBinder.Eval(e.Item.DataItem, "OrderId");
                switch (status)
                {
                    case OrderStatus.WaitBuyerPay:
                        link.Visible = true;
                        control.Visible = true;
                        break;

                    case OrderStatus.SellerAlreadySent:
                        {
                            OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
                            if ((orderInfo == null) || (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid))
                            {
                                button.Visible = true;
                                break;
                            }
                            link2.Visible = true;
                            break;
                        }
                }
            }
        }

        private PurchaseOrderQuery GetPurchaseOrderQuery()
        {
            PurchaseOrderQuery query = new PurchaseOrderQuery();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["PurchaseOrderId"]))
            {
                query.PurchaseOrderId = Globals.UrlDecode(Page.Request.QueryString["PurchaseOrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["startDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["endDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["endDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["PurchaseStatus"]))
            {
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["PurchaseStatus"], out result))
                {
                    query.PurchaseStatus = (OrderStatus)result;
                }
            }
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dlstPurchaseOrders.ItemDataBound += new DataListItemEventHandler(dlstPurchaseOrders_ItemDataBound);
            dlstPurchaseOrders.ItemCommand += new DataListCommandEventHandler(dlstPurchaseOrders_ItemCommand);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            btnClosePurchaseOrder.Click += new EventHandler(btnClosePurchaseOrder_Click);
            if (!Page.IsPostBack)
            {
                SetPurchaseOrderStatusLink();
                BindPurchaseOrders();
            }
        }

        private void ReBinderPurchaseOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", txtOrderId.Text.Trim());
            queryStrings.Add("PurchaseOrderId", txtPurchaseOrderId.Text.Trim());
            queryStrings.Add("ProductName", txtProductName.Text.Trim());
            queryStrings.Add("PurchaseStatus", lblStatus.Text);
            if (calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", calendarStartDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", calendarEndDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetPurchaseOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyPurchaseOrder.aspx?PurchaseStatus={0}";
            hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            hlinkNotPay.NavigateUrl = string.Format(format, 1);
            hlinkYetPay.NavigateUrl = string.Format(format, 2);
            hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            hlinkClose.NavigateUrl = string.Format(format, 4);
            hlinkHistory.NavigateUrl = string.Format(format, 0x63);
            hlinkFinish.NavigateUrl = string.Format(format, 5);
        }
    }
}

