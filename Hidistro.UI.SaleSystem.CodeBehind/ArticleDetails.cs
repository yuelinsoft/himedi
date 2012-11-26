namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    [ParseChildren(true)]
    public class ArticleDetails : HtmlTemplatedWebControl
    {
       HtmlAnchor aFront;
       HtmlAnchor aNext;
       Common_ArticleRelative ariticlative;
       int articleId;
       Label lblFront;
       Label lblFrontTitle;
       Label lblNext;
       Label lblNextTitle;
       FormatedTimeLabel litArticleAddedDate;
       Literal litArticleContent;
       Literal litArticleDescription;
       Literal litArticleTitle;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
            {
                base.GotoResourceNotFound();
            }
            this.litArticleAddedDate = (FormatedTimeLabel) this.FindControl("litArticleAddedDate");
            this.litArticleContent = (Literal) this.FindControl("litArticleContent");
            this.litArticleDescription = (Literal) this.FindControl("litArticleDescription");
            this.litArticleTitle = (Literal) this.FindControl("litArticleTitle");
            this.lblFront = (Label) this.FindControl("lblFront");
            this.lblNext = (Label) this.FindControl("lblNext");
            this.lblFrontTitle = (Label) this.FindControl("lblFrontTitle");
            this.lblNextTitle = (Label) this.FindControl("lblNextTitle");
            this.aFront = (HtmlAnchor) this.FindControl("front");
            this.aNext = (HtmlAnchor) this.FindControl("next");
            this.ariticlative = (Common_ArticleRelative) this.FindControl("list_Common_ArticleRelative");
            if (!this.Page.IsPostBack)
            {
                ArticleInfo article = CommentBrowser.GetArticle(this.articleId);
                if ((article != null) && article.IsRelease)
                {
                    PageTitle.AddTitle(article.Title, HiContext.Current.Context);
                    if (!string.IsNullOrEmpty(article.MetaKeywords))
                    {
                        MetaTags.AddMetaKeywords(article.MetaKeywords, HiContext.Current.Context);
                    }
                    if (!string.IsNullOrEmpty(article.MetaDescription))
                    {
                        MetaTags.AddMetaDescription(article.MetaDescription, HiContext.Current.Context);
                    }
                    this.litArticleTitle.Text = article.Title;
                    this.litArticleDescription.Text = article.Description;
                    string str = HiContext.Current.HostPath + Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { this.articleId });
                    this.litArticleContent.Text = article.Content.Replace("href=\"#\"", "href=\"" + str + "\"");
                    this.litArticleAddedDate.Time = article.AddedDate;
                    ArticleInfo info2 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Front", article.CategoryId);
                    if ((info2 != null) && (info2.ArticleId > 0))
                    {
                        if (this.lblFront != null)
                        {
                            this.lblFront.Visible = true;
                            this.aFront.HRef = Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { info2.ArticleId });
                            this.lblFrontTitle.Text = info2.Title;
                        }
                    }
                    else if (this.lblFront != null)
                    {
                        this.lblFront.Visible = false;
                    }
                    ArticleInfo info3 = CommentBrowser.GetFrontOrNextArticle(this.articleId, "Next", article.CategoryId);
                    if ((info3 != null) && (info3.ArticleId > 0))
                    {
                        if (this.lblNext != null)
                        {
                            this.lblNext.Visible = true;
                            this.aNext.HRef = Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[] { info3.ArticleId });
                            this.lblNextTitle.Text = info3.Title;
                        }
                    }
                    else if (this.lblNext != null)
                    {
                        this.lblNext.Visible = false;
                    }
                    DataSet articlProductList = CommentBrowser.GetArticlProductList(this.articleId);
                    if (((articlProductList == null) || (articlProductList.Tables.Count <= 0)) || (articlProductList.Tables[0].Rows.Count <= 0))
                    {
                        this.BindList();
                    }
                    else
                    {
                        this.ariticlative.DataSource = articlProductList.Tables[0];
                        this.ariticlative.DataBind();
                    }
                }
            }
        }

       void BindList()
        {
            int num = 2;
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            XmlNode node = ProductBrowser.GetProductSubjectDocument().SelectSingleNode("root/Subject[SubjectId='" + num + "']");
            if (node != null)
            {
                query.ProductType = (SubjectType) int.Parse(node.SelectSingleNode("Type").InnerText);
                query.CategoryIds = node.SelectSingleNode("Categories").InnerText;
                string innerText = node.SelectSingleNode("MaxPrice").InnerText;
                if (!string.IsNullOrEmpty(innerText))
                {
                    int result = 0;
                    if (int.TryParse(innerText, out result))
                    {
                        query.MaxPrice = new decimal?(result);
                    }
                }
                string str2 = node.SelectSingleNode("MinPrice").InnerText;
                if (!string.IsNullOrEmpty(str2))
                {
                    int num3 = 0;
                    if (int.TryParse(str2, out num3))
                    {
                        query.MinPrice = new decimal?(num3);
                    }
                }
                query.Keywords = node.SelectSingleNode("Keywords").InnerText;
                query.MaxNum = int.Parse(node.SelectSingleNode("MaxNum").InnerText);
            }
            this.ariticlative.DataSource = ProductBrowser.GetSubjectList(query);
            this.ariticlative.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ArticleDetails.html";
            }
            base.OnInit(e);
        }
    }
}

