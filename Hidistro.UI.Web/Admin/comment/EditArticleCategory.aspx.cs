    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
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
    public partial class EditArticleCategory : AdminPage
    {
        int categoryId;

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(categoryId);
            try
            {
                ResourcesHelper.DeleteImage(articleCategory.IconUrl);
            }
            catch
            {
            }
            articleCategory.IconUrl = (string) (imgPic.ImageUrl = null);
            if (ArticleHelper.UpdateArticleCategory(articleCategory))
            {
                btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
            }
        }

        private void btnSubmitArticleCategory_Click(object sender, EventArgs e)
        {
            ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(categoryId);
            if (articleCategory != null)
            {
                if (fileUpload.HasFile)
                {
                    try
                    {
                        ResourcesHelper.DeleteImage(articleCategory.IconUrl);
                        articleCategory.IconUrl = ArticleHelper.UploadArticleImage(fileUpload.PostedFile);
                    }
                    catch
                    {
                        ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                articleCategory.Name = txtArticleCategoryiesName.Text.Trim();
                articleCategory.Description = txtArticleCategoryiesDesc.Text.Trim();
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleCategoryInfo>(articleCategory, new string[] { "ValArticleCategoryInfo" });
                string msg = string.Empty;
                if (results.IsValid)
                {
                    UpdateCategory(articleCategory);
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitArticleCategory.Click += new EventHandler(btnSubmitArticleCategory_Click);
            btnPicDelete.Click += new EventHandler(btnPicDelete_Click);
            if (!int.TryParse(base.Request.QueryString["CategoryId"], out categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(categoryId);
                if (articleCategory == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(articleCategory, false);
                    txtArticleCategoryiesName.Text = articleCategory.Name;
                    txtArticleCategoryiesDesc.Text = articleCategory.Description;
                    imgPic.ImageUrl = articleCategory.IconUrl;
                    btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                    imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                }
            }
        }

        private void UpdateCategory(ArticleCategoryInfo category)
        {
            if (ArticleHelper.UpdateArticleCategory(category))
            {
                imgPic.ImageUrl = null;
                ShowMsg("成功修改了文章分类", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
            btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
            imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
        }
    }
}

