using Hidistro.Membership.Core.Enums;
using System;
using System.Runtime.CompilerServices;
using System.Web.Security;

namespace Hidistro.Membership.Core
{
    public class HiMembershipUser
    {

        DateTime? _BirthDate;

        DateTime _CreateDate;

        Hidistro.Membership.Core.Enums.Gender _Gender;

        bool _IsAnonymous;

        bool _IsLockedOut;

        bool _IsOpenBalance;

        DateTime _LastLockoutDate;

        DateTime _LastPasswordChangedDate;

        MembershipUser _Membership;

        string _MobilePIN;

        string _Password;

        MembershipPasswordFormat _PasswordFormat;

        string _PasswordQuestion;

        string _TradePassword;

        MembershipPasswordFormat _TradePasswordFormat;

        int _UserId;

        string _Username;

        Hidistro.Membership.Core.Enums.UserRole _UserRole;
        string comment;
        string email;
        bool isApproved;
        DateTime lastActivityDate;
        DateTime lastLoginDate;

        public HiMembershipUser(bool isAnonymous, Hidistro.Membership.Core.Enums.UserRole userRole)
        {
            if (isAnonymous && (userRole != Hidistro.Membership.Core.Enums.UserRole.Anonymous))
            {
                throw new Exception(string.Format("Current user is Anonymous, But the user role is '{0}'", userRole.ToString()));
            }
            this.UserRole = userRole;
            this.IsAnonymous = userRole == Hidistro.Membership.Core.Enums.UserRole.Anonymous;
        }

        public HiMembershipUser(bool isAnonymous, Hidistro.Membership.Core.Enums.UserRole userRole, MembershipUser mu)
            : this(isAnonymous, userRole)
        {
            this.RefreshMembershipUser(mu);
        }

        public virtual bool ChangePassword(string password, string newPassword)
        {
            return this.Membership.ChangePassword(password, newPassword);
        }

        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
            {
                return false;
            }
            if ((newQuestion.Length > 0x100) || (newAnswer.Length > 0x80))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(this.PasswordQuestion))
            {
                return false;
            }
            return MemberUserProvider.Instance().ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer);
        }

        public virtual bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            if (string.IsNullOrEmpty(newQuestion) || string.IsNullOrEmpty(newAnswer))
            {
                return false;
            }
            if ((newQuestion.Length > 0x100) || (newAnswer.Length > 0x80))
            {
                return false;
            }
            return (this.ValidatePasswordAnswer(oldAnswer) && MemberUserProvider.Instance().ChangePasswordQuestionAndAnswer(this.Username, newQuestion, newAnswer));
        }

        public virtual bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            try
            {
                string str = this.ResetPassword(answer);
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
                return this.ChangePassword(str, newPassword);
            }
            catch
            {
                return false;
            }
        }

        public void RefreshMembershipUser(MembershipUser mu)
        {
            if (mu == null)
            {
                throw new Exception("A null MembershipUser is not valid to instantiate a new User");
            }
            this.Membership = mu;
            this.Username = mu.UserName;
            this.UserId = (int)mu.ProviderUserKey;
            this.Comment = mu.Comment;
            this.LastLockoutDate = mu.LastLockoutDate;
            this.LastPasswordChangedDate = mu.LastPasswordChangedDate;
            this.LastLoginDate = mu.LastLoginDate;
            this.CreateDate = mu.CreationDate;
            this.IsLockedOut = mu.IsLockedOut;
            this.IsApproved = mu.IsApproved;
            this.PasswordQuestion = mu.PasswordQuestion;
            this.Email = mu.Email;
            this.LastActivityDate = mu.LastActivityDate;
        }

        public virtual string ResetPassword(string answer)
        {
            try
            {
                if (this.ValidatePasswordAnswer(answer))
                {
                    return this.Membership.ResetPassword();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public virtual bool ValidatePasswordAnswer(string answer)
        {
            return MemberUserProvider.Instance().ValidatePasswordAnswer(this.Username, answer);
        }

        public DateTime? BirthDate
        {

            get
            {
                return this._BirthDate;
            }

            set
            {
                this._BirthDate = value;
            }
        }

        public string Comment
        {
            get
            {
                return this.comment;
            }
            set
            {
                this.comment = value;
                if (this.Membership != null)
                {
                    this.Membership.Comment = value;
                }
            }
        }

        public DateTime CreateDate
        {

            get
            {
                return this._CreateDate;
            }

            set
            {
                this._CreateDate = value;
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = value;
                if (this.Membership != null)
                {
                    this.Membership.Email = value;
                }
            }
        }

        public Hidistro.Membership.Core.Enums.Gender Gender
        {

            get
            {
                return this._Gender;
            }

            set
            {
                this._Gender = value;
            }
        }

        public bool IsAnonymous
        {

            get
            {
                return this._IsAnonymous;
            }

            set
            {
                this._IsAnonymous = value;
            }
        }

        public bool IsApproved
        {
            get
            {
                return this.isApproved;
            }
            set
            {
                this.isApproved = value;
                if (this.Membership != null)
                {
                    this.Membership.IsApproved = value;
                }
            }
        }

        public bool IsLockedOut
        {

            get
            {
                return this._IsLockedOut;
            }

            set
            {
                this._IsLockedOut = value;
            }
        }

        public bool IsOpenBalance
        {

            get
            {
                return this._IsOpenBalance;
            }

            set
            {
                this._IsOpenBalance = value;
            }
        }

        public DateTime LastActivityDate
        {
            get
            {
                return this.lastActivityDate;
            }
            set
            {
                this.lastActivityDate = value;
                if (this.Membership != null)
                {
                    this.Membership.LastActivityDate = value;
                }
            }
        }

        public DateTime LastLockoutDate
        {

            get
            {
                return this._LastLockoutDate;
            }

            set
            {
                this._LastLockoutDate = value;
            }
        }

        public DateTime LastLoginDate
        {
            get
            {
                return this.lastLoginDate;
            }
            set
            {
                this.lastLoginDate = value;
                if (this.Membership != null)
                {
                    this.Membership.LastLoginDate = value;
                }
            }
        }

        public DateTime LastPasswordChangedDate
        {

            get
            {
                return this._LastPasswordChangedDate;
            }

            set
            {
                this._LastPasswordChangedDate = value;
            }
        }

        public MembershipUser Membership
        {

            get
            {
                return this._Membership;
            }

            set
            {
                this._Membership = value;
            }
        }

        public string MobilePIN
        {

            get
            {
                return this._MobilePIN;
            }

            set
            {
                this._MobilePIN = value;
            }
        }

        public string Password
        {

            get
            {
                return this._Password;
            }

            set
            {
                this._Password = value;
            }
        }

        public MembershipPasswordFormat PasswordFormat
        {

            get
            {
                return this._PasswordFormat;
            }

            set
            {
                this._PasswordFormat = value;
            }
        }

        public string PasswordQuestion
        {

            get
            {
                return this._PasswordQuestion;
            }

            set
            {
                this._PasswordQuestion = value;
            }
        }

        public string TradePassword
        {

            get
            {
                return this._TradePassword;
            }

            set
            {
                this._TradePassword = value;
            }
        }

        public MembershipPasswordFormat TradePasswordFormat
        {

            get
            {
                return this._TradePasswordFormat;
            }

            set
            {
                this._TradePasswordFormat = value;
            }
        }

        public int UserId
        {

            get
            {
                return this._UserId;
            }

            set
            {
                this._UserId = value;
            }
        }

        public string Username
        {

            get
            {
                return this._Username;
            }

            set
            {
                this._Username = value;
            }
        }

        public Hidistro.Membership.Core.Enums.UserRole UserRole
        {

            get
            {
                return this._UserRole;
            }

            set
            {
                this._UserRole = value;
            }
        }
    }
}

