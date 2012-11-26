namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class ProductUnSales : HtmlTemplatedWebControl
    {
       IButton btnSearch;
       Literal litSearchResultPage;
       Pager pager;
       ThemedTemplatedRepeater rptProducts;
       HtmlInputText txtEndPrice;
       HtmlInputText txtKeywords;
       HtmlInputText txtSKU;
       HtmlInputText txtStartPrice;

        protected override void AttachChildControls()
        {
            this.rptProducts = (ThemedTemplatedRepeater) this.FindControl("rptProducts");
            this.pager = (Pager) this.FindControl("pager");
            this.litSearchResultPage = (Literal) this.FindControl("litSearchResultPage");
            this.txtSKU = (HtmlInputText) this.FindControl("txtSKU");
            this.txtKeywords = (HtmlInputText) this.FindControl("txtKeywords");
            this.txtStartPrice = (HtmlInputText) this.FindControl("txtStartPrice");
            this.txtEndPrice = (HtmlInputText) this.FindControl("txtEndPrice");
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!this.Page.IsPostBack)
            {
                string title = "商品下架区";
                PageTitle.AddTitle(title, HiContext.Current.Context);
                this.BindProducts(this.GetProductBrowseQuery());
            }
        }

        protected void BindProducts(ProductBrowseQuery query)
        {
            DbQueryResult unSaleProductList = ProductBrowser.GetUnSaleProductList(query);
            this.rptProducts.DataSource = unSaleProductList.Data;
            this.rptProducts.DataBind();
            int totalRecords = unSaleProductList.TotalRecords;
            this.pager.TotalRecords = totalRecords;
            int num2 = 0;
            if ((totalRecords % this.pager.PageSize) > 0)
            {
                num2 = (totalRecords / this.pager.PageSize) + 1;
            }
            else
            {
                num2 = totalRecords / this.pager.PageSize;
            }
            this.litSearchResultPage.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[] { totalRecords, this.pager.PageSize, num2, this.pager.PageIndex });
        }

       void btnSearch_Click(object sender, EventArgs e)
        {
            decimal num;
            decimal num2;
            string str = "ProductUnSales.aspx?";
            string str2 = string.Empty;
            if (!string.IsNullOrEmpty(this.txtKeywords.Value.Trim()))
            {
                str2 = str2 + "keywords=" + Globals.UrlEncode(this.txtKeywords.Value.Trim().Replace("'", "").Replace("\"", "")) + "&";
            }
            if (!string.IsNullOrEmpty(this.txtStartPrice.Value.Trim()) && decimal.TryParse(this.txtStartPrice.Value.Trim(), out num))
            {
                object obj2 = str2;
                str2 = string.Concat(new object[] { obj2, "minSalePrice=", num, "&" });
            }
            if (!string.IsNullOrEmpty(this.txtEndPrice.Value.Trim()) && decimal.TryParse(this.txtEndPrice.Value.Trim(), out num2))
            {
                object obj3 = str2;
                str2 = string.Concat(new object[] { obj3, "&maxSalePrice=", num2, "&" });
            }
            if (!string.IsNullOrEmpty(this.txtSKU.Value))
            {
                str2 = str2 + "&productCode=" + Globals.UrlEncode(this.txtSKU.Value);
            }
            if (string.IsNullOrEmpty(str2))
            {
                str = "ProductUnSales.aspx";
            }
            this.Page.Response.Redirect(str + str2);
        }

        protected ProductBrowseQuery GetProductBrowseQuery()
        {
            ProductBrowseQuery entity = new ProductBrowseQuery();
            entity.PageIndex = this.pager.PageIndex;
            entity.PageSize = this.pager.PageSize;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
            {
                entity.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
                this.txtKeywords.Value = entity.Keywords;
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
            {
                decimal result = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out result))
                {
                    entity.MinSalePrice = new decimal?(result);
                    this.txtStartPrice.Value = result.ToString();
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
            {
                decimal num2 = 0M;
                if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out num2))
                {
                    entity.MaxSalePrice = new decimal?(num2);
                    this.txtEndPrice.Value = num2.ToString();
                }
            }
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
                entity.ProductCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
                this.txtSKU.Value = entity.ProductCode;
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
                this.SkinName = "Skin-ProductUnSales.html";
            }
            base.OnInit(e);
        }

       void SearchProducts()
        {
            decimal num;
            decimal num2;
            ProductBrowseQuery query = new ProductBrowseQuery();
            if (!string.IsNullOrEmpty(this.txtKeywords.Value.Trim()))
            {
                query.Keywords = this.txtKeywords.Value.Trim().Replace("'", "").Replace("\"", "");
            }
            if (!string.IsNullOrEmpty(this.txtStartPrice.Value.Trim()) && decimal.TryParse(this.txtStartPrice.Value.Trim(), out num))
            {
                query.MinSalePrice = new decimal?(num);
            }
            if (!string.IsNullOrEmpty(this.txtEndPrice.Value.Trim()) && decimal.TryParse(this.txtEndPrice.Value.Trim(), out num2))
            {
                query.MaxSalePrice = new decimal?(num2);
            }
            if (!string.IsNullOrEmpty(this.txtSKU.Value.Trim()))
            {
                query.ProductCode = Globals.UrlEncode(this.txtSKU.Value);
            }
            query.PageIndex = 1;
            query.PageSize = 10;
            this.BindProducts(query);
        }
    }
}

