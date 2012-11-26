namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;

    public class Common_ProductSubCategories : ThemedTemplatedRepeater
    {
       int categoryId;

        protected override void OnLoad(EventArgs e)
        {
            IList<CategoryInfo> mainCategories;
            int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId);
            if (this.categoryId > 0)
            {
                mainCategories = CategoryBrowser.SearchCategories(this.categoryId, null);
                if ((mainCategories == null) || (mainCategories.Count == 0))
                {
                    CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
                    if ((category != null) && category.ParentCategoryId.HasValue)
                    {
                        mainCategories = CategoryBrowser.SearchCategories(category.ParentCategoryId.Value, null);
                    }
                }
            }
            else
            {
                mainCategories = CategoryBrowser.GetMainCategories();
            }
            base.DataSource = mainCategories;
            base.DataBind();
        }
    }
}

