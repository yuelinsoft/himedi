namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.Subsites.Promotions;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public partial class SendMyMessageSelectUser : DistributorPage
    {
         int userId;

        private void btnSendToRank_Click(object sender, EventArgs e)
        {
            IList<Member> list = new List<Member>();
            IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
            IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
            if (this.rdoName.Checked)
            {
                if (string.IsNullOrEmpty(this.txtMemberNames.Text.Trim()))
                {
                    this.ShowMsg("请输入您要发送的用户", false);
                    return;
                }
                IList<string> names = new List<string>();
                string[] strArray = this.txtMemberNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (this.IsMembers(strArray[i]))
                    {
                        names.Add(strArray[i]);
                    }
                }
                foreach (Member member in SubsitePromoteHelper.GetMemdersByNames(names))
                {
                    SendMessageInfo item = new SendMessageInfo();
                    ReceiveMessageInfo info2 = new ReceiveMessageInfo();
                    item.Addressee = info2.Addressee = member.Username;
                    item.Addresser = info2.Addresser = HiContext.Current.User.Username;
                    item.Title = info2.Title = this.MessageTitle;
                    item.PublishContent = info2.PublishContent = this.Content;
                    sendMessageList.Add(item);
                    receiveMessageList.Add(info2);
                }
                if (sendMessageList.Count <= 0)
                {
                    this.ShowMsg("没有要发送的对象", false);
                    return;
                }
                SubsiteCommentsHelper.SendMessage(sendMessageList, receiveMessageList);
                this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", sendMessageList.Count), true);
            }
            if (this.rdoRank.Checked)
            {
                foreach (Member member2 in SubsitePromoteHelper.GetMembersByRank(this.rankList.SelectedValue))
                {
                    SendMessageInfo info3 = new SendMessageInfo();
                    ReceiveMessageInfo info4 = new ReceiveMessageInfo();
                    info3.Addressee = info4.Addressee = member2.Username;
                    info3.Addresser = info4.Addresser = HiContext.Current.User.Username;
                    info3.Title = info4.Title = this.MessageTitle;
                    info3.PublishContent = info4.PublishContent = this.Content;
                    sendMessageList.Add(info3);
                    receiveMessageList.Add(info4);
                }
                if (sendMessageList.Count > 0)
                {
                    SubsiteCommentsHelper.SendMessage(sendMessageList, receiveMessageList);
                    this.ShowMsg(string.Format("成功给{0}个用户发送了消息.", sendMessageList.Count), true);
                }
                else
                {
                    this.ShowMsg("没有要发送的对象", false);
                }
            }
        }

        private bool IsMembers(string name)
        {
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            return ((regex.IsMatch(name) && (name.Length >= 2)) && (name.Length <= 20));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) || int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId)))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSendToRank.Click += new EventHandler(this.btnSendToRank_Click);
                if (!this.Page.IsPostBack)
                {
                    this.rankList.DataBind();
                    if (this.userId > 0)
                    {
                        Member user = Users.GetUser(this.userId) as Member;
                        if (user == null)
                        {
                            base.GotoResourceNotFound();
                            return;
                        }
                        this.txtMemberNames.Text = user.Username;
                    }
                }
                CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
            }
        }

        public string Content
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Session["Content"].ToString()))
                {
                    return Globals.UrlDecode(this.Session["Content"].ToString());
                }
                return string.Empty;
            }
        }

        public string MessageTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Session["Title"].ToString()))
                {
                    return Globals.UrlDecode(this.Session["Title"].ToString());
                }
                return string.Empty;
            }
        }
    }
}

