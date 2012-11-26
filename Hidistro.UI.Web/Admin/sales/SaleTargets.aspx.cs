using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.SaleTargets)]
    public partial class SaleTargets : AdminPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DbQueryResult saleTargets = SalesHelper.GetSaleTargets();
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

