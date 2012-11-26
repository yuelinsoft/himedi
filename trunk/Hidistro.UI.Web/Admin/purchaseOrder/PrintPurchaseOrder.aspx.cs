using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    public partial class PrintPurchaseOrder : AdminPage
    {
        string purchaseOrderId = string.Empty;

        private void BindOrderInfo(PurchaseOrderInfo order)
        {
            litAddress.Text = order.ShippingRegion + order.Address;
            litCellPhone.Text = order.CellPhone;
            litTelPhone.Text = order.TelPhone;
            litZipCode.Text = order.ZipCode;
            litOrderId.Text = order.OrderId;
            litOrderDate.Text = order.PurchaseDate.ToString();
            litRemark.Text = order.Remark;
            litShipperMode.Text = order.RealModeName;
            litShippNo.Text = order.ShipOrderNumber;
            litSkipTo.Text = order.ShipTo;
            string str = string.Empty;
            if (order.PurchaseOrderOptions.Count > 0)
            {
                IList<PurchaseOrderOptionInfo> purchaseOrderOptions = order.PurchaseOrderOptions;
                str = str + "（";
                foreach (PurchaseOrderOptionInfo info in purchaseOrderOptions)
                {
                    string str2 = str;
                    str = str2 + info.ListDescription + "：" + info.ItemDescription + "；" + info.CustomerTitle + "：" + info.CustomerDescription;
                }
                str = str + "）";
                litOptionDes.Text = str;
            }
            switch (order.PurchaseStatus)
            {
                case OrderStatus.WaitBuyerPay:
                    litOrderStatus.Text = "等待付款";
                    break;

                case OrderStatus.BuyerAlreadyPaid:
                    litOrderStatus.Text = "已付款等待发货";
                    break;

                case OrderStatus.SellerAlreadySent:
                    litOrderStatus.Text = "已发货";
                    break;

                case OrderStatus.Closed:
                    litOrderStatus.Text = "已关闭";
                    break;

                case OrderStatus.Finished:
                    litOrderStatus.Text = "已完成";
                    break;
            }
            litOptionMoney.Money = order.GetOptionPrice();
            litShippMoney.Money = order.AdjustedFreight;
            litTotalMoney.Money = order.OrderTotal;
            litDiscountMoney.Money = order.AdjustedDiscount;
            decimal num = 0M;
            foreach (PurchaseOrderItemInfo info2 in order.PurchaseOrderItems)
            {
                num += info2.ItemListPrice * info2.Quantity;
            }
            litItemsMoney.Money = num;
        }

        private void BindOrderItems(PurchaseOrderInfo order)
        {
            grdOrderItems.DataSource = order.PurchaseOrderItems;
            grdOrderItems.DataBind();
            if (order.PurchaseOrderGifts.Count > 0)
            {
                grdOrderGifts.DataSource = order.PurchaseOrderGifts;
                grdOrderGifts.DataBind();
            }
            else
            {
                grdOrderGifts.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            purchaseOrderId = Page.Request.Params["PurchaseOrderId"];
            if (!(Page.IsPostBack || string.IsNullOrEmpty(purchaseOrderId)))
            {
                PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
                BindOrderInfo(purchaseOrder);
                BindOrderItems(purchaseOrder);
            }
        }
    }
}

