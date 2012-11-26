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
    public partial class EditShippingType : AdminPage
    {

        int modeId;


        private void BindControl(ShippingModeInfo modeItem)
        {
            txtModeName.Text = Globals.HtmlDecode(modeItem.Name);
            fck.Text = modeItem.Description;
            if (modeItem.TemplateId > 0)
            {
                shippingTemplatesDropDownList.SelectedValue = new int?(modeItem.TemplateId);
            }
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            ShippingModeInfo info3 = new ShippingModeInfo();
            info3.Name = Globals.HtmlEncode(txtModeName.Text.Trim());
            info3.ModeId = modeId;
            info3.Description = fck.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            ShippingModeInfo target = info3;
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
                    ExpressCompanyInfo info2 = new ExpressCompanyInfo();
                    info2.ExpressCompanyName = item.Text;
                    info2.ExpressCompanyAbb = item.Value;
                    target.ExpressCompany.Add(info2);
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
                else if (SalesHelper.UpdateShippMode(target))
                {
                    Page.Response.Redirect("EditShippingType.aspx?modeId=" + target.ModeId + "&isUpdate=true");
                }
                else
                {
                    ShowMsg("更新失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["modeId"], out modeId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnUpDate.Click += new EventHandler(btnUpDate_Click);
                if (!Page.IsPostBack)
                {
                    if ((Page.Request.QueryString["isUpdate"] != null) && (Page.Request.QueryString["isUpdate"] == "true"))
                    {
                        ShowMsg("成功修改了一个配送方式", true);
                    }
                    ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(modeId, true);
                    if (shippingMode == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        BindControl(shippingMode);
                        shippingTemplatesDropDownList.DataBind();
                        BindControl(shippingMode);
                        expressCheckBoxList.ExpressCompany = shippingMode.ExpressCompany;
                        expressCheckBoxList.DataBind();
                    }
                }
            }
        }
    }
}

