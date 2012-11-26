using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.Products)]
    public partial class MakeTaobaoProducts : AdminPage
    {
        int? categoryId;
        DateTime? endDate;
        int? isPub = -1;
        int? lineId;
        string productCode;
        string productName;
        DateTime? startDate;


        private void BindProducts()
        {
            LoadParameters();
            ProductQuery entity = new ProductQuery();
            entity.IsMakeTaobao = isPub;
            entity.Keywords = productName;
            entity.ProductCode = productCode;
            entity.CategoryId = categoryId;
            entity.ProductLineId = lineId;
            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.SaleStatus = ProductSaleStatus.OnSale;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            entity.StartDate = startDate;
            entity.EndDate = endDate;
            if (categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            grdProducts.DataSource = products.Data;
            grdProducts.DataBind();
            txtSearchText.Text = entity.Keywords;
            txtSKU.Text = entity.ProductCode;
            dropCategories.SelectedValue = entity.CategoryId;
            dropLines.SelectedValue = entity.ProductLineId;
            dpispub.SelectedValue = entity.IsMakeTaobao.ToString();
            pager1.TotalRecords = pager.TotalRecords = products.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadProductOnSales(true);
        }

        protected void dpispub_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadProductOnSales(true);
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
            {
                productName = Globals.UrlDecode(Page.Request.QueryString["productName"]);
            }
            int result = -1;
            if (int.TryParse(Page.Request.QueryString["ismaketaobao"], out result))
            {
                isPub = new int?(result);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
            {
                productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
            }
            int num2 = 0;
            if (int.TryParse(Page.Request.QueryString["categoryId"], out num2))
            {
                categoryId = new int?(num2);
            }
            int num3 = 0;
            if (int.TryParse(Page.Request.QueryString["lineId"], out num3))
            {
                lineId = new int?(num3);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["startDate"]))
            {
                startDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["endDate"]))
            {
                endDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["endDate"]));
            }
            txtSearchText.Text = productName;
            txtSKU.Text = productCode;
            dropCategories.DataBind();
            dropCategories.SelectedValue = categoryId;
            dropLines.DataBind();
            dropLines.SelectedValue = lineId;
            calendarStartDate.SelectedDate = startDate;
            calendarEndDate.SelectedDate = endDate;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropLines.DataBind();
                BindProducts();
            }
        }

        private void ReloadProductOnSales(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", Globals.UrlEncode(txtSearchText.Text.Trim()));
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", dropCategories.SelectedValue.ToString());
            }
            if (dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", dropLines.SelectedValue.ToString());
            }
            if (!string.IsNullOrEmpty(dpispub.SelectedValue))
            {
                queryStrings.Add("ismaketaobao", dpispub.SelectedValue.ToString());
            }
            queryStrings.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(txtSKU.Text.Trim())));
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            if (calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", calendarStartDate.SelectedDate.Value.ToString());
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", calendarEndDate.SelectedDate.Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

