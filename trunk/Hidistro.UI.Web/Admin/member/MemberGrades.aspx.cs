using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MemberGrades)]
    public partial class MemberGrades : AdminPage
    {
        private void BindMemberRanks()
        {
            grdMemberRankList.DataSource = MemberHelper.GetMemberGrades();
            grdMemberRankList.DataBind();
        }

        private void grdMemberRankList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int gradeId = (int)grdMemberRankList.DataKeys[namingContainer.RowIndex].Value;
                if (!MemberHelper.GetMemberGrade(gradeId).IsDefault)
                {
                    MemberHelper.SetDefalutMemberGrade(gradeId);
                    BindMemberRanks();
                }
            }
        }

        private void grdMemberRankList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int gradeId = (int)grdMemberRankList.DataKeys[e.RowIndex].Value;
            if (MemberHelper.DeleteMemberGrade(gradeId))
            {
                BindMemberRanks();
                ShowMsg("已经成功删除选择的会员等级", true);
            }
            else
            {
                ShowMsg("不能删除默认的会员等级或有会员的等级", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdMemberRankList.RowDeleting += new GridViewDeleteEventHandler(grdMemberRankList_RowDeleting);
            grdMemberRankList.RowCommand += new GridViewCommandEventHandler(grdMemberRankList_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindMemberRanks();
            }
        }
    }
}

