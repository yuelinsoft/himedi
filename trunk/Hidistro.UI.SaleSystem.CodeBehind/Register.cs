namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Configuration;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Member;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    public class Register : HtmlTemplatedWebControl
    {
       IButton btnRegister;
       CheckBox chkAgree;
       TextBox txtEmail;
       TextBox txtNumber;
       TextBox txtPassword;
       TextBox txtPassword2;
       TextBox txtUserName;

        protected override void AttachChildControls()
        {
            this.chkAgree = (CheckBox) this.FindControl("chkAgree");
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtPassword = (TextBox) this.FindControl("txtPassword");
            this.txtPassword2 = (TextBox) this.FindControl("txtPassword2");
            this.txtEmail = (TextBox) this.FindControl("txtEmail");
            this.txtNumber = (TextBox) this.FindControl("txtNumber");
            this.btnRegister = ButtonManager.Create(this.FindControl("btnRegister"));
            PageTitle.AddSiteNameTitle("会员注册", HiContext.Current.Context);
            this.btnRegister.Click += new EventHandler(this.btnRegister_Click);
        }

       void btnRegister_Click(object sender, EventArgs e)
        {
            if (!this.chkAgree.Checked)
            {
                this.ShowMessage("您必须先阅读并同意注册协议", false);
            }
            else if (string.Compare(this.txtUserName.Text.Trim().ToLower(CultureInfo.InvariantCulture), "anonymous", false, CultureInfo.InvariantCulture) == 0)
            {
                this.ShowMessage("已经存在相同的用户名", false);
            }
            else
            {
                string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
                Regex regex = new Regex(pattern);
                if ((!regex.IsMatch(this.txtUserName.Text.Trim()) || (this.txtUserName.Text.Trim().Length < 2)) || (this.txtUserName.Text.Trim().Length > 20))
                {
                    this.ShowMessage("用户名不能为空，必须以汉字或是字母开头,且在2-20个字符之间", false);
                }
                else if (string.Compare(this.txtPassword.Text, this.txtPassword2.Text) != 0)
                {
                    this.ShowMessage("两次输入的密码不相同", false);
                }
                else if (this.txtPassword.Text.Length == 0)
                {
                    this.ShowMessage("密码不能为空", false);
                }
                else if ((this.txtPassword.Text.Length < Membership.Provider.MinRequiredPasswordLength) || (this.txtPassword.Text.Length > HiConfiguration.GetConfig().PasswordMaxLength))
                {
                    this.ShowMessage(string.Format("密码的长度只能在{0}和{1}个字符之间", Membership.Provider.MinRequiredPasswordLength, HiConfiguration.GetConfig().PasswordMaxLength), false);
                }
                else
                {
                    Member member = null;
                    if (HiContext.Current.SiteSettings.IsDistributorSettings)
                    {
                        member = new Member(UserRole.Underling);
                        member.ParentUserId = HiContext.Current.SiteSettings.UserId;
                    }
                    else
                    {
                        member = new Member(UserRole.Member);
                    }
                    if (HiContext.Current.ReferralUserId > 0)
                    {
                        member.ReferralUserId = new int?(HiContext.Current.ReferralUserId);
                    }
                    member.GradeId = MemberProcessor.GetDefaultMemberGrade();
                    member.Username = Globals.HtmlEncode(this.txtUserName.Text.Trim());
                    member.Email = this.txtEmail.Text;
                    member.Password = this.txtPassword.Text;
                    member.PasswordFormat = MembershipPasswordFormat.Hashed;
                    member.TradePasswordFormat = MembershipPasswordFormat.Hashed;
                    member.TradePassword = this.txtPassword.Text;
                    member.IsApproved = true;
                    member.RealName = string.Empty;
                    member.Address = string.Empty;
                    if (this.ValidationMember(member))
                    {
                        if (!HiContext.Current.CheckVerifyCode(this.txtNumber.Text))
                        {
                            this.ShowMessage("验证码输入错误", false);
                        }
                        else
                        {
                            switch (MemberProcessor.CreateMember(member))
                            {
                                case CreateUserStatus.UnknownFailure:
                                    this.ShowMessage("未知错误", false);
                                    return;

                                case CreateUserStatus.Created:
                                {
                                    Messenger.UserRegister(member, this.txtPassword.Text);
                                    member.OnRegister(new UserEventArgs(member.Username, this.txtPassword.Text, null));
                                    IUser user = Users.GetUser(0, member.Username, false, true);
                                    ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                                    CookieShoppingProvider.Instance().ClearShoppingCart();
                                    HiContext.Current.User = user;
                                    if (shoppingCart != null)
                                    {
                                        ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
                                    }
                                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(member.Username, false);
                                    user.GetUserCookie().WriteCookie(authCookie, 30, false);
                                    this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("registerUserSave") + "?UserId=" + user.UserId);
                                    return;
                                }
                                case CreateUserStatus.DuplicateUsername:
                                    this.ShowMessage("已经存在相同的用户名", false);
                                    return;

                                case CreateUserStatus.DuplicateEmailAddress:
                                    this.ShowMessage("电子邮件地址已经存在", false);
                                    return;

                                case CreateUserStatus.InvalidFirstCharacter:
                                case CreateUserStatus.Updated:
                                case CreateUserStatus.Deleted:
                                case CreateUserStatus.InvalidQuestionAnswer:
                                    return;

                                case CreateUserStatus.DisallowedUsername:
                                    this.ShowMessage("用户名禁止注册", false);
                                    return;

                                case CreateUserStatus.InvalidPassword:
                                    this.ShowMessage("无效的密码", false);
                                    return;

                                case CreateUserStatus.InvalidEmail:
                                    this.ShowMessage("无效的电子邮件地址", false);
                                    return;
                            }
                        }
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-Register.html";
            }
            base.OnInit(e);
        }

       bool ValidationMember(Member member)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Member>(member, new string[] { "ValMember" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMessage(msg, false);
            }
            return results.IsValid;
        }
    }
}

