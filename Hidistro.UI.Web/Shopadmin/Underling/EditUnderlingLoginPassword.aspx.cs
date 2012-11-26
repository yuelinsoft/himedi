using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditUnderlingLoginPassword : DistributorPage
    {
        int currentUserId;

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            Member member = UnderlingHelper.GetMember(currentUserId);
            if ((string.IsNullOrEmpty(txtNewPassWord.Text) || (txtNewPassWord.Text.Length > 20)) || (txtNewPassWord.Text.Length < 6))
            {
                ShowMsg("登录密码不能为空，长度限制在6-20个字符之间", false);
            }
            else if (txtNewPassWord.Text != txtPassWordCompare.Text)
            {
                ShowMsg("输入的两次密码不一致", false);
            }
            else if (member.ChangePassword(txtNewPassWord.Text))
            {
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
                    Member member = UnderlingHelper.GetMember(currentUserId);
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

