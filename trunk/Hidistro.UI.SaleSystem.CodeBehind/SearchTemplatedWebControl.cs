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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public class SearchTemplatedWebControl : HtmlTemplatedWebControl
    {
       Common_Search_SortPopularity btnSortPopularity;
       Common_Search_SortPrice btnSortPrice;
       Common_Search_SortSaleCounts btnSortSaleCounts;
       Common_Search_SortTime btnSortTime;
       Common_Location common_Location;
       Common_CutdownSearch cutdownSearch;
       Literal litSearchResultPage;
       Pager pager;
       Common_GoodsList_Search rptProducts;

        protected override void AttachChildControls()
        {
            this.common_Location = (Common_Location) this.FindControl("common_Location");
            this.rptProducts = (Common_GoodsList_Search) this.FindControl("list_Common_GoodsList_Search");
            this.pager = (Pager) this.FindControl("pager");
            this.litSearchResultPage = (Literal) this.FindControl("litSearchResultPage");
            this.btnSortPrice = (Common_Search_SortPrice) this.FindControl("btn_Common_Search_SortPrice");
            this.btnSortTime = (Common_Search_SortTime) this.FindControl("btn_Common_Search_SortTime");
            this.btnSortPopularity = (Common_Search_SortPopularity) this.FindControl("btn_Common_Search_SortPopularity");
            this.btnSortSaleCounts = (Common_Search_SortSaleCounts) this.FindControl("btn_Common_Search_SortSaleCounts");
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
            this.cutdownSearch = (Common_CutdownSearch) this.FindControl("search_Common_CutdownSearch");
            this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
            if (!this.Page.IsPostBack)
            {
                if (this.common_Location != null)
                {
                    int result = 0;
                    int.TryParse(this.Page.Request.QueryString["categoryId"], out result);
                    CategoryInfo category = CategoryBrowser.GetCategory(result);
                    if (category != null)
                    {
                        this.common_Location.CateGoryPath = category.Path;
                    }
                }
                this.BindSearch();
            }
        }

        protected void BindSearch()
        {
            ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
            DbQueryResult browseProductList = null;
            browseProductList = ProductBrowser.GetBrowseProductList(productBrowseQuery);
            this.rptProducts.DataSource = browseProductList.Data;
            this.rptProducts.DataBind();
            this.pager.TotalRecords = (int) (browseProductList.TotalRecords * (Convert.ToDouble(this.pager.PageSize) / ((double) this.rptProducts.PageSize)));
            int num = 0;
            if ((Convert.ToDouble(browseProductList.TotalRecords) % ((double) this.rptProducts.PageSize)) > 0.0)
            {
                num = (browseProductList.TotalRecords / this.rptProducts.PageSize) + 1;
            }
            else
            {
                num = browseProductList.TotalRecords / this.rptProducts.PageSize;
            }
            this.litSearchResultPage.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[] { browseProductList.TotalRecords, this.rptProducts.PageSize, num, this.pager.PageIndex });
        }

       void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadSearchResult(sortOrder, sortOrderBy);
        }

       void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadSearchResult(sortOrder, sortOrderBy);
        }

       void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadSearchResult(sortOrder, sortOrderBy);
        }

       void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
        {
            this.ReloadSearchResult(sortOrder, sortOrderBy);
        }

        protected void cutdownSearch_ReSearch(object sender, EventArgs e)
        {
            this.ReloadSearchResult(string.Empty, string.Empty);
        }

        protected ProductBrowseQuery GetProductBrowseQuery()
        {
            int num2;
            ProductBrowseQuery entity = new ProductBrowseQuery();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
            {
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["categoryId"], out result))
                {
                    entity.CategoryId = new int?(result);
                }
            }
            if (int.TryParse(this.Page.Request.QueryString["brand"], out num2))
            {
                entity.BrandId = new int?(num2);
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
            {
                IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
                string str = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]));
                string[] strArray = str.Split(new char[] { '-' });
                if (!string.IsNullOrEmpty(str))
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        string[] strArray2 = strArray[i].Split(new char[] { '_' });
                        if (((strArray2.Length > 0) && !string.IsNullOrEmpty(strArray2[1])) && (strArray2[1] != "0"))
                        {
                            AttributeValueInfo item = new AttributeValueInfo();
                            item.AttributeId = Convert.ToInt32(strArray2[0]);
                            item.ValueId = Convert.ToInt32(strArray2[1]);
                            list.Add(item);
                        }
                    }
                }
                entity.AttributeValues = list;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isPrecise"]))
            {
                entity.IsPrecise = bool.Parse(Globals.UrlDecode(this.Page.Request.QueryString["isPrecise"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                entity.Keywords = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
            {
                decimal num4 = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out num4))
                {
                    entity.MinSalePrice = new decimal?(num4);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
            {
                decimal num5 = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out num5))
                {
                    entity.MaxSalePrice = new decimal?(num5);
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                entity.ProductCode = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["productCode"]));
            }
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.rptProducts.PageSize;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
            {
                entity.SortBy = Globals.HtmlEncode(this.Page.Request.QueryString["sortOrderBy"]);
            }
            else
            {
                entity.SortBy = "DisplaySequence";
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
            {
                entity.SortOrder = (SortAction) Enum.Parse(typeof(SortAction), Globals.HtmlEncode(this.Page.Request.QueryString["sortOrder"]));
            }
            else
            {
                entity.SortOrder = SortAction.Desc;
            }
            Globals.EntityCoding(entity, true);
            return entity;
        }

        protected void ReloadSearchResult(string sortOrder, string sortOrderBy)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
            {
                queryStrings.Add("categoryId", Globals.UrlEncode(this.Page.Request.QueryString["categoryId"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["brand"]))
            {
                queryStrings.Add("brand", Globals.UrlEncode(this.Page.Request.QueryString["brand"]));
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
            {
                queryStrings.Add("valueStr", Globals.UrlEncode(this.Page.Request.QueryString["valueStr"]));
            }
            queryStrings.Add("keywords", Globals.UrlEncode(this.cutdownSearch.Item.Keywords));
            queryStrings.Add("minSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString()));
            queryStrings.Add("maxSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString()));
            queryStrings.Add("productCode", Globals.UrlEncode(this.cutdownSearch.Item.ProductCode));
            queryStrings.Add("pageIndex", "1");
            queryStrings.Add("pageSize", this.rptProducts.PageSize.ToString());
            queryStrings.Add("sortOrderBy", sortOrderBy);
            queryStrings.Add("sortOrder", sortOrder);
            base.ReloadPage(queryStrings);
        }
    }
}

