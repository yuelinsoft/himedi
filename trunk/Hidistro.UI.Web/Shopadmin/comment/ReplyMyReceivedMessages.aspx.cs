namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public partial class ReplyMyReceivedMessages : DistributorPage
    {

         long receiveMessageId;
 

        private void BindReplyReceivedMessages()
        {
            ReceiveMessageInfo receiveMessage = SubsiteCommentsHelper.GetReceiveMessage(this.receiveMessageId);
            if (receiveMessage != null)
            {
                this.litAddresser.Text = receiveMessage.Addresser;
                this.litTitle.Text = receiveMessage.Title;
                this.litContent.Text = receiveMessage.PublishContent;
                this.litDate.Text = receiveMessage.PublishDate.ToString();
                SubsiteCommentsHelper.PostMessageIsRead(this.receiveMessageId);
                DbQueryResult sendedMessagesForReceivedMessage = SubsiteCommentsHelper.GetSendedMessagesForReceivedMessage(this.receiveMessageId);
                this.dtlistReceivedMessagesReplyed.DataSource = sendedMessagesForReceivedMessage.Data;
                this.dtlistReceivedMessagesReplyed.DataBind();
            }
            else
            {
                base.GotoResourceNotFound();
            }
        }

        protected void btnReplyReplyReceivedMessages_Click(object sender, EventArgs e)
        {
            SendMessageInfo target = new SendMessageInfo();
            ReceiveMessageInfo receiveMessage = SubsiteCommentsHelper.GetReceiveMessage(this.receiveMessageId);
            target.Title = this.txtReplyTitle.Text;
            target.PublishContent = this.txtContes.Text.Trim();
            target.ReceiveMessageId = new long?(this.receiveMessageId);
            target.Addresser = HiContext.Current.User.Username;
            target.Addressee = receiveMessage.Addresser;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SendMessageInfo>(target, new string[] { "ValSendMessage" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            else if (SubsiteCommentsHelper.ReplyMessage(target))
            {
                this.ShowMsg("成功回复客户消息", true);
                this.BindReplyReceivedMessages();
                this.txtReplyTitle.Text = string.Empty;
                this.txtContes.Text = string.Empty;
            }
            else
            {
                this.ShowMsg("回复客户消息失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(base.Request.QueryString["ReceiveMessageId"], out this.receiveMessageId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnReplyReplyReceivedMessages.Click += new EventHandler(this.btnReplyReplyReceivedMessages_Click);
                if (!this.Page.IsPostBack)
                {
                    this.BindReplyReceivedMessages();
                }
            }
        }
    }
}

