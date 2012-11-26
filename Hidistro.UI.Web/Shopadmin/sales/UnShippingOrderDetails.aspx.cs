using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Hidistro.Entities.Sales;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnShippingOrderDetails : OrderDetailsBasePage
    {
        public UnShippingOrderDetails()
            : base(OrderStatus.BuyerAlreadyPaid)
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
                if (SubsiteSalesHelper.SaveRemark(base.Order))
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
            userInfo.UserId = base.Order.UserId;
            userInfo.Order = base.Order;
            itemsList.Order = base.Order;
            chargesList.Order = base.Order;
            shippingAddress.Order = base.Order;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadUserControl();
            btnRemark.Click += new EventHandler(btnRemark_Click);
            if (!Page.IsPostBack)
            {
                SetControl();
                BindRemark();
                if (base.Order.GroupBuyId > 0)
                {
                    lkbtnSendGoods.Visible = base.Order.GroupBuyStatus == GroupBuyStatus.Success;
                }
            }
        }

        private void SetControl()
        {
            lkbtnSendGoods.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/SendMyGoods.aspx?OrderId=" + base.Order.OrderId;
            int userId = base.Order.UserId;
            if (userId == 0x44c)
            {
                userId = 0;
            }
            if (Users.GetUser(userId) is Member)
            {
                liRefund.Visible = true;
                hlkRefund.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/RefundMyOrder.aspx?OrderId=" + base.Order.OrderId;
            }
        }
    }
}

