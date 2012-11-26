using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorDetailsHyperLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if ((DetailsId != null) && (DetailsId != DBNull.Value))
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                base.NavigateUrl = string.Concat(new object[] { "http://", siteSettings.SiteUrl, Globals.ApplicationPath, DetailsPageUrl, DetailsId });
            }
            if ((DetailsId != null) && (DetailsId != DBNull.Value))
            {
                base.Text = Title.ToString();
            }
            base.Target = "_blank";
            base.Render(writer);
        }

        public object DetailsId
        {
            get
            {
                if (ViewState["DetailsId"] != null)
                {
                    return ViewState["DetailsId"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    ViewState["DetailsId"] = value;
                }
            }
        }

        public string DetailsPageUrl
        {
            get
            {
                if (ViewState["DetailsPageUrl"] != null)
                {
                    return ViewState["DetailsPageUrl"].ToString();
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    ViewState["DetailsPageUrl"] = value;
                }
            }
        }

        public object Title
        {
            get
            {
                if (ViewState["Title"] != null)
                {
                    return ViewState["Title"];
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    ViewState["Title"] = value;
                }
            }
        }
    }
}

