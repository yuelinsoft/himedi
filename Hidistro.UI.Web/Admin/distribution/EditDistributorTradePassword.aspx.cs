using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditDistributor)]
    public partial class EditDistributorTradePassword : AdminPage
    {

        int userId;


        private void btnEditDistributorTradePassword_Click(object sender, EventArgs e)
        {
            Distributor user = DistributorHelper.GetDistributor(userId);
            if ((string.IsNullOrEmpty(txtNewTradePassword.Text) || (txtNewTradePassword.Text.Length > 20)) || (txtNewTradePassword.Text.Length < 6))
            {
                ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (txtNewTradePassword.Text != txtTradePasswordCompare.Text)
            {
                ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangeTradePassword(txtNewTradePassword.Text))
            {
                Messenger.UserDealPasswordChanged(user, txtNewTradePassword.Text);
                user.OnDealPasswordChanged(new UserEventArgs(user.Username, null, txtNewTradePassword.Text));
                ShowMsg("交易密码修改成功", true);
            }
            else
            {
                ShowMsg("交易密码修改失败", false);
            }
        }

        private void LoadControl()
        {
            Distributor distributor = DistributorHelper.GetDistributor(userId);
            if (distributor == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                litUserName.Text = distributor.Username;
                WangWangConversations.WangWangAccounts = distributor.Wangwang;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnEditDistributorTradePassword.Click += new EventHandler(btnEditDistributorTradePassword_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                LoadControl();
            }
        }
    }
}

