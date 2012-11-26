using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Distribution;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class SendMessageSelectDistributor : AdminPage
    {
        int userId;

        private void btnSendToRank_Click(object sender, EventArgs e)
        {
            IList<Distributor> list = new List<Distributor>();
            IList<SendMessageInfo> sendMessageList = new List<SendMessageInfo>();
            IList<ReceiveMessageInfo> receiveMessageList = new List<ReceiveMessageInfo>();
            if (rdoName.Checked)
            {
                if (string.IsNullOrEmpty(txtDistributorNames.Text.Trim()))
                {
                    ShowMsg("请输入您要发送的用户", false);
                    return;
                }
                IList<string> names = new List<string>();
                string[] strArray = txtDistributorNames.Text.Trim().Replace("\r\n", "\n").Replace("\n", "*").Split(new char[] { '*' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (IsMembers(strArray[i]))
                    {
                        names.Add(strArray[i]);
                    }
                }
                foreach (Distributor distributor in NoticeHelper.GetDistributorsByNames(names))
                {
                    SendMessageInfo item = new SendMessageInfo();
                    ReceiveMessageInfo info2 = new ReceiveMessageInfo();
                    item.Addressee = info2.Addressee = distributor.Username;
                    item.Addresser = info2.Addresser = "admin";
                    item.Title = info2.Title = MessageTitle;
                    item.PublishContent = info2.PublishContent = Content;
                    sendMessageList.Add(item);
                    receiveMessageList.Add(info2);
                    if (chkIsSendEmail.Checked)
                    {
                        string str;
                        Messenger.SendMail(MessageTitle, Content, distributor.Email, HiContext.Current.SiteSettings, out str);
                    }
                }
                if (sendMessageList.Count <= 0)
                {
                    ShowMsg("没有要发送的对象", false);
                    return;
                }
                NoticeHelper.SendMessage(sendMessageList, receiveMessageList);
                ShowMsg(string.Format("成功给{0}个用户发送了消息.", sendMessageList.Count), true);
            }
            if (rdoRank.Checked)
            {
                foreach (Distributor distributor2 in NoticeHelper.GetDistributorsByRank(rankList.SelectedValue))
                {
                    SendMessageInfo info3 = new SendMessageInfo();
                    ReceiveMessageInfo info4 = new ReceiveMessageInfo();
                    info3.Addressee = info4.Addressee = distributor2.Username;
                    info3.Addresser = info4.Addresser = "admin";
                    info3.Title = info4.Title = MessageTitle;
                    info3.PublishContent = info4.PublishContent = Content;
                    sendMessageList.Add(info3);
                    receiveMessageList.Add(info4);
                    if (chkIsSendEmail.Checked)
                    {
                        string str2;
                        Messenger.SendMail(MessageTitle, Content, distributor2.Email, HiContext.Current.SiteSettings, out str2);
                    }
                }
                if (sendMessageList.Count > 0)
                {
                    NoticeHelper.SendMessage(sendMessageList, receiveMessageList);
                    ShowMsg(string.Format("成功给{0}个用户发送了消息.", sendMessageList.Count), true);
                }
                else
                {
                    ShowMsg("没有要发送的对象", false);
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
            if (!(string.IsNullOrEmpty(Page.Request.QueryString["UserId"]) || int.TryParse(Page.Request.QueryString["UserId"], out userId)))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnSendToRank.Click += new EventHandler(btnSendToRank_Click);
                if (!Page.IsPostBack)
                {
                    rankList.DataBind();
                    if (userId > 0)
                    {
                        Distributor distributor = DistributorHelper.GetDistributor(userId);
                        if (distributor == null)
                        {
                            base.GotoResourceNotFound();
                        }
                        else
                        {
                            txtDistributorNames.Text = distributor.Username;
                        }
                    }
                }
            }
        }

        public string Content
        {
            get
            {
                if (!string.IsNullOrEmpty(Session["Content"].ToString()))
                {
                    return Globals.UrlDecode(Session["Content"].ToString());
                }
                return string.Empty;
            }
        }

        public string MessageTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(Session["Title"].ToString()))
                {
                    return Globals.UrlDecode(Session["Title"].ToString());
                }
                return string.Empty;
            }
        }
    }
}

