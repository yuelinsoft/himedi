namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.WebControls;

    public class ReplyReceivedMessage : MemberTemplatedWebControl
    {
       Button btnReplyReceivedMessage;
       Literal litAddresser;
       Literal litContent;
       FormatedTimeLabel litDate;
       Literal litTitle;
       long receiveMessageId;
       TextBox txtReplyContent;
       TextBox txtReplyTitle;

        protected override void AttachChildControls()
        {
            this.litAddresser = (Literal) this.FindControl("litAddresser");
            this.litTitle = (Literal) this.FindControl("litTitle");
            this.litDate = (FormatedTimeLabel) this.FindControl("litDate");
            this.litContent = (Literal) this.FindControl("litContent");
            this.txtReplyTitle = (TextBox) this.FindControl("txtReplyTitle");
            this.txtReplyContent = (TextBox) this.FindControl("txtReplyContent");
            this.btnReplyReceivedMessage = (Button) this.FindControl("btnReplyReceivedMessage");
            this.btnReplyReceivedMessage.Click += new EventHandler(this.btnReplyReceivedMessage_Click);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ReceiveMessageId"]))
            {
                this.receiveMessageId = long.Parse(this.Page.Request.QueryString["ReceiveMessageId"]);
            }
            if (!this.Page.IsPostBack)
            {
                this.BindReceivedMessage();
            }
        }

       void BindReceivedMessage()
        {
            ReceiveMessageInfo receiveMessage = CommentsHelper.GetReceiveMessage(this.receiveMessageId);
            this.litAddresser.Text = "管理员";
            this.litTitle.Text = receiveMessage.Title;
            this.litContent.Text = receiveMessage.PublishContent;
            this.litDate.Time = receiveMessage.PublishDate;
            CommentsHelper.PostMessageIsRead(this.receiveMessageId);
        }

       void btnReplyReceivedMessage_Click(object sender, EventArgs e)
        {
            string str = "";
            if (string.IsNullOrEmpty(this.txtReplyTitle.Text) || (this.txtReplyTitle.Text.Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(this.txtReplyContent.Text) || (this.txtReplyContent.Text.Length > 300))
            {
                str = str + Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMessage(str, false);
            }
            else
            {
                SendMessageInfo reply = new SendMessageInfo();
                reply.Addresser = HiContext.Current.User.Username;
                reply.Addressee = CommentsHelper.GetReceiveMessage(this.receiveMessageId).Addresser;
                reply.Title = this.txtReplyTitle.Text;
                reply.PublishContent = this.txtReplyContent.Text;
                reply.PublishDate = DateTime.Now;
                reply.ReceiveMessageId = new long?(this.receiveMessageId);
                if (CommentsHelper.ReplyMessage(reply))
                {
                    this.ShowMessage("回复成功", true);
                }
                else
                {
                    this.ShowMessage("回复失败", false);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-ReplyReceivedMessage.html";
            }
            base.OnInit(e);
        }
    }
}

