using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class AddManager : AdminPage
    {

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateUserStatus unknownFailure = CreateUserStatus.UnknownFailure;
            SiteManager siteManager = new SiteManager();
            siteManager.IsApproved = true;
            siteManager.Username = txtUserName.Text.Trim();
            siteManager.Email = txtEmail.Text.Trim();
            siteManager.Password = txtPassword.Text.Trim();
            siteManager.PasswordFormat = MembershipPasswordFormat.Hashed;
            if (string.Compare(txtPassword.Text, txtPasswordagain.Text) != 0)
            {
                ShowMsg("请确保两次输入的密码相同", false);
            }
            else if (ValidationAddManager(siteManager))
            {
                try
                {
                    string text = dropRole.SelectedItem.Text;
                    if (string.Compare(text, "超级管理员") == 0)
                    {
                        text = "SystemAdministrator";
                    }
                    unknownFailure = ManagerHelper.Create(siteManager, text);
                }
                catch (CreateUserException exception)
                {
                    unknownFailure = exception.CreateUserStatus;
                }
                switch (unknownFailure)
                {
                    case CreateUserStatus.UnknownFailure:
                        ShowMsg("未知错误", false);
                        return;

                    case CreateUserStatus.Created:
                        txtEmail.Text = string.Empty;
                        txtUserName.Text = string.Empty;
                        ShowMsg("成功添加了一个管理员", true);
                        return;

                    case CreateUserStatus.DuplicateUsername:
                        ShowMsg("您输入的用户名已经被注册使用", false);
                        return;

                    case CreateUserStatus.DuplicateEmailAddress:
                        ShowMsg("您输入的电子邮件地址已经被注册使用", false);
                        return;

                    case CreateUserStatus.InvalidFirstCharacter:
                    case CreateUserStatus.Updated:
                    case CreateUserStatus.Deleted:
                    case CreateUserStatus.InvalidQuestionAnswer:
                        return;

                    case CreateUserStatus.DisallowedUsername:
                        ShowMsg("用户名被禁止注册", false);
                        return;

                    case CreateUserStatus.InvalidPassword:
                        ShowMsg("无效的密码", false);
                        return;

                    case CreateUserStatus.InvalidEmail:
                        ShowMsg("无效的电子邮件地址", false);
                        return;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnCreate.Click += new EventHandler(btnCreate_Click);
            if (!Page.IsPostBack)
            {
                dropRole.DataBind();
            }
        }

        private bool ValidationAddManager(SiteManager siteManager)
        {
            bool flag = true;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerName" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                flag = false;
            }
            results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerPassword" });
            if (!results.IsValid)
            {
                foreach (ValidationResult result2 in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result2.Message);
                }
                flag = false;
            }
            results = Hishop.Components.Validation.Validation.Validate<SiteManager>(siteManager, new string[] { "ValManagerEmail" });
            if (!results.IsValid)
            {
                foreach (ValidationResult result3 in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result3.Message);
                }
                flag = false;
            }
            if (!flag)
            {
                ShowMsg(msg, false);
            }
            return flag;
        }
    }
}

