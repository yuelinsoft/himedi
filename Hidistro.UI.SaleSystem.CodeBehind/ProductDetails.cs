using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    public class ProductDetails : HtmlTemplatedWebControl
    {
        AddCartButton btnaddgouwu;
        BuyButton btnBuy;
        Common_Location common_Location;
        Common_ProductConsultations consultations;
        Common_GoodsList_Correlative correlative;
        HyperLink hpkProductConsultations;
        HyperLink hpkProductReviews;
        Common_ProductImages images;
        Label lblBuyPrice;
        FormatedMoneyLabel lblMarkerPrice;
        Literal lblProductCode;
        SkuLabel lblSku;
        StockLabel lblStock;
        TotalLabel lblTotalPrice;
        Literal litBrand;
        Literal litBrosedNum;
        Literal litDescription;
        Literal litProductName;
        Literal litSaleCounts;
        Literal litShortDescription;
        Literal litUnit;
        WeightLabel litWeight;
        int productId;
        Common_ProductReview reviews;
        ThemedTemplatedRepeater rptExpandAttributes;
        SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(Page.Request.QueryString["productId"], out productId))
            {
                base.GotoResourceNotFound();
            }
            common_Location = (Common_Location)FindControl("common_Location");
            litProductName = (Literal)FindControl("litProductName");
            lblProductCode = (Literal)FindControl("lblProductCode");
            lblSku = (SkuLabel)FindControl("lblSku");
            lblStock = (StockLabel)FindControl("lblStock");
            litUnit = (Literal)FindControl("litUnit");
            litWeight = (WeightLabel)FindControl("litWeight");
            litBrosedNum = (Literal)FindControl("litBrosedNum");
            litBrand = (Literal)FindControl("litBrand");
            litSaleCounts = (Literal)FindControl("litSaleCounts");
            lblMarkerPrice = (FormatedMoneyLabel)FindControl("lblMarkerPrice");
            lblBuyPrice = (Label)FindControl("lblBuyPrice");
            lblTotalPrice = (TotalLabel)FindControl("lblTotalPrice");
            litDescription = (Literal)FindControl("litDescription");
            litShortDescription = (Literal)FindControl("litShortDescription");
            btnBuy = (BuyButton)FindControl("btnBuy");
            btnaddgouwu = (AddCartButton)FindControl("btnaddgouwu");
            hpkProductConsultations = (HyperLink)FindControl("hpkProductConsultations");
            hpkProductReviews = (HyperLink)FindControl("hpkProductReviews");
            images = (Common_ProductImages)FindControl("common_ProductImages");
            rptExpandAttributes = (ThemedTemplatedRepeater)FindControl("rptExpandAttributes");
            skuSelector = (SKUSelector)FindControl("SKUSelector");
            reviews = (Common_ProductReview)FindControl("list_Common_ProductReview");
            consultations = (Common_ProductConsultations)FindControl("list_Common_ProductConsultations");
            correlative = (Common_GoodsList_Correlative)FindControl("list_Common_GoodsList_Correlative");
            if (!Page.IsPostBack)
            {
                ProductBrowseInfo info = ProductBrowser.GetProductBrowseInfo(productId, new int?(reviews.MaxNum), new int?(consultations.MaxNum));
                if ((info.Product == null) || (info.Product.SaleStatus == ProductSaleStatus.Delete))
                {
                    Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除"));
                }
                else
                {
                    if (info.Product.SaleStatus == ProductSaleStatus.UnSale)
                    {
                        Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[] { Page.Request.QueryString["productId"] }));
                    }
                    LoadPageSearch(info.Product);
                    hpkProductReviews.Text = "查看全部" + ProductBrowser.GetProductReviewNumber(productId).ToString() + "条评论";
                    hpkProductConsultations.Text = "查看全部" + ProductBrowser.GetProductConsultationNumber(productId).ToString() + "条咨询";
                    hpkProductConsultations.NavigateUrl = string.Format("ProductConsultationsAndReplay.aspx?productId={0}", productId);
                    hpkProductReviews.NavigateUrl = string.Format("LookProductReviews.aspx?productId={0}", productId);
                    LoadProductInfo(info.Product, info.BrandName);
                    btnBuy.Stock = info.Product.Stock;
                    btnaddgouwu.Stock = info.Product.Stock;
                    BrowsedProductQueue.EnQueue(productId);
                    images.ImageInfo = info.Product;
                    if (info.DbAttribute != null)
                    {
                        rptExpandAttributes.DataSource = info.DbAttribute;
                        rptExpandAttributes.DataBind();
                    }
                    if (info.DbSKUs != null)
                    {
                        skuSelector.ProductId = productId;
                        skuSelector.DataSource = info.DbSKUs;
                    }
                    if (info.DBReviews != null)
                    {
                        reviews.DataSource = info.DBReviews;
                        reviews.DataBind();
                    }
                    if (info.DBConsultations != null)
                    {
                        consultations.DataSource = info.DBConsultations;
                        consultations.DataBind();
                    }
                    if (info.DbCorrelatives != null)
                    {
                        correlative.DataSource = info.DbCorrelatives;
                        correlative.DataBind();
                    }
                }
            }
        }

        void LoadPageSearch(ProductInfo productDetails)
        {
            if (!string.IsNullOrEmpty(productDetails.MetaKeywords))
            {
                MetaTags.AddMetaKeywords(productDetails.MetaKeywords, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(productDetails.MetaDescription))
            {
                MetaTags.AddMetaDescription(productDetails.MetaDescription, HiContext.Current.Context);
            }
            if (!string.IsNullOrEmpty(productDetails.Title))
            {
                PageTitle.AddTitle(productDetails.Title, HiContext.Current.Context);
            }
            else
            {
                PageTitle.AddTitle(productDetails.ProductName, HiContext.Current.Context);
            }
        }

        void LoadProductInfo(ProductInfo productDetails, string brandName)
        {
            if ((common_Location != null) && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
            {
                common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
                common_Location.ProductName = productDetails.ProductName;
            }
            litProductName.Text = productDetails.ProductName;
            lblProductCode.Text = productDetails.ProductCode;
            lblSku.Text = productDetails.SKU;
            lblSku.Value = productDetails.SkuId;
            lblStock.Stock = productDetails.Stock;
            lblStock.AlertStock = productDetails.AlertStock;
            litUnit.Text = productDetails.Unit;
            if (productDetails.Weight > 0)
            {
                litWeight.Text = string.Format("{0} g", productDetails.Weight);
            }
            else
            {
                litWeight.Text = "无";
            }
            litBrosedNum.Text = productDetails.VistiCounts.ToString();
            litBrand.Text = brandName;
            if (litSaleCounts != null)
            {
                litSaleCounts.Text = productDetails.SaleCounts.ToString();
            }
            if (productDetails.MinSalePrice == productDetails.MaxSalePrice)
            {
                lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2");
                lblTotalPrice.Value = new decimal?(productDetails.MinSalePrice);
            }
            else
            {
                lblBuyPrice.Text = productDetails.MinSalePrice.ToString("F2") + " - " + productDetails.MaxSalePrice.ToString("F2");
            }
            lblMarkerPrice.Money = productDetails.MarketPrice;
            litDescription.Text = productDetails.Description;
            if (litShortDescription != null)
            {
                litShortDescription.Text = productDetails.ShortDescription;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (SkinName == null)
            {
                SkinName = "Skin-ProductDetails.html";
            }
            base.OnInit(e);
        }
    }
}

