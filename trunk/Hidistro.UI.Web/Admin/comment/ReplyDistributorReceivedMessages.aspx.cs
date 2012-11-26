using Hidistro.ControlPanel.Comments;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class ReplyDistributorReceivedMessages : AdminPage
    {

        long receiveMessageId;

        private void BindReplyReceivedMessages()
        {
            ReceiveMessageInfo receiveMessage = NoticeHelper.GetReceiveMessage(receiveMessageId);
            if (receiveMessage == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                litAddresser.Text = receiveMessage.Addresser;
                litTitle.Text = receiveMessage.Title;
                litContent.Text = receiveMessage.PublishContent;
                litDate.Time = receiveMessage.PublishDate;
                NoticeHelper.PostMessageIsRead(receiveMessageId);
                DbQueryResult sendedMessagesForReceivedMessage = NoticeHelper.GetSendedMessagesForReceivedMessage(receiveMessageId);
                dtlistMessageReply.DataSource = sendedMessagesForReceivedMessage.Data;
                dtlistMessageReply.DataBind();
            }
        }

        protected void btnReplyReplyReceivedMessages_Click(object sender, EventArgs e)
        {
            SendMessageInfo target = new SendMessageInfo();
            ReceiveMessageInfo receiveMessage = NoticeHelper.GetReceiveMessage(receiveMessageId);
            target.Title = txtTitle.Text;
            target.PublishContent = txtContes.Text;
            target.ReceiveMessageId = new long?(receiveMessageId);
            target.Addresser = "admin";
            target.Addressee = receiveMessage.Addresser;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SendMessageInfo>(target, new string[] { "ValSendMessage" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else if (NoticeHelper.ReplyMessage(target))
            {
                txtTitle.Text = "";
                txtContes.Text = "";
                ShowMsg("成功回复客户消息", true);
                BindReplyReceivedMessages();
                txtTitle.Text = string.Empty;
                txtContes.Text = string.Empty;
            }
            else
            {
                ShowMsg("回复客户消息失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!long.TryParse(Page.Request.QueryString["ReceiveMessageId"], out receiveMessageId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnReplyReplyReceivedMessages.Click += new EventHandler(btnReplyReplyReceivedMessages_Click);
                if (!Page.IsCallback)
                {
                    BindReplyReceivedMessages();
                }
            }
        }
    }
}

