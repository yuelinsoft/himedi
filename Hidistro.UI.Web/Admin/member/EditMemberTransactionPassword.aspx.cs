
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditMember)]
    public partial class EditMemberTransactionPassword : AdminPage
    {

        int currentUserId;


        private void btnEditUser_Click(object sender, EventArgs e)
        {
            Member user = MemberHelper.GetMember(currentUserId);
            if (!user.IsOpenBalance)
            {
                ShowMsg("该会员没有开启预付款账户，无法修改交易密码", false);
            }
            else if ((string.IsNullOrEmpty(txtTransactionPassWord.Text) || (txtTransactionPassWord.Text.Length > 20)) || (txtTransactionPassWord.Text.Length < 6))
            {
                ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (txtTransactionPassWord.Text != txtTransactionPassWordCompare.Text)
            {
                ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangeTradePassword(txtTransactionPassWord.Text))
            {
                Messenger.UserDealPasswordChanged(user, txtTransactionPassWord.Text);
                user.OnDealPasswordChanged(new UserEventArgs(user.Username, null, txtTransactionPassWord.Text));
                ShowMsg("交易密码修改成功", true);
            }
            else
            {
                ShowMsg("交易密码修改失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditUser.Click += new EventHandler(btnEditUser_Click);
                if (!Page.IsPostBack)
                {
                    Member member = MemberHelper.GetMember(currentUserId);
                    if (member == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litlUserName.Text = member.Username;
                    }
                }
            }
        }
    }
}

