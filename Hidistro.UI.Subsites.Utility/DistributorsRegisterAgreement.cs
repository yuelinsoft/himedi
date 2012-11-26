using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorsRegisterAgreement : HtmlTemplatedWebControl
    {
        Literal litRequestInstruction;
        Literal litRequestProtocols;

        protected override void AttachChildControls()
        {
            litRequestInstruction = (Literal)FindControl("litRequestInstruction");
            litRequestProtocols = (Literal)FindControl("litRequestProtocols");
            if (!Page.IsPostBack)
            {
                SiteSettings siteSettings = HiContext.Current.SiteSettings;
                litRequestInstruction.Text = siteSettings.DistributorRequestInstruction;
                litRequestProtocols.Text = siteSettings.DistributorRequestProtocols;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
            }
            if (SkinName == null)
            {
                SkinName = "Skin-DistributorsRegisterAgreement.html";
            }
            base.OnInit(e);
        }
    }
}

