namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web.UI.WebControls;

    public class CountDownProductsDetails : HtmlTemplatedWebControl
    {
       BuyButton btnOrder;
       Common_Location common_Location;
       Common_ProductConsultations consultations;
       Common_GoodsList_Correlative correlative;
       HyperLink hpkProductConsultations;
       HyperLink hpkProductReviews;
       Common_ProductImages images;
       FormatedMoneyLabel lblCurrentSalePrice;
       FormatedTimeLabel lblEndTime;
       FormatedMoneyLabel lblSalePrice;
       SkuLabel lblSku;
       StockLabel lblStock;
       TotalLabel lblTotalPrice;
       Literal litBrand;
       Literal litBrosedNum;
       Literal litContent;
       Literal litDescription;
       Literal litProductCode;
       Literal litProductName;
       Literal litRemainTime;
       Literal litShortDescription;
       Literal litUnit;
       WeightLabel litWeight;
       int productId;
       Common_ProductReview reviews;
       ThemedTemplatedRepeater rptExpandAttributes;
       SKUSelector skuSelector;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.common_Location = (Common_Location) this.FindControl("common_Location");
            this.litProductCode = (Literal) this.FindControl("litProductCode");
            this.litProductName = (Literal) this.FindControl("litProductName");
            this.lblSku = (SkuLabel) this.FindControl("lblSku");
            this.lblStock = (StockLabel) this.FindControl("lblStock");
            this.litUnit = (Literal) this.FindControl("litUnit");
            this.litWeight = (WeightLabel) this.FindControl("litWeight");
            this.litBrosedNum = (Literal) this.FindControl("litBrosedNum");
            this.litBrand = (Literal) this.FindControl("litBrand");
            this.lblSalePrice = (FormatedMoneyLabel) this.FindControl("lblSalePrice");
            this.lblTotalPrice = (TotalLabel) this.FindControl("lblTotalPrice");
            this.litDescription = (Literal) this.FindControl("litDescription");
            this.litShortDescription = (Literal) this.FindControl("litShortDescription");
            this.btnOrder = (BuyButton) this.FindControl("btnOrder");
            this.hpkProductConsultations = (HyperLink) this.FindControl("hpkProductConsultations");
            this.hpkProductReviews = (HyperLink) this.FindControl("hpkProductReviews");
            this.lblCurrentSalePrice = (FormatedMoneyLabel) this.FindControl("lblCurrentSalePrice");
            this.litContent = (Literal) this.FindControl("litContent");
            this.lblEndTime = (FormatedTimeLabel) this.FindControl("lblEndTime");
            this.litRemainTime = (Literal) this.FindControl("litRemainTime");
            this.images = (Common_ProductImages) this.FindControl("common_ProductImages");
            this.rptExpandAttributes = (ThemedTemplatedRepeater) this.FindControl("rptExpandAttributes");
            this.skuSelector = (SKUSelector) this.FindControl("SKUSelector");
            this.reviews = (Common_ProductReview) this.FindControl("list_Common_ProductReview");
            this.consultations = (Common_ProductConsultations) this.FindControl("list_Common_ProductConsultations");
            this.correlative = (Common_GoodsList_Correlative) this.FindControl("list_Common_GoodsList_Correlative");
            if (!this.Page.IsPostBack)
            {
                ProductBrowseInfo info = ProductBrowser.GetProductBrowseInfo(this.productId, new int?(this.reviews.MaxNum), new int?(this.consultations.MaxNum));
                CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(this.productId);
                if ((info.Product == null) || (countDownInfo == null))
                {
                    this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品参与的限时抢购活动已经结束；或被管理员删除"));
                }
                else
                {
                    this.LoadPageSearch(info.Product);
                    this.hpkProductReviews.Text = "查看全部" + ProductBrowser.GetProductReviewNumber(this.productId).ToString() + "条评论";
                    this.hpkProductConsultations.Text = "查看全部" + ProductBrowser.GetProductConsultationNumber(this.productId).ToString() + "条咨询";
                    this.hpkProductConsultations.NavigateUrl = string.Format("ProductConsultationsAndReplay.aspx?productId={0}", this.productId);
                    this.hpkProductReviews.NavigateUrl = string.Format("LookProductReviews.aspx?productId={0}", this.productId);
                    this.LoadProductInfo(info.Product, info.BrandName);
                    this.LoadProductGroupBuyInfo(countDownInfo);
                    this.btnOrder.Stock = info.Product.Stock;
                    BrowsedProductQueue.EnQueue(this.productId);
                    this.images.ImageInfo = info.Product;
                    if (info.DbAttribute != null)
                    {
                        this.rptExpandAttributes.DataSource = info.DbAttribute;
                        this.rptExpandAttributes.DataBind();
                    }
                    if (info.DbSKUs != null)
                    {
                        this.skuSelector.ProductId = this.productId;
                        this.skuSelector.DataSource = info.DbSKUs;
                    }
                    if (info.DBReviews != null)
                    {
                        this.reviews.DataSource = info.DBReviews;
                        this.reviews.DataBind();
                    }
                    if (info.DBConsultations != null)
                    {
                        this.consultations.DataSource = info.DBConsultations;
                        this.consultations.DataBind();
                    }
                    if (info.DbCorrelatives != null)
                    {
                        this.correlative.DataSource = info.DbCorrelatives;
                        this.correlative.DataBind();
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

       void LoadProductGroupBuyInfo(CountDownInfo countDownInfo)
        {
            this.lblCurrentSalePrice.Money = countDownInfo.CountDownPrice;
            this.litContent.Text = countDownInfo.Content;
            this.lblTotalPrice.Value = new decimal?(countDownInfo.CountDownPrice);
            this.lblEndTime.Time = countDownInfo.EndDate;
            this.litRemainTime.Text = "待实现";
        }

       void LoadProductInfo(ProductInfo productDetails, string brandName)
        {
            if ((this.common_Location != null) && !string.IsNullOrEmpty(productDetails.MainCategoryPath))
            {
                this.common_Location.CateGoryPath = productDetails.MainCategoryPath.Remove(productDetails.MainCategoryPath.Length - 1);
                this.common_Location.ProductName = productDetails.ProductName;
            }
            this.litProductCode.Text = productDetails.ProductCode;
            this.litProductName.Text = productDetails.ProductName;
            this.lblSku.Value = productDetails.SkuId;
            this.lblSku.Text = productDetails.SKU;
            this.lblStock.Stock = productDetails.Stock;
            this.litUnit.Text = productDetails.Unit;
            if (productDetails.Weight > 0)
            {
                this.litWeight.Text = string.Format("{0} g", productDetails.Weight);
            }
            else
            {
                this.litWeight.Text = "无";
            }
            this.litBrosedNum.Text = productDetails.VistiCounts.ToString();
            this.litBrand.Text = brandName;
            this.lblSalePrice.Money = productDetails.MaxSalePrice;
            this.litDescription.Text = productDetails.Description;
            if (this.litShortDescription != null)
            {
                this.litShortDescription.Text = productDetails.ShortDescription;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-CountDownProductsDetails.html";
            }
            base.OnInit(e);
        }
    }
}

