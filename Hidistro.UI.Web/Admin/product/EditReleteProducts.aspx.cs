using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public partial class EditReleteProducts : AdminPage
    {
        int? categoryId;
        string keywords;
        int productId;


        private void BindProducts()
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = keywords;
            query.CategoryId = categoryId;
            if (categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(categoryId.Value).Path;
            }
            query.PageSize = 10;
            query.PageIndex = pager.PageIndex;
            query.SaleStatus = ProductSaleStatus.OnSale;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            DbQueryResult products = ProductHelper.GetProducts(query);
            dlstProducts.DataSource = products.Data;
            dlstProducts.DataBind();
            pager.TotalRecords = products.TotalRecords;
        }

        private void BindRelatedProducts()
        {
            Pagination page = new Pagination();
            page.PageSize = 10;
            page.PageIndex = pagerSubject.PageIndex;
            page.SortOrder = SortAction.Desc;
            page.SortBy = "DisplaySequence";
            DbQueryResult relatedProducts = ProductHelper.GetRelatedProducts(page, productId);
            dlstSearchProducts.DataSource = relatedProducts.Data;
            dlstSearchProducts.DataBind();
            pagerSubject.TotalRecords = relatedProducts.TotalRecords;
        }

        private void btnAddSearch_Click(object sender, EventArgs e)
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = keywords;
            query.CategoryId = categoryId;
            query.SaleStatus = ProductSaleStatus.OnSale;
            foreach (int num in ProductHelper.GetProductIds(query))
            {
                if (productId != num)
                {
                    ProductHelper.AddRelatedProduct(productId, num);
                }
            }
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ProductHelper.ClearRelatedProducts(productId);
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBindPage(true);
        }

        private void dlstProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                int relatedProductId = int.Parse(dlstProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
                if (productId != relatedProductId)
                {
                    ProductHelper.AddRelatedProduct(productId, relatedProductId);
                }
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        private void dlstSearchProducts_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            int relatedProductId = int.Parse(dlstSearchProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
            ProductHelper.RemoveRelatedProduct(productId, relatedProductId);
            base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void LoadParameters()
        {
            int.TryParse(base.Request.QueryString["productId"], out productId);
            if (!string.IsNullOrEmpty(base.Request.QueryString["Keywords"]))
            {
                keywords = base.Request.QueryString["Keywords"];
            }
            if (!string.IsNullOrEmpty(base.Request.QueryString["CategoryId"]))
            {
                int result = 0;
                if (int.TryParse(base.Request.QueryString["CategoryId"], out result))
                {
                    categoryId = new int?(result);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
            dlstProducts.ItemCommand += new DataListCommandEventHandler(dlstProducts_ItemCommand);
            dlstSearchProducts.DeleteCommand += new DataListCommandEventHandler(dlstSearchProducts_DeleteCommand);
            btnAddSearch.Click += new EventHandler(btnAddSearch_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                BindProducts();
                BindRelatedProducts();
            }
        }

        private void ReBindPage(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productId", productId.ToString());
            queryStrings.Add("Keywords", txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", dropCategories.SelectedValue.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("pageIndex1", pagerSubject.PageIndex.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

