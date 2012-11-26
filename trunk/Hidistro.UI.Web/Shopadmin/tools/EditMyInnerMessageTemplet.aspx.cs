using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditMyInnerMessageTemplet : DistributorPage
    {
        string messageType;

        private void btnSaveMessageTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(messageType, HiContext.Current.User.UserId);
            if (distributorMessageTemplate != null)
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
                if ((txtContent.Text.Trim().Length < 1) || (txtContent.Text.Trim().Length > 0xfa0))
                {
                    msg = msg + Formatter.FormatErrorMessage("消息长度限制在4000个字符以内");
                    flag = false;
                }
                if (!flag)
                {
                    ShowMsg(msg, false);
                }
                else
                {
                    distributorMessageTemplate.InnerMessageSubject = txtMessageSubject.Text.Trim();
                    distributorMessageTemplate.InnerMessageBody = txtContent.Text;
                    MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
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
                    MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(messageType, HiContext.Current.User.UserId);
                    if (distributorMessageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litEmailType.Text = distributorMessageTemplate.Name;
                        litTagDescription.Text = distributorMessageTemplate.TagDescription;
                        txtMessageSubject.Text = distributorMessageTemplate.InnerMessageSubject;
                        txtContent.Text = distributorMessageTemplate.InnerMessageBody;
                    }
                }
            }
        }
    }
}

