using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Orders)]
    public partial class FinishedOrderDetails : OrderDetailsBasePage
    {

        public FinishedOrderDetails()
            : base(OrderStatus.Finished)
        {
        }

        private void BindRemark()
        {
            spanOrderId.Text = base.Order.OrderId;
            lblorderDateForRemark.Time = base.Order.OrderDate;
            lblorderTotalForRemark.Money = base.Order.GetTotal();
            txtRemark.Text = Globals.HtmlDecode(base.Order.ManagerRemark);
            orderRemarkImageForRemark.SelectedValue = base.Order.ManagerMark;
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (txtRemark.Text.Length > 300)
            {
                ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                base.Order.OrderId = spanOrderId.Text;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    base.Order.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                base.Order.ManagerRemark = Globals.HtmlEncode(txtRemark.Text.Trim());
                if (OrderHelper.SaveRemark(base.Order))
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

        private void LoadControl()
        {
            if ((base.Order.RefundStatus == RefundStatus.Refund) || (base.Order.RefundStatus == RefundStatus.Below))
            {
                divRefundDetails.Visible = true;
                hlkRefundDetails.NavigateUrl = Globals.ApplicationPath + "/Admin/sales/RefundOrderDetails.aspx?OrderId=" + base.Order.OrderId;
            }
        }

        private void LoadUserControl()
        {
            userInfo.UserId = base.Order.UserId;
            userInfo.Order = base.Order;
            itemsList.Order = base.Order;
            chargesList.Order = base.Order;
            shippingAddress.Order = base.Order;
            if (!string.IsNullOrEmpty(base.Order.ExpressCompanyAbb))
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
            if (!Page.IsPostBack)
            {
                BindRemark();
                LoadControl();
            }
        }
    }
}

