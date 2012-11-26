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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Articles)]
    public partial class EditArticle : AdminPage
    {
        int articleId;

        private void btnAddArticle_Click(object sender, EventArgs e)
        {
            if (!dropArticleCategory.SelectedValue.HasValue)
            {
                ShowMsg("请选择文章分类", false);
            }
            else
            {
                ArticleInfo article = ArticleHelper.GetArticle(articleId);
                if (fileUpload.HasFile)
                {
                    try
                    {
                        ResourcesHelper.DeleteImage(article.IconUrl);
                        article.IconUrl = ArticleHelper.UploadArticleImage(fileUpload.PostedFile);
                        imgPic.ImageUrl = article.IconUrl;
                    }
                    catch
                    {
                        ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                article.ArticleId = articleId;
                article.CategoryId = dropArticleCategory.SelectedValue.Value;
                article.Title = txtArticleTitle.Text.Trim();
                article.MetaDescription = txtMetaDescription.Text.Trim();
                article.MetaKeywords = txtMetaKeywords.Text.Trim();
                article.Description = txtShortDesc.Text.Trim();
                article.Content = fcContent.Text;
                article.AddedDate = DateTime.Now;
                article.IsRelease = ckrrelease.Checked;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleInfo>(article, new string[] { "ValArticleInfo" });
                string msg = string.Empty;
                if (results.IsValid)
                {
                    if (ArticleHelper.UpdateArticle(article))
                    {
                        ShowMsg("已经成功修改当前文章", true);
                    }
                    else
                    {
                        ShowMsg("修改文章失败", false);
                    }
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
        }

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            ArticleInfo article = ArticleHelper.GetArticle(articleId);
            try
            {
                ResourcesHelper.DeleteImage(article.IconUrl);
            }
            catch
            {
            }
            article.IconUrl = (string)(imgPic.ImageUrl = null);
            if (ArticleHelper.UpdateArticle(article))
            {
                btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["articleId"], out articleId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnAddArticle.Click += new EventHandler(btnAddArticle_Click);
                btnPicDelete.Click += new EventHandler(btnPicDelete_Click);
                if (!Page.IsPostBack)
                {
                    dropArticleCategory.DataBind();
                    ArticleInfo article = ArticleHelper.GetArticle(articleId);
                    if (article == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(article, false);
                        txtArticleTitle.Text = article.Title;
                        txtMetaDescription.Text = article.MetaDescription;
                        txtMetaKeywords.Text = article.MetaKeywords;
                        imgPic.ImageUrl = article.IconUrl;
                        txtShortDesc.Text = article.Description;
                        fcContent.Text = article.Content;
                        dropArticleCategory.SelectedValue = new int?(article.CategoryId);
                        btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                        ckrrelease.Checked = article.IsRelease;
                    }
                }
            }
        }
    }
}

