using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class EditManager : AdminPage
    {

        int userId;

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SiteManager siteManager = ManagerHelper.GetManager(userId);
                siteManager.Email = txtprivateEmail.Text;
                if (ValidationManageEamilr(siteManager))
                {
                    foreach (string str in RoleHelper.GetUserRoleNames(siteManager.Username))
                    {
                        if (!(RoleHelper.IsBuiltInRole(str) && (string.Compare(str, "SystemAdministrator") != 0)))
                        {
                            RoleHelper.RemoveUserFromRole(siteManager.Username, str);
                        }
                    }
                    string text = dropRole.SelectedItem.Text;
                    if (string.Compare(text, "超级管理员") == 0)
                    {
                        text = "SystemAdministrator";
                    }
                    RoleHelper.AddUserToRole(siteManager.Username, text);
                    if (ManagerHelper.Update(siteManager))
                    {
                        ShowMsg("成功修改了当前管理员的个人资料", true);
                    }
                    else
                    {
                        ShowMsg("当前管理员的个人信息修改失败", false);
                    }
                }
            }
        }

        private void GetAccountInfo(SiteManager user)
        {
            lblLoginNameValue.Text = user.Username;
            lblRegsTimeValue.Time = user.CreateDate;
            lblLastLoginTimeValue.Time = user.LastLoginDate;
            foreach (string str in RoleHelper.GetUserRoleNames(user.Username))
            {
                if (string.Compare(str, "SystemAdministrator") == 0)
                {
                    dropRole.SelectedIndex = dropRole.Items.IndexOf(dropRole.Items.FindByText("超级管理员"));
                }
                if (HiContext.Current.User.UserId == userId)
                {
                    dropRole.Enabled = false;
                }
                if (!RoleHelper.IsBuiltInRole(str))
                {
                    dropRole.SelectedIndex = dropRole.Items.IndexOf(dropRole.Items.FindByText(str));
                    break;
                }
            }
        }

        private void GetPersonaInfo(SiteManager user)
        {
            txtprivateEmail.Text = user.Email;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out userId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditProfile.Click += new EventHandler(btnEditProfile_Click);
                if (!Page.IsPostBack)
                {
                    dropRole.DataBind();
                    SiteManager user = ManagerHelper.GetManager(userId);
                    if (user == null)
                    {
                        ShowMsg("匿名用户或非管理员用户不能编辑", false);
                    }
                    else
                    {
                        GetAccountInfo(user);
                        GetPersonaInfo(user);
                    }
                }
            }
        }

        private bool ValidationManageEamilr(SiteManager siteManager)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerEmail" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

