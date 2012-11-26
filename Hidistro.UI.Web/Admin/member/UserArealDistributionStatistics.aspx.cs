using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MemberArealDistributionStatistics)]
    public partial class UserArealDistributionStatistics : AdminPage
    {
        private void BindUserStatistics()
        {
            int totalProductSaleVisits = 0;
            Pagination page = new Pagination();
            page.SortBy = grdUserStatistics.SortOrderBy;
            if (grdUserStatistics.SortOrder.ToLower() == "desc")
            {
                page.SortOrder = SortAction.Desc;
            }
            grdUserStatistics.DataSource = SalesHelper.GetUserStatistics(page, out totalProductSaleVisits);
            grdUserStatistics.DataBind();
        }

        private void grdUserStatistics_ReBindData(object sender)
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
            grdUserStatistics.RowDataBound += new GridViewRowEventHandler(grdUserStatistics_RowDataBound);
            grdUserStatistics.ReBindData += new Grid.ReBindDataEventHandler(grdUserStatistics_ReBindData);
            grdUserStatistics.RowDataBound += new GridViewRowEventHandler(grdUserStatistics_RowDataBound);
            if (!base.IsPostBack)
            {
                BindUserStatistics();
            }
        }
    }
}

