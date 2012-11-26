namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class Brand : HtmlTemplatedWebControl
    {
       Common_Search_SortPopularity btnSortPopularity;
       Common_Search_SortPrice btnSortPrice;
       Common_Search_SortSaleCounts btnSortSaleCounts;
       Common_Search_SortTime btnSortTime;
       Common_CutdownSearch cutdownSearch;
       Literal litBrandProductResult;
       Pager pager;
       ThemedTemplatedRepeater rptProduct;

        protected override void AttachChildControls()
        {
            this.rptProduct = (ThemedTemplatedRepeater) this.FindControl("rptProduct");
            this.pager = (Pager) this.FindControl("pager");
            this.litBrandProductResult = (Literal) this.FindControl("litBrandProductResult");
            this.cutdownSearch = (Common_CutdownSearch) this.FindControl("search_Common_CutdownSearch");
            this.btnSortPrice = (Common_Search_SortPrice) this.FindControl("btn_Common_Search_SortPrice");
            this.btnSortTime = (Common_Search_SortTime) this.FindControl("btn_Common_Search_SortTime");
            this.btnSortPopularity = (Common_Search_SortPopularity) this.FindControl("btn_Common_Search_SortPopularity");
            this.btnSortSaleCounts = (Common_Search_SortSaleCounts) this.FindControl("btn_Common_Search_SortSaleCounts");
            this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
            this.btnSortPrice.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortPrice_Sorting);
            this.btnSortTime.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortTime_Sorting);
            if (this.btnSortPopularity != null)
            {
                this.btnSortPopularity.Sorting += new Common_Search_SortPopularity.SortingHandler(this.btnSortPopularity_Sorting);
            }
            if (this.btnSortSaleCounts != null)
            {
                this.btnSortSaleCounts.Sorting += new Common_Search_SortSaleCounts.SortingHandler(this.btnSortSaleCounts_Sorting);
            }
            if (!this.Page.IsPostBack)
            {
                this.BindBrandProduct();
            }
        }

       void BindBrandProduct()
        {
            DbQueryResult browseProductList = ProductBrowser.GetBrowseProductList(this.GetProductBrowseQuery());
            this.rptProduct.DataSource = browseProductList.Data;
            this.rptProduct.DataBind();
            this.pager.TotalRecords = (int) (browseProductList.TotalRecords * (Convert.ToDouble(this.pager.PageSize) / 20.0));
            int num = 0;
            if ((Convert.ToDouble(browseProductList.TotalRecords) % 20.0) > 0.0)
            {
                num = (browseProductList.TotalRecords / 20) + 1;
            }
            else
            {
                num = browseProductList.TotalRecords / 20;
            }
            this.litBrandProductResult.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[] { browseProductList.TotalRecords, 20, num, this.pager.PageIndex });
        }

       void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

       void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

       void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

       void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadBrand(sortOrder, sortOrderBy);
        }

       void cutdownSearch_ReSearch(object sender, EventArgs e)
        {
            this.ReloadBrand(string.Empty, string.Empty);
        }

       ProductBrowseQuery GetProductBrowseQuery()
        {
            ProductBrowseQuery entity = new ProductBrowseQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                entity.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
            {
                decimal num = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out num))
                {
                    entity.MinSalePrice = new decimal?(num);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
            {
                decimal num2 = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out num2))
                {
                    entity.MaxSalePrice = new decimal?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                entity.ProductCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
            }
            entity.PageIndex = this.pager.PageIndex;
            int result = 20;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageSize"]) && int.TryParse(this.Page.Request.QueryString["pageSize"], out result))
            {
                entity.PageSize = result;
            }
            else
            {
                entity.PageSize = 20;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
            {
                entity.SortBy = this.Page.Request.QueryString["sortOrderBy"];
            }
            else
            {
                entity.SortBy = "DisplaySequence";
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                entity.SortOrder = (SortAction) Enum.Parse(typeof(SortAction), this.Page.Request.QueryString["sortOrder"]);
            }
            else
            {
                entity.SortOrder = SortAction.Desc;
            }
            Globals.EntityCoding(entity, true);
            return entity;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Brand.html";
            }
            base.OnInit(e);
        }

       void ReloadBrand(string sortOrder, string sortOrderBy)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("keywords", Globals.UrlEncode(this.cutdownSearch.Item.Keywords));
            queryStrings.Add("minSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString()));
            queryStrings.Add("maxSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString()));
            queryStrings.Add("productCode", Globals.UrlEncode(this.cutdownSearch.Item.ProductCode));
            queryStrings.Add("pageIndex", this.pager.PageIndex.ToString());
            queryStrings.Add("pageSize", "20");
            queryStrings.Add("sortOrderBy", sortOrderBy);
            queryStrings.Add("sortOrder", sortOrder);
            base.ReloadPage(queryStrings);
        }
    }
}

