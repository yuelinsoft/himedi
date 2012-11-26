    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.HelpCategories)]
    public partial class AddHelpCategory : AdminPage
    {

        private void AddNewCategory(HelpCategoryInfo category)
        {
            if (ArticleHelper.CreateHelpCategory(category))
            {
                ShowMsg("成功添加了一个帮助分类", true);
            }
            else
            {
                ShowMsg("操作失败,未知错误", false);
            }
        }

        private void btnSubmitHelpCategory_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (fileUpload.HasFile)
            {
                try
                {
                    str = ArticleHelper.UploadHelpImage(fileUpload.PostedFile);
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            HelpCategoryInfo info2 = new HelpCategoryInfo();
            info2.Name = txtHelpCategoryName.Text.Trim();
            info2.IconUrl = str;
            info2.Description = txtHelpCategoryDesc.Text.Trim();
            info2.IsShowFooter = radioShowFooter.SelectedValue;
            HelpCategoryInfo target = info2;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<HelpCategoryInfo>(target, new string[] { "ValHelpCategoryInfo" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                AddNewCategory(target);
                Reset();
            }
            else
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitHelpCategory.Click += new EventHandler(btnSubmitHelpCategory_Click);
        }

        private void Reset()
        {
            txtHelpCategoryName.Text = string.Empty;
            txtHelpCategoryDesc.Text = string.Empty;
        }
    }
}

