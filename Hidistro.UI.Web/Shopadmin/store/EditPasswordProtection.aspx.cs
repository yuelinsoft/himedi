using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditPasswordProtection : DistributorPage
    {

        private void btnEditProtection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewQuestion.Text))
            {
                ShowMsg("请输入新密保问题", false);
            }
            else if (string.IsNullOrEmpty(txtNewAnswer.Text))
            {
                ShowMsg("请输入新密保答案", false);
            }
            else
            {
                Distributor user = SubsiteStoreHelper.GetDistributor();
                if (string.IsNullOrEmpty(user.PasswordQuestion))
                {
                    if (user.ChangePasswordQuestionAndAnswer(txtNewQuestion.Text.Trim(), txtNewAnswer.Text.Trim()))
                    {
                        Users.ClearUserCache(user);
                        LoadOldControl();
                        ShowMsg("成功修改了密码答案", true);
                    }
                    else
                    {
                        ShowMsg("修改密码答案失败", false);
                    }
                }
                else if (user.ChangePasswordQuestionAndAnswer(txtOldAnswer.Text.Trim(), txtNewQuestion.Text.Trim(), txtNewAnswer.Text.Trim()))
                {
                    Users.ClearUserCache(user);
                    LoadOldControl();
                    ShowMsg("成功修改了密码答案", true);
                }
                else
                {
                    ShowMsg("修改密码答案失败，可能是您原来的问题答案输入错误", false);
                }
            }
        }

        private void LoadOldControl()
        {
            IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
            if (user != null)
            {
                ulOld.Visible = !string.IsNullOrEmpty(user.PasswordQuestion);
                litOldQuestion.Text = user.PasswordQuestion;
                txtOldAnswer.Text = string.Empty;
                txtNewQuestion.Text = string.Empty;
                txtNewAnswer.Text = string.Empty;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnEditProtection.Click += new EventHandler(btnEditProtection_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadOldControl();
            }
        }
    }
}

