using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyProducts : DistributorPage
    {

        int? categoryId;
        string productCode;
        string productName;

        private void BindProducts()
        {
            ProductQuery entity = new ProductQuery();
            entity.Keywords = productName;
            entity.ProductCode = productCode;
            entity.PageIndex = pager.PageIndex;
            entity.PageSize = pager.PageSize;
            entity.CategoryId = categoryId;
            if (categoryId.HasValue)
            {
                entity.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(categoryId.Value).Path;
            }
            entity.SaleStatus = ProductSaleStatus.OnSale;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult products = SubSiteProducthelper.GetProducts(entity);
            grdProducts.DataSource = products.Data;
            grdProducts.DataBind();
            pager.TotalRecords = products.TotalRecords;
            pager1.TotalRecords = products.TotalRecords;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要删除的商品", false);
            }
            else if (SubSiteProducthelper.DeleteProducts(str) > 0)
            {
                ShowMsg("成功删除了选择的商品", true);
                ReBindProducts();
            }
            else
            {
                ShowMsg("删除商品失败，未知错误", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(string.Concat(new object[] { Globals.ApplicationPath, "/Shopadmin/product/MyProducts.aspx?productName=", Globals.UrlEncode(txtSearchText.Text), "&categoryId=", dropCategories.SelectedValue, "&productCode=", Globals.UrlEncode(txtSKU.Text), "&pageSize=", pager.PageSize }));
        }

        private void grdProducts_ReBindData(object sender)
        {
            BindProducts();
        }

        private void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SubSiteProducthelper.DeleteProducts(grdProducts.DataKeys[e.RowIndex].Value.ToString());
            BindProducts();
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
                {
                    productName = Globals.UrlDecode(Page.Request.QueryString["productName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
                {
                    productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["categoryId"]))
                {
                    int result = 0;
                    if (int.TryParse(Page.Request.QueryString["categoryId"], out result))
                    {
                        categoryId = new int?(result);
                    }
                }
                txtSearchText.Text = productName;
                txtSKU.Text = productCode;
                dropCategories.DataBind();
                dropCategories.SelectedValue = categoryId;
            }
            else
            {
                productName = txtSearchText.Text;
                productCode = txtSKU.Text;
                categoryId = dropCategories.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdProducts.ReBindData += new Grid.ReBindDataEventHandler(grdProducts_ReBindData);
            grdProducts.RowDeleting += new GridViewDeleteEventHandler(grdProducts_RowDeleting);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["SortOrder"]))
                {
                    grdProducts.SortOrder = Page.Request.QueryString["SortOrder"];
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["SortOrderBy"]))
                {
                    grdProducts.SortOrderBy = Page.Request.QueryString["SortOrderBy"];
                }
                BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBindProducts()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", txtSearchText.Text);
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("productCode", txtSKU.Text);
            queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }

        private IList<int> SelectedProducts
        {
            get
            {
                IList<int> list = new List<int>();
                foreach (GridViewRow row in grdProducts.Rows)
                {
                    CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                    if (box.Checked)
                    {
                        int item = (int)grdProducts.DataKeys[row.RowIndex].Value;
                        list.Add(item);
                    }
                }
                return list;
            }
        }
    }
}

