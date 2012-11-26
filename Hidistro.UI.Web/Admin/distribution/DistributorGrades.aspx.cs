using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorGrades)]
    public partial class DistributorGrades : AdminPage
    {
        private void BindDistributorRanks()
        {
            grdDistributorRankList.DataSource = DistributorHelper.GetDistributorGrades();
            grdDistributorRankList.DataBind();
        }

        private void grdDistributorRankList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int gradeId = (int)grdDistributorRankList.DataKeys[e.RowIndex].Value;
            if (DistributorHelper.DeleteDistributorGrade(gradeId))
            {
                BindDistributorRanks();
                ShowMsg("已经成功删除选择的分销商等级", true);
            }
            else
            {
                ShowMsg("不能删除有分销商的分销商等级", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdDistributorRankList.RowDeleting += new GridViewDeleteEventHandler(grdDistributorRankList_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDistributorRanks();
            }
        }
    }
}

