namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.IO;
    using System.Web.UI;

    [ParseChildren(true)]
    public class SubCategory : SearchTemplatedWebControl
    {
       int categoryId;
       Common_CategoryLeadBuy litLeadBuy;

        public SubCategory()
        {
            int.TryParse(this.Page.Request.QueryString["CategoryId"], out this.categoryId);
            CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
            if (category != null)
            {
                if (category.Depth == 1)
                {
                    if (!string.IsNullOrEmpty(category.Theme) && File.Exists(HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/categorythemes/" + category.Theme)))
                    {
                        this.SkinName = "/categorythemes/" + category.Theme;
                    }
                }
                else
                {
                    category = CategoryBrowser.GetCategory(category.TopCategoryId);
                    if (((category != null) && !string.IsNullOrEmpty(category.Theme)) && File.Exists(HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/categorythemes/" + category.Theme)))
                    {
                        this.SkinName = "/categorythemes/" + category.Theme;
                    }
                }
            }
        }

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId))
            {
                base.GotoResourceNotFound();
            }
            this.litLeadBuy = (Common_CategoryLeadBuy) this.FindControl("Common_CategoryLeadBuy");
            if (!this.Page.IsPostBack)
            {
                CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
                if (category != null)
                {
                    if (!string.IsNullOrEmpty(category.MetaKeywords))
                    {
                        MetaTags.AddMetaKeywords(category.MetaKeywords, HiContext.Current.Context);
                    }
                    if (!string.IsNullOrEmpty(category.MetaDescription))
                    {
                        MetaTags.AddMetaDescription(category.MetaDescription, HiContext.Current.Context);
                    }
                    if (!string.IsNullOrEmpty(category.Name))
                    {
                        PageTitle.AddTitle(category.MetaTitle, HiContext.Current.Context);
                    }
                    if (this.litLeadBuy != null)
                    {
                        this.litLeadBuy.Text = category.Notes1;
                    }
                }
                this.LoadPageSearch(category);
            }
            base.AttachChildControls();
        }

       void LoadPageSearch(CategoryInfo category)
        {
            if (!string.IsNullOrEmpty(category.MetaKeywords))
            {
                MetaTags.AddMetaKeywords(category.MetaKeywords, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(category.MetaDescription))
            {
                MetaTags.AddMetaDescription(category.MetaDescription, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(category.MetaTitle))
            {
                PageTitle.AddTitle(category.MetaTitle, HiContext.Current.Context);
            }
            else
            {
                PageTitle.AddTitle(category.Name, HiContext.Current.Context);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-SubCategory.html";
            }
            base.OnInit(e);
        }
    }
}

