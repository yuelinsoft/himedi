namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Script : Literal
    {
       string src;
       const string srcFormat = "<script src=\"{0}\" type=\"text/javascript\"></script>";

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Src))
            {
                writer.Write("<script src=\"{0}\" type=\"text/javascript\"></script>", Src);
            }
        }

        public virtual string Src
        {
            get
            {
                if (string.IsNullOrEmpty(src))
                {
                    return null;
                }
                if (src.StartsWith("/"))
                {
                    return (Globals.ApplicationPath + src);
                }
                return (Globals.ApplicationPath + "/" + src);
            }
            set
            {
                src = value;
            }
        }
    }
}

