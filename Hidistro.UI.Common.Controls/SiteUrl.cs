namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SiteUrl : HyperLink
    {
       string requstName;
       string urlName;

        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(base.NavigateUrl) && !string.IsNullOrEmpty(UrlName))
            {
                if (!string.IsNullOrEmpty(RequstName))
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(UrlName, new object[] { Page.Request.QueryString[RequstName] });
                }
                else
                {
                    base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(UrlName);
                }
            }
            base.Render(writer);
        }

        public string RequstName
        {
            get
            {
                return requstName;
            }
            set
            {
                requstName = value;
            }
        }

        public string UrlName
        {
            get
            {
                return urlName;
            }
            set
            {
                urlName = value;
            }
        }
    }
}

