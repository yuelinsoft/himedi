using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class BatchSendOrderGoods : AdminPage
    {

        string path = "";
        string strIds;


        private void BindData()
        {
            DropdownColumn column = (DropdownColumn)grdOrderGoods.Columns[4];
            column.DataSource = SalesHelper.GetShippingModes();
            DropdownColumn column2 = (DropdownColumn)grdOrderGoods.Columns[5];
            column2.DataSource = GetDataSource();
            string orderIds = "'" + strIds.Replace(",", "','") + "'";
            grdOrderGoods.DataSource = OrderHelper.GetSendGoodsOrders(orderIds);
            grdOrderGoods.DataBind();
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
                DropdownColumn column2 = (DropdownColumn)grdOrderGoods.Columns[5];
                ListItemCollection items2 = column2.SelectedItems;
                int num = 0;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    string orderId = (string)grdOrderGoods.DataKeys[grdOrderGoods.Rows[i].RowIndex].Value;
                    TextBox box = (TextBox)grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
                    ListItem item = selectedItems[i];
                    ListItem item2 = items2[i];
                    int result = 0;
                    int.TryParse(item.Value, out result);
                    if ((!string.IsNullOrEmpty(box.Text.Trim()) && !string.IsNullOrEmpty(item.Value)) && ((int.Parse(item.Value) > 0) && !string.IsNullOrEmpty(item2.Value)))
                    {
                        OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                        if (((orderInfo.GroupBuyId <= 0) || (orderInfo.GroupBuyStatus == GroupBuyStatus.Success)) && (((orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid) && (result > 0)) && (!string.IsNullOrEmpty(box.Text.Trim()) && (box.Text.Trim().Length <= 20))))
                        {
                            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(result, true);
                            orderInfo.RealShippingModeId = shippingMode.ModeId;
                            orderInfo.RealModeName = shippingMode.Name;
                            orderInfo.ExpressCompanyAbb = item2.Value;
                            orderInfo.ExpressCompanyName = item2.Text;
                            orderInfo.ShipOrderNumber = box.Text;
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
                                num++;
                            }
                        }
                    }
                }
                if (num == 0)
                {
                    ShowMsg("批量发货失败！", false);
                }
                else if (num > 0)
                {
                    BindData();
                    ShowMsg(string.Format("批量发货成功！发货数量{0}个", num), true);
                }
            }
        }

        private void btnSetExpressComputerpe_Click(object sender, EventArgs e)
        {
            string purchaseOrderIds = "'" + strIds.Replace(",", "','") + "'";
            if (!string.IsNullOrEmpty(dropExpressComputerpe.SelectedValue))
            {
                OrderHelper.SetOrderExpressComputerpe(purchaseOrderIds, dropExpressComputerpe.SelectedItem.Text, dropExpressComputerpe.SelectedValue);
            }
            BindData();
        }

        private void btnSetShipOrderNumber_Click(object sender, EventArgs e)
        {
            string[] purchaseOrderIds = strIds.Split(new char[] { ',' });
            long result = 0;
            if (!(string.IsNullOrEmpty(txtStartShipOrderNumber.Text.Trim()) || !long.TryParse(txtStartShipOrderNumber.Text.Trim(), out result)))
            {
                OrderHelper.SetOrderShipNumber(purchaseOrderIds, txtStartShipOrderNumber.Text.Trim());
                BindData();
            }
            else
            {
                ShowMsg("起始发货单号不允许为空且必须为正整数", false);
            }
        }

        private void btnSetShippingMode_Click(object sender, EventArgs e)
        {
            string purchaseOrderIds = "'" + strIds.Replace(",", "','") + "'";
            if (dropShippingMode.SelectedValue.HasValue)
            {
                OrderHelper.SetOrderShippingMode(purchaseOrderIds, dropShippingMode.SelectedValue.Value, dropShippingMode.SelectedItem.Text);
            }
            BindData();
        }

        private IList<ExpressCompanyInfo> GetDataSource()
        {
            IList<ExpressCompanyInfo> list = null;
            XmlDocument xmlNode = GetXmlNode();
            XmlNode node = null;
            if (xmlNode != null)
            {
                XmlNode node2 = xmlNode.SelectSingleNode("expressapi");
                if (node2 == null)
                {
                    return list;
                }
                XmlNode node3 = node2.SelectSingleNode("kuaidi100");
                if (node3 != null)
                {
                    node = node3.SelectSingleNode("companys");
                }
                if ((node == null) || (node.ChildNodes.Count <= 0))
                {
                    return list;
                }
                list = new List<ExpressCompanyInfo>();
                foreach (XmlNode node4 in node.ChildNodes)
                {
                    string str = node4["name"].InnerText.ToString();
                    string str2 = node4["abb"].InnerText.ToString();
                    if (!(string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2)))
                    {
                        ExpressCompanyInfo item = new ExpressCompanyInfo();
                        item.ExpressCompanyAbb = str2;
                        item.ExpressCompanyName = str;
                        list.Add(item);
                    }
                }
            }
            return list;
        }

        private XmlDocument GetXmlNode()
        {
            XmlDocument document = new XmlDocument();
            HttpContext current = HttpContext.Current;
            if (Express.GetExpressType() == "kuaidi100")
            {
                if (current != null)
                {
                    path = current.Request.MapPath("~/Express.xml");
                }
                if (!string.IsNullOrEmpty(path))
                {
                    document.Load(path);
                }
            }
            return document;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["OrderIds"]))
            {
                strIds = base.Request.QueryString["OrderIds"];
            }
            if (strIds.Length > 0)
            {
                btnSetShippingMode.Click += new EventHandler(btnSetShippingMode_Click);
                btnSetExpressComputerpe.Click += new EventHandler(btnSetExpressComputerpe_Click);
                btnSetShipOrderNumber.Click += new EventHandler(btnSetShipOrderNumber_Click);
                btnBatchSendGoods.Click += new EventHandler(btnSendGoods_Click);
                if (!Page.IsPostBack)
                {
                    dropShippingMode.DataBind();
                    dropExpressComputerpe.DataSource = GetDataSource();
                    dropExpressComputerpe.DataTextField = "ExpressCompanyName";
                    dropExpressComputerpe.DataValueField = "ExpressCompanyAbb";
                    dropExpressComputerpe.DataBind();
                    dropExpressComputerpe.Items.Insert(0, new ListItem("", ""));
                    BindData();
                }
            }
        }
    }
}

