using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.Security;

namespace Hidistro.Membership.Context
{
    [HasSelfValidation]
    public class Distributor : IUser
    {
        public static event EventHandler<UserEventArgs> DealPasswordChanged;

        public static event EventHandler<UserEventArgs> FindPassword;

        public static event EventHandler<EventArgs> Login;

        public static event EventHandler<UserEventArgs> Logout;

        public static event EventHandler<UserEventArgs> PasswordChanged;

        public static event EventHandler<UserEventArgs> Register;

        public Distributor()
        {
            MembershipUser = new HiMembershipUser(false, Hidistro.Membership.Core.Enums.UserRole.Distributor);
        }

        public Distributor(HiMembershipUser membershipUser)
        {
            MembershipUser = membershipUser;
        }

        public bool ChangePassword(string newPassword)
        {
            if (HiContext.Current.User.UserRole == Hidistro.Membership.Core.Enums.UserRole.SiteManager)
            {
                string password = MembershipUser.Membership.ResetPassword();

                if (MembershipUser.ChangePassword(password, newPassword))
                {
                    return true;
                }

            }

            return false;

        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return MembershipUser.ChangePassword(oldPassword, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            return MembershipUser.ChangePasswordQuestionAndAnswer(newQuestion, newAnswer);
        }

        public bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            return MembershipUser.ChangePasswordQuestionAndAnswer(oldAnswer, newQuestion, newAnswer);
        }

        public bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            return MembershipUser.ChangePasswordWithAnswer(answer, newPassword);
        }

        public bool ChangeTradePassword(string newPassword)
        {
            return DistributorFactory.Instance().ChangeTradePassword(Username, newPassword);
        }

        public bool ChangeTradePassword(string oldPassword, string newPassword)
        {
            return DistributorFactory.Instance().ChangeTradePassword(Username, oldPassword, newPassword);
        }

        [SelfValidation(Ruleset = "ValDistributor")]
        public void CheckDistributor(ValidationResults results)
        {
            HiConfiguration config = HiConfiguration.GetConfig();
            if ((string.IsNullOrEmpty(Username) || (Username.Length > config.UsernameMaxLength)) || (Username.Length < config.UsernameMinLength))
            {
                results.AddResult(new ValidationResult(string.Format("用户名不能为空，长度必须在{0}-{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength), this, "", "", null));
            }
            else if (!Regex.IsMatch(Username, config.UsernameRegex))
            {
                results.AddResult(new ValidationResult("用户名的格式错误", this, "", "", null));
            }
            if (string.IsNullOrEmpty(Email) || (Email.Length > 0x100))
            {
                results.AddResult(new ValidationResult("电子邮件不能为空，长度必须小于256个字符", this, "", "", null));
            }
            else if (!Regex.IsMatch(Email, config.EmailRegex))
            {
                results.AddResult(new ValidationResult("电子邮件的格式错误", this, "", "", null));
            }
            if (IsCreate)
            {
                if ((string.IsNullOrEmpty(Password) || (Password.Length > config.PasswordMaxLength)) || (Password.Length < 6))
                {
                    results.AddResult(new ValidationResult(string.Format("密码不能为空，长度必须在{0}-{1}个字符之间", 6, config.PasswordMaxLength), this, "", "", null));
                }
                if ((string.IsNullOrEmpty(TradePassword) || (TradePassword.Length > config.PasswordMaxLength)) || (TradePassword.Length < 6))
                {
                    results.AddResult(new ValidationResult(string.Format("交易密码不能为空，长度必须在{0}-{1}个字符之间", 6, config.PasswordMaxLength), this, "", "", null));
                }
            }
            if (!(string.IsNullOrEmpty(QQ) || (((QQ.Length <= 20) && (QQ.Length >= 3)) && Regex.IsMatch(QQ, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("QQ号长度限制在3-20个字符之间，只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(Zipcode) || (((Zipcode.Length <= 10) && (Zipcode.Length >= 3)) && Regex.IsMatch(Zipcode, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("邮编长度限制在3-10个字符之间，只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(Wangwang) || ((Wangwang.Length <= 20) && (Wangwang.Length >= 3))))
            {
                results.AddResult(new ValidationResult("旺旺长度限制在3-20个字符之间", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(MSN) || (((MSN.Length <= 0x100) && (MSN.Length >= 1)) && Regex.IsMatch(MSN, config.EmailRegex))))
            {
                results.AddResult(new ValidationResult("请输入正确MSN帐号，长度在1-256个字符以内", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(CellPhone) || (((CellPhone.Length <= 20) && (CellPhone.Length >= 3)) && Regex.IsMatch(CellPhone, "^[0-9]*$"))))
            {
                results.AddResult(new ValidationResult("手机号码长度限制在3-20个字符之间,只能输入数字", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(TelPhone) || (((TelPhone.Length <= 20) && (TelPhone.Length >= 3)) && Regex.IsMatch(TelPhone, "^[0-9-]*$"))))
            {
                results.AddResult(new ValidationResult("电话号码长度限制在3-20个字符之间，只能输入数字和字符“-”", this, "", "", null));
            }
        }

        public IUserCookie GetUserCookie()
        {
            return new UserCookie(this);
        }

        public bool IsInRole(string roleName)
        {
            return roleName.Equals(HiContext.Current.Config.RolesConfiguration.Distributor);
        }

        public void OnDealPasswordChanged(UserEventArgs args)
        {
            if (DealPasswordChanged != null)
            {
                DealPasswordChanged(this, args);
            }
        }

        public static void OnDealPasswordChanged(Member member, UserEventArgs args)
        {
            if (DealPasswordChanged != null)
            {
                DealPasswordChanged(member, args);
            }
        }

        public void OnFindPassword(UserEventArgs args)
        {
            if (FindPassword != null)
            {
                FindPassword(this, args);
            }
        }

        public static void OnFindPassword(Member member, UserEventArgs args)
        {
            if (FindPassword != null)
            {
                FindPassword(member, args);
            }
        }

        public void OnLogin()
        {
            if (Login != null)
            {
                Login(this, new EventArgs());
            }
        }

        public static void OnLogin(Member member)
        {
            if (Login != null)
            {
                Login(member, new EventArgs());
            }
        }

        public static void OnLogout(UserEventArgs args)
        {
            if (Logout != null)
            {
                Logout(null, args);
            }
        }

        public void OnPasswordChanged(UserEventArgs args)
        {
            if (PasswordChanged != null)
            {
                PasswordChanged(this, args);
            }
        }

        public static void OnPasswordChanged(Member member, UserEventArgs args)
        {
            if (PasswordChanged != null)
            {
                PasswordChanged(member, args);
            }
        }

        public void OnRegister(UserEventArgs args)
        {
            if (Register != null)
            {
                Register(this, args);
            }
        }

        public static void OnRegister(Member member, UserEventArgs args)
        {
            if (Register != null)
            {
                Register(member, args);
            }
        }

        public string ResetPassword(string answer)
        {
            return MembershipUser.ResetPassword(answer);
        }

        public bool ValidatePasswordAnswer(string answer)
        {
            return MembershipUser.ValidatePasswordAnswer(answer);
        }

        [StringLengthValidator(0, 100, Ruleset = "ValDistributor", MessageTemplate = "详细地址必须控制在100个字符以内"), HtmlCoding]
        public string Address { get; set; }

        public decimal Balance { get; set; }

        public DateTime? BirthDate
        {
            get
            {
                return MembershipUser.BirthDate;
            }
            set
            {
                MembershipUser.BirthDate = value;
            }
        }

        public string CellPhone { get; set; }

        public string Comment
        {
            get
            {
                return MembershipUser.Comment;
            }
            set
            {
                MembershipUser.Comment = value;
            }
        }

        [StringLengthValidator(0, 60, Ruleset = "ValDistributor", MessageTemplate = "公司名称必须控制在60个字符以内"), HtmlCoding]
        public string CompanyName { get; set; }

        public DateTime CreateDate
        {
            get
            {
                return MembershipUser.CreateDate;
            }
        }

        public string Email
        {
            get
            {
                return MembershipUser.Email;
            }
            set
            {
                MembershipUser.Email = value;
            }
        }

        public decimal Expenditure { get; set; }

        public Hidistro.Membership.Core.Enums.Gender Gender
        {
            get
            {
                return MembershipUser.Gender;
            }
            set
            {
                MembershipUser.Gender = value;
            }
        }

        public int GradeId { get; set; }

        public bool IsAnonymous
        {
            get
            {
                return MembershipUser.IsAnonymous;
            }
        }

        public bool IsApproved
        {
            get
            {
                return MembershipUser.IsApproved;
            }
            set
            {
                MembershipUser.IsApproved = value;
            }
        }

        public bool IsCreate { get; set; }

        public bool IsLockedOut
        {
            get
            {
                return MembershipUser.IsLockedOut;
            }
        }

        public DateTime LastActivityDate
        {
            get
            {
                return MembershipUser.LastActivityDate;
            }
            set
            {
                MembershipUser.LastActivityDate = value;
            }
        }

        public DateTime LastLockoutDate
        {
            get
            {
                return MembershipUser.LastLockoutDate;
            }
        }

        public DateTime LastLoginDate
        {
            get
            {
                return MembershipUser.LastLoginDate;
            }
        }

        public DateTime LastPasswordChangedDate
        {
            get
            {
                return MembershipUser.LastPasswordChangedDate;
            }
        }

        public int MemberCount { get; set; }

        public HiMembershipUser MembershipUser { get;set; }

        public string MobilePIN
        {
            get
            {
                return MembershipUser.MobilePIN;
            }
            set
            {
                MembershipUser.MobilePIN = value;
            }
        }

        public string MSN { get; set; }

        public string Password
        {
            get
            {
                return MembershipUser.Password;
            }
            set
            {
                MembershipUser.Password = value;
            }
        }

        public MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return MembershipUser.PasswordFormat;
            }
            set
            {
                MembershipUser.PasswordFormat = value;
            }
        }

        public string PasswordQuestion
        {
            get
            {
                return MembershipUser.PasswordQuestion;
            }
        }

        public int PurchaseOrder { get; set; }

        public string QQ { get; set; }

        [StringLengthValidator(0, 20, Ruleset = "ValDistributor", MessageTemplate = "真实姓名必须控制在20个字符以内")]
        public string RealName { get; set; }

        public int RegionId { get; set; }

        [StringLengthValidator(0, 300, Ruleset = "ValDistributor", MessageTemplate = "合作备忘录必须控制在300个字符以内")]
        public string Remark { get; set; }

        public decimal RequestBalance { get; set; }

        public string TelPhone { get; set; }

        public int TopRegionId { get; set; }

        public string TradePassword
        {
            get
            {
                return MembershipUser.TradePassword;
            }
            set
            {
                MembershipUser.TradePassword = value;
            }
        }

        public MembershipPasswordFormat TradePasswordFormat
        {
            get
            {
                return MembershipUser.TradePasswordFormat;
            }
            set
            {
                MembershipUser.TradePasswordFormat = value;
            }
        }

        public int UserId
        {
            get
            {
                return MembershipUser.UserId;
            }
            set
            {
                MembershipUser.UserId = value;
            }
        }

        public string Username
        {
            get
            {
                return MembershipUser.Username;
            }
            set
            {
                MembershipUser.Username = value;
            }
        }

        public Hidistro.Membership.Core.Enums.UserRole UserRole
        {
            get
            {
                return MembershipUser.UserRole;
            }
        }

        public string Wangwang { get; set; }

        public string Zipcode { get; set; }
    }
}

