using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MySubjectProducts : DistributorPage
    {
        int? categoryId;
        string keywords;
        SubjectType subjectType = SubjectType.Latest;


        private void BindProducts()
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = keywords;
            query.CategoryId = categoryId;
            ;
            if (categoryId.HasValue)
            {
                query.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(categoryId.Value).Path;
            }
            query.PageSize = 10;
            query.PageIndex = pager.PageIndex;
            query.SaleStatus = ProductSaleStatus.OnSale;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            DbQueryResult products = SubSiteProducthelper.GetProducts(query);
            dlstProducts.DataSource = products.Data;
            dlstProducts.DataBind();
            pager.TotalRecords = products.TotalRecords;
        }

        private void BindSubjectProducts()
        {
            Pagination pagination2 = new Pagination();
            pagination2.PageSize = 10;
            pagination2.PageIndex = pagerSubject.PageIndex;
            pagination2.SortOrder = SortAction.Desc;
            pagination2.SortBy = "DisplaySequence";
            Pagination page = pagination2;
            DbQueryResult subjectProducts = SubSiteProducthelper.GetSubjectProducts(subjectType, page);
            dlstSearchProducts.DataSource = subjectProducts.Data;
            dlstSearchProducts.DataBind();
            pagerSubject.TotalRecords = subjectProducts.TotalRecords;
        }

        private void btnAddSearch_Click(object sender, EventArgs e)
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = txtSearchText.Text;
            query.CategoryId = categoryId;
            query.SaleStatus = ProductSaleStatus.OnSale;
            IList<int> productIds = SubSiteProducthelper.GetProductIds(query);
            if (SubSiteProducthelper.AddSubjectProducts(subjectType, productIds))
            {
                BindSubjectProducts();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SubSiteProducthelper.ClearSubjectProducts(subjectType);
            BindSubjectProducts();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBindPage(true);
        }

        private void dlstProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                int productId = int.Parse(dlstProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
                if (SubSiteProducthelper.AddSubjectProduct(subjectType, productId))
                {
                    BindSubjectProducts();
                }
            }
        }

        private void dlstSearchProducts_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            int productId = int.Parse(dlstSearchProducts.DataKeys[e.Item.ItemIndex].ToString(), NumberStyles.None);
            if (SubSiteProducthelper.RemoveSubjectProduct(subjectType, productId))
            {
                BindSubjectProducts();
            }
        }

        private void LoadParameters()
        {
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
            if (!string.IsNullOrEmpty(base.Request.QueryString["subjectType"]))
            {
                int num2 = 0;
                if (int.TryParse(base.Request.QueryString["subjectType"], out num2))
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
                dropCategories.SelectedValue = categoryId;
                txtSearchText.Text = keywords;
                BindProducts();
                BindSubjectProducts();
            }
        }

        private void ReBindPage(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(base.Request.QueryString["subjectType"]))
            {
                queryStrings.Add("subjectType", base.Request.QueryString["subjectType"]);
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
            base.ReloadPage(queryStrings);
        }
    }
}

