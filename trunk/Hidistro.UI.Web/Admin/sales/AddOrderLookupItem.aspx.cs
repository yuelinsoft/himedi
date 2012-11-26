
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.OrderLookup)]
    public partial class AddOrderLookupItem : AdminPage
    {
        int lookupListId;


        private void BindData()
        {
            grdOrderLookupItems.DataSource = OrderHelper.GetOrderLookupItems(lookupListId);
            grdOrderLookupItems.DataBind();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            decimal num;
            if ((radlCalculateMode.SelectedValue == 1) && (string.IsNullOrEmpty(txtAppendMoney.Text) || !decimal.TryParse(txtAppendMoney.Text.Trim(), out num)))
            {
                ShowMsg("附加金额不能为空,只能为非负数字,大小限制在1000万以内", false);
            }
            else
            {
                decimal num2;
                if ((radlCalculateMode.SelectedValue == 2) && (string.IsNullOrEmpty(txtPercentage.Text) || !decimal.TryParse(txtPercentage.Text.Trim(), out num2)))
                {
                    ShowMsg("购物车金额百分比不能为空,只能为非负数字,大小限制在100以内", false);
                }
                else
                {
                    OrderLookupItemInfo lookupItem = GetLookupItem();
                    if (lookupItem.IsUserInputRequired && string.IsNullOrEmpty(lookupItem.UserInputTitle))
                    {
                        ShowMsg("用户填写信息的标题不能为空", false);
                    }
                    else
                    {
                        ValidationResults results = Hishop.Components.Validation.Validation.Validate<OrderLookupItemInfo>(lookupItem, new string[] { "ValOrderLookupItemInfo" });
                        string msg = string.Empty;
                        if (!results.IsValid)
                        {
                            foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                            {
                                msg = msg + Formatter.FormatErrorMessage(result.Message);
                            }
                            ShowMsg(msg, false);
                        }
                        else
                        {
                            if (!OrderHelper.CreateOrderLookupItem(lookupItem))
                            {
                                ShowMsg("未知错误", false);
                            }
                            BindData();
                        }
                    }
                }
            }
        }

        private OrderLookupItemInfo GetLookupItem()
        {
            OrderLookupItemInfo info = new OrderLookupItemInfo();
            info.Name = txtName.Text.Trim();
            info.CalculateMode = new int?(radlCalculateMode.SelectedValue);
            info.UserInputTitle = txtUserInputTitle.Text.Trim();
            if (!((radlCalculateMode.SelectedValue != 1) || string.IsNullOrEmpty(txtAppendMoney.Text.Trim())))
            {
                info.AppendMoney = new decimal?(decimal.Parse(txtAppendMoney.Text.Trim()));
            }
            if (!((radlCalculateMode.SelectedValue != 2) || string.IsNullOrEmpty(txtPercentage.Text.Trim())))
            {
                info.AppendMoney = new decimal?(decimal.Parse(txtPercentage.Text.Trim()));
            }
            info.Remark = txtRemark.Text.Trim();
            info.IsUserInputRequired = radlUserInput.SelectedValue;
            info.LookupListId = lookupListId;
            return info;
        }

        private void grdOrderLookupItems_DataBound(object sender, EventArgs e)
        {
            for (int i = 0; i < grdOrderLookupItems.Rows.Count; i++)
            {
                GridViewRow row = grdOrderLookupItems.Rows[i];
                Label label = (Label)row.FindControl("lblPercentageAppendMoney");
                Label label2 = (FormatedMoneyLabel)row.FindControl("lblAppendMoney");
                int lookupItemId = (int)grdOrderLookupItems.DataKeys[i].Value;

                OrderLookupItemInfo orderLookupItem = OrderHelper.GetOrderLookupItem(lookupItemId);
                if (orderLookupItem.CalculateMode == 2)
                {
                    label.Text = orderLookupItem.AppendMoney.Value.ToString("F", CultureInfo.InvariantCulture) + "%";
                    label.Visible = true;
                    label2.Visible = false;
                }
                else
                {
                    label.Visible = false;
                    label2.Visible = true;
                }
            }
        }

        private void grdOrderLookupItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (OrderHelper.DeleteOrderLookupItem((int)grdOrderLookupItems.DataKeys[e.RowIndex].Value))
            {
                ShowMsg("成功删除了选择的订单选项内容", true);
                BindData();
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        private void lkOver_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("OrderLookupLists.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnCreate.Click += new EventHandler(btnCreate_Click);
            grdOrderLookupItems.RowDeleting += new GridViewDeleteEventHandler(grdOrderLookupItems_RowDeleting);
            grdOrderLookupItems.DataBound += new EventHandler(grdOrderLookupItems_DataBound);
            lkOver.Click += new EventHandler(lkOver_Click);
            if (!int.TryParse(base.Request.QueryString["LookupListId"], out lookupListId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                OrderLookupListInfo orderLookupList = OrderHelper.GetOrderLookupList(lookupListId);
                litName.Text = orderLookupList.Name;
            }
        }
    }
}

