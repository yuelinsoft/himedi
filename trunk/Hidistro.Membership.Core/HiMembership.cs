using Hidistro.Membership.Core.Enums;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.Security;

namespace Hidistro.Membership.Core
{
    public static class HiMembership
    {
        public static MembershipUser Create(string username, string password, string email)
        {
            MembershipUser user = null;
            CreateUserStatus unknownFailure = CreateUserStatus.UnknownFailure;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user = System.Web.Security.Membership.CreateUser(username, password, email);
                    unknownFailure = (user == null) ? CreateUserStatus.UnknownFailure : CreateUserStatus.Created;
                }
                else
                {
                    user = System.Web.Security.Membership.CreateUser(username, password);
                    unknownFailure = (user == null) ? CreateUserStatus.UnknownFailure : CreateUserStatus.Created;
                }
            }
            catch (MembershipCreateUserException exception)
            {
                unknownFailure = GetCreateUserStatus(exception.StatusCode);
            }
            catch (Exception exception2)
            {
                unknownFailure = GetCreateUserStatus(exception2);
            }
            if (unknownFailure != CreateUserStatus.Created)
            {
                throw new CreateUserException(unknownFailure);
            }
            return user;
        }

        public static bool Delete(string username)
        {
            return System.Web.Security.Membership.DeleteUser(username, true);
        }

        public static string GeneratePassword(int length, int alphaNumbericCharacters)
        {
            return System.Web.Security.Membership.GeneratePassword(length, alphaNumbericCharacters);
        }

        public static CreateUserStatus GetCreateUserStatus(Exception ex)
        {
            MembershipCreateUserException exception = ex as MembershipCreateUserException;
            if (exception != null)
            {
                return GetCreateUserStatus(exception.StatusCode);
            }
            return CreateUserStatus.UnknownFailure;
        }

        public static CreateUserStatus GetCreateUserStatus(MembershipCreateStatus msc)
        {
            switch (msc)
            {
                case MembershipCreateStatus.Success:
                    return CreateUserStatus.Created;

                case MembershipCreateStatus.InvalidUserName:
                    return CreateUserStatus.InvalidUserName;

                case MembershipCreateStatus.InvalidPassword:
                    return CreateUserStatus.InvalidPassword;

                case MembershipCreateStatus.InvalidQuestion:
                    return CreateUserStatus.InvalidQuestionAnswer;

                case MembershipCreateStatus.InvalidAnswer:
                    return CreateUserStatus.InvalidQuestionAnswer;

                case MembershipCreateStatus.InvalidEmail:
                    return CreateUserStatus.InvalidEmail;

                case MembershipCreateStatus.DuplicateUserName:
                    return CreateUserStatus.DuplicateUsername;

                case MembershipCreateStatus.DuplicateEmail:
                    return CreateUserStatus.DuplicateEmailAddress;

                case MembershipCreateStatus.UserRejected:
                    return CreateUserStatus.DisallowedUsername;

                case MembershipCreateStatus.InvalidProviderUserKey:
                    return CreateUserStatus.UnknownFailure;

                case MembershipCreateStatus.DuplicateProviderUserKey:
                    return CreateUserStatus.UnknownFailure;

                case MembershipCreateStatus.ProviderError:
                    return CreateUserStatus.UnknownFailure;
            }
            return CreateUserStatus.UnknownFailure;
        }

        public static MembershipUser GetUser(object userId)
        {
            return GetUser(userId, false);
        }

        public static MembershipUser GetUser(string username)
        {
            return GetUser(username, false);
        }

        public static MembershipUser GetUser(object userId, bool userIsOnline)
        {
            return System.Web.Security.Membership.GetUser(userId, userIsOnline);
        }

        public static MembershipUser GetUser(string username, bool userIsOnline)
        {
            return System.Web.Security.Membership.GetUser(username, userIsOnline);
        }

        public static bool PasswordIsMembershipCompliant(string newPassword, out string errorMessage)
        {
            errorMessage = "";
            if (null == newPassword)
            {
                return false;
            }
            int minRequiredPasswordLength = System.Web.Security.Membership.MinRequiredPasswordLength;
            int minRequiredNonAlphanumericCharacters = System.Web.Security.Membership.MinRequiredNonAlphanumericCharacters;
            if (newPassword.Length < minRequiredPasswordLength)
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, "密码太短，最少需要 {0} 个字符", new object[] { System.Web.Security.Membership.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture) });
                return false;
            }
            int num3 = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                {
                    num3++;
                }
            }
            if (num3 < minRequiredNonAlphanumericCharacters)
            {
                errorMessage = string.Format(CultureInfo.InvariantCulture, "密码包含的特殊字符太少, 最少要包含 {0} 个特殊字符", new object[] { System.Web.Security.Membership.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture) });
                return false;
            }
            return true;
        }

        public static void Update(MembershipUser user)
        {
            if (user == null)
            {
                throw new Exception("Member can not be null");
            }
            System.Web.Security.Membership.UpdateUser(user);
        }

        public static bool ValidateUser(string username, string password)
        {
            return System.Web.Security.Membership.ValidateUser(username, password);
        }
    }
}

