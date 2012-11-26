using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.PurchaseorderRefund)]
    public partial class RefundPurchaseOrder : AdminPage
    {

        string purchaseOrderId;


        private void BtnRefund_Click(object sender, EventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_REJECT_REFUND))
            {
                ShowMsg("未付款或不在进行中的订单没有退款操作", false);
            }
            else
            {
                int length = 0;
                decimal result = 0M;
                if (purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent)
                {
                    if (txtRefundTotal.Text.Trim().IndexOf(".") > 0)
                    {
                        length = txtRefundTotal.Text.Trim().Substring(txtRefundTotal.Text.Trim().IndexOf(".") + 1).Length;
                    }
                    if (!(decimal.TryParse(txtRefundTotal.Text.Trim(), out result) && (length <= 2)))
                    {
                        ShowMsg("退款金额只能是数值，且不能超过2位小数", false);
                        return;
                    }
                    if ((result <= 0M) || (result > purchaseOrder.GetPurchaseTotal()))
                    {
                        ShowMsg("退款金额必须大于0,小于等于实收款", false);
                        return;
                    }
                }
                if (txtRefundRemark.Text.Length > 200)
                {
                    ShowMsg("退款说明限制在200个字符以内", false);
                }
                else
                {
                    decimal num3;
                    Distributor user = Users.GetUser(purchaseOrder.DistributorId) as Distributor;
                    if (purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent)
                    {
                        num3 = result;
                    }
                    else
                    {
                        num3 = decimal.Parse(lblRefundTotal.Money.ToString());
                    }
                    BalanceDetailInfo balanceDetails = new BalanceDetailInfo();
                    balanceDetails.UserId = purchaseOrder.DistributorId;
                    balanceDetails.UserName = purchaseOrder.Distributorname;
                    balanceDetails.TradeDate = DateTime.Now;
                    balanceDetails.TradeType = TradeTypes.RefundOrder;
                    balanceDetails.Income = new decimal?(num3);
                    balanceDetails.Balance = user.Balance + num3;
                    balanceDetails.Remark = "采购单退款到预付款";
                    if (DistributorHelper.AddBalance(balanceDetails))
                    {
                        purchaseOrder.RefundAmount = num3;
                        purchaseOrder.RefundRemark = Globals.HtmlEncode(txtRefundRemark.Text.Trim());
                        purchaseOrder.FinishDate = DateTime.Now;
                        if (purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)
                        {
                            purchaseOrder.PurchaseStatus = OrderStatus.Closed;
                        }
                        if (purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent)
                        {
                            purchaseOrder.PurchaseStatus = OrderStatus.Finished;
                        }
                        if (SalesHelper.RefundPurchaseOrder(purchaseOrder))
                        {
                            Page.Response.Redirect(Globals.ApplicationPath + string.Format("/Admin/purchaseOrder/RefundPurchaseOrderDetails.aspx?PurchaseOrderId={0}", purchaseOrder.PurchaseOrderId));
                        }
                        else
                        {
                            ShowMsg("退款失败", false);
                        }
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Page.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                purchaseOrderId = Page.Request.QueryString["PurchaseOrderId"];
                BtnRefund.Click += new EventHandler(BtnRefund_Click);
                if (!Page.IsPostBack)
                {
                    PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
                    if (purchaseOrder == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        SetControl(purchaseOrder);
                    }
                }
            }
        }

        private void SetControl(PurchaseOrderInfo purchaseOrder)
        {
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_REJECT_REFUND))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Distributor distributor = DistributorHelper.GetDistributor(purchaseOrder.DistributorId);
                if (distributor == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    litDistributorName.Text = distributor.Username;
                    litOrderId.Text = purchaseOrder.OrderId;
                    lblOrderDate.Time = purchaseOrder.PurchaseDate;
                    litPurchaseOrderId.Text = purchaseOrder.PurchaseOrderId;
                    lblTotalPrice.Money = purchaseOrder.GetPurchaseTotal();
                    lblPurchaseStatus.PuchaseStatusCode = (int)purchaseOrder.PurchaseStatus;
                    if (purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent)
                    {
                        lblRefundTotal.Visible = false;
                        litRefundComment.Text = "退款金额不得大于" + lblTotalPrice.Money + "元.已发货订单允许全额或部分退款。";
                    }
                    else
                    {
                        txtRefundTotal.Style.Add(HtmlTextWriterStyle.Display, "none");
                        lblRefundTotal.Money = lblTotalPrice.Money;
                        litRefundComment.Text = "已付款等待发货订单只允许全额退款.退款后采购单自动变为关闭状态。";
                    }
                }
            }
        }
    }
}

