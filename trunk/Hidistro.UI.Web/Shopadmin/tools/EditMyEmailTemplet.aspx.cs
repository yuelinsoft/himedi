using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using kindeditor.Net;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditMyEmailTemplet : DistributorPage
    {

        string emailType;


        private void btnSaveEmailTemplet_Click(object sender, EventArgs e)
        {
            MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(emailType, HiContext.Current.User.UserId);
            if (distributorMessageTemplate != null)
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
                    distributorMessageTemplate.EmailBody = fcContent.Text;
                    distributorMessageTemplate.EmailSubject = txtEmailSubject.Text.Trim();
                    MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
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
                    MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(emailType, HiContext.Current.User.UserId);
                    if (distributorMessageTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litEmailType.Text = distributorMessageTemplate.Name;
                        litEmailDescription.Text = distributorMessageTemplate.TagDescription;
                        txtEmailSubject.Text = distributorMessageTemplate.EmailSubject;
                        fcContent.Text = distributorMessageTemplate.EmailBody;
                    }
                }
            }
        }
    }
}

