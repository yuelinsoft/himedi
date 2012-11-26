using Hidistro.ControlPanel.Comments;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class ReplyedLeaveCommentsSuccsed : AdminPage
    {
        long leaveId;

        private void BindList()
        {
            dtlistLeaveCommentsReply.DataSource = NoticeHelper.GetReplyLeaveComments(leaveId);
            dtlistLeaveCommentsReply.DataBind();
        }

        private void dtlistLeaveCommentsReply_DeleteCommand(object sender, DataListCommandEventArgs e)
        {
            if (NoticeHelper.DeleteLeaveCommentReply(Convert.ToInt64(dtlistLeaveCommentsReply.DataKeys[e.Item.ItemIndex])))
            {
                ShowMsg("删除成功", true);
                BindList();
            }
            else
            {
                ShowMsg("删除失败，请重试", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(Page.Request.QueryString["leaveId"], out leaveId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                dtlistLeaveCommentsReply.DeleteCommand += new DataListCommandEventHandler(dtlistLeaveCommentsReply_DeleteCommand);
                if (!base.IsPostBack)
                {
                    SetControl(leaveId);
                    hlReply.NavigateUrl = Globals.ApplicationPath + "/admin/comment/ReplyLeaveComments.aspx?LeaveId=" + leaveId;
                    BindList();
                }
            }
        }

        private void SetControl(long leaveId)
        {
            LeaveCommentInfo leaveComment = NoticeHelper.GetLeaveComment(leaveId);
            Globals.EntityCoding(leaveComment, false);
            litTitle.Text = leaveComment.Title;
            lblUserName.Text = leaveComment.UserName;
            litLeaveDate.Time = leaveComment.PublishDate;
            litPublishContent.Text = leaveComment.PublishContent;
        }
    }
}

