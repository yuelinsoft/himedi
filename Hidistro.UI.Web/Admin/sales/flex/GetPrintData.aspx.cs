﻿using Hidistro.ControlPanel.Sales;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class GetPrintData : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string shipperId = base.Request.Form["shipperId"];
            string orderId = base.Request.Form["orderId"];
            if (!string.IsNullOrEmpty(shipperId) && !string.IsNullOrEmpty(orderId))
            {
                int result = 0;
                if (int.TryParse(shipperId, out result))
                {
                    ShippersInfo shipper = SalesHelper.GetShipper(result);
                    if (shipper != null)
                    {
                        OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                        if (orderInfo != null)
                        {
                            WriteOrderInfo(orderInfo, shipper);
                        }
                        else
                        {
                            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(orderId);
                            if (purchaseOrder != null)
                            {
                                WritPurchaseOrderInfo(purchaseOrder, shipper);
                            }
                        }
                    }
                }
            }
        }

        private void WriteOrderInfo(OrderInfo order, ShippersInfo shipper)
        {
            string[] strArray = RegionHelper.GetFullRegion(order.RegionId, ",").Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<nodes>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.ShipTo);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.TelPhone + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.CellPhone + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.ZipCode + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.Address);
            builder.AppendLine("</item>");
            if (strArray.Length > 0)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区1级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[0]);
                builder.AppendLine("</item>");
            }
            if (strArray.Length > 1)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区2级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[1]);
                builder.AppendLine("</item>");
            }
            if (strArray.Length > 2)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区3级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[2]);
                builder.AppendLine("</item>");
            }
            if (shipper != null)
            {
                string[] strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, ",").Split(new char[] { ',' });
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-姓名</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.ShipperName);
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-手机</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.CellPhone + "_");
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-电话</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.TelPhone + "_");
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-地址</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.Address);
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-邮编</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.Zipcode + "_");
                builder.AppendLine("</item>");
                if (strArray2.Length > 0)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区1级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[0]);
                    builder.AppendLine("</item>");
                }
                if (strArray2.Length > 1)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区2级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[1]);
                    builder.AppendLine("</item>");
                }
                if (strArray2.Length > 2)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区3级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[2]);
                    builder.AppendLine("</item>");
                }
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-订单号</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.OrderId);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-总金额</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.GetTotal() + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-物品总重量</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.Weight);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-备注</name>");
            builder.AppendFormat("<rename>{0}</rename>", order.ManagerRemark);
            builder.AppendLine("</item>");
            string shipperId = "";
            if ((order.LineItems != null) && (order.LineItems.Count > 0))
            {
                foreach (LineItemInfo info in order.LineItems.Values)
                {
                    object obj2 = shipperId;
                    shipperId = string.Concat(new object[] { obj2, "货号 ", info.SKU, " ", info.SKUContent, " \x00d7", info.ShipmentQuantity, "\n" });
                }
                shipperId = shipperId.Replace("；", "").Replace(";", "").Replace("：", ":");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-详情</name>");
            builder.AppendFormat("<rename>{0}</rename>", shipperId);
            builder.AppendLine("</item>");
            if (order.ShippingDate == DateTime.Parse("0001-1-1"))
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>订单-送货时间</name>");
                builder.AppendFormat("<rename>{0}</rename>", "null");
                builder.AppendLine("</item>");
            }
            else
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>订单-送货时间</name>");
                builder.AppendFormat("<rename>{0}</rename>", order.ShippingDate);
                builder.AppendLine("</item>");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>网店名称</name>");
            builder.AppendFormat("<rename>{0}</rename>", HiContext.Current.SiteSettings.SiteName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>自定义内容</name>");
            builder.AppendFormat("<rename>{0}</rename>", "null");
            builder.AppendLine("</item>");
            builder.AppendLine("</nodes>");
            base.Response.Write(builder.ToString());
        }

        private void WritPurchaseOrderInfo(PurchaseOrderInfo prurchaseOrder, ShippersInfo shipper)
        {
            string[] strArray = RegionHelper.GetFullRegion(prurchaseOrder.RegionId, ",").Split(new char[] { ',' });
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<nodes>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-姓名</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.ShipTo);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-电话</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.TelPhone + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-手机</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.CellPhone + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-邮编</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.ZipCode + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>收货人-地址</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.Address);
            builder.AppendLine("</item>");
            if (strArray.Length > 0)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区1级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[0]);
                builder.AppendLine("</item>");
            }
            if (strArray.Length > 1)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区2级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[1]);
                builder.AppendLine("</item>");
            }
            if (strArray.Length > 2)
            {
                builder.AppendLine("<item>");
                builder.AppendLine("<name>收货人-地区3级</name>");
                builder.AppendFormat("<rename>{0}</rename>", strArray[2]);
                builder.AppendLine("</item>");
            }
            if (shipper != null)
            {
                string[] strArray2 = RegionHelper.GetFullRegion(shipper.RegionId, ",").Split(new char[] { ',' });
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-姓名</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.ShipperName);
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-手机</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.CellPhone + "_");
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-电话</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.TelPhone + "_");
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-地址</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.Address);
                builder.AppendLine("</item>");
                builder.AppendLine("<item>");
                builder.AppendLine("<name>发货人-邮编</name>");
                builder.AppendFormat("<rename>{0}</rename>", shipper.Zipcode + "_");
                builder.AppendLine("</item>");
                if (strArray2.Length > 0)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区1级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[0]);
                    builder.AppendLine("</item>");
                }
                if (strArray2.Length > 1)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区2级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[1]);
                    builder.AppendLine("</item>");
                }
                if (strArray2.Length > 2)
                {
                    builder.AppendLine("<item>");
                    builder.AppendLine("<name>发货人-地区3级</name>");
                    builder.AppendFormat("<rename>{0}</rename>", strArray2[2]);
                    builder.AppendLine("</item>");
                }
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-订单号</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.PurchaseOrderId);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-总金额</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.GetPurchaseTotal() + "_");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-物品总重量</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.Weight);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-备注</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.ManagerRemark);
            builder.AppendLine("</item>");
            string shipperId = "";
            if ((prurchaseOrder.PurchaseOrderItems != null) && (prurchaseOrder.PurchaseOrderItems.Count > 0))
            {
                foreach (PurchaseOrderItemInfo info in prurchaseOrder.PurchaseOrderItems)
                {
                    object obj2 = shipperId;
                    shipperId = string.Concat(new object[] { obj2, "货号 ", info.SKU, " ", info.SKUContent, " \x00d7", info.Quantity, "\n" });
                }
                shipperId = shipperId.Replace("；", "").Replace(";", "").Replace("：", ":");
            }
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-详情</name>");
            builder.AppendFormat("<rename>{0}</rename>", shipperId);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>订单-送货时间</name>");
            builder.AppendFormat("<rename>{0}</rename>", prurchaseOrder.ShippingDate);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>网店名称</name>");
            builder.AppendFormat("<rename>{0}</rename>", HiContext.Current.SiteSettings.SiteName);
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>√</name>");
            builder.AppendFormat("<rename>{0}</rename>", "√");
            builder.AppendLine("</item>");
            builder.AppendLine("<item>");
            builder.AppendLine("<name>自定义内容</name>");
            builder.AppendFormat("<rename>{0}</rename>", "null");
            builder.AppendLine("</item>");
            builder.AppendLine("</nodes>");
            base.Response.Write(builder.ToString());
        }
    }
}

