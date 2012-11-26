using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    /// <summary>
    /// 编辑商品信息
    /// </summary>
    [PrivilegeCheck(Privilege.EditProducts)]
    public partial class EditProduct : ProductBasePage
    {

        int categoryId;
        int productId;
        string toline = "";

        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int displaySequence = 0;
            int stock = 0;// num2;
            int alertStock = 0;// num3;
            decimal purchasePrice = 0m;
            decimal lowestSalePrice = 0m;// num5;
            decimal salePrice = 0m;// num6;
            decimal? costPrice = null;// nullable;
            decimal? marketPrice = null;// nullable2;
            int? weight = null;// nullable3;

            if (categoryId == 0)
            {
                categoryId = (int)ViewState["ProductCategoryId"];
            }

           // (bool skuEnabled, out int displaySequence, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out int? weight)

            if (ValidateConverts(chkSkuEnabled.Checked, out displaySequence, out purchasePrice, out lowestSalePrice, out salePrice, out costPrice, out marketPrice, out stock, out alertStock, out weight))
            {
                if (!chkSkuEnabled.Checked)
                {
                    if (salePrice <= 0M)
                    {
                        ShowMsg("商品一口价必须大于0", false);
                        return;
                    }
                    if (costPrice.HasValue && (costPrice.Value >= salePrice))
                    {
                        ShowMsg("商品成本价必须小于商品一口价", false);
                        return;
                    }
                    if (purchasePrice > lowestSalePrice)
                    {
                        ShowMsg("分销商采购价必须要小于等于其最低零售价", false);
                        return;
                    }
                }
                string text = fckDescription.Text;
                if (ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                ProductInfo productInfo = new ProductInfo();
                productInfo.ProductId = productId;
                productInfo.CategoryId = categoryId;
                productInfo.TypeId = dropProductTypes.SelectedValue;
                productInfo.ProductName = txtProductName.Text;
                productInfo.ProductCode = txtProductCode.Text;
                productInfo.DisplaySequence = displaySequence;
                productInfo.LineId = dropProductLines.SelectedValue.Value;
                productInfo.LowestSalePrice = lowestSalePrice;
                productInfo.MarketPrice = marketPrice;// nullable2;
                productInfo.Unit = txtUnit.Text;
                productInfo.ImageUrl1 = uploader1.UploadedImageUrl;
                productInfo.ImageUrl2 = uploader2.UploadedImageUrl;
                productInfo.ImageUrl3 = uploader3.UploadedImageUrl;
                productInfo.ImageUrl4 = uploader4.UploadedImageUrl;
                productInfo.ImageUrl5 = uploader5.UploadedImageUrl;
                productInfo.ThumbnailUrl40 = uploader1.ThumbnailUrl40;
                productInfo.ThumbnailUrl60 = uploader1.ThumbnailUrl60;
                productInfo.ThumbnailUrl100 = uploader1.ThumbnailUrl100;
                productInfo.ThumbnailUrl160 = uploader1.ThumbnailUrl160;
                productInfo.ThumbnailUrl180 = uploader1.ThumbnailUrl180;
                productInfo.ThumbnailUrl220 = uploader1.ThumbnailUrl220;
                productInfo.ThumbnailUrl310 = uploader1.ThumbnailUrl310;
                productInfo.ThumbnailUrl410 = uploader1.ThumbnailUrl410;
                productInfo.ShortDescription = txtShortDescription.Text;
                productInfo.Description = (!string.IsNullOrEmpty(text) && (text.Length > 0)) ? text : null;
                productInfo.PenetrationStatus = chkPenetration.Checked ? PenetrationStatus.Already : PenetrationStatus.Notyet;
                productInfo.Title = txtTitle.Text;
                productInfo.MetaDescription = txtMetaDescription.Text;
                productInfo.MetaKeywords = txtMetaKeywords.Text;
                productInfo.AddedDate = DateTime.Now;
                productInfo.BrandId = dropBrandCategories.SelectedValue;
                //ProductInfo info = productInfo;
                //ProductInfo target = info;
                productInfo.BrandId = dropBrandCategories.SelectedValue;
                ProductSaleStatus onSale = ProductSaleStatus.OnSale;
                if (radInStock.Checked)
                {
                    onSale = ProductSaleStatus.OnStock;
                }
                if (radUnSales.Checked)
                {
                    onSale = ProductSaleStatus.UnSale;
                }
                if (radOnSales.Checked)
                {
                    onSale = ProductSaleStatus.OnSale;
                }
                productInfo.SaleStatus = onSale;
                CategoryInfo category = CatalogHelper.GetCategory(categoryId);
                if (category != null)
                {
                    productInfo.MainCategoryPath = category.Path + "|";
                }
                Dictionary<string, SKUItem> skus = new Dictionary<string,SKUItem>();
                Dictionary<int, IList<int>> attrs = null;
                if (chkSkuEnabled.Checked)
                {
                    productInfo.HasSKU = true;
                    skus = base.GetSkus(txtSkus.Text);
                }
                else
                {
                    //Dictionary<string, SKUItem> skuList = new Dictionary<string, SKUItem>();

                    SKUItem item = new SKUItem();
                    item.SkuId = "0";
                    item.SKU = txtSku.Text;
                    item.SalePrice = salePrice;
                    item.CostPrice = costPrice.HasValue ? costPrice.Value : 0M;
                    item.PurchasePrice = purchasePrice;// num4;
                    item.Stock = stock;// num2;
                    item.AlertStock = alertStock;// num3;
                    item.Weight = weight.HasValue ? weight.Value : 0;

                    //skuList.Add("0", item);
                    skus.Add("0", item);
                    //skus = dictionary3;

                    if (txtMemberPrices.Text.Length > 0)
                    {
                        base.GetMemberPrices(skus["0"], txtMemberPrices.Text);
                    }
                    if (txtDistributorPrices.Text.Length > 0)
                    {
                        base.GetDistributorPrices(skus["0"], txtDistributorPrices.Text);
                    }
                }
                if (!(string.IsNullOrEmpty(txtAttributes.Text) || (txtAttributes.Text.Length <= 0)))
                {
                    attrs = base.GetAttributes(txtAttributes.Text);
                }
                ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(productInfo);
                if (!validateResults.IsValid)
                {
                    ShowMsg(validateResults);
                }
                else
                {
                    if (ViewState["distributorUserIds"] == null)
                    {
                        ViewState["distributorUserIds"] = new List<int>();
                    }
                    int type = 0;
                    if (((productInfo.LineId > 0) && (int.Parse(hdlineId.Value) > 0)) && (productInfo.LineId != int.Parse(hdlineId.Value)))
                    {
                        type = 6;
                    }
                    if (!chkPenetration.Checked)
                    {
                        type = 5;
                    }
                    if (type == 5)
                    {
                        AdminPage.SendMessageToDistributors(productInfo.ProductId.ToString(), type);
                    }
                    else if (type == 6)
                    {
                        toline = dropProductLines.SelectedItem.Text;
                        AdminPage.SendMessageToDistributors(hdlineId.Value + "|" + toline, type);
                    }
                    switch (ProductHelper.UpdateProduct(productInfo, skus, attrs, (IList<int>)ViewState["distributorUserIds"]))
                    {
                        case ProductActionStatus.Success:
                            ShowMsg("修改商品成功", true);
                            base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/EditProductComplete.aspx?productId={0}", productInfo.ProductId)), true);
                            return;

                        case ProductActionStatus.DuplicateName:
                            ShowMsg("修改商品失败，商品名称不能重复", false);
                            return;

                        case ProductActionStatus.DuplicateSKU:
                            ShowMsg("修改商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.SKUError:
                            ShowMsg("修改商品失败，商家编码不能重复", false);
                            return;

                        case ProductActionStatus.AttributeError:
                            ShowMsg("修改商品失败，保存商品属性时出错", false);
                            return;

                        case ProductActionStatus.OffShelfError:
                            ShowMsg("修改商品失败， 子站没在零售价范围内的商品无法下架", false);
                            return;
                    }
                    ShowMsg("修改商品失败，未知错误", false);
                }
            }
        }






        private void LoadProduct(ProductInfo product, Dictionary<int, IList<int>> attrs)
        {
            dropProductTypes.SelectedValue = product.TypeId;
            dropProductLines.SelectedValue = new int?(product.LineId);
            hdlineId.Value = product.LineId.ToString();
            dropBrandCategories.SelectedValue = product.BrandId;
            txtDisplaySequence.Text = product.DisplaySequence.ToString();
            txtProductName.Text = product.ProductName;
            txtProductCode.Text = product.ProductCode;
            txtUnit.Text = product.Unit;
            if (product.MarketPrice.HasValue)
            {
                txtMarketPrice.Text = product.MarketPrice.Value.ToString("F2");
            }
            txtShortDescription.Text = product.ShortDescription;
            fckDescription.Text = product.Description;
            txtTitle.Text = product.Title;
            txtMetaDescription.Text = product.MetaDescription;
            txtMetaKeywords.Text = product.MetaKeywords;
            txtLowestSalePrice.Text = product.LowestSalePrice.ToString("F2");
            chkPenetration.Checked = product.PenetrationStatus == PenetrationStatus.Already;
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
            if ((attrs != null) && (attrs.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<xml><attributes>");
                foreach (int num in attrs.Keys)
                {
                    builder.Append("<item attributeId=\"").Append(num.ToString(CultureInfo.InvariantCulture)).Append("\" usageMode=\"").Append(((int)ProductTypeHelper.GetAttribute(num).UsageMode).ToString()).Append("\" >");
                    foreach (int num2 in attrs[num])
                    {
                        builder.Append("<attValue valueId=\"").Append(num2.ToString(CultureInfo.InvariantCulture)).Append("\" />");
                    }
                    builder.Append("</item>");
                }
                builder.Append("</attributes></xml>");
                txtAttributes.Text = builder.ToString();
            }
            chkSkuEnabled.Checked = product.HasSKU;
            if (product.HasSKU)
            {
                StringBuilder builder2 = new StringBuilder();
                builder2.Append("<xml><productSkus>");
                foreach (string str in product.Skus.Keys)
                {
                    SKUItem item = product.Skus[str];
                    string str2 = "<item skuCode=\"" + item.SKU + "\" salePrice=\"" + item.SalePrice.ToString("F2") + "\" costPrice=\"" + ((item.CostPrice > 0M) ? item.CostPrice.ToString("F2") : "") + "\" purchasePrice=\"" + item.PurchasePrice.ToString("F2") + "\" qty=\"" + item.Stock.ToString(CultureInfo.InvariantCulture) + "\" alertQty=\"" + item.AlertStock.ToString(CultureInfo.InvariantCulture) + "\" weight=\"" + ((item.Weight > 0) ? item.Weight.ToString(CultureInfo.InvariantCulture) : "") + "\"><skuFields>";
                    foreach (int num3 in item.SkuItems.Keys)
                    {
                        string[] strArray = new string[] { "<sku attributeId=\"", num3.ToString(CultureInfo.InvariantCulture), "\" valueId=\"", item.SkuItems[num3].ToString(CultureInfo.InvariantCulture), "\" />" };
                        string str3 = string.Concat(strArray);
                        str2 = str2 + str3;
                    }
                    str2 = str2 + "</skuFields>";
                    if (item.MemberPrices.Count > 0)
                    {
                        str2 = str2 + "<memberPrices>";
                        foreach (int num4 in item.MemberPrices.Keys)
                        {
                            decimal num5 = item.MemberPrices[num4];
                            str2 = str2 + string.Format("<memberGrande id=\"{0}\" price=\"{1}\" />", num4.ToString(CultureInfo.InvariantCulture), num5.ToString("F2"));
                        }
                        str2 = str2 + "</memberPrices>";
                    }
                    if (item.DistributorPrices.Count > 0)
                    {
                        str2 = str2 + "<distributorPrices>";
                        foreach (int num6 in item.DistributorPrices.Keys)
                        {
                            decimal num7 = item.DistributorPrices[num6];
                            str2 = str2 + string.Format("<distributorGrande id=\"{0}\" price=\"{1}\" />", num6.ToString(CultureInfo.InvariantCulture), num7.ToString("F2"));
                        }
                        str2 = str2 + "</distributorPrices>";
                    }
                    str2 = str2 + "</item>";
                    builder2.Append(str2);
                }
                builder2.Append("</productSkus></xml>");
                txtSkus.Text = builder2.ToString();
            }
            SKUItem defaultSku = product.DefaultSku;
            txtSku.Text = product.ProductCode;
            txtSalePrice.Text = defaultSku.SalePrice.ToString("F2");
            txtCostPrice.Text = (defaultSku.CostPrice > 0M) ? defaultSku.CostPrice.ToString("F2") : "";
            txtPurchasePrice.Text = defaultSku.PurchasePrice.ToString("F2");
            txtStock.Text = defaultSku.Stock.ToString(CultureInfo.InvariantCulture);
            txtAlertStock.Text = defaultSku.AlertStock.ToString(CultureInfo.InvariantCulture);
            txtWeight.Text = (defaultSku.Weight > 0) ? defaultSku.Weight.ToString(CultureInfo.InvariantCulture) : "";
            if (defaultSku.MemberPrices.Count > 0)
            {
                txtMemberPrices.Text = "<xml><gradePrices>";
                foreach (int num8 in defaultSku.MemberPrices.Keys)
                {
                    decimal num9 = defaultSku.MemberPrices[num8];
                    txtMemberPrices.Text = txtMemberPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num8.ToString(CultureInfo.InvariantCulture), num9.ToString("F2"));
                }
                txtMemberPrices.Text = txtMemberPrices.Text + "</gradePrices></xml>";
            }
            if (defaultSku.DistributorPrices.Count > 0)
            {
                txtDistributorPrices.Text = "<xml><gradePrices>";
                foreach (int num10 in defaultSku.DistributorPrices.Keys)
                {
                    decimal num11 = defaultSku.DistributorPrices[num10];
                    txtDistributorPrices.Text = txtDistributorPrices.Text + string.Format("<grande id=\"{0}\" price=\"{1}\" />", num10.ToString(CultureInfo.InvariantCulture), num11.ToString("F2"));
                }
                txtDistributorPrices.Text = txtDistributorPrices.Text + "</gradePrices></xml>";
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(base.Request.QueryString["productId"], out productId);
            int.TryParse(base.Request.QueryString["categoryId"], out categoryId);
            if (!Page.IsPostBack)
            {
                Dictionary<int, IList<int>> dictionary;
                IList<int> distributorUserIds = null;
                ProductInfo product = ProductHelper.GetProductDetails(productId, out dictionary, out distributorUserIds);
                if (product == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                    {
                        litCategoryName.Text = CatalogHelper.GetFullCategory(categoryId);
                        ViewState["ProductCategoryId"] = categoryId;
                        lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + categoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        litCategoryName.Text = CatalogHelper.GetFullCategory(product.CategoryId);
                        ViewState["ProductCategoryId"] = product.CategoryId;
                        lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + product.CategoryId.ToString(CultureInfo.InvariantCulture);
                    }
                    lnkEditCategory.NavigateUrl = lnkEditCategory.NavigateUrl + "&productId=" + product.ProductId.ToString(CultureInfo.InvariantCulture);
                    if ((distributorUserIds != null) && (distributorUserIds.Count > 0))
                    {
                        ViewState["distributorUserIds"] = distributorUserIds;
                        hlinkDistributor.NavigateUrl = "../distribution/ManageDistributor.aspx?LineId=" + product.LineId.ToString(CultureInfo.InvariantCulture);
                        hlinkDistributor.Text = string.Format("{0}位分销商", distributorUserIds.Count);
                    }
                    dropProductTypes.DataBind();
                    dropProductLines.DataBind();
                    dropBrandCategories.ProductTypeId = product.TypeId;
                    dropBrandCategories.DataBind();
                    LoadProduct(product, dictionary);
                }
            }
        }

        private bool ValidateConverts(bool skuEnabled, out int displaySequence, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out int? weight)
        {
            int num;
            decimal num2;
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            alertStock = num = 0;
            displaySequence = stock = num;
            salePrice = num2 = 0M;
            purchasePrice = lowestSalePrice = num2;
            if (!dropProductLines.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择产品线");
            }
            if (!(!string.IsNullOrEmpty(txtDisplaySequence.Text) && int.TryParse(txtDisplaySequence.Text, out displaySequence)))
            {
                str = str + Formatter.FormatErrorMessage("请正确填写商品排序");
            }
            if (txtProductCode.Text.Length > 20)
            {
                str = str + Formatter.FormatErrorMessage("商家编码的长度不能超过20个字符");
            }
            if (!string.IsNullOrEmpty(txtMarketPrice.Text))
            {
                decimal num3;
                if (decimal.TryParse(txtMarketPrice.Text, out num3))
                {
                    marketPrice = new decimal?(num3);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的市场价");
                }
            }
            if (!(!string.IsNullOrEmpty(txtLowestSalePrice.Text) && decimal.TryParse(txtLowestSalePrice.Text, out lowestSalePrice)))
            {
                str = str + Formatter.FormatErrorMessage("请正确填写分销商最低零售价");
            }
            if (!skuEnabled)
            {
                if (!(!string.IsNullOrEmpty(txtSalePrice.Text) && decimal.TryParse(txtSalePrice.Text, out salePrice)))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品一口价");
                }
                if (!string.IsNullOrEmpty(txtCostPrice.Text))
                {
                    decimal num4;
                    if (decimal.TryParse(txtCostPrice.Text, out num4))
                    {
                        costPrice = new decimal?(num4);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的成本价");
                    }
                }
                if (!(!string.IsNullOrEmpty(txtPurchasePrice.Text) && decimal.TryParse(txtPurchasePrice.Text, out purchasePrice)))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写分销商采购价格");
                }
                if (!(!string.IsNullOrEmpty(txtStock.Text) && int.TryParse(txtStock.Text, out stock)))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的库存数量");
                }
                if (!(!string.IsNullOrEmpty(txtAlertStock.Text) && int.TryParse(txtAlertStock.Text, out alertStock)))
                {
                    str = str + Formatter.FormatErrorMessage("请正确填写商品的警戒库存");
                }
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    int num5;
                    if (int.TryParse(txtWeight.Text, out num5))
                    {
                        weight = new int?(num5);
                    }
                    else
                    {
                        str = str + Formatter.FormatErrorMessage("请正确填写商品的重量");
                    }
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

