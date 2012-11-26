using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class ProductAlert : AdminPage
    {
        int? brandId;
        int? lineId;
        int? categoryId;
        DateTime? endDate;
        string productCode;
        ProductSaleStatus salestaus;
        string searchkey;
        DateTime? startDate;

        private void BindProducts()
        {
            LoadParameters();
            ProductQuery query = new ProductQuery();
            query.Keywords = searchkey;
            query.ProductCode = productCode;
            query.SaleStatus = salestaus;
            query.PageSize = pager.PageSize;
            query.CategoryId = categoryId;
            query.StartDate = startDate;
            query.EndDate = endDate;
            query.ProductLineId = lineId;
            query.BrandId = brandId;
            query.PageIndex = pager.PageIndex;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            if (categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(query.CategoryId.Value).Path;
            }
            DbQueryResult alertProducts = ProductHelper.GetAlertProducts(query);
            grdProducts.DataSource = alertProducts.Data;
            grdProducts.DataBind();
            pager1.TotalRecords = pager.TotalRecords = alertProducts.TotalRecords;
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            LoadParameters();
            ProductQuery query2 = new ProductQuery();
            query2.Keywords = searchkey;
            query2.ProductCode = productCode;
            query2.SaleStatus = salestaus;
            query2.PageSize = pager.PageSize;
            query2.CategoryId = categoryId;
            query2.StartDate = startDate;
            query2.EndDate = endDate;
            query2.ProductLineId = lineId;
            query2.BrandId = brandId;
            query2.PageIndex = pager.PageIndex;
            query2.SortOrder = SortAction.Desc;
            query2.SortBy = "DisplaySequence";
            ProductQuery query = query2;
            if (categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(query.CategoryId.Value).Path;
            }
            DbQueryResult alertProducts = ProductHelper.GetAlertProducts(query);
            DataTable data = new DataTable();
            if (alertProducts.Data != null)
            {
                data = (DataTable)alertProducts.Data;
            }
            string s = string.Empty + "排序号,商品名称,商家编码,库存,状态,市场价,成本价,一口价,采购价\r\n";
            foreach (DataRow row in data.Rows)
            {
                s = s + (data.Rows.IndexOf(row) + 1);
                s = s + "," + row["ProductName"].ToString();
                s = s + "," + row["ProductCode"].ToString();
                s = s + "," + row["Stock"].ToString();
                s = s + "," + ShowStatus(row["SaleStatus"]);
                if (!string.IsNullOrEmpty(row["MarketPrice"].ToString()))
                {
                    s = s + "," + decimal.Parse(row["MarketPrice"].ToString()).ToString("F2");
                }
                else
                {
                    s = s + ",0.00";
                }
                if (!string.IsNullOrEmpty(row["CostPrice"].ToString()))
                {
                    s = s + "," + decimal.Parse(row["CostPrice"].ToString()).ToString("F2");
                }
                else
                {
                    s = s + ",0.00";
                }
                if (!string.IsNullOrEmpty(row["SalePrice"].ToString()))
                {
                    s = s + "," + decimal.Parse(row["SalePrice"].ToString()).ToString("F2");
                }
                else
                {
                    s = s + ",0.00";
                }
                if (!string.IsNullOrEmpty(row["PurchasePrice"].ToString()))
                {
                    s = s + "," + decimal.Parse(row["PurchasePrice"].ToString()).ToString("F2") + "\r\n";
                }
                else
                {
                    s = s + ",0.00\r\n";
                }
            }
            object obj2 = s;
            s = string.Concat(new object[] { obj2, "总记录数：", alertProducts.TotalRecords, "条" });
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "GB2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=ProductAlert.csv");
            Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Page.Response.ContentType = "application/octet-stream";
            Page.EnableViewState = false;
            Page.Response.Write(s);
            Page.Response.End();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要删除的商品", false);
            }
            else
            {
                List<int> productIds = new List<int>();
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    productIds.Add(Convert.ToInt32(str2));
                }
                AdminPage.SendMessageToDistributors(str, 3);
                if (ProductHelper.CanclePenetrationProducts(productIds) >= 1)
                {
                    if (ProductHelper.RemoveProduct(str) > 0)
                    {
                        ShowMsg("成功删除了选择的商品", true);
                        ReloadProductAlert(false);
                    }
                    else
                    {
                        ShowMsg("删除商品失败，未知错误", false);
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要下架的商品", false);
            }
            else
            {
                if (hdPenetrationStatus.Value.Equals("1"))
                {
                    List<int> productIds = new List<int>();
                    foreach (string str2 in str.Split(new char[] { ',' }))
                    {
                        productIds.Add(Convert.ToInt32(str2));
                    }
                    AdminPage.SendMessageToDistributors(str, 1);
                    if (ProductHelper.CanclePenetrationProducts(productIds) == 0)
                    {
                        ShowMsg("取消铺货失败！", false);
                        return;
                    }
                }
                if (ProductHelper.OffShelf(str) > 0)
                {
                    ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                    ReloadProductAlert(false);
                }
                else
                {
                    ShowMsg("下架商品失败，未知错误", false);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadProductAlert(true);
        }

        private void btnStockPentrationStauts_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要入库的商品", false);
            }
            else
            {
                if (hdPenetrationStatus.Value.Equals("1"))
                {
                    List<int> productIds = new List<int>();
                    foreach (string str2 in str.Split(new char[] { ',' }))
                    {
                        productIds.Add(Convert.ToInt32(str2));
                    }
                    AdminPage.SendMessageToDistributors(str, 2);
                    if (ProductHelper.CanclePenetrationProducts(productIds) == 0)
                    {
                        ShowMsg("取消铺货失败！", false);
                        return;
                    }
                }
                if (ProductHelper.InStock(str) > 0)
                {
                    ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
                    BindProducts();
                }
                else
                {
                    ShowMsg("入库商品失败，未知错误", false);
                }
            }
        }

        protected void btnUpShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要上架的商品", false);
            }
            else if (ProductHelper.UpShelf(str) > 0)
            {
                ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
                ReloadProductAlert(false);
            }
            else
            {
                ShowMsg("上架商品失败，未知错误", false);
            }
        }

        private void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<int> productIds = new List<int>();
            string str = grdProducts.DataKeys[e.RowIndex].Value.ToString();
            if (str != "")
            {
                productIds.Add(Convert.ToInt32(str));
            }
            AdminPage.SendMessageToDistributors(str, 3);
            if ((ProductHelper.CanclePenetrationProducts(productIds) == 1) && (ProductHelper.RemoveProduct(str) > 0))
            {
                ShowMsg("删除商品成功", true);
                ReloadProductAlert(false);
            }
        }

        private void LoadParameters()
        {
            int result = 0;
            if (int.TryParse(Page.Request.QueryString["categoryId"], out result))
            {
                categoryId = new int?(result);
            }
            int num2 = 0;
            if (int.TryParse(Page.Request.QueryString["brandId"], out num2))
            {
                brandId = new int?(num2);
            }
            salestaus = ProductSaleStatus.All;
            int num3 = 0;
            if (int.TryParse(Page.Request.QueryString["salesatus"], out num3))
            {
                switch (num3)
                {
                    case 1:
                        salestaus = ProductSaleStatus.OnSale;
                        goto Label_00D9;

                    case 2:
                        salestaus = ProductSaleStatus.UnSale;
                        goto Label_00D9;
                }
                salestaus = ProductSaleStatus.OnStock;
            }
        Label_00D9:
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
            {
                productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
            }
            int num4 = 0;
            if (int.TryParse(Page.Request.QueryString["lineId"], out num4))
            {
                lineId = new int?(num4);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
            {
                searchkey = Globals.UrlDecode(Page.Request.QueryString["searchKey"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["startDate"]))
            {
                startDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["endDate"]))
            {
                endDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["endDate"]));
            }
            dropCategories.DataBind();
            dropCategories.SelectedValue = categoryId;
            dropLines.DataBind();
            dropLines.SelectedValue = lineId;
            dropBrandList.DataBind();
            dropBrandList.SelectedValue = brandId;
            dropproductsale.DataBind();
            if (salestaus != ProductSaleStatus.All)
            {
                dropproductsale.SelectedValue = new int?((int)salestaus);
            }
            txtSearchText.Text = searchkey;
            txtSKU.Text = productCode;
            calendarStartDate.SelectedDate = startDate;
            calendarEndDate.SelectedDate = endDate;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnUpShelf.Click += new EventHandler(btnUpShelf_Click);
            grdProducts.RowDeleting += new GridViewDeleteEventHandler(grdProducts_RowDeleting);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnStockPentrationStauts.Click += new EventHandler(btnStockPentrationStauts_Click);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
            if (!Page.IsPostBack)
            {
                BindProducts();
            }
        }

        private void ReloadProductAlert(bool isSearch)
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
            if (dropproductsale.SelectedValue.HasValue)
            {
                queryStrings.Add("salesatus", dropproductsale.SelectedValue.ToString());
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

        public string ShowStatus(object objstatu)
        {
            string str = "";
            if (objstatu == null)
            {
                return str;
            }
            switch (((int)objstatu))
            {
                case 1:
                    return "上架";

                case 2:
                    return "下架";

                case 3:
                    return "入库";
            }
            return "已删除";
        }
    }
}

