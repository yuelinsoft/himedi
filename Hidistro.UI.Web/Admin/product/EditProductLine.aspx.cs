using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
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
    [PrivilegeCheck(Privilege.EditProductLine)]
    public partial class EditProductLine : AdminPage
    {
        int lineId;

        private void btnSave_Click(object sender, EventArgs e)
        {
            ProductLineInfo target = new ProductLineInfo();
            target.LineId = lineId;
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
            else if (ProductLineHelper.UpdateProductLine(target))
            {
                ShowMsg("已经成功修改当前产品线信息", true);
            }
            else
            {
                ShowMsg("修改产品线失败", false);
            }
        }

        private void LoadControl()
        {
            ProductLineInfo productLine = ProductLineHelper.GetProductLine(lineId);
            if (productLine == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Globals.EntityCoding(productLine, false);
                txtProductLineName.Text = productLine.Name;
                dropSuppliers.SelectedValue = productLine.SupplierName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["LineId"], out lineId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnSave.Click += new EventHandler(btnSave_Click);
                if (!base.IsPostBack)
                {
                    dropSuppliers.Items.Add(new ListItem("-请选择-", ""));
                    foreach (string str in ProductLineHelper.GetSuppliers())
                    {
                        dropSuppliers.Items.Add(new ListItem(str, str));
                    }
                    LoadControl();
                }
            }
        }
    }
}

