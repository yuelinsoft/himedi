using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Membership.Context;
using System;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
    [PersistChildren(true), ParseChildren(false)]
    public class HeadContainer : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            HiContext current = HiContext.Current;
            writer.Write("<script language=\"javascript\" type=\"text/javascript\"> \r\n            var applicationPath = \"{0}\";\r\n            var skinPath = \"{1}\";\r\n            var subsiteuserId = \"{2}\";\r\n        </script>", Globals.ApplicationPath, current.GetSkinPath(), current.SiteSettings.UserId.HasValue ? current.SiteSettings.UserId.Value.ToString() : "0");
            //writer.Write("<script language=\"javascript\" src=\"http://code.54kefu.net/kefu/js/197/262597.js\" charset=\"utf-8\"></script>");
           // writer.Write("<!-- JiaThis Button BEGIN -->");
            //writer.WriteLine(); 
            //writer.Write("<script type=\"text/javascript\" src=\"http://v3.jiathis.com/code/jiathis_r.js?uid=1334751641000509&move=0&amp;btn=r4.gif\" charset=\"utf-8\"></script>");
           // writer.Write("<!-- JiaThis Button END -->");
            writer.WriteLine();
            RenderMetaCharset(writer);
            RenderMetaLanguage(writer);
            RenderFavicon(writer);
            RenderMetaAuthor(writer);
            RenderMetaGenerator(writer);
        }

        void RenderFavicon(HtmlTextWriter writer)
        {
            string str = Globals.FullPath(Globals.GetSiteUrls().Favicon);
            writer.WriteLine("<link rel=\"icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", str);
            writer.WriteLine("<link rel=\"shortcut icon\" type=\"image/x-icon\" href=\"{0}\" media=\"screen\" />", str);
        }

        void RenderMetaAuthor(HtmlTextWriter writer)
        {
            writer.WriteLine(string.Format("<meta name=\"author\" content=\"{0}\" />", SettingsManager.GetMasterSettings(true).Author));
        }

        void RenderMetaCharset(HtmlTextWriter writer)
        {
            switch (HiContext.Current.Config.AppLocation.CurrentApplicationType)
            {
                case ApplicationType.Admin:
                case ApplicationType.Installer:
                    {
                        writer.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\" />");
                        break;
                    }
            }
        }

        void RenderMetaGenerator(HtmlTextWriter writer)
        {
            writer.WriteLine(string.Format("<meta name=\"GENERATOR\" content=\"{0}\" />", SettingsManager.GetMasterSettings(true).Generator));
        }

        void RenderMetaLanguage(HtmlTextWriter writer)
        {
            writer.WriteLine("<meta http-equiv=\"content-language\" content=\"zh-CN\" />");
        }
    }
}

