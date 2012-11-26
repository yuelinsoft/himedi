    using Hidistro.ControlPanel.Comments;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using kindeditor.Net;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ManageLeaveComments)]
    public partial class ReplyLeaveComments : AdminPage
    {
        long leaveId;


        protected void btnReplyLeaveComments_Click(object sender, EventArgs e)
        {
            LeaveCommentReplyInfo target = new LeaveCommentReplyInfo();
            target.LeaveId = leaveId;
            if (string.IsNullOrEmpty(fckReplyContent.Text))
            {
                target.ReplyContent = null;
            }
            else
            {
                target.ReplyContent = fckReplyContent.Text;
            }
            target.UserId = HiContext.Current.User.UserId;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<LeaveCommentReplyInfo>(target, new string[] { "ValLeaveCommentReply" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else
            {
                if (NoticeHelper.ReplyLeaveComment(target) > 0)
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/comment/ReplyedLeaveCommentsSuccsed.aspx?leaveId={0}", leaveId)), true);
                }
                else
                {
                    ShowMsg("回复客户留言失败", false);
                }
                fckReplyContent.Text = string.Empty;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(Page.Request.QueryString["LeaveId"], out leaveId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnReplyLeaveComments.Click += new EventHandler(btnReplyLeaveComments_Click);
                if (!Page.IsPostBack)
                {
                    LeaveCommentInfo leaveComment = NoticeHelper.GetLeaveComment(leaveId);
                    litTitle.Text = Globals.HtmlDecode(leaveComment.Title);
                    litContent.Text = Globals.HtmlDecode(leaveComment.PublishContent);
                    litUserName.Text = Globals.HtmlDecode(leaveComment.UserName);
                }
            }
        }
    }
}

