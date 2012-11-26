
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ManagePurchaseorder)]
    public partial class DeliveredPurchaseOrderDetails : PurchaseOrderDetailsBasePage
    {
        public DeliveredPurchaseOrderDetails()
            : base(OrderStatus.SellerAlreadySent)
        {
        }

        private void BindRemark()
        {
            spanOrderId.Text = base.purchaseOrder.OrderId;
            spanpurcharseOrderId.Text = base.purchaseOrder.PurchaseOrderId;
            lblpurchaseDateForRemark.Time = base.purchaseOrder.PurchaseDate;
            lblpurchaseTotalForRemark.Money = base.purchaseOrder.GetPurchaseTotal();
            txtRemark.Text = Globals.HtmlDecode(base.purchaseOrder.ManagerRemark);
            orderRemarkImageForRemark.SelectedValue = base.purchaseOrder.ManagerMark;
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (txtRemark.Text.Length > 300)
            {
                ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                base.purchaseOrder.PurchaseOrderId = spanpurcharseOrderId.Text;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    base.purchaseOrder.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                base.purchaseOrder.ManagerRemark = Globals.HtmlEncode(txtRemark.Text);
                if (SalesHelper.SavePurchaseOrderRemark(base.purchaseOrder))
                {
                    BindRemark();
                    ShowMsg("保存备忘录成功", true);
                }
                else
                {
                    ShowMsg("保存失败", false);
                }
            }
        }

        private void LoadUserControl()
        {
            userInfo.DistributorId = base.purchaseOrder.DistributorId;
            userInfo.PurchaseOrder = base.purchaseOrder;
            itemsList.PurchaseOrder = base.purchaseOrder;
            chargesList.PurchaseOrder = base.purchaseOrder;
            shippingAddress.PurchaseOrder = base.purchaseOrder;
            userInfo.PurchaseOrder = base.purchaseOrder;
            if (!string.IsNullOrEmpty(base.purchaseOrder.ExpressCompanyAbb))
            {
                plExpress.Visible = true;
                if (Express.GetExpressType() == "kuaidi100")
                {
                    power.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnRemark.Click += new EventHandler(btnRemark_Click);
            LoadUserControl();
            if (!base.IsPostBack)
            {
                BindRemark();
                if (base.purchaseOrder.RefundStatus == RefundStatus.Refund)
                {
                    divRefundDetails.Visible = true;
                    hlkRefundDetails.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/RefundPurchaseOrderDetails.aspx?PurchaseOrderId=" + base.purchaseOrder.PurchaseOrderId;
                }
                else
                {
                    divRefundDetails.Visible = false;
                }
                Distributor user = Users.GetUser(base.purchaseOrder.DistributorId) as Distributor;
                divRefund.Visible = !divRefundDetails.Visible && (user != null);
                hlkRefund.NavigateUrl = Globals.ApplicationPath + "/Admin/purchaseOrder/RefundPurchaseOrder.aspx?PurchaseOrderId=" + base.purchaseOrder.PurchaseOrderId;
            }
        }
    }
}

