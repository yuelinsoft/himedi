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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Products)]
    public partial class ProductOnDeleted : AdminPage
    {

        int? categoryId;
        DateTime? endDate;
        int? lineId;
        string productCode;
        string productName;
        DateTime? startDate;

        private void BindProducts()
        {
            LoadParameters();
            ProductQuery entity = new ProductQuery();
            entity.Keywords = productName;
            entity.ProductCode = productCode;
            entity.CategoryId = categoryId;
            entity.StartDate = startDate;
            entity.EndDate = endDate;
            entity.ProductLineId = lineId;
            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.SaleStatus = ProductSaleStatus.Delete;
            entity.SortOrder = SortAction.Desc;
            int? selectedValue = dropBrandList.SelectedValue;
            entity.BrandId = selectedValue.HasValue ? dropBrandList.SelectedValue : null;
            entity.SortBy = "DisplaySequence";
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
            pager1.TotalRecords = pager.TotalRecords = products.TotalRecords;
        }

        private void btnInStock_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要入库的商品", false);
            }
            else if (ProductHelper.InStock(str) > 0)
            {
                ShowMsg("成功入库了选择的商品，您可以在仓库里的商品里面找到入库以后的商品", true);
                BindProducts();
            }
            else
            {
                ShowMsg("入库商品失败，未知错误", false);
            }
        }

        private void btnOffShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要下架的商品", false);
            }
            else if (ProductHelper.OffShelf(str) > 0)
            {
                ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                BindProducts();
            }
            else
            {
                ShowMsg("下架商品失败，未知错误", false);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string str = currentProductId.Value;
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要删除的商品", false);
            }
            else if (ProductHelper.DeleteProduct(str, chkDeleteImage.Checked) > 0)
            {
                ShowMsg("成功的删除了商品", true);
                BindProducts();
            }
            else
            {
                ShowMsg("删除商品失败，未知错误", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadProductOnSales(true);
        }

        private void btnUpShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要上架的商品", false);
            }
            else if (ProductHelper.UpShelf(str) > 0)
            {
                ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                BindProducts();
            }
            else
            {
                ShowMsg("上架商品失败，未知错误", false);
            }
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
            {
                productName = Globals.UrlDecode(Page.Request.QueryString["productName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
            {
                productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
            }
            int result = 0;
            if (int.TryParse(Page.Request.QueryString["categoryId"], out result))
            {
                categoryId = new int?(result);
            }
            int num2 = 0;
            if (int.TryParse(Page.Request.QueryString["brandId"], out num2))
            {
                dropBrandList.SelectedValue = new int?(num2);
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
            btnUpShelf.Click += new EventHandler(btnUpShelf_Click);
            btnOffShelf.Click += new EventHandler(btnOffShelf_Click);
            btnInStock.Click += new EventHandler(btnInStock_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            if (!Page.IsPostBack)
            {
                dropBrandList.DataBind();
                dropCategories.DataBind();
                BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
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
            if (dropBrandList.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", dropBrandList.SelectedValue.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

