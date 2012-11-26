namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ProductDetailsLink : HyperLink
    {
        
       int? _StringLenth ;
       bool imageLink;
       bool isCountDownProduct;
        public bool isGroupBuyProduct;
       bool isUn;
        public const string TagID = "ProductDetailsLink";

        public ProductDetailsLink()
        {
            base.ID = "ProductDetailsLink";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((ProductId != null) && (ProductId != DBNull.Value))
            {
                if (isGroupBuyProduct)
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("groupBuyProductDetails", new object[] { ProductId });
                }
                else if (IsCountDownProduct)
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("countdownProductsDetails", new object[] { ProductId });
                }
                else if (isUn)
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("unproductdetails", new object[] { ProductId });
                }
                else
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { ProductId });
                }
            }
            if ((!imageLink && (ProductId != null)) && (ProductId != DBNull.Value))
            {
                if (StringLenth.HasValue && (ProductName.ToString().Length > StringLenth.Value))
                {
                    base.Text = ProductName.ToString().Substring(0, StringLenth.Value) + "...";
                }
                else
                {
                    base.Text = ProductName.ToString();
                }
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
                return isCountDownProduct;
            }
            set
            {
                isCountDownProduct = value;
            }
        }

        public bool IsGroupBuyProduct
        {
            get
            {
                return isGroupBuyProduct;
            }
            set
            {
                isGroupBuyProduct = value;
            }
        }

        public bool IsUn
        {
            get
            {
                return isUn;
            }
            set
            {
                isUn = value;
            }
        }

        public object ProductId
        {
            get
            {
                return ViewState["ProductId"];
            }
            set
            {
                ViewState["ProductId"] = value;
            }
        }

        public object ProductName
        {
            get
            {
                return ViewState["ProductName"];
            }
            set
            {
                ViewState["ProductName"] = value;
            }
        }

        public int? StringLenth
        {
            
            get
            {
                return _StringLenth ;
            }
            
            set
            {
                _StringLenth  = value;
            }
        }
    }
}

