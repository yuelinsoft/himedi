namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.Caching;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class Common_ShowAds : Literal
    {
        
       string _AdsName;

       XmlDocument GetAdsDocument()
        {
            string key = "AdsFileCache-Admin";
            if (HiContext.Current.SiteSettings.UserId.HasValue)
            {
                key = string.Format("AdsFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
            }
            XmlDocument document = HiCache.Get(key) as XmlDocument;
            if (document == null)
            {
                string filename = HiContext.Current.Context.Request.MapPath(HiContext.Current.GetSkinPath() + "/AdvPositions.xml");
                document = new XmlDocument();
                document.Load(filename);
                HiCache.Max(key, document, new CacheDependency(filename));
            }
            return document;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (XmlElement element in this.GetAdsDocument().SelectSingleNode("root").ChildNodes)
            {
                if (element.ChildNodes[0].InnerText == this.AdsName)
                {
                    base.Text = element.ChildNodes[1].InnerText;
                    break;
                }
            }
            base.Render(writer);
        }

        public string AdsName
        {
            
            get
            {
                return this._AdsName;
            }
            
            set
            {
                this._AdsName = value;
            }
        }
    }
}

