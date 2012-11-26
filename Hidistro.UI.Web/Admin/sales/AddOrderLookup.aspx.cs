using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
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
    public partial class AddOrderLookup : AdminPage
    {


        private void btnCreate_Click(object sender, EventArgs e)
        {

            OrderLookupListInfo orderLookupList = GetOrderLookupList();

            if (ValidationOrderLookupList(orderLookupList))
            {
                int lookupListId = OrderHelper.CreateOrderLookupList(orderLookupList);
                if (lookupListId < 0)
                {
                    ShowMsg("未知错误", false);
                }
                else
                {
                    base.Response.Redirect("AddOrderLookupItem.aspx?LookupListId=" + lookupListId);
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
            btnCreate.Click += new EventHandler(btnCreate_Click);
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

