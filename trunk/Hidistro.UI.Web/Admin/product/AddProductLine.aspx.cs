
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AddProductLine)]
    public partial class AddProductLine : AdminPage
    {
        private void btnCreate_Click(object sender, EventArgs e)
        {
            ProductLineInfo target = new ProductLineInfo();
            target.Name = txtProductLineName.Text.Trim();
            target.SupplierName = (dropSuppliers.SelectedValue.Length > 0) ? dropSuppliers.SelectedValue : null;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductLineInfo>(target, new string[] { "ValProductLine" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else if (ProductLineHelper.AddProductLine(target))
            {
                Reset();
                ShowMsg("成功的添加了一条产品线", true);
            }
            else
            {
                ShowMsg("添加产品线失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnCreate.Click += new EventHandler(btnCreate_Click);
            if (!Page.IsPostBack)
            {
                dropSuppliers.Items.Add(new ListItem("-请选择-", ""));
                foreach (string str in ProductLineHelper.GetSuppliers())
                {
                    dropSuppliers.Items.Add(new ListItem(str, str));
                }
            }
        }

        private void Reset()
        {
            txtProductLineName.Text = string.Empty;
        }
    }
}

