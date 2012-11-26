using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    public partial class BatchSendPurchaseOrderGoods : AdminPage
    {

        string path = "";
        string strIds = "";


        private void BindData()
        {
            DropdownColumn column = (DropdownColumn)grdOrderGoods.Columns[4];
            column.DataSource = SalesHelper.GetShippingModes();
            DropdownColumn column2 = (DropdownColumn)grdOrderGoods.Columns[5];
            column2.DataSource = GetDataSource();
            string purchaseOrderIds = "'" + strIds.Replace(",", "','") + "'";
            grdOrderGoods.DataSource = SalesHelper.GetSendGoodsPurchaseOrders(purchaseOrderIds);
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
                    string purchaseOrderId = (string)grdOrderGoods.DataKeys[grdOrderGoods.Rows[i].RowIndex].Value;
                    TextBox box = (TextBox)grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
                    ListItem item = selectedItems[i];
                    ListItem item2 = items2[i];
                    int result = 0;
                    int.TryParse(item.Value, out result);
                    if ((!string.IsNullOrEmpty(box.Text.Trim()) && (box.Text.Length <= 20)) && (result > 0))
                    {
                        PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
                        if (((purchaseOrder != null) && (purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)) && !string.IsNullOrEmpty(item2.Value))
                        {
                            ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(int.Parse(item.Value), true);
                            purchaseOrder.RealShippingModeId = shippingMode.ModeId;
                            purchaseOrder.RealModeName = shippingMode.Name;
                            purchaseOrder.ExpressCompanyAbb = item2.Value;
                            purchaseOrder.ExpressCompanyName = item2.Text;
                            purchaseOrder.ShipOrderNumber = box.Text;
                            if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
                            {
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
            string orderIds = "'" + strIds.Replace(",", "','") + "'";
            if (!string.IsNullOrEmpty(dropExpressComputerpe.SelectedValue))
            {
                SalesHelper.SetPurchaseOrderExpressComputerpe(orderIds, dropExpressComputerpe.SelectedItem.Text, dropExpressComputerpe.SelectedValue);
            }
            BindData();
        }

        private void btnSetShipOrderNumber_Click(object sender, EventArgs e)
        {
            string[] orderIds = strIds.Split(new char[] { ',' });
            long result = 0;
            if (!(string.IsNullOrEmpty(txtStartShipOrderNumber.Text.Trim()) || !long.TryParse(txtStartShipOrderNumber.Text.Trim(), out result)))
            {
                SalesHelper.SetPurchaseOrderShipNumber(orderIds, txtStartShipOrderNumber.Text.Trim());
                BindData();
            }
            else
            {
                ShowMsg("起始发货单号不允许为空且必须为正整数", false);
            }
        }

        private void btnSetShippingMode_Click(object sender, EventArgs e)
        {
            string orderIds = "'" + strIds.Replace(",", "','") + "'";
            if (dropShippingMode.SelectedValue.HasValue)
            {
                SalesHelper.SetPurchaseOrderShippingMode(orderIds, dropShippingMode.SelectedValue.Value, dropShippingMode.SelectedItem.Text);
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
            if (!string.IsNullOrEmpty(base.Request.QueryString["PurchaseOrderIds"]))
            {
                strIds = base.Request.QueryString["PurchaseOrderIds"];
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

