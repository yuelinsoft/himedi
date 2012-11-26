using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
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
    [PrivilegeCheck(Privilege.Helps)]
    public partial class AddHelp : AdminPage
    {
        private void btnAddHelp_Click(object sender, EventArgs e)
        {
            HelpInfo target = new HelpInfo();
            if (!dropHelpCategory.SelectedValue.HasValue)
            {
                ShowMsg("请选择帮助分类", false);
            }
            else
            {
                target.AddedDate = DateTime.Now;
                target.CategoryId = dropHelpCategory.SelectedValue.Value;
                target.Title = txtHelpTitle.Text.Trim();
                target.MetaDescription = txtMetaDescription.Text.Trim();
                target.MetaKeywords = txtMetaKeywords.Text.Trim();
                target.Description = txtShortDesc.Text.Trim();
                target.Content = fcContent.Text;
                target.IsShowFooter = radioShowFooter.SelectedValue;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<HelpInfo>(target, new string[] { "ValHelpInfo" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    ShowMsg(msg, false);
                }
                else if (!(!radioShowFooter.SelectedValue || ArticleHelper.GetHelpCategory(target.CategoryId).IsShowFooter))
                {
                    ShowMsg("当选中的帮助分类设置不在底部帮助显示时，此分类下的帮助主题就不能设置在底部帮助显示", false);
                }
                else if (ArticleHelper.CreateHelp(target))
                {
                    txtHelpTitle.Text = string.Empty;
                    txtShortDesc.Text = string.Empty;
                    fcContent.Text = string.Empty;
                    ShowMsg("成功添加了一个帮助主题", true);
                }
                else
                {
                    ShowMsg("添加帮助主题错误", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddHelp.Click += new EventHandler(btnAddHelp_Click);
            if (!Page.IsPostBack)
            {
                dropHelpCategory.DataBind();
                if (Page.Request.QueryString["categoryId"] != null)
                {
                    int result = 0;
                    int.TryParse(Page.Request.QueryString["categoryId"], out result);
                    dropHelpCategory.SelectedValue = new int?(result);
                }
            }
        }
    }
}

