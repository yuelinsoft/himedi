using Hidistro.ControlPanel.Distribution;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class PurchaseOrder_DistributorInfo : UserControl
    {
        int distributorId;
        PurchaseOrderInfo purchaseOrder;

        protected override void OnLoad(EventArgs e)
        {
            Distributor distributor = DistributorHelper.GetDistributor(distributorId);
            if (distributor != null)
            {
                distributorName.Text = distributor.Username;
                distributorRealName.Text = distributor.RealName;
                if (!string.IsNullOrEmpty(distributor.Email))
                {
                    lkbtnEmail.Text = distributor.Email;
                    email.HRef = "mailto:" + distributor.Email;
                }
                distributorTel.Text = distributor.TelPhone;
                lkbtnSendMessage.NavigateUrl = Globals.GetAdminAbsolutePath("/comment/SendMessageToDistributor.aspx?UserId=" + distributor.UserId);
            }
            litOrderId.Text = PurchaseOrder.OrderId;
            LitPurchaseOrderId.Text = PurchaseOrder.PurchaseOrderId;
        }

        public int DistributorId
        {
            get
            {
                return distributorId;
            }
            set
            {
                distributorId = value;
            }
        }

        public PurchaseOrderInfo PurchaseOrder
        {
            get
            {
                return purchaseOrder;
            }
            set
            {
                purchaseOrder = value;
            }
        }
    }
}

