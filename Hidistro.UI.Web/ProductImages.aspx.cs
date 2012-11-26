using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web
{
    public partial class ProductImages : Page
    {
        private void BindImages(ProductInfo prductImageInfo)
        {
            productName.Text = prductImageInfo.ProductName;

            productName.NavigateUrl = Utils.ApplicationPath + "/ProductDetails.aspx?ProductId=" + prductImageInfo.ProductId;

            imgBig.Src = image1url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl1;

            image2url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl2;

            image3url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl3;

            image4url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl4;

            image5url.Value = Utils.ApplicationPath + prductImageInfo.ImageUrl5;

            if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl1))
            {
                image1.ImageUrl = prductImageInfo.ImageUrl1.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
            }

            if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl2))
            {
                image2.ImageUrl = prductImageInfo.ImageUrl2.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
            }

            if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl3))
            {
                image3.ImageUrl = prductImageInfo.ImageUrl3.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
            }

            if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl4))
            {
                image4.ImageUrl = prductImageInfo.ImageUrl4.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
            }

            if (!string.IsNullOrEmpty(prductImageInfo.ImageUrl5))
            {
                image5.ImageUrl = prductImageInfo.ImageUrl5.Replace("/Storage/master/product/images/", "/Storage/master/product/thumbs40/40_");
            }


            if ((((prductImageInfo.ImageUrl1 == null) && (prductImageInfo.ImageUrl2 == null)) && ((prductImageInfo.ImageUrl3 == null) && (prductImageInfo.ImageUrl4 == null))) && (prductImageInfo.ImageUrl5 == null))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                imgBig.Src = Globals.ApplicationPath + masterSettings.DefaultProductImage;
                imgBig.Align = "center";
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int result = 0;

            int.TryParse(Page.Request.QueryString["ProductId"], out result);

            if (!Page.IsPostBack)
            {
                ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(result);

                if (productSimpleInfo != null)
                {
                    BindImages(productSimpleInfo);
                }

                if (!string.IsNullOrEmpty(productSimpleInfo.Title))
                {

                    PageTitle.AddTitle(productSimpleInfo.Title, HiContext.Current.Context);

                }
                else
                {

                    PageTitle.AddTitle(productSimpleInfo.ProductName, HiContext.Current.Context);

                }

            }

        }

    }

}

