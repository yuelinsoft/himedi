using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SubjectProducts)]
    public partial class SubjectProducts : AdminPage
    {

        int? categoryId;
        string keywords;
        SubjectType subjectType = SubjectType.Latest;

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

        private void BindSubjectProducts()
        {
            Pagination page = new Pagination();
            page.PageSize = 10;
            page.PageIndex = pagerSubject.PageIndex;
            page.SortOrder = SortAction.Desc;
            page.SortBy = "DisplaySequence";
            DbQueryResult subjectProducts = ProductHelper.GetSubjectProducts(subjectType, page);
            dlstSearchProducts.DataSource = subjectProducts.Data;
            dlstSearchProducts.DataBind();
            pagerSubject.TotalRecords = subjectProducts.TotalRecords;
        }

        private void btnAddSearch_Click(object sender, EventArgs e)
        {
            ProductQuery query = new ProductQuery();

            query.Keywords = keywords;
            query.CategoryId = categoryId;
            query.SaleStatus = ProductSaleStatus.OnSale;

            IList<int> productIds = ProductHelper.GetProductIds(query);
            IList<int> subjectProductIds = ProductHelper.GetSubjectProductIds(subjectType);

            IList<int> idsList = new List<int>();
            foreach (int pid in productIds)
            {
                if (!subjectProductIds.Contains(pid))
                {
                    idsList.Add(pid);
                }
            }

            if ((idsList.Count > 0) && ProductHelper.AddSubjectProducts(subjectType, idsList))
            {
                Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ProductHelper.ClearSubjectProducts(subjectType);
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBindPage(true);
        }

        private void dlstProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                int item = int.Parse(dlstProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
                if (!(ProductHelper.GetSubjectProductIds(subjectType).Contains(item) || !ProductHelper.AddSubjectProduct(subjectType, item)))
                {
                    Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
                }
            }
        }

        private void dlstSearchProducts_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            int productId = int.Parse(dlstSearchProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
            if (ProductHelper.RemoveSubjectProduct(subjectType, productId))
            {
                Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Keywords"]))
            {
                keywords = Request.QueryString["Keywords"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["CategoryId"]))
            {
                int result = 0;
                if (int.TryParse(Request.QueryString["CategoryId"], out result))
                {
                    categoryId = new int?(result);
                }
            }
            if (!string.IsNullOrEmpty(Request.QueryString["subjectType"]))
            {
                int num2 = 0;
                if (int.TryParse(Request.QueryString["subjectType"], out num2))
                {
                    subjectType = (SubjectType)num2;
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
                BindSubjectProducts();
            }
        }

        private void ReBindPage(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(Request.QueryString["subjectType"]))
            {
                queryStrings.Add("subjectType", Request.QueryString["subjectType"]);
            }
            else
            {
                queryStrings.Add("subjectType", 4.ToString());
            }
            queryStrings.Add("Keywords", txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", dropCategories.SelectedValue.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("pageIndex1", pagerSubject.PageIndex.ToString());
            ReloadPage(queryStrings);
        }
    }
}

