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

    public partial class ReplayMyReceivedMessageToAdmin : DistributorPage
    {
        long receiveMessageId;
        private void BindReplyReceivedMessages()
        {
            ReceiveMessageInfo receivedMessageToAdminInfo = SubsiteCommentsHelper.GetReceivedMessageToAdminInfo(this.receiveMessageId);
            if (receivedMessageToAdminInfo != null)
            {
                this.litAddresser.Text = receivedMessageToAdminInfo.Addresser;
                this.litTitle.Text = receivedMessageToAdminInfo.Title;
                this.litContent.Text = receivedMessageToAdminInfo.PublishContent;
                this.litDate.Text = receivedMessageToAdminInfo.PublishDate.ToString();
                SubsiteCommentsHelper.PostMessageToAdminIsRead(this.receiveMessageId);
                DbQueryResult sendedMessagesForReceivedMessageToAdmin = SubsiteCommentsHelper.GetSendedMessagesForReceivedMessageToAdmin(this.receiveMessageId);
                this.dtlistReceivedMessagesReplyed.DataSource = sendedMessagesForReceivedMessageToAdmin.Data;
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
            ReceiveMessageInfo receivedMessageToAdminInfo = SubsiteCommentsHelper.GetReceivedMessageToAdminInfo(this.receiveMessageId);
            target.Title = this.txtReplyTitle.Text;
            target.PublishContent = this.txtContes.Text.Trim();
            target.ReceiveMessageId = new long?(this.receiveMessageId);
            target.Addresser = HiContext.Current.User.Username;
            target.Addressee = receivedMessageToAdminInfo.Addresser;
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
            else if (SubsiteCommentsHelper.ReplyMessageToAdmin(target))
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

