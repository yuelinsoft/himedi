namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Style : Literal
    {
        
       string _Media ;
       string href;
       const string linkFormat = "<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />";

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Href))
            {
                writer.Write("<link rel=\"stylesheet\" href=\"{0}\" type=\"text/css\" media=\"{1}\" />", Href, Media);
            }
        }

        public virtual string Href
        {
            get
            {
                if (string.IsNullOrEmpty(href))
                {
                    return null;
                }
                if (href.StartsWith("/"))
                {
                    return (Globals.ApplicationPath + href);
                }
                return (Globals.ApplicationPath + "/" + href);
            }
            set
            {
                href = value;
            }
        }

        [DefaultValue("screen")]
        public string Media
        {
            
            get
            {
                return _Media ;
            }
            
            set
            {
                _Media  = value;
            }
        }
    }
}

