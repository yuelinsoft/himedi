using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class Order_UserInfo : UserControl
    {

        OrderInfo order;
        int userId;

        protected override void OnLoad(EventArgs e)
        {
            if (userId == 0x44c)
            {
                userId = 0;
            }
            IUser user = Users.GetUser(userId);
            if (!user.IsAnonymous)
            {
                Member member = user as Member;
                UserName.Text = member.Username;
                UserRealName.Text = member.RealName;
                UserTel.Text = member.TelPhone;
                UserEmail.Text = member.Email;
                if (!string.IsNullOrEmpty(member.Email))
                {
                    email.HRef = "mailto:" + member.Email;
                }
                lkbtnMessage.NavigateUrl = Globals.ApplicationPath + "/Admin/comment/SendMessage.aspx?UserId=" + UserId;
            }
            OrderInfo order = Order;
            lblOrderId.Text = Order.OrderId;
            if ((order.OrderStatus != OrderStatus.WaitBuyerPay) && (order.OrderStatus != OrderStatus.Closed))
            {
                lblPayTime.Time = order.PayDate;
                lblPayTime.Visible = true;
                lblpayTimeTitle.Visible = true;
            }
            else
            {
                lblPayTime.Visible = false;
            }
            if (order.OrderStatus == OrderStatus.Finished)
            {
                lblOrderOverTime.Time = order.FinishDate;
                lblOrderOverTime.Visible = true;
                lblFinishTime.Visible = true;
            }
            if ((order.OrderStatus == OrderStatus.SellerAlreadySent) || (order.OrderStatus == OrderStatus.Finished))
            {
                lblOrderSendGoodsTime.Time = order.ShippingDate;
                lblOrderSendGoodsTime.Visible = true;
                lblSendGoodTime.Visible = true;
            }
        }

        public OrderInfo Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }
    }
}

