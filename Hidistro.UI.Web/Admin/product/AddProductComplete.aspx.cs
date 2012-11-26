
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AddProducts)]
    public partial class AddProductComplete : AdminPage
    {
        int categoryId;
        int productId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(base.Request.QueryString["categoryId"], out categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!int.TryParse(base.Request.QueryString["productId"], out productId))
            {
                base.GotoResourceNotFound();
            }
            else if (!Page.IsPostBack)
            {
                hlinkProductDetails.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { productId });
                hlinkAddProduct.NavigateUrl = Globals.GetAdminAbsolutePath(string.Format("/product/AddProduct.aspx?categoryId={0}", categoryId));
            }
        }
    }
}

