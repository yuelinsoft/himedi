using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class EditProductComplete : AdminPage
    {
        int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["productId"], out productId))
            {
                base.GotoResourceNotFound();
            }
            else if (!Page.IsPostBack)
            {
                hlinkProductDetails.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { productId });
            }
        }
    }
}

