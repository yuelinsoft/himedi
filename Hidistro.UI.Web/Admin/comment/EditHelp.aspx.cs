using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Helps)]
    public partial class EditHelp : AdminPage
    {
        int helpId;

        private void btnEditHelp_Click(object sender, EventArgs e)
        {
            HelpInfo target = new HelpInfo();
            if (!dropHelpCategory.SelectedValue.HasValue)
            {
                ShowMsg("请选择帮助分类", false);
            }
            else
            {
                target.HelpId = helpId;
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
                else if (ArticleHelper.UpdateHelp(target))
                {
                    ShowMsg("已经成功修改当前帮助", true);
                }
                else
                {
                    ShowMsg("编辑底部帮助错误", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["helpId"], out helpId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditHelp.Click += new EventHandler(btnEditHelp_Click);
                if (!Page.IsPostBack)
                {
                    dropHelpCategory.DataBind();
                    HelpInfo help = ArticleHelper.GetHelp(helpId);
                    if (help == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(help, false);
                        txtHelpTitle.Text = help.Title;
                        txtMetaDescription.Text = help.MetaDescription;
                        txtMetaKeywords.Text = help.MetaKeywords;
                        txtShortDesc.Text = help.Description;
                        fcContent.Text = help.Content;
                        lblEditHelp.Text = help.HelpId.ToString(CultureInfo.InvariantCulture);
                        dropHelpCategory.SelectedValue = new int?(help.CategoryId);
                        radioShowFooter.SelectedValue = help.IsShowFooter;
                    }
                }
            }
        }
    }
}

