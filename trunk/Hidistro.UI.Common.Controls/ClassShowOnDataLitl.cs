namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ClassShowOnDataLitl : Literal
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(base.Text))
            {
                base.Text = string.Format("<span>{0}</span>", DefaultText);
            }
            else
            {
                base.Text = string.Format("<span style=\"{0}\" class=\"{1}\">{2}</span>", Style, Class, base.Text);
                if (IsShowLink)
                {
                    base.Text = string.Format("<a href=\"{0}\">{1}</a>", Link, base.Text);
                }
            }
            base.Render(writer);
        }

        public string Class
        {
            get
            {
                if (ViewState["Class"] == null)
                {
                    return string.Empty;
                }
                return (string) ViewState["Class"];
            }
            set
            {
                ViewState["Class"] = value;
            }
        }

        public string DefaultText
        {
            get
            {
                if (ViewState["DefaultText"] == null)
                {
                    return string.Empty;
                }
                return (string) ViewState["DefaultText"];
            }
            set
            {
                ViewState["DefaultText"] = value;
            }
        }

        public bool IsShowLink
        {
            get
            {
                if (ViewState["IsShowLink"] == null)
                {
                    return false;
                }
                return (bool) ViewState["IsShowLink"];
            }
            set
            {
                ViewState["IsShowLink"] = value;
            }
        }

        public string Link
        {
            get
            {
                if (ViewState["Link"] == null)
                {
                    return string.Empty;
                }
                return (Globals.GetSiteUrls().UrlData.FormatUrl((string) ViewState["Link"]) + LinkQuery);
            }
            set
            {
                ViewState["Link"] = value;
            }
        }

        public string LinkQuery
        {
            get
            {
                if (ViewState["LinkQuery"] == null)
                {
                    return string.Empty;
                }
                return (string) ViewState["LinkQuery"];
            }
            set
            {
                ViewState["LinkQuery"] = value;
            }
        }

        public string Style
        {
            get
            {
                if (ViewState["Style"] == null)
                {
                    return string.Empty;
                }
                return (string) ViewState["Style"];
            }
            set
            {
                ViewState["Style"] = value;
            }
        }
    }
}

