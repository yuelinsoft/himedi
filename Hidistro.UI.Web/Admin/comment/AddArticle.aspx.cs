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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Articles)]
    public partial class AddArticle : AdminPage
    {

        private void btnAddArticle_Click(object sender, EventArgs e)
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
            ArticleInfo target = new ArticleInfo();
            if (dropArticleCategory.SelectedValue.HasValue)
            {
                target.CategoryId = dropArticleCategory.SelectedValue.Value;
                target.Title = txtArticleTitle.Text.Trim();
                target.MetaDescription = txtMetaDescription.Text.Trim();
                target.MetaKeywords = txtMetaKeywords.Text.Trim();
                target.IconUrl = str;
                target.Description = txtShortDesc.Text.Trim();
                target.Content = fcContent.Text;
                target.AddedDate = DateTime.Now;
                target.IsRelease = ckrrelease.Checked;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ArticleInfo>(target, new string[] { "ValArticleInfo" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    ShowMsg(msg, false);
                }
                else if (ArticleHelper.CreateArticle(target))
                {
                    txtArticleTitle.Text = string.Empty;
                    txtShortDesc.Text = string.Empty;
                    fcContent.Text = string.Empty;
                    ShowMsg("成功添加了一篇文章", true);
                }
                else
                {
                    ShowMsg("添加文章错误", false);
                }
            }
            else
            {
                ShowMsg("请选择文章分类", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddArticle.Click += new EventHandler(btnAddArticle_Click);
            if (!Page.IsPostBack)
            {
                dropArticleCategory.DataBind();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["categoryId"]))
                {
                    int result = 0;
                    int.TryParse(Page.Request.QueryString["categoryId"], out result);
                    dropArticleCategory.SelectedValue = new int?(result);
                }
            }
        }
    }
}

