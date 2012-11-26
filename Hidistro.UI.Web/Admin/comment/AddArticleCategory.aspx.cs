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
    [PrivilegeCheck(Privilege.ArticleCategories)]
    public partial class AddArticleCategory : AdminPage
    {

        private void AddNewCategory(ArticleCategoryInfo category)
        {
            if (ArticleHelper.CreateArticleCategory(category))
            {
                Reset();
                ShowMsg("成功添加了一个文章分类", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        private void btnSubmitArticleCategory_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (fileUpload.HasFile)
            {
                try
                {
                    str = ArticleHelper.UploadArticleImage(fileUpload.PostedFile);
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            ArticleCategoryInfo info2 = new ArticleCategoryInfo();
            info2.Name = txtArticleCategoryiesName.Text.Trim();
            info2.IconUrl = str;
            info2.Description = txtArticleCategoryiesDesc.Text.Trim();
            ArticleCategoryInfo target = info2;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleCategoryInfo>(target, new string[] { "ValArticleCategoryInfo" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                AddNewCategory(target);
            }
            else
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitArticleCategory.Click += new EventHandler(btnSubmitArticleCategory_Click);
        }

        private void Reset()
        {
            txtArticleCategoryiesName.Text = string.Empty;
            txtArticleCategoryiesDesc.Text = string.Empty;
        }
    }
}

