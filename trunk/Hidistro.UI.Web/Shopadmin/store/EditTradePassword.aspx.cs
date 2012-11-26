using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditTradePassword : DistributorPage
    {

        private void btnEditTradePassword_Click(object sender, EventArgs e)
        {
            Distributor user = SubsiteStoreHelper.GetDistributor();
            if (string.IsNullOrEmpty(txtOldTradePassword.Text))
            {
                ShowMsg("请输入旧交易密码", false);
            }
            else if ((string.IsNullOrEmpty(txtNewTradePassword.Text) || (txtNewTradePassword.Text.Length > 20)) || (txtNewTradePassword.Text.Length < 6))
            {
                ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (txtNewTradePassword.Text != txtTradePasswordCompare.Text)
            {
                ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangeTradePassword(txtOldTradePassword.Text, txtNewTradePassword.Text))
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

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnEditTradePassword.Click += new EventHandler(btnEditTradePassword_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

