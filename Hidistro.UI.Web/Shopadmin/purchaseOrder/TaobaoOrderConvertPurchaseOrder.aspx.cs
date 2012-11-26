using Hidistro.Core.Cryptography;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Sales;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class TaobaoOrderConvertPurchaseOrder : Page
    {
        ITopClient client;
        HttpCookie cookie;
        Hidistro.Membership.Context.Distributor distributor;
        string orderIds = string.Empty;
        ShippingModeInfo shippingMode;

        private PurchaseOrderInfo GetPurchaseOrder(Trade trade)
        {
            PurchaseOrderInfo info = new PurchaseOrderInfo();

            info.PurchaseOrderId = "MPO" + trade.Tid.ToString();
            info.TaobaoOrderId = trade.Tid.ToString();
            info.ShipTo = trade.ReceiverName;
            info.ShippingRegion = trade.ReceiverState + trade.ReceiverCity + trade.ReceiverDistrict;
            info.RegionId = RegionHelper.GetRegionIdByName(trade.ReceiverState, trade.ReceiverCity, trade.ReceiverDistrict);
            info.Address = trade.ReceiverAddress;
            info.TelPhone = trade.ReceiverPhone;
            info.ZipCode = trade.ReceiverZip;
            info.CellPhone = trade.ReceiverMobile;
            info.ShippingModeId = shippingMode.ModeId;
            info.ModeName = shippingMode.Name;
            info.RealShippingModeId = shippingMode.ModeId;
            info.RealModeName = shippingMode.Name;
            info.AdjustedDiscount = 0M;
            info.OrderTotal = decimal.Parse(trade.Payment);
            int totalWeight = 0;

            foreach (Order order in trade.Orders)
            {

                DataTable skuContent = SubSiteProducthelper.GetSkuContent(order.NumIid, order.OuterSkuId);

                if ((skuContent != null) && (skuContent.Rows.Count > 0))
                {
                    PurchaseOrderItemInfo item = new PurchaseOrderItemInfo();

                    foreach (DataRow row in skuContent.Rows)
                    {
                        if (!(string.IsNullOrEmpty(row["AttributeName"].ToString()) || string.IsNullOrEmpty(row["ValueStr"].ToString())))
                        {
                            object sKUContent = item.SKUContent;
                            item.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                        }
                    }
                    item.PurchaseOrderId = info.PurchaseOrderId;
                    item.SkuId = (string)skuContent.Rows[0]["SkuId"];
                    item.ProductId = (int)skuContent.Rows[0]["ProductId"];
                    if (skuContent.Rows[0]["SKU"] != DBNull.Value)
                    {
                        item.SKU = (string)skuContent.Rows[0]["SKU"];
                    }
                    if (skuContent.Rows[0]["Weight"] != DBNull.Value)
                    {
                        item.ItemWeight = (int)skuContent.Rows[0]["Weight"];
                    }
                    item.ItemPurchasePrice = (decimal)skuContent.Rows[0]["PurchasePrice"];
                    item.Quantity = int.Parse(order.Num.ToString());
                    item.ItemListPrice = (decimal)skuContent.Rows[0]["SalePrice"];
                    if (skuContent.Rows[0]["CostPrice"] != DBNull.Value)
                    {
                        item.ItemCostPrice = (decimal)skuContent.Rows[0]["CostPrice"];
                    }
                    item.ItemDescription = (string)skuContent.Rows[0]["ProductName"];
                    item.ItemHomeSiteDescription = (string)skuContent.Rows[0]["ProductName"];
                    if (skuContent.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
                    {
                        item.ThumbnailsUrl = (string)skuContent.Rows[0]["ThumbnailUrl40"];
                    }
                    totalWeight += item.ItemWeight * item.Quantity;
                    info.PurchaseOrderItems.Add(item);
                }
            }
            if (info.PurchaseOrderItems.Count <= 0)
            {
                return null;
            }
            info.Weight = totalWeight;
            info.AdjustedFreight = SubsiteSalesHelper.CalcFreight(info.RegionId, totalWeight, shippingMode);
            info.Freight = info.AdjustedFreight;
            info.PurchaseStatus = OrderStatus.WaitBuyerPay;
            info.RefundStatus = RefundStatus.None;
            info.DistributorId = distributor.UserId;
            info.Distributorname = distributor.Username;
            info.DistributorEmail = distributor.Email;
            info.DistributorRealName = distributor.RealName;
            info.DistributorQQ = distributor.QQ;
            info.DistributorWangwang = distributor.Wangwang;
            info.DistributorMSN = distributor.MSN;
            return info;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            orderIds = Page.Request.QueryString["orderIds"];
            if (string.IsNullOrEmpty(orderIds))
            {
                litmsg.Text = "没有要转换的订单";
            }
            else
            {
                int result = 0;
                int.TryParse(Page.Request.QueryString["shippingModeId"], out result);
                shippingMode = SubsiteSalesHelper.GetShippingMode(result, true);
                if (shippingMode == null)
                {
                    litmsg.Text = "没有选择配送方式";
                }
                else
                {
                    distributor = HiContext.Current.User as Hidistro.Membership.Context.Distributor;
                    cookie = HiContext.Current.Context.Request.Cookies["TopSession_" + HiContext.Current.User.UserId.ToString()];
                    string serverUrl = "http://gw.api.taobao.com/router/rest";
                    SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                    string appKey = Cryptographer.Decrypt(masterSettings.Topkey);
                    string appSecret = Cryptographer.Decrypt(masterSettings.TopSecret);
                    client = new DefaultTopClient(serverUrl, appKey, appSecret, "json");
                    TradeFullinfoGetRequest request2 = new TradeFullinfoGetRequest();
                    request2.Fields = "tid,receiver_name,receiver_state,payment,receiver_district,receiver_city,receiver_address,receiver_phone,receiver_zip,receiver_mobile,post_fee,adjust_fee,total_fee,orders";
                    TradeFullinfoGetRequest request = request2;
                    string[] strArray = orderIds.Split(new char[] { ',' });
                    int count = 0;
                    decimal expenditure = 0M;
                    foreach (string str4 in strArray)
                    {
                        request.Tid = new long?(long.Parse(str4));
                        TradeFullinfoGetResponse response = client.Execute<TradeFullinfoGetResponse>(request, cookie.Value);
                        if (!response.IsError)
                        {
                            try
                            {
                                PurchaseOrderInfo purchaseOrder = GetPurchaseOrder(response.Trade);
                                if ((purchaseOrder != null) && SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrder))
                                {
                                    expenditure += purchaseOrder.OrderTotal;
                                    count++;
                                }
                            }
                            catch
                            {
                                litmsg.Text = "生成采购单发生错误，请重新尝试";
                            }
                        }
                    }
                    if (count > 0)
                    {
                        SendHttpRequest(count, expenditure);
                        litmsg.Text = string.Format("你选择的{0}淘宝订单已经成功的转换了{1}个采购单", strArray.Length, count);
                    }
                    else
                    {
                        litmsg.Text = "生成采购单失败，可能您选择的淘宝订单在系统中没有找到对应的商品";
                    }
                }
            }
        }

        private string SendHttpRequest(int count, decimal expenditure)
        {
            string str;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            WebRequest request = WebRequest.Create("http://saas.92hi.com/CreateDistributors.aspx");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string s = string.Concat(new object[] { 
                "Host=", masterSettings.SiteUrl, "&DistributorUserId=", distributor.UserId, "&Email=", Page.Server.UrlEncode(distributor.Email), "&RealName=", Page.Server.UrlEncode(distributor.RealName), "&CompanyName=", Page.Server.UrlEncode(distributor.CompanyName), "&Address=", Page.Server.UrlEncode(distributor.Address), "&TelPhone=", Page.Server.UrlEncode(distributor.TelPhone), "&QQ=", Page.Server.UrlEncode(distributor.QQ), 
                "&Wangwang=", Page.Server.UrlEncode(distributor.Wangwang), "&count=", count.ToString(), "&expenditure=", expenditure.ToString("F2")
             });
            byte[] bytes = new ASCIIEncoding().GetBytes(s);
            request.ContentLength = bytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();
                using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.Default))
                {
                    str = reader.ReadToEnd();
                }
            }
            return str;
        }
    }
}

