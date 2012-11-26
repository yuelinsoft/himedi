namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_Logo : PlaceHolder
    {
       int height;
       int width;

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(HiContext.Current.SiteSettings.LogoUrl))
            {
                HiImage image = new HiImage();
                image.ImageUrl = HiContext.Current.SiteSettings.LogoUrl;
                writer.Write(string.Format("<a href=\"{0}\">", Globals.GetSiteUrls().UrlData.FormatUrl("home")));
                image.RenderControl(writer);
                writer.Write("</a>");
            }

          //  writer.Write("<script type=\"text/javascript\" src=\"http://v3.jiathis.com/code/jiathis_r.js?uid=1334751641000509&move=0&amp;btn=r4.gif\" charset=\"utf-8\"></script>");

        }

        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        public int Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

