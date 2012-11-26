using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.OrderLookup)]
    public partial class EditOrderLookup : AdminPage
    {

        int lookupListId;


        private void btnSave_Click(object sender, EventArgs e)
        {
            OrderLookupListInfo orderLookupList = GetOrderLookupList();
            orderLookupList.LookupListId = lookupListId;
            if (ValidationOrderLookupList(orderLookupList))
            {
                if (!OrderHelper.UpdateOrderLookupList(orderLookupList))
                {
                    ShowMsg("未知错误", false);
                }
                else
                {
                    ShowMsg("修改可选项成功", true);
                }
            }
        }

        private OrderLookupListInfo GetOrderLookupList()
        {
            OrderLookupListInfo orderLookupListInfo = new OrderLookupListInfo();
            orderLookupListInfo.Name = txtListName.Text.Trim();
            orderLookupListInfo.SelectMode = dropSelectMode.SelectedValue;
            orderLookupListInfo.Description = txtDescription.Text.Trim();
            return orderLookupListInfo;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["LookupListId"], out lookupListId))
            {
                GotoResourceNotFound();
            }
            else
            {
                btnSave.Click += new EventHandler(btnSave_Click);
                if (!IsPostBack)
                {
                    OrderLookupListInfo orderLookupList = OrderHelper.GetOrderLookupList(lookupListId);
                    if (orderLookupList == null)
                    {
                        GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(orderLookupList, false);
                        txtListName.Text = orderLookupList.Name;
                        dropSelectMode.SelectedValue = orderLookupList.SelectMode;
                        txtDescription.Text = orderLookupList.Description;
                    }
                }
            }
        }

        private bool ValidationOrderLookupList(OrderLookupListInfo lookupList)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<OrderLookupListInfo>(lookupList, new string[] { "ValOrderLookupListInfo" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

