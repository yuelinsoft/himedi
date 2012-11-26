using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditLoginPassword : DistributorPage
    {


        private void btnEditLoginPassword_Click(object sender, EventArgs e)
        {
            Distributor user = SubsiteStoreHelper.GetDistributor();
            if (string.IsNullOrEmpty(txtOldPassword.Text))
            {
                ShowMsg("旧登录密码不能为空", false);
            }
            else if ((string.IsNullOrEmpty(txtNewPassword.Text) || (txtNewPassword.Text.Length > 20)) || (txtNewPassword.Text.Length < 6))
            {
                ShowMsg("新登录密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (txtNewPassword.Text != txtPasswordCompare.Text)
            {
                ShowMsg("两次输入的密码不一致", false);
            }
            else if (user.ChangePassword(txtOldPassword.Text, txtNewPassword.Text))
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

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnEditLoginPassword.Click += new EventHandler(btnEditLoginPassword_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

