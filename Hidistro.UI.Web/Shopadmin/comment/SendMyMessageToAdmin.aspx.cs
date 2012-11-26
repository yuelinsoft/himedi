namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public partial class SendMyMessageToAdmin : DistributorPage
    {

        private void btnRefer_Click(object sender, EventArgs e)
        {
            if (this.ValidateValues())
            {
                string str = this.txtTitle.Text.Trim();
                string str2 = this.txtContent.Text.Trim();
                SendMessageInfo message = new SendMessageInfo();
                ReceiveMessageInfo receiveMessage = new ReceiveMessageInfo();
                message.Addressee = receiveMessage.Addressee = "admin";
                message.Addresser = receiveMessage.Addresser = HiContext.Current.User.Username;
                message.Title = receiveMessage.Title = str;
                message.PublishContent = receiveMessage.PublishContent = str2;
                if (!SubsiteCommentsHelper.InsertSendMessageToAdmin(message, receiveMessage))
                {
                    this.ShowMsg("发送失败", false);
                }
                else
                {
                    this.ShowMsg("发送成功", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
        }

        private bool ValidateValues()
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()) || (this.txtTitle.Text.Trim().Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(this.txtContent.Text.Trim()) || (this.txtContent.Text.Trim().Length > 300))
            {
                str = str + Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

