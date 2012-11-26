using Hidistro.Core;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web
{
    public partial class loginEntry : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            string returnUrl = Page.Request.QueryString["returnUrl"].ToLower();

            if (!(string.IsNullOrEmpty(returnUrl) || !returnUrl.StartsWith(Globals.GetSiteUrls().Locations["admin"].ToLower())))
            {
                
                Response.Redirect(Globals.GetAdminAbsolutePath("/login.aspx?returnUrl=" + returnUrl), true);

            }

            else if (!(string.IsNullOrEmpty(returnUrl) || !returnUrl.StartsWith(Globals.GetSiteUrls().Locations["distributor"].ToLower())))
            {

                Response.Redirect(Globals.ApplicationPath + "/shopadmin/DistributorLogin.aspx?returnUrl=" + returnUrl, true);

            }
            else
            {

                Response.Redirect(Globals.GetSiteUrls().Login, true);

            }

        }


    }


}

