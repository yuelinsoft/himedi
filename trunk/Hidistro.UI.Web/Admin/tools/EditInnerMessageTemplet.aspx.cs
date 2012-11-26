using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MessageTemplets)]
    public partial class EditInnerMessageTemplet : AdminPage
    {
        string messageType;

        private void btnSaveMessageTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
            if (messageTemplate != null)
            {
                string msg = string.Empty;
                bool flag = true;
                if (string.IsNullOrEmpty(txtMessageSubject.Text))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息标题不能为空");
                    flag = false;
                }
                if ((txtMessageSubject.Text.Trim().Length < 1) || (txtMessageSubject.Text.Trim().Length > 60))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息标题长度限制在1-60个字符之间");
                    flag = false;
                }
                if (string.IsNullOrEmpty(txtContent.Text))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息内容不能为空");
                    flag = false;
                }
                if ((txtContent.Text.Trim().Length < 1) || (txtContent.Text.Trim().Length > 300))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息长度限制在300个字符以内");
                    flag = false;
                }
                if (!flag)
                {
                    ShowMsg(msg, false);
                }
                else
                {
                    messageTemplate.InnerMessageSubject = txtMessageSubject.Text.Trim();
                    messageTemplate.InnerMessageBody = txtContent.Text;
                    MessageTemplateHelper.UpdateTemplate(messageTemplate);
                    Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            messageType = Page.Request.QueryString["MessageType"];
            btnSaveMessageTemplet.Click += new EventHandler(btnSaveMessageTemplet_Click);
            if (!base.IsPostBack)
            {
                if (string.IsNullOrEmpty(messageType))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
                    if (messageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litEmailType.Text = messageTemplate.Name;
                        litTagDescription.Text = messageTemplate.TagDescription;
                        txtMessageSubject.Text = messageTemplate.InnerMessageSubject;
                        txtContent.Text = messageTemplate.InnerMessageBody;
                    }
                }
            }
        }
    }
}

