using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.RefundOrder)]
    public partial class RefundOrderDetails : AdminPage
    {
        string orderId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                GotoResourceNotFound();
            }
            else
            {
                orderId = Page.Request.QueryString["OrderId"];
                if (!Page.IsPostBack)
                {
                    OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
                    if (orderInfo == null)
                    {
                        GotoResourceNotFound();
                    }
                    else if ((orderInfo.RefundStatus != RefundStatus.Refund) && (orderInfo.RefundStatus != RefundStatus.Below))
                    {
                        GotoResourceNotFound();
                    }
                    else
                    {
                        litOrderId.Text = orderInfo.OrderId;
                        lblOrderDate.Time = orderInfo.OrderDate;
                        lblTotalPrice.Money = orderInfo.GetTotal();
                        lblRefundDate.Time = orderInfo.FinishDate;
                        lblRefundAmount.Money = orderInfo.RefundAmount;
                        litRefundRemark.Text = orderInfo.RefundRemark;
                        if (orderInfo.RefundStatus == RefundStatus.Below)
                        {
                            lblRefundMode.InnerHtml = "线下退款";
                            lblDescription.InnerHtml = "退款成功，通过线下操作返回给了买家";
                        }
                        else if (orderInfo.RefundStatus == RefundStatus.Refund)
                        {
                            lblRefundMode.InnerHtml = "预付款退款";
                            lblDescription.InnerHtml = "退款成功直接返还到买家的预付款帐户中";
                        }
                    }
                }
            }
        }
    }
}

