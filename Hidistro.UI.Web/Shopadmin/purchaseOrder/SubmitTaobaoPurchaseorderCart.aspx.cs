using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    public partial class SubmitTaobaoPurchaseorderCart : DistributorPage
    {
        IList<tbOrder> tbOrders;


        protected void btnMatch_Click(object sender, EventArgs e)
        {
            if (Request.Form["radioSerachResult"] != null)
            {
                DataTable puchaseProduct = SubSiteProducthelper.GetPuchaseProduct(base.Request.Form["radioSerachResult"].Trim());
                if ((puchaseProduct != null) && (puchaseProduct.Rows.Count > 0))
                {
                    string str = base.Request.Form["serachProductId"].Trim();
                    string str2 = str.Substring(0, str.IndexOf('_'));
                    foreach (tbOrder order in tbOrders)
                    {
                        if (order.orderId == str2)
                        {
                            foreach (tbOrderItem item in order.items)
                            {
                                if (item.id == str)
                                {
                                    order.orderCost -= Convert.ToDouble(item.localPrice) * Convert.ToInt32(item.number);
                                    item.localSkuId = puchaseProduct.Rows[0]["SkuId"].ToString();
                                    item.localSKU = puchaseProduct.Rows[0]["SKU"].ToString();
                                    item.localProductId = puchaseProduct.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = puchaseProduct.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = puchaseProduct.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = puchaseProduct.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = puchaseProduct.Rows[0]["Stock"].ToString();
                                    order.orderCost += Convert.ToDouble(item.localPrice) * Convert.ToInt32(item.number);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                pageDataBind();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateCreateOrder())
            {
                string str = "";

                PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();

                Distributor user = Users.GetUser(HiContext.Current.User.UserId) as Distributor;

                purchaseOrderInfo.PurchaseOrderId = GeneratePurchaseOrderId();

                int totalWeight = 0;

                for (int i = 0; i < rpTaobaoOrder.Items.Count; i++)
                {
                    CheckBox box = (CheckBox)rpTaobaoOrder.Items[i].FindControl("chkTbOrder");

                    if (box.Checked)
                    {
                        str = str + tbOrders[i].orderId + ",";
                        Repeater repeater = (Repeater)rpTaobaoOrder.Items[i].FindControl("reOrderItems");
                        IList<tbOrderItem> items = tbOrders[i].items;
                        for (int j = 0; j < repeater.Items.Count; j++)
                        {
                            if (items[j].localSkuId.Trim() == "")
                            {
                                string msg = string.Format("在授权给分销商的商品中没有找到淘宝商品：{0}！请重新查找", items[j].title);
                                ShowMsg(msg, false);
                                return;
                            }
                            string localSkuId = items[j].localSkuId;
                            TextBox box2 = (TextBox)repeater.Items[j].FindControl("productNumber");
                            int num4 = Convert.ToInt32(box2.Text);
                            bool flag = false;
                            foreach (PurchaseOrderItemInfo info2 in purchaseOrderInfo.PurchaseOrderItems)
                            {
                                if (info2.SKU == localSkuId)
                                {
                                    flag = true;
                                    info2.Quantity += num4;
                                    totalWeight += info2.ItemWeight * num4;
                                }
                            }
                            if (!flag)
                            {
                                DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(localSkuId);
                                PurchaseOrderItemInfo item = new PurchaseOrderItemInfo();
                                if (num4 > ((int)skuContentBySku.Rows[0]["Stock"]))
                                {
                                    ShowMsg("商品库存不够", false);
                                    return;
                                }
                                foreach (DataRow row in skuContentBySku.Rows)
                                {
                                    if (!(string.IsNullOrEmpty(row["AttributeName"].ToString()) || string.IsNullOrEmpty(row["ValueStr"].ToString())))
                                    {
                                        object sKUContent = item.SKUContent;
                                        item.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                                    }
                                }
                                item.PurchaseOrderId = purchaseOrderInfo.PurchaseOrderId;
                                item.SkuId = localSkuId;
                                item.ProductId = (int)skuContentBySku.Rows[0]["ProductId"];
                                if (skuContentBySku.Rows[0]["SKU"] != DBNull.Value)
                                {
                                    item.SKU = (string)skuContentBySku.Rows[0]["SKU"];
                                }
                                if (skuContentBySku.Rows[0]["Weight"] != DBNull.Value)
                                {
                                    item.ItemWeight = (int)skuContentBySku.Rows[0]["Weight"];
                                }
                                item.ItemPurchasePrice = (decimal)skuContentBySku.Rows[0]["PurchasePrice"];
                                item.Quantity = num4;
                                item.ItemListPrice = (decimal)skuContentBySku.Rows[0]["SalePrice"];
                                if (skuContentBySku.Rows[0]["CostPrice"] != DBNull.Value)
                                {
                                    item.ItemCostPrice = (decimal)skuContentBySku.Rows[0]["CostPrice"];
                                }
                                item.ItemDescription = (string)skuContentBySku.Rows[0]["ProductName"];
                                item.ItemHomeSiteDescription = (string)skuContentBySku.Rows[0]["ProductName"];
                                if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
                                {
                                    item.ThumbnailsUrl = (string)skuContentBySku.Rows[0]["ThumbnailUrl40"];
                                }
                                totalWeight += item.ItemWeight * num4;
                                purchaseOrderInfo.PurchaseOrderItems.Add(item);
                            }
                        }
                    }
                }
                if (str == "")
                {
                    ShowMsg("至少选择一个淘宝订单！！", false);
                }
                else
                {
                    ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(radioShippingMode.SelectedValue.Value, true);
                    purchaseOrderInfo.ShipTo = txtShipTo.Text.Trim();
                    if (rsddlRegion.GetSelectedRegionId().HasValue)
                    {
                        purchaseOrderInfo.RegionId = rsddlRegion.GetSelectedRegionId().Value;
                    }
                    purchaseOrderInfo.Address = txtAddress.Text.Trim();
                    purchaseOrderInfo.TelPhone = txtTel.Text.Trim();
                    purchaseOrderInfo.ZipCode = txtZipcode.Text.Trim();
                    purchaseOrderInfo.CellPhone = txtMobile.Text.Trim();
                    purchaseOrderInfo.OrderId = null;
                    purchaseOrderInfo.RealShippingModeId = radioShippingMode.SelectedValue.Value;
                    purchaseOrderInfo.RealModeName = shippingMode.Name;
                    purchaseOrderInfo.ShippingModeId = radioShippingMode.SelectedValue.Value;
                    purchaseOrderInfo.ModeName = shippingMode.Name;
                    purchaseOrderInfo.AdjustedFreight = SubsiteSalesHelper.CalcFreight(purchaseOrderInfo.RegionId, totalWeight, shippingMode);
                    purchaseOrderInfo.Freight = purchaseOrderInfo.AdjustedFreight;
                    purchaseOrderInfo.ShippingRegion = rsddlRegion.SelectedRegions;
                    purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
                    purchaseOrderInfo.DistributorId = user.UserId;
                    purchaseOrderInfo.Distributorname = user.Username;
                    purchaseOrderInfo.DistributorEmail = user.Email;
                    purchaseOrderInfo.DistributorRealName = user.RealName;
                    purchaseOrderInfo.DistributorQQ = user.QQ;
                    purchaseOrderInfo.DistributorWangwang = user.Wangwang;
                    purchaseOrderInfo.DistributorMSN = user.MSN;
                    purchaseOrderInfo.RefundStatus = RefundStatus.None;
                    purchaseOrderInfo.Weight = totalWeight;
                    purchaseOrderInfo.Remark = null;
                    purchaseOrderInfo.TaobaoOrderId = str;
                    if (purchaseOrderInfo.PurchaseOrderItems.Count == 0)
                    {
                        ShowMsg("您暂时未选择您要添加的商品", false);
                    }
                    else if (SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
                    {
                        SubsiteSalesHelper.ClearPurchaseShoppingCart();
                        ResponseCookies();
                        base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/Pay.aspx?PurchaseOrderId=" + purchaseOrderInfo.PurchaseOrderId);
                    }
                    else
                    {
                        ShowMsg("提交采购单失败", false);
                    }
                }
            }
        }

        protected void ButtonAdd_Command(object sender, CommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            string[] strArray = str.Split(new char[] { '_' });
            foreach (tbOrder order in tbOrders)
            {
                if (order.orderId == strArray[0])
                {
                    foreach (tbOrderItem item in order.items)
                    {
                        if (!(item.id == str))
                        {
                            continue;
                        }
                        if (item.number != "")
                        {
                            if (Convert.ToInt32(item.number) >= Convert.ToInt32(item.localStock))
                            {
                                ShowMsg("库存不足，请检查后再下单", false);
                            }
                            else
                            {
                                item.number = (Convert.ToInt32(item.number) + 1).ToString();
                            }
                        }
                        else
                        {
                            item.number = "1";
                        }
                        if ((item.localSkuId != "") && (item.localPrice != ""))
                        {
                            order.orderCost += Convert.ToDouble(item.localPrice);
                        }
                        break;
                    }
                    break;
                }
            }
            pageDataBind();
        }

        protected void ButtonDec_Command(object sender, CommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            string[] strArray = str.Split(new char[] { '_' });
            foreach (tbOrder order in tbOrders)
            {
                if (order.orderId == strArray[0])
                {
                    foreach (tbOrderItem item in order.items)
                    {
                        if (item.id == str)
                        {
                            if ((item.number != "") && (Convert.ToInt32(item.number) > 1))
                            {
                                item.number = (Convert.ToInt32(item.number) - 1).ToString();
                                if ((item.localSkuId != "") && (item.localPrice != ""))
                                {
                                    order.orderCost -= Convert.ToDouble(item.localPrice);
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            pageDataBind();
        }

        protected void ButtonDelete_Command(object sender, CommandEventArgs e)
        {
            string str = e.CommandArgument.ToString();
            string[] strArray = str.Split(new char[] { '_' });
            foreach (tbOrder order in tbOrders)
            {
                if (order.orderId == strArray[0])
                {
                    if (order.items.Count <= 1)
                    {
                        ShowMsg("每个淘宝订单至少保留一件商品！", false);
                        return;
                    }
                    foreach (tbOrderItem item in order.items)
                    {
                        if (item.id == str)
                        {
                            order.items.Remove(item);
                            if ((item.localSkuId != "") && (item.localPrice != ""))
                            {
                                order.orderCost -= Convert.ToDouble(item.localPrice);
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            pageDataBind();
        }

        private string GeneratePurchaseOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num2 = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num2 % 10)))).ToString();
            }
            return ("MPO" + DateTime.Now.ToString("yyyyMMdd") + str);
        }

        protected string isFindProduct(string productId, int type)
        {
            if (productId.Trim() == "")
            {
                if (type != 0)
                {
                    return "block";
                }
                return "none";
            }
            if (type != 1)
            {
                return "block";
            }
            return "none";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.Compare(base.Request.RequestType, "post", true) != 0)
            {
                btnSubmit.Enabled = false;
            }
            else if (base.IsPostBack)
            {
                tbOrders = (IList<tbOrder>)Session["tbOrders"];
            }
            else
            {
                XmlDocument document = new XmlDocument();
                if (string.IsNullOrEmpty(base.Request.Form["data"]))
                {
                    ShowMsg("数据丢失，请关闭此页重新操作", false);
                }
                else
                {
                    document.LoadXml(base.Request.Form["data"]);
                    tbOrders = new List<tbOrder>();
                    XmlNodeList list = document.FirstChild.SelectNodes("order");
                    string innerText = null;
                    string str2 = null;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string str3;
                        tbOrder order = new tbOrder();
                        XmlNode node = list.Item(i);
                        if (innerText == null)
                        {
                            innerText = node.SelectSingleNode("ship_addr").InnerText;
                            str2 = node.SelectSingleNode("ship_name").InnerText;
                        }
                        else
                        {
                            str3 = node.SelectSingleNode("ship_addr").InnerText;
                            string str4 = node.SelectSingleNode("ship_name").InnerText;
                            if ((innerText != str3) || (str2 != str4))
                            {
                                ShowMsg("收货人地址/姓名不一致不能合并下单！", false);
                                break;
                            }
                            str2 = str4;
                            innerText = str3;
                        }
                        string[] strArray = innerText.Split(new char[] { ' ' });
                        txtShipTo.Text = str2;
                        if (strArray.Length >= 4)
                        {
                            str3 = strArray[0] + "," + strArray[1] + "," + strArray[2];
                            rsddlRegion.SelectedRegions = str3;
                            txtAddress.Text = strArray[3];
                        }
                        txtZipcode.Text = node.SelectSingleNode("ship_zipcode").InnerText;
                        txtTel.Text = node.SelectSingleNode("ship_tel").InnerText;
                        txtMobile.Text = node.SelectSingleNode("ship_mobile").InnerText;
                        radioShippingMode.DataBind();
                        if (radioShippingMode.Items.Count > 0)
                        {
                            radioShippingMode.Items[0].Selected = true;
                        }
                        order.orderId = node.SelectSingleNode("order_id").InnerText;
                        order.buyer = node.SelectSingleNode("buyer").InnerText;
                        order.createTime = node.SelectSingleNode("createtime").InnerText;
                        order.orderMemo = node.SelectSingleNode("order_memo").InnerText;
                        XmlNode node2 = node.SelectSingleNode("items");
                        double num2 = 0.0;
                        for (int j = 0; j < node2.ChildNodes.Count; j++)
                        {
                            tbOrderItem item2 = new tbOrderItem();
                            item2.id = string.Format("{0}_{1}", order.orderId, j);
                            item2.title = node2.ChildNodes[j].SelectSingleNode("title").InnerText;
                            item2.spec = node2.ChildNodes[j].SelectSingleNode("spec").InnerText;
                            item2.price = node2.ChildNodes[j].SelectSingleNode("price").InnerText;
                            item2.number = node2.ChildNodes[j].SelectSingleNode("number").InnerText;
                            item2.url = node2.ChildNodes[j].SelectSingleNode("url").InnerText;
                            tbOrderItem item = item2;
                            HttpRequest request = HttpContext.Current.Request;
                            if (request.Cookies[Globals.UrlEncode(item.title + item.spec)] != null)
                            {
                                ProductQuery query2 = new ProductQuery();
                                query2.PageSize = 1;
                                query2.PageIndex = 1;
                                query2.ProductCode = Globals.UrlDecode(request.Cookies[Globals.UrlEncode(item.title + item.spec)].Value);
                                ProductQuery query = query2;
                                int count = 0;
                                DataTable puchaseProducts = SubSiteProducthelper.GetPuchaseProducts(query, out count);
                                if (puchaseProducts.Rows.Count > 0)
                                {
                                    item.localSkuId = puchaseProducts.Rows[0]["SkuId"].ToString();
                                    item.localSKU = puchaseProducts.Rows[0]["SKU"].ToString();
                                    item.localProductId = puchaseProducts.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = puchaseProducts.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = puchaseProducts.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = puchaseProducts.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = puchaseProducts.Rows[0]["Stock"].ToString();
                                    num2 += Convert.ToDouble(puchaseProducts.Rows[0]["PurchasePrice"]) * Convert.ToInt32(item.number);
                                }
                            }
                            else
                            {
                                ProductQuery query4 = new ProductQuery();
                                query4.PageSize = 1;
                                query4.PageIndex = 1;
                                query4.Keywords = item.title;
                                ProductQuery query3 = query4;
                                int num5 = 0;
                                DataTable table2 = SubSiteProducthelper.GetPuchaseProducts(query3, out num5);
                                if (num5 == 1)
                                {
                                    item.localSkuId = table2.Rows[0]["SkuId"].ToString();
                                    item.localSKU = table2.Rows[0]["SKU"].ToString();
                                    item.localProductId = table2.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = table2.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = table2.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = table2.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = table2.Rows[0]["Stock"].ToString();
                                    num2 += Convert.ToDouble(table2.Rows[0]["PurchasePrice"]) * Convert.ToInt32(item.number);
                                }
                            }
                            order.items.Add(item);
                            order.orderCost = num2;
                        }
                        tbOrders.Add(order);
                    }
                    Session["tbOrders"] = tbOrders;
                    pageDataBind();
                }
            }
        }

        private void pageDataBind()
        {
            rpTaobaoOrder.DataSource = tbOrders;
            rpTaobaoOrder.DataBind();
        }

        public static void RemoveCookie(string cookieName, string key)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!(string.IsNullOrEmpty(key) || !cookie.HasKeys))
                    {
                        cookie.Values.Remove(key);
                    }
                    else
                    {
                        response.Cookies.Remove(cookieName);
                    }
                }
            }
        }

        private void ResponseCookies()
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;
            for (int i = 0; i < rpTaobaoOrder.Items.Count; i++)
            {
                Repeater repeater = (Repeater)rpTaobaoOrder.Items[i].FindControl("reOrderItems");
                for (int j = 0; j < repeater.Items.Count; j++)
                {
                    CheckBox box = (CheckBox)repeater.Items[j].FindControl("chkSave");
                    if (box.Checked)
                    {
                        Label label = (Label)repeater.Items[j].FindControl("lblSKU");
                        Label label2 = (Label)repeater.Items[j].FindControl("lblTitle");
                        Label label3 = (Label)repeater.Items[j].FindControl("lblSpec");
                        if (request.Cookies[Globals.UrlEncode(label2.Text + label3.Text)] != null)
                        {
                            response.Cookies.Remove(Globals.UrlEncode(label2.Text + label3.Text));
                        }
                        HttpCookie cookie2 = new HttpCookie(Globals.UrlEncode(label2.Text + label3.Text));
                        cookie2.Value = Globals.UrlEncode(label.Text);
                        cookie2.Path = "/";
                        cookie2.Expires = DateTime.Now.AddDays(3650.0);
                        HttpCookie cookie = cookie2;
                        response.Cookies.Add(cookie);
                    }
                }
            }
        }

        protected void rpTaobaoOrder_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                IList<tbOrder> dataSource = (IList<tbOrder>)rpTaobaoOrder.DataSource;
                Repeater repeater = (Repeater)e.Item.FindControl("reOrderItems");
                repeater.DataSource = dataSource[e.Item.ItemIndex].items;
                repeater.DataBind();
            }
        }

        private bool ValidateCreateOrder()
        {
            string str = string.Empty;
            if (!(rsddlRegion.GetSelectedRegionId().HasValue && (rsddlRegion.GetSelectedRegionId().Value != 0)))
            {
                str = str + Formatter.FormatErrorMessage("请选择收货地址");
            }
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if (!(!string.IsNullOrEmpty(txtShipTo.Text) && regex.IsMatch(txtShipTo.Text.Trim())))
            {
                str = str + Formatter.FormatErrorMessage("请输入正确的收货人姓名");
            }
            if (!(string.IsNullOrEmpty(txtShipTo.Text) || ((txtShipTo.Text.Trim().Length >= 2) && (txtShipTo.Text.Trim().Length <= 20))))
            {
                str = str + Formatter.FormatErrorMessage("收货人姓名的长度限制在2-20个字符");
            }
            if (string.IsNullOrEmpty(txtAddress.Text.Trim()) || (txtAddress.Text.Trim().Length > 100))
            {
                str = str + Formatter.FormatErrorMessage("请输入收货人详细地址,在100个字符以内");
            }
            regex = new Regex("^[0-9]*$");
            if ((string.IsNullOrEmpty(txtZipcode.Text.Trim()) || (txtZipcode.Text.Trim().Length > 10)) || ((txtZipcode.Text.Trim().Length < 6) || !regex.IsMatch(txtZipcode.Text.Trim())))
            {
                str = str + Formatter.FormatErrorMessage("请输入收货人邮政编码,在6-10个数字之间");
            }
            regex = new Regex("^[0-9]*$");
            if (!(string.IsNullOrEmpty(txtMobile.Text.Trim()) || ((regex.IsMatch(txtMobile.Text.Trim()) && (txtMobile.Text.Trim().Length <= 20)) && (txtMobile.Text.Trim().Length >= 3))))
            {
                str = str + Formatter.FormatErrorMessage("手机号码长度限制在3-20个字符之间,只能输入数字");
            }
            regex = new Regex("^[0-9-]*$");
            if (!(string.IsNullOrEmpty(txtTel.Text.Trim()) || ((regex.IsMatch(txtTel.Text.Trim()) && (txtTel.Text.Trim().Length <= 20)) && (txtTel.Text.Trim().Length >= 3))))
            {
                str = str + Formatter.FormatErrorMessage("电话号码长度限制在3-20个字符之间,只能输入数字和字符“-”");
            }
            if (!radioShippingMode.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择配送方式");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }

        private class tbOrder
        {

            private string _buyer;

            private string _createTime;

            private double _orderCost;

            private string _orderId;

            private string _orderMemo;
            public IList<SubmitTaobaoPurchaseorderCart.tbOrderItem> items;

            public tbOrder()
            {
                orderId = "";
                buyer = "";
                createTime = "";
                orderMemo = "";
                orderCost = 0.0;
                items = new List<SubmitTaobaoPurchaseorderCart.tbOrderItem>();
            }

            public string buyer
            {

                get
                {
                    return _buyer;
                }

                set
                {
                    _buyer = value;
                }
            }

            public string createTime
            {

                get
                {
                    return _createTime;
                }

                set
                {
                    _createTime = value;
                }
            }

            public double orderCost
            {

                get
                {
                    return _orderCost;
                }

                set
                {
                    _orderCost = value;
                }
            }

            public string orderId
            {

                get
                {
                    return _orderId;
                }

                set
                {
                    _orderId = value;
                }
            }

            public string orderMemo
            {

                get
                {
                    return _orderMemo;
                }

                set
                {
                    _orderMemo = value;
                }
            }
        }

        private class tbOrderItem
        {

            private string _bn;

            private string _id;

            private string _localPrice;

            private string _localProductId;

            private string _localProductName;

            private string _localSKU;

            private string _localSkuId;

            private string _localStock;

            private string _memo;

            private string _number;

            private string _price;

            private string _spec;

            private string _thumbnailUrl100;

            private string _title;

            private string _url;

            public tbOrderItem()
            {
                id = "";
                title = "";
                spec = "";
                price = "";
                number = "0";
                bn = "";
                memo = "";
                url = "";
                localSkuId = "";
                localSKU = "";
                localStock = "0";
                localPrice = "0";
                localProductId = "";
                localProductName = "";
                thumbnailUrl100 = "";
            }

            public string bn
            {

                get
                {
                    return _bn;
                }

                set
                {
                    _bn = value;
                }
            }

            public string id
            {

                get
                {
                    return _id;
                }

                set
                {
                    _id = value;
                }
            }

            public string localPrice
            {

                get
                {
                    return _localPrice;
                }

                set
                {
                    _localPrice = value;
                }
            }

            public string localProductId
            {

                get
                {
                    return _localProductId;
                }

                set
                {
                    _localProductId = value;
                }
            }

            public string localProductName
            {

                get
                {
                    return _localProductName;
                }

                set
                {
                    _localProductName = value;
                }
            }

            public string localSKU
            {

                get
                {
                    return _localSKU;
                }

                set
                {
                    _localSKU = value;
                }
            }

            public string localSkuId
            {

                get
                {
                    return _localSkuId;
                }

                set
                {
                    _localSkuId = value;
                }
            }

            public string localStock
            {

                get
                {
                    return _localStock;
                }

                set
                {
                    _localStock = value;
                }
            }

            public string memo
            {

                get
                {
                    return _memo;
                }

                set
                {
                    _memo = value;
                }
            }

            public string number
            {

                get
                {
                    return _number;
                }

                set
                {
                    _number = value;
                }
            }

            public string price
            {

                get
                {
                    return _price;
                }

                set
                {
                    _price = value;
                }
            }

            public string spec
            {

                get
                {
                    return _spec;
                }

                set
                {
                    _spec = value;
                }
            }

            public string thumbnailUrl100
            {

                get
                {
                    return _thumbnailUrl100;
                }

                set
                {
                    _thumbnailUrl100 = value;
                }
            }

            public string title
            {

                get
                {
                    return _title;
                }

                set
                {
                    _title = value;
                }
            }

            public string url
            {

                get
                {
                    return _url;
                }

                set
                {
                    _url = value;
                }
            }
        }
    }
}

