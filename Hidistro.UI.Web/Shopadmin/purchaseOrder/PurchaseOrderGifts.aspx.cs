using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class PurchaseOrderGifts : DistributorPage
    {

        PurchaseOrderInfo purchaseOrder;

        string purchaseOrderId;

        private void BindGifts()
        {
            GiftQuery query = new GiftQuery();
            query.Page.PageSize = 10;
            query.Page.PageIndex = pager.PageIndex;
            query.Name = txtSearchText.Text.Trim();
            DbQueryResult gifts = SubsiteSalesHelper.GetGifts(query);
            dlstGifts.DataSource = gifts.Data;
            dlstGifts.DataBind();
            pager.TotalRecords = gifts.TotalRecords;
        }

        private void BindOrderGifts()
        {
            PurchaseOrderGiftQuery query = new PurchaseOrderGiftQuery();
            query.PageSize = 10;
            query.PageIndex = pagerOrderGifts.PageIndex;
            query.PurchaseOrderId = purchaseOrderId;
            DbQueryResult purchaseOrderGifts = SubsiteSalesHelper.GetPurchaseOrderGifts(query);
            dlstOrderGifts.DataSource = purchaseOrderGifts.Data;
            dlstOrderGifts.DataBind();
            pagerOrderGifts.TotalRecords = purchaseOrderGifts.TotalRecords;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
            {
                ShowMsg("当前采购单状态没有订单礼品操作", false);
            }
            else if (!SubsiteSalesHelper.ClearPurchaseOrderGifts(purchaseOrder))
            {
                ShowMsg("清空礼品列表失败", false);
            }
            else
            {
                BindOrderGifts();
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (!((purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay) || string.IsNullOrEmpty(purchaseOrder.OrderId)))
            {
                base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnPaymentPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
            }
            if (!((purchaseOrder.PurchaseStatus != OrderStatus.BuyerAlreadyPaid) || string.IsNullOrEmpty(purchaseOrder.OrderId)))
            {
                base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnShippingPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
            }
            if ((purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay) && string.IsNullOrEmpty(purchaseOrder.OrderId))
            {
                base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnPaymentManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
            }
            if ((purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid) && string.IsNullOrEmpty(purchaseOrder.OrderId))
            {
                base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/UnShippingManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + purchaseOrderId);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGifts();
        }

        private void dlstGifts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
                {
                    ShowMsg("当前采购单状态没有订单礼品操作", false);
                }
                else
                {
                    int num;
                    int itemIndex = e.Item.ItemIndex;
                    int giftId = int.Parse(dlstGifts.DataKeys[itemIndex].ToString());
                    TextBox box = dlstGifts.Items[itemIndex].FindControl("txtQuantity") as TextBox;
                    if (!int.TryParse(box.Text.Trim(), out num))
                    {
                        ShowMsg("礼品数量填写错误", false);
                    }
                    else if (num <= 0)
                    {
                        ShowMsg("礼品赠送数量不能为0", false);
                    }
                    else
                    {
                        GiftInfo giftDetails = SubsiteSalesHelper.GetGiftDetails(giftId);
                        if (giftDetails == null)
                        {
                            base.GotoResourceNotFound();
                        }
                        else if (!SubsiteSalesHelper.AddPurchaseOrderGift(purchaseOrder, giftDetails, num))
                        {
                            ShowMsg("添加采购单礼品失败", false);
                        }
                        else
                        {
                            BindOrderGifts();
                        }
                    }
                }
            }
        }

        private void dlstOrderGifts_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS))
            {
                ShowMsg("当前采购单状态没有订单礼品操作", false);
            }
            else
            {
                int itemIndex = e.Item.ItemIndex;
                int giftId = int.Parse(dlstOrderGifts.DataKeys[itemIndex].ToString());
                if (!SubsiteSalesHelper.DeletePurchaseOrderGift(purchaseOrder, giftId))
                {
                    ShowMsg("删除采购单礼品失败", false);
                }
                BindOrderGifts();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["PurchaseOrderId"] == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                purchaseOrderId = Page.Request.QueryString["PurchaseOrderId"];
                purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
                btnSearch.Click += new EventHandler(btnSearch_Click);
                btnClear.Click += new EventHandler(btnClear_Click);
                dlstGifts.ItemCommand += new DataListCommandEventHandler(dlstGifts_ItemCommand);
                dlstOrderGifts.DeleteCommand += new DataListCommandEventHandler(dlstOrderGifts_DeleteCommand);
                btnReturn.Click += new EventHandler(btnReturn_Click);
                if (!base.IsPostBack)
                {
                    if (purchaseOrder == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else if (purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (purchaseOrder.PurchaseOrderGifts.Count > 0)
                        {
                            litPageTitle.Text = "编辑采购单礼品";
                        }
                        BindGifts();
                        BindOrderGifts();
                    }
                }
            }
        }
    }
}

