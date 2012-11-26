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
    public partial class EditDistributorLoginPassword : AdminPage
    {
        int userId;

        private void btnEditDistributorLoginPassword_Click(object sender, EventArgs e)
        {
            Distributor user = DistributorHelper.GetDistributor(userId);
            if ((string.IsNullOrEmpty(txtNewPassword.Text) || (txtNewPassword.Text.Length > 20)) || (txtNewPassword.Text.Length < 6))
            {
                ShowMsg("登录密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (txtNewPassword.Text != txtPasswordCompare.Text)
            {
                ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangePassword(txtNewPassword.Text))
            {
                Messenger.UserPasswordChanged(user, txtNewPassword.Text);
                user.OnPasswordChanged(new UserEventArgs(user.Username, txtNewPassword.Text, null));
                ShowMsg("登录密码修改成功", true);
            }
            else
            {
                ShowMsg("登录密码修改失败", false);
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
            btnEditDistributorLoginPassword.Click += new EventHandler(btnEditDistributorLoginPassword_Click);
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

