using Hidistro.Membership.Core.Enums;
using System;
using System.Runtime.CompilerServices;
using System.Web.Security;

namespace Hidistro.Membership.Core
{
    public class AnonymousUser : IUser
    {

        HiMembershipUser _MembershipUser;

        public AnonymousUser(HiMembershipUser membershipUser)
        {
            if ((!membershipUser.IsAnonymous || (membershipUser.UserRole != Hidistro.Membership.Core.Enums.UserRole.Anonymous)) || (membershipUser.Username != "Anonymous"))
            {
                throw new Exception("Invalid AnonymousUser");
            }
            MembershipUser = membershipUser;
        }

        public bool ChangePassword(string newPassword)
        {
            return true;
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            return true;
        }

        public bool ChangePasswordQuestionAndAnswer(string newQuestion, string newAnswer)
        {
            return true;
        }

        public bool ChangePasswordQuestionAndAnswer(string oldAnswer, string newQuestion, string newAnswer)
        {
            return true;
        }

        public bool ChangePasswordWithAnswer(string answer, string newPassword)
        {
            return true;
        }

        public bool ChangeTradePassword(string newPassword)
        {
            return true;
        }

        public bool ChangeTradePassword(string oldPassword, string newPassword)
        {
            return true;
        }

        public IUserCookie GetUserCookie()
        {
            return null;
        }

        public bool IsInRole(string roleName)
        {
            return false;
        }

        public string ResetPassword(string answer)
        {
            return null;
        }

        public bool ValidatePasswordAnswer(string answer)
        {
            return true;
        }

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

        public HiMembershipUser MembershipUser
        {

            get
            {
                return _MembershipUser;
            }

            set
            {
                _MembershipUser = value;
            }
        }

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
    }
}

