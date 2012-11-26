using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
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

        private void LoadControl()
        {
            if ((base.Order.RefundStatus == RefundStatus.Refund) || (base.Order.RefundStatus == RefundStatus.Below))
            {
                divRefundDetails.Visible = true;
                hlkRefundDetails.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/RefundOrderDetails.aspx?OrderId=" + base.Order.OrderId;
            }
        }

        private void LoadUserControl()
        {
            itemsList.Order = base.Order;
            userInfo.UserId = base.Order.UserId;
            userInfo.Order = base.Order;
            chargesList.Order = base.Order;
            shippingAddress.Order = base.Order;
            if (Express.GetExpressType() == "kuaidi100")
            {
                power.Visible = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnRemark.Click += new EventHandler(btnRemark_Click);
            LoadUserControl();
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                string s = "{";
                if (!string.IsNullOrEmpty(Page.Request["OrderId"]))
                {
                    string orderId = Page.Request["OrderId"];
                    OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
                    if (!(((orderInfo == null) || ((orderInfo.OrderStatus != OrderStatus.SellerAlreadySent) && (orderInfo.OrderStatus != OrderStatus.Finished))) || string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb)))
                    {
                        string expressData = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
                        s = s + "\"Express\":\"" + expressData + "\"";
                    }
                }
                s = s + "}";
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write(s);
                base.Response.End();
            }
            if (!Page.IsPostBack)
            {
                BindRemark();
                LoadControl();
            }
        }
    }
}

