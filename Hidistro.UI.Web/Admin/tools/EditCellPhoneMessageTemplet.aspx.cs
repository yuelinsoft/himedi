using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MessageTemplets)]
    public partial class EditCellPhoneMessageTemplet : AdminPage
    {

        string messageType;

        private void btnSaveCellPhoneMessageTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(messageType);
            if (messageTemplate != null)
            {
                if (string.IsNullOrEmpty(txtContent.Text))
                {
                    ShowMsg("短信内容不能为空", false);
                }
                else if ((txtContent.Text.Trim().Length < 1) || (txtContent.Text.Trim().Length > 300))
                {
                    ShowMsg("长度限制在1-300个字符之间", false);
                }
                else
                {
                    messageTemplate.SMSBody = txtContent.Text.Trim();
                    MessageTemplateHelper.UpdateTemplate(messageTemplate);
                    Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            messageType = Page.Request.QueryString["MessageType"];
            btnSaveCellPhoneMessageTemplet.Click += new EventHandler(btnSaveCellPhoneMessageTemplet_Click);
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
                        txtContent.Text = messageTemplate.SMSBody;
                    }
                }
            }
        }
    }
}

