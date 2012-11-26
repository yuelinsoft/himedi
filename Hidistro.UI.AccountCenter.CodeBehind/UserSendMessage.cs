namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class UserSendMessage : MemberTemplatedWebControl
    {
       IButton btnRefer;
       RadioButton radioAdminSelect;
       TextBox txtContent;
       TextBox txtTitle;

        protected override void AttachChildControls()
        {
            this.radioAdminSelect = (RadioButton) this.FindControl("radioAdminSelect");
            this.txtTitle = (TextBox) this.FindControl("txtTitle");
            this.txtContent = (TextBox) this.FindControl("txtContent");
            this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            if (!this.Page.IsPostBack)
            {
                this.radioAdminSelect.Enabled = false;
                this.radioAdminSelect.Checked = true;
                this.txtTitle.Text = this.txtTitle.Text.Trim();
                this.txtContent.Text = this.txtContent.Text.Trim();
            }
        }

       void btnRefer_Click(object sender, EventArgs e)
        {
            string str = "";
            if (string.IsNullOrEmpty(this.txtTitle.Text) || (this.txtTitle.Text.Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
            }
            if (string.IsNullOrEmpty(this.txtContent.Text) || (this.txtContent.Text.Length > 300))
            {
                str = str + Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMessage(str, false);
            }
            else
            {
                IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
                IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
                SendMessageInfo item = new SendMessageInfo();
                ReceiveMessageInfo info2 = new ReceiveMessageInfo();
                item.Addresser = info2.Addresser = HiContext.Current.User.Username;
                item.Addressee = info2.Addressee = "admin";
                item.Title = info2.Title = this.txtTitle.Text.Replace("~", "");
                item.PublishContent = info2.PublishContent = this.txtContent.Text.Replace("~", "");
                sendMessageList.Add(item);
                receiveMessageList.Add(info2);
                this.txtTitle.Text = string.Empty;
                this.txtContent.Text = string.Empty;
                if (CommentsHelper.SendMessage(sendMessageList, receiveMessageList) > 0)
                {
                    this.ShowMessage("发送信息成功", true);
                }
                else
                {
                    this.ShowMessage("发送信息失败", true);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserSendMessage.html";
            }
            base.OnInit(e);
        }
    }
}

