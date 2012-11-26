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
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyProductInStock : DistributorPage
    {

        int? categoryId;
        string productCode;
        string productName;

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
            entity.SaleStatus = ProductSaleStatus.OnStock;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            Globals.EntityCoding(entity, true);
            DbQueryResult products = SubSiteProducthelper.GetProducts(entity);
            grdProducts.DataSource = products.Data;
            grdProducts.DataBind();
            pager.TotalRecords = products.TotalRecords;
            pager1.TotalRecords = products.TotalRecords;
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
                ReBindProducts(false);
            }
            else
            {
                ShowMsg("删除商品失败，未知错误", false);
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
                ShowMsg("成功下架了选择的商品, 您可以到下架区的商品中找到下架的商品", true);
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
            ReBindProducts(true);
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
            ReBindProducts(false);
        }

        private void grdProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SubSiteProducthelper.DeleteProducts(grdProducts.DataKeys[e.RowIndex].Value.ToString());
            ReBindProducts(false);
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
                if (!string.IsNullOrEmpty(Page.Request.QueryString["SortOrder"]))
                {
                    grdProducts.SortOrder = Page.Request.QueryString["SortOrder"];
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["SortOrderBy"]))
                {
                    grdProducts.SortOrderBy = Page.Request.QueryString["SortOrderBy"];
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
            btnUpShelf.Click += new EventHandler(btnUpShelf_Click);
            btnOffShelf.Click += new EventHandler(btnOffShelf_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnAddOK.Click += new EventHandler(btnAddOK_Click);
            btnReplaceOK.Click += new EventHandler(btnReplaceOK_Click);
            grdProducts.RowDeleting += new GridViewDeleteEventHandler(grdProducts_RowDeleting);
            if (!Page.IsPostBack)
            {
                BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBindProducts(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", txtSearchText.Text);
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("productCode", txtSKU.Text);
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

