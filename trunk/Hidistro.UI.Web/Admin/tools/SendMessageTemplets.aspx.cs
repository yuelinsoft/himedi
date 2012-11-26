using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MessageTemplets)]
    public partial class SendMessageTemplets : AdminPage
    {
        private void btnSaveSendSetting_Click(object sender, EventArgs e)
        {
            List<MessageTemplate> templates = new List<MessageTemplate>();
            foreach (GridViewRow row in grdEmailTemplets.Rows)
            {
                MessageTemplate item = new MessageTemplate();
                CheckBox box = (CheckBox)row.FindControl("chkSendEmail");
                item.SendEmail = box.Checked;
                CheckBox box2 = (CheckBox)row.FindControl("chkInnerMessage");
                item.SendInnerMessage = box2.Checked;
                CheckBox box3 = (CheckBox)row.FindControl("chkCellPhoneMessage");
                item.SendSMS = box3.Checked;
                item.MessageType = (string)grdEmailTemplets.DataKeys[row.RowIndex].Value;
                templates.Add(item);
            }
            MessageTemplateHelper.UpdateSettings(templates);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveSendSetting.Click += new EventHandler(btnSaveSendSetting_Click);
            if (!Page.IsPostBack)
            {
                grdEmailTemplets.DataSource = MessageTemplateHelper.GetMessageTemplates();
                grdEmailTemplets.DataBind();
            }
        }
    }
}

