namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;

    public class HotKeywords : ThemedTemplatedRepeater
    {
       int categoryId;
       int maxNum = 10;
       int topCategoryId;

        protected override void OnLoad(EventArgs e)
        {
            int.TryParse(this.Page.Request.QueryString["CategoryId"], out this.categoryId);
            CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
            if (category != null)
            {
                this.topCategoryId = category.TopCategoryId;
            }
            base.DataSource = CommentBrowser.GetHotKeywords(this.topCategoryId, this.MaxNum);
            base.DataBind();
        }

        public int MaxNum
        {
            get
            {
                return this.maxNum;
            }
            set
            {
                this.maxNum = value;
            }
        }
    }
}

