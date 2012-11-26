using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.OrderSendGoods)]
    public partial class SendGoods : AdminPage
    {
         string orderId;

        private void BindExpressCompany(int modeId)
        {
            expressRadioButtonList.ExpressCompany = SalesHelper.GetExpressCompanysByMode(modeId);
            expressRadioButtonList.BindSelectExpressCompany();
        }

        private void BindOrderItems(OrderInfo order)
        {
            lblOrderId.Text = order.OrderId;
            lblOrderTime.Time = order.OrderDate;
            itemsList.Order = order;
        }

        private void BindShippingAddress(OrderInfo order)
        {
            string shippingRegion = string.Empty;
            if (!string.IsNullOrEmpty(order.ShippingRegion))
            {
                shippingRegion = order.ShippingRegion;
            }
            if (!string.IsNullOrEmpty(order.Address))
            {
                shippingRegion = shippingRegion + order.Address;
            }
            if (!string.IsNullOrEmpty(order.ShipTo))
            {
                shippingRegion = shippingRegion + "  " + order.ShipTo;
            }
            if (!string.IsNullOrEmpty(order.ZipCode))
            {
                shippingRegion = shippingRegion + "  " + order.ZipCode;
            }
            if (!string.IsNullOrEmpty(order.TelPhone))
            {
                shippingRegion = shippingRegion + "  " + order.TelPhone;
            }
            if (!string.IsNullOrEmpty(order.CellPhone))
            {
                shippingRegion = shippingRegion + "  " + order.CellPhone;
            }
            litReceivingInfo.Text = shippingRegion;
        }

        private void BindUpdateSippingAddress(OrderInfo order)
        {
            txtShipTo.Text = order.ShipTo;
            dropRegions.SetSelectedRegionId(new int?(order.RegionId));
            txtAddress.Text = order.Address;
            txtZipcode.Text = order.ZipCode;
            txtTelPhone.Text = order.TelPhone;
            txtCellPhone.Text = order.CellPhone;
        }

        private void btnMondifyAddress_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            orderInfo.ShipTo = txtShipTo.Text.Trim();
            orderInfo.RegionId = dropRegions.GetSelectedRegionId().Value;
            orderInfo.Address = txtAddress.Text.Trim();
            orderInfo.TelPhone = txtTelPhone.Text.Trim();
            orderInfo.CellPhone = txtCellPhone.Text.Trim();
            orderInfo.ZipCode = txtZipcode.Text.Trim();
            orderInfo.ShippingRegion = dropRegions.SelectedRegions;
            if (string.IsNullOrEmpty(txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(txtCellPhone.Text.Trim()))
            {
                ShowMsg("电话号码和手机号码必填其一", false);
            }
            else if (OrderHelper.MondifyAddress(orderInfo))
            {
                OrderInfo order = OrderHelper.GetOrderInfo(orderId);
                BindShippingAddress(order);
                BindUpdateSippingAddress(order);
                ShowMsg("修改成功", true);
            }
            else
            {
                ShowMsg("修改失败", false);
            }
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
            if (orderInfo != null)
            {
                if ((orderInfo.GroupBuyId > 0) && (orderInfo.GroupBuyStatus != GroupBuyStatus.Success))
                {
                    ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
                }
                else if (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid)
                {
                    ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
                }
                else if (!radioShippingMode.SelectedValue.HasValue)
                {
                    ShowMsg("请选择配送方式", false);
                }
                else if (string.IsNullOrEmpty(txtShipOrderNumber.Text.Trim()) || (txtShipOrderNumber.Text.Trim().Length > 20))
                {
                    ShowMsg("运单号码不能为空，在1至20个字符之间", false);
                }
                else if (string.IsNullOrEmpty(expressRadioButtonList.SelectedValue))
                {
                    ShowMsg("请选择物流公司", false);
                }
                else
                {
                    ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(radioShippingMode.SelectedValue.Value, true);
                    orderInfo.RealShippingModeId = radioShippingMode.SelectedValue.Value;
                    orderInfo.RealModeName = shippingMode.Name;
                    orderInfo.ExpressCompanyAbb = expressRadioButtonList.SelectedValue;
                    orderInfo.ExpressCompanyName = expressRadioButtonList.SelectedItem.Text;
                    orderInfo.ShipOrderNumber = txtShipOrderNumber.Text;
                    if (OrderHelper.SendGoods(orderInfo))
                    {
                        if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && (orderInfo.GatewayOrderId.Trim().Length > 0))
                        {
                            PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
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
                        ShowMsg("发货成功", true);
                    }
                    else
                    {
                        ShowMsg("发货失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                orderId = Page.Request.QueryString["OrderId"];
                OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                BindOrderItems(orderInfo);
                btnSendGoods.Click += new EventHandler(btnSendGoods_Click);
                btnMondifyAddress.Click += new EventHandler(btnMondifyAddress_Click);
                radioShippingMode.SelectedIndexChanged += new EventHandler(radioShippingMode_SelectedIndexChanged);
                if (!Page.IsPostBack)
                {
                    if (orderInfo == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        radioShippingMode.DataBind();
                        radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
                        BindExpressCompany(orderInfo.ShippingModeId);
                        expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyAbb;
                        BindShippingAddress(orderInfo);
                        BindUpdateSippingAddress(orderInfo);
                        litShippingModeName.Text = orderInfo.ModeName;
                        litRemark.Text = orderInfo.Remark;
                    }
                }
            }
        }

        private void radioShippingMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radioShippingMode.SelectedValue.HasValue)
            {
                BindExpressCompany(radioShippingMode.SelectedValue.Value);
            }
        }
    }
}

