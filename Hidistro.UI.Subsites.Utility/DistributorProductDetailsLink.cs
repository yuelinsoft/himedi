using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorProductDetailsLink : HyperLink
    {
        
        bool _IsCountDownProduct;
        bool _IsGroupBuyProduct;
        bool imageLink;
        public const string TagID = "DistributorProductDetailsLink";
        bool unSale;

        public DistributorProductDetailsLink()
        {
            base.ID = "DistributorProductDetailsLink";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((ProductId != null) && (ProductId != DBNull.Value))
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (IsGroupBuyProduct)
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("groupBuyProductDetails", new object[] { ProductId });
                }
                else if (IsCountDownProduct)
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("countdownProductsDetails", new object[] { ProductId });
                }
                else if (unSale)
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[] { ProductId });
                }
                else
                {
                    base.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { ProductId });
                }
            }
            if ((!imageLink && (ProductId != null)) && (ProductId != DBNull.Value))
            {
                base.Text = ProductName.ToString();
            }
            base.Target = "_blank";
            base.Render(writer);
        }

        public bool ImageLink
        {
            get
            {
                return imageLink;
            }
            set
            {
                imageLink = value;
            }
        }

        public bool IsCountDownProduct
        {

            get
            {
                return _IsCountDownProduct;
            }

            set
            {
                _IsCountDownProduct = value;
            }
        }

        public bool IsGroupBuyProduct
        {

            get
            {
                return _IsGroupBuyProduct;
            }

            set
            {
                _IsGroupBuyProduct = value;
            }
        }

        public object ProductId
        {
            get
            {
                if (ViewState["ProductId"] != null)
                {
                    return ViewState["ProductId"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    ViewState["ProductId"] = value;
                }
            }
        }

        public object ProductName
        {
            get
            {
                if (ViewState["ProductName"] != null)
                {
                    return ViewState["ProductName"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    ViewState["ProductName"] = value;
                }
            }
        }

        public bool UnSale
        {
            get
            {
                return unSale;
            }
            set
            {
                unSale = value;
            }
        }
    }
}

