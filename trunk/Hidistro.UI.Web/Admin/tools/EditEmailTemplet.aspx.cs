using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MessageTemplets)]
    public partial class EditEmailTemplet : AdminPage
    {

        string emailType;


        private void btnSaveEmailTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(emailType);
            if (messageTemplate != null)
            {
                string msg = string.Empty;
                bool flag = true;
                if (string.IsNullOrEmpty(txtEmailSubject.Text))
                {
                    msg = msg + Formatter.FormatErrorMessage("邮件标题不能为空");
                    flag = false;
                }
                else if ((txtEmailSubject.Text.Trim().Length < 1) || (txtEmailSubject.Text.Trim().Length > 60))
                {
                    msg = msg + Formatter.FormatErrorMessage("邮件标题长度限制在1-60个字符之间");
                    flag = false;
                }
                if (string.IsNullOrEmpty(fcContent.Text) || (fcContent.Text.Trim().Length == 0))
                {
                    msg = msg + Formatter.FormatErrorMessage("邮件内容不能为空");
                    flag = false;
                }
                if (!flag)
                {
                    ShowMsg(msg, false);
                }
                else
                {
                    messageTemplate.EmailBody = fcContent.Text;
                    messageTemplate.EmailSubject = txtEmailSubject.Text.Trim();
                    MessageTemplateHelper.UpdateTemplate(messageTemplate);
                    Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveEmailTemplet.Click += new EventHandler(btnSaveEmailTemplet_Click);
            emailType = Page.Request.QueryString["MessageType"];
            if (!Page.IsPostBack)
            {
                if (string.IsNullOrEmpty(emailType))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(emailType);
                    if (messageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litEmailType.Text = messageTemplate.Name;
                        litEmailDescription.Text = messageTemplate.TagDescription;
                        txtEmailSubject.Text = messageTemplate.EmailSubject;
                        fcContent.Text = messageTemplate.EmailBody;
                    }
                }
            }
        }
    }
}

