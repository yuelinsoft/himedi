using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditMyCellPhoneMessageTemplet : DistributorPage
    {

        string messageType;

        private void btnSaveCellPhoneMessageTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(messageType, HiContext.Current.User.UserId);
            if (distributorMessageTemplate != null)
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
                    distributorMessageTemplate.SMSBody = txtContent.Text.Trim();
                    MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
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
                    MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(messageType, HiContext.Current.User.UserId);
                    if (distributorMessageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litEmailType.Text = distributorMessageTemplate.Name;
                        litTagDescription.Text = distributorMessageTemplate.TagDescription;
                        txtContent.Text = distributorMessageTemplate.SMSBody;
                    }
                }
            }
        }
    }
}

