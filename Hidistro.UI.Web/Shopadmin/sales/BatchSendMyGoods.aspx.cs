using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin.sales
{
    public partial class BatchSendMyGoods : DistributorPage
    {

        private void BindData()
        {
            DropdownColumn column = (DropdownColumn)grdOrderGoods.Columns[4];
            column.DataSource = SalesHelper.GetShippingModes();
            DbQueryResult sendGoodsOrders = SubsiteSalesHelper.GetSendGoodsOrders(GetOrderQuery());
            grdOrderGoods.DataSource = sendGoodsOrders.Data;
            grdOrderGoods.DataBind();
            pager2.TotalRecords = pager1.TotalRecords = sendGoodsOrders.TotalRecords;
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            if (grdOrderGoods.Rows.Count <= 0)
            {
                ShowMsg("没有要进行发货的订单。", false);
            }
            else
            {
                DropdownColumn column = (DropdownColumn)grdOrderGoods.Columns[4];
                ListItemCollection selectedItems = column.SelectedItems;
                int num = 0;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    string orderId = (string)grdOrderGoods.DataKeys[grdOrderGoods.Rows[i].RowIndex].Value;
                    TextBox box = (TextBox)grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
                    ListItem item = selectedItems[i];
                    int result = 0;
                    int.TryParse(item.Value, out result);
                    OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
                    if (((orderInfo != null) && ((orderInfo.GroupBuyId <= 0) || (orderInfo.GroupBuyStatus == GroupBuyStatus.Success))) && (((orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid) && (result > 0)) && (!string.IsNullOrEmpty(box.Text) && (box.Text.Length <= 20))))
                    {
                        ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(result, true);
                        orderInfo.RealShippingModeId = shippingMode.ModeId;
                        orderInfo.RealModeName = shippingMode.Name;
                        orderInfo.ExpressCompanyAbb = shippingMode.ExpressCompanyAbb;
                        orderInfo.ExpressCompanyName = shippingMode.ExpressCompanyName;
                        orderInfo.ShipOrderNumber = box.Text;
                        if (SubsiteSalesHelper.SendGoods(orderInfo))
                        {
                            if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
                            {
                                PaymentModeInfo paymentMode = SubsiteSalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
                                if (paymentMode != null)
                                {
                                    PaymentRequest.CreateInstance(paymentMode.Gateway, Cryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[] { paymentMode.Gateway })), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[] { paymentMode.Gateway })), "").SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
                                }
                            }
                            int userId = orderInfo.UserId;
                            if (userId == 0x44c)
                            {
                                userId = 0;
                            }
                            IUser user = Users.GetUser(userId);
                            Messenger.OrderShipping(orderInfo, user);
                            orderInfo.OnDeliver();
                        }
                        num++;
                    }
                }
                if (num == 0)
                {
                    ShowMsg("批量发货失败！，发货数量0个", false);
                }
                else if (num > 0)
                {
                    BindData();
                    ShowMsg(string.Format("批量发货成功！，发货数量{0}个", num), true);
                }
            }
        }

        private OrderQuery GetOrderQuery()
        {
            OrderQuery query = new OrderQuery();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ShipTo"]))
            {
                query.ShipTo = Globals.UrlDecode(Page.Request.QueryString["ShipTo"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["UserName"]))
            {
                query.UserName = Globals.UrlDecode(Page.Request.QueryString["UserName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["StartDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["StartDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["EndDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["EndDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderStatus"]))
            {
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["OrderStatus"], out result))
                {
                    query.Status = (OrderStatus)result;
                }
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["GroupBuyId"]))
            {
                query.GroupBuyId = new int?(int.Parse(Page.Request.QueryString["GroupBuyId"]));
            }
            query.PageIndex = pager1.PageIndex;
            query.PageSize = pager1.PageSize;
            query.SortBy = "OrderDate";
            query.SortOrder = SortAction.Desc;
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnBatchSendGoods.Click += new EventHandler(btnSendGoods_Click);
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }
    }
}

