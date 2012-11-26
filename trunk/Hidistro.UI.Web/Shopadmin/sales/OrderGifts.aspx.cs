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
    public partial class OrderGifts : DistributorPage
    {
        OrderInfo order;
        string orderId;


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
            OrderGiftQuery query = new OrderGiftQuery();
            query.PageSize = 10;
            query.PageIndex = pagerOrderGifts.PageIndex;
            query.OrderId = orderId;
            DbQueryResult orderGifts = SubsiteSalesHelper.GetOrderGifts(query);
            dlstOrderGifts.DataSource = orderGifts.Data;
            dlstOrderGifts.DataBind();
            pagerOrderGifts.TotalRecords = orderGifts.TotalRecords;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_GIFTS))
            {
                ShowMsg("当前订单状态没有订单礼品操作", false);
            }
            else if (!SubsiteSalesHelper.ClearOrderGifts(order))
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
            if (order.OrderStatus == OrderStatus.WaitBuyerPay)
            {
                base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/sales/UnPaymentOrderDetails.aspx?OrderId=" + orderId);
            }
            if (order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
            {
                base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/sales/UnShippingOrderDetails.aspx?OrderId=" + orderId);
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
                if (!order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_GIFTS))
                {
                    ShowMsg("当前订单状态没有订单礼品操作", false);
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
                        else if (!SubsiteSalesHelper.AddOrderGift(order, giftDetails, num))
                        {
                            ShowMsg("添加订单礼品失败", false);
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
            if (!order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_GIFTS))
            {
                ShowMsg("当前订单状态没有订单礼品操作", false);
            }
            else
            {
                int itemIndex = e.Item.ItemIndex;
                int giftId = int.Parse(dlstOrderGifts.DataKeys[itemIndex].ToString());
                if (!SubsiteSalesHelper.DeleteOrderGift(order, giftId))
                {
                    ShowMsg("删除订单礼品失败", false);
                }
                BindOrderGifts();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.Request.QueryString["OrderId"] == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                orderId = Page.Request.QueryString["OrderId"];
                order = SubsiteSalesHelper.GetOrderInfo(orderId);
                btnReturn.Click += new EventHandler(btnReturn_Click);
                btnSearch.Click += new EventHandler(btnSearch_Click);
                btnClear.Click += new EventHandler(btnClear_Click);
                dlstGifts.ItemCommand += new DataListCommandEventHandler(dlstGifts_ItemCommand);
                dlstOrderGifts.DeleteCommand += new DataListCommandEventHandler(dlstOrderGifts_DeleteCommand);
                if (!base.IsPostBack)
                {
                    if (order == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else if (order.OrderStatus != OrderStatus.WaitBuyerPay)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (order.Gifts.Count > 0)
                        {
                            litPageTitle.Text = "编辑订单礼品";
                            litPageNote.Text = "修改赠送给买家的礼品.";
                        }
                        BindGifts();
                        BindOrderGifts();
                    }
                }
            }
        }
    }
}

