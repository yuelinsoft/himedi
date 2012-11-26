namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class Common_BrandCategoryList : ThemedTemplatedRepeater
    {
       int maxNum = 20;

        protected override void OnLoad(EventArgs e)
        {
            base.DataSource = CategoryBrowser.GetBrandCategories(this.MaxNum);
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

