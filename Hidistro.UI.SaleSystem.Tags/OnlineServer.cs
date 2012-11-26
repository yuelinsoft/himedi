namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI.WebControls;

    public class OnlineServer : Literal
    {
        protected override void OnLoad(EventArgs e)
        {
            base.Text = HiContext.Current.SiteSettings.HtmlOnlineServiceCode;
        }
    }
}

