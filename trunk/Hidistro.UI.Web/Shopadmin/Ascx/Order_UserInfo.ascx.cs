using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class Order_UserInfo : UserControl
    {

        OrderInfo order;
        int userId;


        protected override void OnLoad(EventArgs e)
        {
            if (this.userId == 0x44c)
            {
                this.userId = 0;
            }
            IUser user = Users.GetUser(this.UserId);
            if (!user.IsAnonymous)
            {
                Member member = user as Member;
                this.UserName.Text = member.Username;
                this.UserRealName.Text = member.RealName;
                this.UserTel.Text = member.TelPhone;
                this.UserEmail.Text = member.Email;
                if (!string.IsNullOrEmpty(member.Email))
                {
                    this.email.HRef = "mailto:" + member.Email;
                }
                this.lkbtnMessage.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/SendMyMessage.aspx?UserId=" + this.UserId;
            }
            OrderInfo order = this.Order;
            this.lblOrderId.Text = this.Order.OrderId;
            if ((order.OrderStatus != OrderStatus.WaitBuyerPay) && (order.OrderStatus != OrderStatus.Closed))
            {
                this.lblPayTime.Time = order.PayDate;
                this.lblPayTime.Visible = true;
                this.lblpayTimeTitle.Visible = true;
            }
            else
            {
                this.lblPayTime.Visible = false;
            }
            if (order.OrderStatus == OrderStatus.Finished)
            {
                this.lblOrderOverTime.Time = order.FinishDate;
                this.lblOrderOverTime.Visible = true;
                this.lblFinishTime.Visible = true;
            }
            if ((order.OrderStatus == OrderStatus.SellerAlreadySent) || (order.OrderStatus == OrderStatus.Finished))
            {
                this.lblOrderSendGoodsTime.Time = order.ShippingDate;
                this.lblOrderSendGoodsTime.Visible = true;
                this.lblSendGoodTime.Visible = true;
            }
        }

        public OrderInfo Order
        {
            get
            {
                return this.order;
            }
            set
            {
                this.order = value;
            }
        }

        public int UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
        }
    }
}

