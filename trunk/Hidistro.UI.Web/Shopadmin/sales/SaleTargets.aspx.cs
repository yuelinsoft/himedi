using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class SaleTargets : DistributorPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DbQueryResult saleTargets = SubsiteSalesHelper.GetSaleTargets();
                grdOrderAvPrice.DataSource = saleTargets.Data;
                grdOrderAvPrice.DataBind();
                grdOrderTranslatePercentage.DataSource = saleTargets.Data;
                grdOrderTranslatePercentage.DataBind();
                grdUserOrderAvNumb.DataSource = saleTargets.Data;
                grdUserOrderAvNumb.DataBind();
                grdVisitOrderAvPrice.DataSource = saleTargets.Data;
                grdVisitOrderAvPrice.DataBind();
                grdUserOrderPercentage.DataSource = saleTargets.Data;
                grdUserOrderPercentage.DataBind();
            }
        }
    }
}

