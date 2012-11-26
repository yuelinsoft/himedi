using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Hidistro.UI.Web.Shopadmin;

namespace Hidistro.UI.Web.Shopadmin
{

    public partial class Pay : DistributorPage
    {

        PurchaseOrderInfo purchaseOrder;

        string purchaseOrderId;

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (!purchaseOrder.IsManualPurchaseOrder)
            {
                Response.Redirect("UnShippingPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
            }
            else
            {
                Response.Redirect("UnShippingManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
            }
        }

        private void btnConfirmPay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTradePassword.Text))
            {
                ShowMsg("请输入交易密码", false);
            }
            else if (((decimal)lblUseableBalance.Money) < ((decimal)lblTotalPrice.Money))
            {
                ShowMsg("您的预付款金额不足", false);
            }
            else
            {
                Distributor user = SubsiteStoreHelper.GetDistributor();

                if ((user.Balance - user.RequestBalance) < purchaseOrder.GetPurchaseTotal())
                {

                    ShowMsg("您的预付款金额不足", false);

                }
                else
                {

                    BalanceDetailInfo balance = new BalanceDetailInfo();

                    balance.UserId = user.UserId;
                    balance.UserName = user.Username;
                    balance.TradeType = TradeTypes.Consume;
                    balance.TradeDate = DateTime.Now;
                    balance.Expenses = new decimal?(purchaseOrder.GetPurchaseTotal());
                    balance.Balance = user.Balance - purchaseOrder.GetPurchaseTotal();
                    user.TradePassword = txtTradePassword.Text;

                    if (Users.ValidTradePassword(user))
                    {
                        if (!SubsiteSalesHelper.ConfirmPay(balance, purchaseOrder))
                        {
                            ShowMsg("付款失败", false);
                        }
                        else
                        {
                            PaySucceess.Visible = true;
                        }
                    }
                    else
                    {
                        ShowMsg("交易密码错误", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnConfirmPay.Click += new EventHandler(btnConfirmPay_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            btnBack1.Click += new EventHandler(btnBack_Click);
            imgBtnBack.Click += new System.Web.UI.ImageClickEventHandler(btnBack_Click);
            if (string.IsNullOrEmpty(base.Request.QueryString["PurchaseOrderId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                purchaseOrderId = base.Request.QueryString["PurchaseOrderId"];
                purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
                if (!base.IsPostBack)
                {
                    if (purchaseOrder == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (purchaseOrder.IsManualPurchaseOrder)
                        {
                            litorder.Visible = false;
                            litOrderId.Visible = false;
                        }
                        else
                        {
                            litOrderId.Text = purchaseOrder.OrderId;
                        }
                        litPurchaseOrderId.Text = purchaseOrder.PurchaseOrderId;
                        lblPurchaseDate.Time = purchaseOrder.PurchaseDate;
                        lblTotalPrice.Money = purchaseOrder.GetPurchaseTotal();
                        AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
                        lblUseableBalance.Money = myAccountSummary.UseableBalance;
                    }
                }
            }
        }
    }
}

