namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_GroupBuyLink : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (HiContext.Current.SiteSettings.IsShowGroupBuy)
            {
                writer.Write("<span>");
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("GroupBuyProducts");
                base.Render(writer);
                writer.Write("</span>");
            }
        }
    }
}

