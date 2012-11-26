namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Globalization;
    using System.Web.UI;

    public class DisplayThemesImages : Control
    {
       string imageFormat = "<a><img border=\"0\" src=\"{0}\" /></a>";
       bool isDistributorThemes;

        protected string GetImagePath()
        {
            if (IsDistributorThemes)
            {
                return string.Concat(new object[] { Globals.ApplicationPath, "/Templates/sites/", HiContext.Current.User.UserId, "/", ThemeName });
            }
            return (Globals.ApplicationPath + "/Templates/library/" + ThemeName);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Src.StartsWith("~"))
            {
                Src = base.ResolveUrl(Src);
            }
            else if (Src.StartsWith("/"))
            {
                Src = GetImagePath() + Src;
            }
            else
            {
                Src = GetImagePath() + "/" + Src;
            }
            writer.Write(string.Format(CultureInfo.InvariantCulture, imageFormat, new object[] { Src }));
        }

        public bool IsDistributorThemes
        {
            get
            {
                return isDistributorThemes;
            }
            set
            {
                isDistributorThemes = value;
            }
        }

        public string Src
        {
            get
            {
                if (ViewState["Src"] == null)
                {
                    return null;
                }
                return (string) ViewState["Src"];
            }
            set
            {
                ViewState["Src"] = value;
            }
        }

        public string ThemeName
        {
            get
            {
                if (ViewState["ThemeName"] == null)
                {
                    return null;
                }
                return (string) ViewState["ThemeName"];
            }
            set
            {
                ViewState["ThemeName"] = value;
            }
        }
    }
}

