namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web;
    using System.Web.UI;

    [PersistChildren(true), ParseChildren(false)]
    public class MetaTags : Control
    {
       const string metaDescriptionKey = "Hishop.Meta_Description";
       const string metaFormat = "<meta name=\"{0}\" content=\"{1}\" />";
       const string metaKeywordsKey = "Hishop.Meta_Keywords";

        public static void AddMetaDescription(string value, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Items["Hishop.Meta_Description"] = value;
        }

        public static void AddMetaKeywords(string value, HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Items["Hishop.Meta_Keywords"] = value;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderMetaDescription(writer);
            RenderMetaKeywords(writer);
        }

        protected virtual void RenderMetaDescription(HtmlTextWriter writer)
        {
            if (Context.Items.Contains("Hishop.Meta_Description"))
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "description", Context.Items["Hishop.Meta_Description"]);
            }
            else
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "description", HiContext.Current.SiteSettings.SearchMetaDescription);
            }
        }

        protected virtual void RenderMetaKeywords(HtmlTextWriter writer)
        {
            if (Context.Items.Contains("Hishop.Meta_Keywords"))
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "keywords", Context.Items["Hishop.Meta_Keywords"]);
            }
            else
            {
                writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", "keywords", HiContext.Current.SiteSettings.SearchMetaKeywords);
            }
        }
    }
}

