using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnderlingArealDistributionStatistics : DistributorPage
    {

        private void BindUserStatistics()
        {
            int totalProductSaleVisits = 0;
            Pagination pagination2 = new Pagination();
            pagination2.SortBy = grdUserStatistics.SortOrderBy;
            Pagination page = pagination2;
            if (grdUserStatistics.SortOrder.ToLower() == "desc")
            {
                page.SortOrder = SortAction.Desc;
            }
            grdUserStatistics.DataSource = SubsiteSalesHelper.GetUserStatistics(page, out totalProductSaleVisits);
            grdUserStatistics.DataBind();
        }

        protected void grdUserStatistics_ReBindData(object sender)
        {
            BindUserStatistics();
        }

        private void grdUserStatistics_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int currentRegionId = int.Parse(grdUserStatistics.DataKeys[e.Row.RowIndex].Value.ToString(), NumberStyles.None);
                Label label = (Label)e.Row.FindControl("lblReionName");
                if ((currentRegionId != 0) && (label != null))
                {
                    label.Text = RegionHelper.GetFullRegion(currentRegionId, "");
                }
                if ((currentRegionId == 0) && (label != null))
                {
                    label.Text = "其它";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdUserStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdUserStatistics_ReBindData);
            grdUserStatistics.RowDataBound += new GridViewRowEventHandler(grdUserStatistics_RowDataBound);
            if (!base.IsPostBack)
            {
                BindUserStatistics();
            }
        }
    }
}

