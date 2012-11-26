namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class UserSendedMessageDetails : MemberTemplatedWebControl
    {
       Literal litAddresser;
       Literal litContent;
       FormatedTimeLabel litDate;
       Literal litTitle;

        protected override void AttachChildControls()
        {
            this.litAddresser = (Literal) this.FindControl("litAddresser");
            this.litTitle = (Literal) this.FindControl("litTitle");
            this.litDate = (FormatedTimeLabel) this.FindControl("litDate");
            this.litContent = (Literal) this.FindControl("litContent");
            if (!this.Page.IsPostBack)
            {
                this.BindSendMessage();
            }
        }

       void BindSendMessage()
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["SendMessageId"]))
            {
                base.GotoResourceNotFound();
            }
            SendMessageInfo sendedMessage = CommentsHelper.GetSendedMessage(long.Parse(this.Page.Request.QueryString["SendMessageId"]));
            this.litAddresser.Text = HiContext.Current.User.Username;
            this.litTitle.Text = sendedMessage.Title;
            this.litDate.Time = sendedMessage.PublishDate;
            this.litContent.Text = sendedMessage.PublishContent;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserSendedMessageDetails.html";
            }
            base.OnInit(e);
        }
    }
}

