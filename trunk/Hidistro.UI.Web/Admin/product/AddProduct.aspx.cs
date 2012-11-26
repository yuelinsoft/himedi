using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{

    /// <summary>
    /// 增加商品
    /// </summary>
    [PrivilegeCheck(Privilege.AddProducts)]
    public partial class AddProduct : ProductBasePage
    {
        
        int categoryId;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) || !(base.Request.QueryString["isCallback"] == "true")))
            {
                base.DoCallback();
            }
            else if (!int.TryParse(base.Request.QueryString["categoryId"], out categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!Page.IsPostBack)
            {
                litCategoryName.Text = CatalogHelper.GetFullCategory(categoryId);
                CategoryInfo category = CatalogHelper.GetCategory(categoryId);
                if (category == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    lnkEditCategory.NavigateUrl = "SelectCategory.aspx?categoryId=" + categoryId.ToString(CultureInfo.InvariantCulture);
                    dropProductTypes.DataBind();
                    dropProductTypes.SelectedValue = category.AssociatedProductType;
                    dropProductLines.DataBind();
                    dropBrandCategories.ProductTypeId = category.AssociatedProductType;
                    dropBrandCategories.DataBind();
                    txtProductCode.Text = txtSku.Text = category.SKUPrefix + new Random(DateTime.Now.Millisecond).Next(1, 0x1869f).ToString(CultureInfo.InvariantCulture).PadLeft(5, '0');
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int stock = 0;// num;
            int alertStock = 0;// num2;
            int lineId = 0;// num3;
            decimal purchasePrice = 0m;// num4;
            decimal lowestSalePrice = 0m;// num5;
            decimal salePrice = 0m;// num6;
            decimal? costPrice = null;// nullable;
            decimal? marketPrice = null;// nullable2;
            int? weight = null;// nullable3;

            // (bool skuEnabled, out int displaySequence, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out int? weight)

            if (ValidateConverts(chkSkuEnabled.Checked, out purchasePrice, out lowestSalePrice, out salePrice, out costPrice, out marketPrice, out stock, out alertStock, out weight, out lineId))
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
                        ShowMsg("分销商采购价必须要小于其最低零售价", false);
                        return;
                    }
                }
                string text = editDescription.Text;
                if (ckbIsDownPic.Checked)
                {
                    text = base.DownRemotePic(text);
                }
                ProductInfo target = new ProductInfo();
                target.CategoryId = categoryId;
                target.TypeId = dropProductTypes.SelectedValue;
                target.ProductName = txtProductName.Text;
                target.ProductCode = txtProductCode.Text;
                target.LineId = lineId;// num3;
                target.LowestSalePrice = lowestSalePrice;// num5;
                target.MarketPrice = marketPrice;// nullable2;
                target.Unit = txtUnit.Text;
                target.ImageUrl1 = uploader1.UploadedImageUrl;
                target.ImageUrl2 = uploader2.UploadedImageUrl;
                target.ImageUrl3 = uploader3.UploadedImageUrl;
                target.ImageUrl4 = uploader4.UploadedImageUrl;
                target.ImageUrl5 = uploader5.UploadedImageUrl;
                target.ThumbnailUrl40 = uploader1.ThumbnailUrl40;
                target.ThumbnailUrl60 = uploader1.ThumbnailUrl60;
                target.ThumbnailUrl100 = uploader1.ThumbnailUrl100;
                target.ThumbnailUrl160 = uploader1.ThumbnailUrl160;
                target.ThumbnailUrl180 = uploader1.ThumbnailUrl180;
                target.ThumbnailUrl220 = uploader1.ThumbnailUrl220;
                target.ThumbnailUrl310 = uploader1.ThumbnailUrl310;
                target.ThumbnailUrl410 = uploader1.ThumbnailUrl410;
                target.ShortDescription = txtShortDescription.Text;
                target.Description = (!string.IsNullOrEmpty(text) && (text.Length > 0)) ? text : null;
                target.PenetrationStatus = chkPenetration.Checked ? PenetrationStatus.Already : PenetrationStatus.Notyet;
                target.Title = txtTitle.Text;
                target.MetaDescription = txtMetaDescription.Text;
                target.MetaKeywords = txtMetaKeywords.Text;
                target.AddedDate = DateTime.Now;
                target.BrandId = dropBrandCategories.SelectedValue;
                target.MainCategoryPath = CatalogHelper.GetCategory(categoryId).Path + "|";
                //ProductInfo info = info3;
                //ProductInfo target = info;
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
                target.SaleStatus = onSale;
                Dictionary<string, SKUItem> skus = null;
                Dictionary<int, IList<int>> attrs = null;
                if (chkSkuEnabled.Checked)
                {
                    target.HasSKU = true;
                    skus = base.GetSkus(txtSkus.Text);
                }
                else
                {
                   // Dictionary<string, SKUItem> dictionary3 = new Dictionary<string, SKUItem>();
                    skus = new Dictionary<string, SKUItem>();
                    SKUItem item = new SKUItem();

                    item.SkuId = "0";
                    item.SKU = txtSku.Text;
                    item.SalePrice = salePrice;// num6;
                    item.CostPrice = costPrice.HasValue ? costPrice.Value : 0M;
                    item.PurchasePrice = purchasePrice;
                    item.Stock = stock;// num;
                    item.AlertStock = alertStock;// num2;
                    item.Weight = weight.HasValue ? weight.Value : 0;

                    //dictionary3.Add("0", item);
                    //skus = dictionary3;
                    
                    skus.Add("0", item);

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
                ValidationResults validateResults = Hishop.Components.Validation.Validation.Validate<ProductInfo>(target, new string[] { "AddProduct" });
                if (!validateResults.IsValid)
                {
                    ShowMsg(validateResults);
                }
                else
                {
                    switch (ProductHelper.AddProduct(target, skus, attrs))
                    {
                        case ProductActionStatus.Success:
                            {
                                ShowMsg("添加商品成功", true);
                                Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/product/AddProductComplete.aspx?categoryId={0}&productId={1}", categoryId, target.ProductId)), true);
                                return;
                            }
                        case ProductActionStatus.DuplicateName:
                            {
                                ShowMsg("添加商品失败，商品名称不能重复", false);
                                return;
                            }
                        case ProductActionStatus.DuplicateSKU:
                            {
                                ShowMsg("添加商品失败，商家编码不能重复", false);
                                return;
                            }
                        case ProductActionStatus.SKUError:
                            {
                                ShowMsg("添加商品失败，商家编码不能重复", false);
                                return;
                            }
                        case ProductActionStatus.AttributeError:
                            {
                                ShowMsg("添加商品失败，保存商品属性时出错", false);
                                return;
                            }
                    }
                    ShowMsg("添加商品失败，未知错误", false);
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnAdd.Click += new EventHandler(btnAdd_Click);
        }

        private bool ValidateConverts(bool skuEnabled, out decimal purchasePrice, out decimal lowestSalePrice, out decimal salePrice, out decimal? costPrice, out decimal? marketPrice, out int stock, out int alertStock, out int? weight, out int lineId)
        {
            int num;
            decimal num2;
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            weight = 0;
            lineId = num = 0;
            stock = alertStock = num;
            salePrice = num2 = 0M;
            purchasePrice = lowestSalePrice = num2;
            if (!dropProductLines.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择商品所属的产品线");
            }
            else
            {
                lineId = dropProductLines.SelectedValue.Value;
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

