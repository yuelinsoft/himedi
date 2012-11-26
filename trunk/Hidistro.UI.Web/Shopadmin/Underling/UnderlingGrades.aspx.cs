using ASPNET.WebControls;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnderlingGrades : DistributorPage
    {

        private void BindUnderlingGrades()
        {
            grdUnderlingGrades.DataSource = UnderlingHelper.GetUnderlingGrades();
            grdUnderlingGrades.DataBind();
        }

        private void grdUnderlingGrades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SetYesOrNo")
            {
                GridViewRow namingContainer = (GridViewRow)((Control)e.CommandSource).NamingContainer;
                int gradeId = (int)grdUnderlingGrades.DataKeys[namingContainer.RowIndex].Value;
                UnderlingHelper.SetDefalutUnderlingGrade(gradeId);
                BindUnderlingGrades();
            }
        }

        private void grdUnderlingGrades_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int gradeId = (int)grdUnderlingGrades.DataKeys[e.RowIndex].Value;
            if (UnderlingHelper.DeleteUnderlingGrade(gradeId))
            {
                BindUnderlingGrades();
                ShowMsg("已经成功删除选择的会员等级", true);
            }
            else
            {
                ShowMsg("不能删除默认的会员等级或有会员的等级", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdUnderlingGrades.RowDeleting += new GridViewDeleteEventHandler(grdUnderlingGrades_RowDeleting);
            grdUnderlingGrades.RowCommand += new GridViewCommandEventHandler(grdUnderlingGrades_RowCommand);
            if (!Page.IsPostBack)
            {
                BindUnderlingGrades();
            }
        }
    }
}

