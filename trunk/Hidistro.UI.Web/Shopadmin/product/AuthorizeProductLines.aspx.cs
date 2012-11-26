using ASPNET.WebControls;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 分销授权产品线
    /// </summary>
    public partial class AuthorizeProductLines : DistributorPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

         void BindData()
        {
            grdProductLine.DataSource = SubSiteProducthelper.GetAuthorizeProductLines();
            grdProductLine.DataBind();
        }

    }
}

