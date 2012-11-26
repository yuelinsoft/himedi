using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 修改分销商品
    /// </summary>
    public partial class EditMyProduct : DistributorPage
    {
        int categoryId;
        int productId;

        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!int.TryParse(Page.Request.QueryString["ProductId"], out productId))
            {

                base.GotoResourceNotFound();

            }
            else
            {
                int.TryParse(base.Request.QueryString["categoryId"], out categoryId);

                if (!Page.IsPostBack)
                {
                    ProductInfo product = SubSiteProducthelper.GetProduct(productId);

                    if (product == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                        {
                            litCategoryName.Text = SubsiteCatalogHelper.GetFullCategory(categoryId);

                            ViewState["ProductCategoryId"] = categoryId;

                            lnkEditCategory.NavigateUrl = "SelectMyCategory.aspx?categoryId=" + categoryId.ToString(CultureInfo.InvariantCulture);

                        }
                        else
                        {

                            litCategoryName.Text = SubsiteCatalogHelper.GetFullCategory(product.CategoryId);

                            ViewState["ProductCategoryId"] = product.CategoryId;

                            lnkEditCategory.NavigateUrl = "SelectMyCategory.aspx?categoryId=" + product.CategoryId.ToString(CultureInfo.InvariantCulture);
                        }

                        lnkEditCategory.NavigateUrl = lnkEditCategory.NavigateUrl + "&productId=" + product.ProductId.ToString(CultureInfo.InvariantCulture);

                        dropProductTypes.Enabled = false;
                        dropProductTypes.DataBind();
                        dropProductTypes.SelectedValue = product.TypeId;
                        dropProductLines.Enabled = false;
                        dropProductLines.DataBind();
                        dropProductLines.SelectedValue = new int?(product.LineId);
                        dropBrandCategories.Enabled = false;
                        dropBrandCategories.DataBind();
                        dropBrandCategories.SelectedValue = product.BrandId;
                        LoadProudct(product);

                    }

                }

            }

        }

        /// <summary>
        /// 更新按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (categoryId == 0)
            {
                categoryId = (int)ViewState["ProductCategoryId"];
            }

            if (categoryId == 0)
            {
                ShowMsg("请选择店铺分类", false);
                return;
            }

            ProductInfo product = new ProductInfo();
            product.ProductId = productId;
            product.CategoryId = categoryId;

            CategoryInfo category = SubsiteCatalogHelper.GetCategory(product.CategoryId);
            if (category != null)
            {
                product.MainCategoryPath = category.Path + "|";
            }

            product.ProductName = txtProductName.Text;
            product.ShortDescription = txtShortDescription.Text;
            product.Description = fckDescription.Text;
            product.Title = txtTitle.Text;
            product.MetaDescription = txtMetaDescription.Text;
            product.MetaKeywords = txtMetaKeywords.Text;
            product.DisplaySequence = int.Parse(txtDisplaySequence.Text);

            if (!string.IsNullOrEmpty(txtMarketPrice.Text))
            {
                product.MarketPrice = new decimal?(decimal.Parse(txtMarketPrice.Text));
            }

            Dictionary<string, decimal> skuSalePrice = null;

            if (!string.IsNullOrEmpty(txtSkuPrice.Text))
            {
                skuSalePrice = GetSkuPrices();
            }

            ProductSaleStatus onStock = ProductSaleStatus.OnStock;
            if (radInStock.Checked)
            {
                onStock = ProductSaleStatus.OnStock;
            }

            if (radUnSales.Checked)
            {
                onStock = ProductSaleStatus.UnSale;
            }

            if (radOnSales.Checked)
            {
                onStock = ProductSaleStatus.OnSale;

                XmlDocument document = new XmlDocument();

                document.LoadXml(txtSkuPrice.Text);

                XmlNodeList list = document.SelectNodes("//item");

                if ((list != null) && (list.Count > 0))
                {

                    foreach (XmlNode node in list)
                    {

                        if (decimal.Parse(node.Attributes["price"].Value) < decimal.Parse(litLowestSalePrice.Text))
                        {
                            ShowMsg("此商品的一口价已经低于了最低零售价,不允许上架", false);
                            return;
                        }

                    }

                }

            }

            product.SaleStatus = onStock;

            if (SubSiteProducthelper.UpdateProduct(product, skuSalePrice))
            {
                ShowMsg("修改商品成功", true);
            }
            else
            {
                ShowMsg("修改商品失败", false);
            }

        }

        /// <summary>
        /// 获取规格价钱
        /// </summary>
        /// <returns></returns>
        Dictionary<string, decimal> GetSkuPrices()
        {

            XmlDocument document = new XmlDocument();

            Dictionary<string, decimal> dictionary = null;

            try
            {
                document.LoadXml(txtSkuPrice.Text);

                XmlNodeList list = document.SelectNodes("//item");

                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }

                IList<SKUItem> skus = SubSiteProducthelper.GetSkus(productId.ToString());

                dictionary = new Dictionary<string, decimal>();

                string skuId = "";
                decimal price = 0m;

                foreach (XmlNode node in list)
                {
                    skuId = node.Attributes["skuId"].Value;

                    price = decimal.Parse(node.Attributes["price"].Value);

                    foreach (SKUItem item in skus)
                    {

                        if ((item.SkuId == skuId) && (item.SalePrice != price))
                        {
                            dictionary.Add(skuId, price);
                        }

                    }

                }

            }
            catch
            {
            }

            return dictionary;

        }

        /// <summary>
        /// 商品加载
        /// </summary>
        /// <param name="product"></param>
        void LoadProudct(ProductInfo product)
        {
            txtProductName.Text = product.ProductName;
            txtDisplaySequence.Text = product.DisplaySequence.ToString();
            litLowestSalePrice.Text = product.LowestSalePrice.ToString("F2");
            if (product.MarketPrice.HasValue)
            {
                txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            litProductCode.Text = product.ProductCode;
            litUnit.Text = product.Unit;
            txtShortDescription.Text = product.ShortDescription;
            fckDescription.Text = product.Description;
            txtTitle.Text = product.Title;
            txtMetaDescription.Text = product.MetaDescription;
            txtMetaKeywords.Text = product.MetaKeywords;
            if (product.SaleStatus == ProductSaleStatus.OnSale)
            {
                radOnSales.Checked = true;
            }
            else if (product.SaleStatus == ProductSaleStatus.UnSale)
            {
                radUnSales.Checked = true;
            }
            else
            {
                radInStock.Checked = true;
            }
            uploader1.UploadedImageUrl = product.ImageUrl1;
            uploader2.UploadedImageUrl = product.ImageUrl2;
            uploader3.UploadedImageUrl = product.ImageUrl3;
            uploader4.UploadedImageUrl = product.ImageUrl4;
            uploader5.UploadedImageUrl = product.ImageUrl5;
        }

    }

}

