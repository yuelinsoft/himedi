using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ShippingModes)]
    public partial class AddShippingType : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                shippingTemplatesDropDownList.DataBind();
                expressCheckBoxList.BindExpressCheckBoxList();
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ShippingModeInfo target = new ShippingModeInfo();
            target.Name = Globals.HtmlEncode(txtModeName.Text.Trim());
            target.Description = fck.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            if (shippingTemplatesDropDownList.SelectedValue.HasValue)
            {
                target.TemplateId = shippingTemplatesDropDownList.SelectedValue.Value;
            }
            else
            {
                ShowMsg("请选择配送模板", false);
                return;
            }
            foreach (ListItem item in expressCheckBoxList.Items)
            {
                if (item.Selected)
                {
                    ExpressCompanyInfo expressCompanyInfo = new ExpressCompanyInfo();
                    expressCompanyInfo.ExpressCompanyName = item.Text;
                    expressCompanyInfo.ExpressCompanyAbb = item.Value;
                    target.ExpressCompany.Add(expressCompanyInfo);
                }
            }
            if (target.ExpressCompany.Count == 0)
            {
                ShowMsg("至少要选择一个配送公司", false);
            }
            else
            {
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ShippingModeInfo>(target, new string[] { "ValShippingModeInfo" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    ShowMsg(msg, false);
                }
                else if (SalesHelper.CreateShippingMode(target))
                {
                    ClearControlValue();
                    ShowMsg("成功添加了一个配送方式", true);
                }
                else
                {
                    ShowMsg("添加失败，请确定填写了所有必填项", false);
                }
            }
        }

        private void ClearControlValue()
        {
            txtModeName.Text = string.Empty;
            fck.Text = string.Empty;
        }


    }
}

