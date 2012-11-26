using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyProductAlert : DistributorPage
    {
        int? categoryId;
        string productCode;
        string productName;
        ProductSaleStatus productstatus = ProductSaleStatus.All;


        private void BindProducts()
        {
            ProductQuery entity = new ProductQuery();
            entity.Keywords = productName;
            entity.ProductCode = productCode;
            entity.CategoryId = categoryId;

            if (categoryId.HasValue)
            {
                entity.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(categoryId.Value).Path;
            }

            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.SaleStatus = productstatus;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult alertProducts = SubSiteProducthelper.GetAlertProducts(entity);
            grdProducts.DataSource = alertProducts.Data;
            grdProducts.DataBind();
            pager.TotalRecords = alertProducts.TotalRecords;
            pager1.TotalRecords = alertProducts.TotalRecords;
        }

        private void btnAddOK_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要下架的商品", false);
            }
            else if (string.IsNullOrEmpty(txtPrefix.Text.Trim()) && string.IsNullOrEmpty(txtSuffix.Text.Trim()))
            {
                ShowMsg("前后缀不能同时为空", false);
            }
            else
            {
                if (SubSiteProducthelper.UpdateProductNames(str, txtPrefix.Text.Trim(), txtSuffix.Text.Trim()))
                {
                    ShowMsg("为商品名称添加前后缀成功", true);
                }
                else
                {
                    ShowMsg("为商品名称添加前后缀失败", false);
                }
                BindProducts();
            }
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {

            LoadParameters();

            ProductQuery entity = new ProductQuery();

            entity.Keywords = productName;
            entity.ProductCode = productCode;
            entity.CategoryId = categoryId;

            if (categoryId.HasValue)
            {
                entity.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(categoryId.Value).Path;
            }
            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.SaleStatus = productstatus;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult alertProducts = SubSiteProducthelper.GetAlertProducts(entity);
            DataTable data = new DataTable();
            if (alertProducts.Data != null)
            {
                data = (DataTable)alertProducts.Data;
            }
            string s = string.Empty + "排序号,商品名称,商家编码,库存,状态,最低零售价(元),一口价,采购价,差价\r\n";
            foreach (DataRow row in data.Rows)
            {
                s = s + (data.Rows.IndexOf(row) + 1);
                s = s + "," + row["ProductName"].ToString();
                s = s + "," + row["ProductCode"].ToString();
                s = s + "," + row["Stock"].ToString();
                s = s + "," + ShowStatus(row["SaleStatus"]);
                if (!string.IsNullOrEmpty(row["LowestSalePrice"].ToString()))
                {
                    s = s + "," + decimal.Parse(row["LowestSalePrice"].ToString()).ToString("F2");
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
                    s = s + "," + decimal.Parse(row["PurchasePrice"].ToString()).ToString("F2");
                }
                else
                {
                    s = s + ",0.00";
                }
                s = s + "," + decimal.Parse(row["PurchasePrice"].ToString()).ToString("F2");
                s = s + "," + ((decimal.Parse(row["SalePrice"].ToString()) - decimal.Parse(row["PurchasePrice"].ToString()))).ToString("F2") + "\r\n";
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

        private void btnInStock_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要入库的商品", false);
            }
            else if (SubSiteProducthelper.UpdateProductSaleStatus(str, ProductSaleStatus.OnStock) > 0)
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
            else if (SubSiteProducthelper.UpdateProductSaleStatus(str, ProductSaleStatus.UnSale) > 0)
            {
                ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
                BindProducts();
            }
            else
            {
                ShowMsg("下架商品失败，未知错误", false);
            }
        }

        private void btnReplaceOK_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要下架的商品", false);
            }
            else if (string.IsNullOrEmpty(txtOleWord.Text.Trim()))
            {
                ShowMsg("查找字符串不能为空", false);
            }
            else
            {
                if (SubSiteProducthelper.ReplaceProductNames(str, txtOleWord.Text.Trim(), txtNewWord.Text.Trim()))
                {
                    ShowMsg("为商品名称替换字符串缀成功", true);
                }
                else
                {
                    ShowMsg("为商品名称替换字符串缀失败", false);
                }
                BindProducts();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(string.Concat(new object[] { Globals.ApplicationPath, "/Shopadmin/product/MyProductAlert.aspx?productName=", Globals.UrlEncode(txtSearchText.Text), "&categoryId=", dropCategories.SelectedValue, "&productStatus=", dropproductsale.SelectedValue, "&productCode=", Globals.UrlEncode(txtSKU.Text), "&pageSize=", pager.PageSize.ToString() }));
        }

        private void btnUpShelf_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要上架的商品", false);
            }
            else if (!SubSiteProducthelper.IsOnSale(str))
            {
                ShowMsg("选择要上架的商品中有一口价低于最低零售价的情况", false);
            }
            else if (SubSiteProducthelper.UpdateProductSaleStatus(str, ProductSaleStatus.OnSale) > 0)
            {
                ShowMsg("成功上架了选择的商品,您可以到出售中的商品中找到上架的商品", true);
                BindProducts();
            }
            else
            {
                ShowMsg("上架商品失败，未知错误", false);
            }
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
            if (Page.IsPostBack)
            {
                productName = txtSearchText.Text;
                productCode = txtSKU.Text;
                categoryId = dropCategories.SelectedValue;
                if (dropproductsale.SelectedValue.HasValue)
                {
                    switch (dropproductsale.SelectedValue.Value)
                    {
                        case 1:
                            productstatus = ProductSaleStatus.OnSale;
                            return;

                        case 2:
                            productstatus = ProductSaleStatus.UnSale;
                            return;
                    }
                    productstatus = ProductSaleStatus.OnStock;
                }
            }
            else
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
                dropproductsale.DataBind();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productStatus"]))
                {
                    int num2 = 0;
                    if (int.TryParse(Page.Request.QueryString["productStatus"], out num2))
                    {
                        switch (num2)
                        {
                            case 1:
                                productstatus = ProductSaleStatus.OnSale;
                                break;

                            case 2:
                                productstatus = ProductSaleStatus.UnSale;
                                break;

                            default:
                                productstatus = ProductSaleStatus.OnStock;
                                break;
                        }
                        dropproductsale.SelectedValue = new int?(num2);
                    }
                }
                txtSearchText.Text = productName;
                txtSKU.Text = productCode;
                dropCategories.DataBind();
                dropCategories.SelectedValue = categoryId;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdProducts.ReBindData += new Grid.ReBindDataEventHandler(grdProducts_ReBindData);
            btnUpShelf.Click += new EventHandler(btnUpShelf_Click);
            btnOffShelf.Click += new EventHandler(btnOffShelf_Click);
            btnInStock.Click += new EventHandler(btnInStock_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            grdProducts.RowDeleting += new GridViewDeleteEventHandler(grdProducts_RowDeleting);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
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
            if (dropproductsale.SelectedValue.HasValue)
            {
                queryStrings.Add("productStatus", dropproductsale.SelectedValue.Value.ToString());
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("productCode", txtSKU.Text);
            queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
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

