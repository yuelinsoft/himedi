
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
    public partial class EditMemberLoginPassword : AdminPage
    {

        int currentUserId;


        private void btnEditUser_Click(object sender, EventArgs e)
        {
            Member user = MemberHelper.GetMember(currentUserId);
            if ((string.IsNullOrEmpty(txtNewPassWord.Text) || (txtNewPassWord.Text.Length > 20)) || (txtNewPassWord.Text.Length < 6))
            {
                ShowMsg("登录密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (txtNewPassWord.Text != txtPassWordCompare.Text)
            {
                ShowMsg("输入的两次密码不一致", false);
            }
            else if (user.ChangePassword(txtNewPassWord.Text))
            {
                Messenger.UserPasswordChanged(user, txtNewPassWord.Text);
                user.OnPasswordChanged(new UserEventArgs(user.Username, txtNewPassWord.Text, null));
                ShowMsg("登录密码修改成功", true);
            }
            else
            {
                ShowMsg("登录密码修改失败", false);
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

