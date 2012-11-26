using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ManagePurchaseorder)]
    public partial class ManagePurchaseOrder : AdminPage
    {

        private void BindPurchaseOrders()
        {
            PurchaseOrderQuery purchaseOrderQuery = GetPurchaseOrderQuery();
            purchaseOrderQuery.SortBy = "PurchaseDate";
            purchaseOrderQuery.SortOrder = SortAction.Desc;
            DbQueryResult purchaseOrders = SalesHelper.GetPurchaseOrders(purchaseOrderQuery);
            dlstPurchaseOrders.DataSource = purchaseOrders.Data;
            dlstPurchaseOrders.DataBind();
            pager.TotalRecords = purchaseOrders.TotalRecords;
            pager1.TotalRecords = purchaseOrders.TotalRecords;
            txtOrderId.Text = purchaseOrderQuery.OrderId;
            txtProductName.Text = purchaseOrderQuery.ProductName;
            txtDistributorName.Text = purchaseOrderQuery.DistributorName;
            txtPurchaseOrderId.Text = purchaseOrderQuery.PurchaseOrderId;
            txtShopTo.Text = purchaseOrderQuery.ShipTo;
            calendarStartDate.SelectedDate = purchaseOrderQuery.StartDate;
            calendarEndDate.SelectedDate = purchaseOrderQuery.EndDate;
            lblStatus.Text = ((int)purchaseOrderQuery.PurchaseStatus).ToString();
            shippingModeDropDownList.SelectedValue = purchaseOrderQuery.ShippingModeId;
            if (purchaseOrderQuery.IsPrinted.HasValue)
            {
                ddlIsPrinted.SelectedValue = purchaseOrderQuery.IsPrinted.Value.ToString();
            }
        }

        private void btnClosePurchaseOrder_Click(object sender, EventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(hidPurchaseOrderId.Value);
            purchaseOrder.CloseReason = ddlCloseReason.SelectedValue;
            if (SalesHelper.ClosePurchaseOrder(purchaseOrder))
            {
                BindPurchaseOrders();
                ShowMsg("关闭采购单成功", true);
            }
            else
            {
                ShowMsg("关闭采购单失败", false);
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            decimal num;
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(lblPurchaseOrderId.Value);
            string msg = string.Empty;
            if (ValidateValues(out num))
            {
                purchaseOrder.AdjustedDiscount += num;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<PurchaseOrderInfo>(purchaseOrder, new string[] { "ValPurchaseOrder" });
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                        ShowMsg(msg, false);
                        return;
                    }
                }
                if (purchaseOrder.GetPurchaseTotal() >= 0M)
                {
                    if (SalesHelper.UpdatePurchaseOrderAmount(purchaseOrder))
                    {
                        BindPurchaseOrders();
                        ShowMsg("修改成功", true);
                    }
                    else
                    {
                        ShowMsg("修改失败", false);
                    }
                }
                else
                {
                    ShowMsg("折扣值不能使得采购单总金额为负", false);
                }
            }
        }

        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                ShowMsg("请选要打印的采购单", false);
            }
            else
            {
                string[] orderList = str.Split(new char[] { ',' });
                int num = SalesHelper.CreatePrintTask(HiContext.Current.User.Username, DateTime.Now, true, orderList);
                if (num > 0)
                {
                    Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/ChoosePrintOrders.aspx?TaskId=" + num));
                }
                else
                {
                    ShowMsg("创建批量打印快递单任务失败！", false);
                }
            }
        }

        private void btnRemark_Click(object sender, EventArgs e)
        {
            if (txtRemark.Text.Length > 300)
            {
                ShowMsg("备忘录长度限制在300个字符以内", false);
            }
            else
            {
                PurchaseOrderInfo info2 = new PurchaseOrderInfo();
                info2.PurchaseOrderId = hidPurchaseOrderId.Value;
                PurchaseOrderInfo purchaseOrder = info2;
                if (orderRemarkImageForRemark.SelectedItem != null)
                {
                    purchaseOrder.ManagerMark = orderRemarkImageForRemark.SelectedValue;
                }
                purchaseOrder.ManagerRemark = Globals.HtmlEncode(txtRemark.Text);
                if (SalesHelper.SavePurchaseOrderRemark(purchaseOrder))
                {
                    BindPurchaseOrders();
                    ShowMsg("保存备忘录成功", true);
                }
                else
                {
                    ShowMsg("保存失败", false);
                }
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBinderPurchaseOrders(true);
        }

        private void btnSendGoods_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                ShowMsg("请选要发货的订单", false);
            }
            else
            {
                Page.Response.Redirect(Globals.GetAdminAbsolutePath("/purchaseOrder/BatchSendPurchaseOrderGoods.aspx?PurchaseOrderIds=" + str));
            }
        }

        private void dlstPurchaseOrders_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(e.CommandArgument.ToString());
            if (purchaseOrder != null)
            {
                if ((e.CommandName == "FINISH_TRADE") && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
                {
                    if (SalesHelper.ConfirmPurchaseOrderFinish(purchaseOrder))
                    {
                        BindPurchaseOrders();
                        ShowMsg("成功的完成了该采购单", true);
                    }
                    else
                    {
                        ShowMsg("完成采购单失败", false);
                    }
                }
                if ((e.CommandName == "CONFIRM_PAY") && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_CONFIRM_PAY))
                {
                    if (SalesHelper.ConfirmPayPurchaseOrder(purchaseOrder))
                    {
                        BindPurchaseOrders();
                        ShowMsg("成功的确认了采购单收款", true);
                    }
                    else
                    {
                        ShowMsg("确认采购单收款失败", false);
                    }
                }
            }
        }

        private void dlstPurchaseOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HyperLink link = (HyperLink)e.Item.FindControl("lkbtnSendGoods");
                ImageLinkButton button = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmPurchaseOrder");
                HtmlGenericControl control = (HtmlGenericControl)e.Item.FindControl("lkbtnEditPurchaseOrder");
                Literal literal = (Literal)e.Item.FindControl("litClosePurchaseOrder");
                ImageLinkButton button2 = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
                OrderStatus status = (OrderStatus)DataBinder.Eval(e.Item.DataItem, "PurchaseStatus");
                if (status == OrderStatus.WaitBuyerPay)
                {
                    literal.Visible = true;
                    control.Visible = true;
                    button2.Visible = true;
                }
                link.Visible = status == OrderStatus.BuyerAlreadyPaid;
                button.Visible = status == OrderStatus.SellerAlreadySent;
            }
        }

        private PurchaseOrderQuery GetPurchaseOrderQuery()
        {
            PurchaseOrderQuery query = new PurchaseOrderQuery();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OrderId"]))
            {
                query.OrderId = Globals.UrlDecode(Page.Request.QueryString["OrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["PurchaseOrderId"]))
            {
                query.PurchaseOrderId = Globals.UrlDecode(Page.Request.QueryString["PurchaseOrderId"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ShopTo"]))
            {
                query.ShipTo = Globals.UrlDecode(Page.Request.QueryString["ShopTo"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ProductName"]))
            {
                query.ProductName = Globals.UrlDecode(Page.Request.QueryString["ProductName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["DistributorName"]))
            {
                query.DistributorName = Globals.UrlDecode(Page.Request.QueryString["DistributorName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["startDate"]))
            {
                query.StartDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["endDate"]))
            {
                query.EndDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["endDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["PurchaseStatus"]))
            {
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["PurchaseStatus"], out result))
                {
                    query.PurchaseStatus = (OrderStatus)result;
                }
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["IsPrinted"]))
            {
                int num2 = 0;
                if (int.TryParse(Page.Request.QueryString["IsPrinted"], out num2))
                {
                    query.IsPrinted = new int?(num2);
                }
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ModeId"]))
            {
                int num3 = 0;
                if (int.TryParse(Page.Request.QueryString["ModeId"], out num3))
                {
                    query.ShippingModeId = new int?(num3);
                }
            }
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "PurchaseDate";
            return query;
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            string str = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                str = base.Request["CheckBoxGroup"];
            }
            if (str.Length <= 0)
            {
                ShowMsg("请选要删除的采购单", false);
            }
            else
            {
                int num = SalesHelper.DeletePurchaseOrders("'" + str.Replace(",", "','") + "'");
                BindPurchaseOrders();
                ShowMsg(string.Format("成功删除了{0}个采购单", num), true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dlstPurchaseOrders.ItemDataBound += new DataListItemEventHandler(dlstPurchaseOrders_ItemDataBound);
            dlstPurchaseOrders.ItemCommand += new DataListCommandEventHandler(dlstPurchaseOrders_ItemCommand);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            btnRemark.Click += new EventHandler(btnRemark_Click);
            btnClosePurchaseOrder.Click += new EventHandler(btnClosePurchaseOrder_Click);
            lkbtnDeleteCheck.Click += new EventHandler(lkbtnDeleteCheck_Click);
            btnBatchPrintData.Click += new EventHandler(btnPrintOrder_Click);
            btnBatchSendGoods.Click += new EventHandler(btnSendGoods_Click);
            btnEditOrder.Click += new EventHandler(btnEditOrder_Click);
            if (!Page.IsPostBack)
            {
                shippingModeDropDownList.DataBind();
                ddlIsPrinted.Items.Clear();
                ddlIsPrinted.Items.Add(new ListItem("全部", string.Empty));
                ddlIsPrinted.Items.Add(new ListItem("已打印", "1"));
                ddlIsPrinted.Items.Add(new ListItem("未打印", "0"));
                SetPurchaseOrderStatusLink();
                BindPurchaseOrders();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBinderPurchaseOrders(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OrderId", txtOrderId.Text.Trim());
            queryStrings.Add("PurchaseOrderId", txtPurchaseOrderId.Text.Trim());
            queryStrings.Add("ShopTo", txtShopTo.Text.Trim());
            queryStrings.Add("ProductName", txtProductName.Text.Trim());
            queryStrings.Add("DistributorName", txtDistributorName.Text.Trim());
            queryStrings.Add("PurchaseStatus", lblStatus.Text);
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            if (calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("StartDate", calendarStartDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("EndDate", calendarEndDate.SelectedDate.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            if (shippingModeDropDownList.SelectedValue.HasValue)
            {
                queryStrings.Add("ModeId", shippingModeDropDownList.SelectedValue.Value.ToString());
            }
            if (!string.IsNullOrEmpty(ddlIsPrinted.SelectedValue))
            {
                queryStrings.Add("IsPrinted", ddlIsPrinted.SelectedValue);
            }
            base.ReloadPage(queryStrings);
        }

        private void SetPurchaseOrderStatusLink()
        {
            string format = Globals.ApplicationPath + "/Admin/purchaseOrder/ManagePurchaseOrder.aspx?PurchaseStatus={0}";
            hlinkAllOrder.NavigateUrl = string.Format(format, 0);
            hlinkNotPay.NavigateUrl = string.Format(format, 1);
            hlinkYetPay.NavigateUrl = string.Format(format, 2);
            hlinkSendGoods.NavigateUrl = string.Format(format, 3);
            hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
            hlinkClose.NavigateUrl = string.Format(format, 4);
            hlinkHistory.NavigateUrl = string.Format(format, 0x63);
        }

        private bool ValidateValues(out decimal discountAmout)
        {
            string str = string.Empty;
            int length = 0;
            if (txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") > 0)
            {
                length = txtPurchaseOrderDiscount.Text.Trim().Substring(txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") + 1).Length;
            }
            if (!(decimal.TryParse(txtPurchaseOrderDiscount.Text.Trim(), out discountAmout) && (length <= 2)))
            {
                str = str + Formatter.FormatErrorMessage("采购单折扣填写错误,采购单折扣只能是数值，且不能超过2位小数");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

