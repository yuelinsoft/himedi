namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_GoodMoreRankPriceLink : HyperLink
    {
       int? categoryId;
       decimal? maxPrice;
       decimal? minPrice;

        protected override void Render(HtmlTextWriter writer)
        {
            string str = Globals.GetSiteUrls().UrlData.FormatUrl("SearchResult") + "?";
            string str2 = string.Empty;
            if (this.categoryId.HasValue)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + "&";
                }
                str2 = str2 + "categoryId=" + this.categoryId;
            }
            if (this.MinPrice.HasValue)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + "&";
                }
                str2 = str2 + "minSalePrice=" + this.MinPrice;
            }
            if (this.MaxPrice.HasValue)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + "&";
                }
                str2 = str2 + "&maxSalePrice=" + this.MaxPrice;
            }
            base.NavigateUrl = str + str2;
            base.Render(writer);
        }

        public int? CategoryId
        {
            get
            {
                return this.categoryId;
            }
            set
            {
                this.categoryId = value;
            }
        }

        public decimal? MaxPrice
        {
            get
            {
                return this.maxPrice;
            }
            set
            {
                this.maxPrice = value;
            }
        }

        public decimal? MinPrice
        {
            get
            {
                return this.minPrice;
            }
            set
            {
                this.minPrice = value;
            }
        }
    }
}

