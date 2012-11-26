using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace Hidistro.Membership.ASPNETProvider
{
    public class SqlMembershipProvider : MembershipProvider
    {
       string _AppName;
       int _CommandTimeout;
       bool _EnablePasswordReset;
       bool _EnablePasswordRetrieval;
       int _MaxInvalidPasswordAttempts;
       int _MinRequiredNonalphanumericCharacters;
       int _MinRequiredPasswordLength;
       int _PasswordAttemptWindow;
       MembershipPasswordFormat _PasswordFormat;
       string _PasswordStrengthRegularExpression;
       bool _RequiresQuestionAndAnswer;
       bool _RequiresUniqueEmail;
       string _sqlConnectionString;
       const int PASSWORD_SIZE = 14;

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            int num;
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            SecUtility.CheckParameter(ref oldPassword, true, true, false, 0x80, "oldPassword");
            SecUtility.CheckParameter(ref newPassword, true, true, false, 0x80, "newPassword");
            string salt = null;
            if (!CheckPassword(username, oldPassword, false, false, out salt, out num))
            {
                return false;
            }
            if (newPassword.Length < MinRequiredPasswordLength)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The length of parameter '{0}' needs to be greater or equal to '{1}'.", "newPassword", MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)));
            }
            int num3 = 0;
            for (int i = 0; i < newPassword.Length; i++)
            {
                if (!char.IsLetterOrDigit(newPassword, i))
                {
                    num3++;
                }
            }
            if (num3 < MinRequiredNonAlphanumericCharacters)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("Non alpha numeric characters in '{0}' needs to be greater than or equal to '{1}'.", "newPassword", MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)));
            }
            if ((PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(newPassword, PasswordStrengthRegularExpression))
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The parameter '{0}' does not match the regular expression specified in config file.", "newPassword"));
            }
            string objValue = EncodePassword(newPassword, num, salt);
            if (objValue.Length > 0x80)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The password is too long: it must not exceed 128 chars after encrypting."), "newPassword");
            }
            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(e);
            if (e.Cancel)
            {
                if (e.FailureInformation != null)
                {
                    throw e.FailureInformation;
                }
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The custom password validation failed."), "newPassword");
            }
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_SetPassword", connection.Connection)
                    {
                        CommandTimeout = CommandTimeout,
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(CreateInputParam("@NewPassword", SqlDbType.NVarChar, objValue));
                    command.Parameters.Add(CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, salt));
                    command.Parameters.Add(CreateInputParam("@PasswordFormat", SqlDbType.Int, num));
                    command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int status = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                    if (status != 0)
                    {
                        string exceptionText = GetExceptionText(status);
                        if (IsStatusDueToBadPassword(status))
                        {
                            throw new MembershipPasswordException(exceptionText);
                        }
                        throw new ProviderException(exceptionText);
                    }
                    flag = true;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return flag;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            string str;
            int num;
            string str2;
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            SecUtility.CheckParameter(ref password, true, true, false, 0x80, "password");
            if (!CheckPassword(username, password, false, false, out str, out num))
            {
                return false;
            }
            SecUtility.CheckParameter(ref newPasswordQuestion, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 0x100, "newPasswordQuestion");
            if (newPasswordAnswer != null)
            {
                newPasswordAnswer = newPasswordAnswer.Trim();
            }
            SecUtility.CheckParameter(ref newPasswordAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 0x80, "newPasswordAnswer");
            if (!string.IsNullOrEmpty(newPasswordAnswer))
            {
                str2 = EncodePassword(newPasswordAnswer.ToLower(CultureInfo.InvariantCulture), num, str);
            }
            else
            {
                str2 = newPasswordAnswer;
            }
            SecUtility.CheckParameter(ref str2, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 0x80, "newPasswordAnswer");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_ChangePasswordQuestionAndAnswer", connection.Connection)
                    {
                        CommandTimeout = CommandTimeout,
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(CreateInputParam("@NewPasswordQuestion", SqlDbType.NVarChar, newPasswordQuestion));
                    command.Parameters.Add(CreateInputParam("@NewPasswordAnswer", SqlDbType.NVarChar, str2));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int status = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                    if (status != 0)
                    {
                        throw new ProviderException(GetExceptionText(status));
                    }
                    flag = status == 0;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return flag;
        }

       bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved)
        {
            string str;
            int num;
            return CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved, out str, out num);
        }

       bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved, out string salt, out int passwordFormat)
        {
            SqlConnectionHolder connection = null;
            string str;
            int num;
            int num2;
            int num3;
            bool flag2;
            DateTime time;
            DateTime time2;
            GetPasswordWithFormat(username, updateLastLoginActivityDate, out num, out str, out passwordFormat, out salt, out num2, out num3, out flag2, out time, out time2);
            if (num != 0)
            {
                return false;
            }
            if (!flag2 && failIfNotApproved)
            {
                return false;
            }
            string str2 = EncodePassword(password, passwordFormat, salt);
            bool objValue = str.Equals(str2);
            if ((objValue && (num2 == 0)) && (num3 == 0))
            {
                return true;
            }
            try
            {
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_UpdateUserInfo", connection.Connection);
                    DateTime now = DateTime.Now;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(CreateInputParam("@IsPasswordCorrect", SqlDbType.Bit, objValue));
                    command.Parameters.Add(CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
                    command.Parameters.Add(CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, MaxInvalidPasswordAttempts));
                    command.Parameters.Add(CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, PasswordAttemptWindow));
                    command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, now));
                    command.Parameters.Add(CreateInputParam("@LastLoginDate", SqlDbType.DateTime, objValue ? now : time));
                    command.Parameters.Add(CreateInputParam("@LastActivityDate", SqlDbType.DateTime, objValue ? now : time2));
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    num = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return objValue;
        }

       SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
        {
            SqlParameter parameter = new SqlParameter(paramName, dbType);
            if (objValue == null)
            {
                parameter.IsNullable = true;
                parameter.Value = DBNull.Value;
                return parameter;
            }
            parameter.Value = objValue;
            return parameter;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            string str3;
            MembershipUser user;
            if (!SecUtility.ValidateParameter(ref password, true, true, false, 0x80))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            string salt = GenerateSalt();
            string objValue = EncodePassword(password, (int)_PasswordFormat, salt);
            if (objValue.Length > 0x80)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }
            if (!string.IsNullOrEmpty(passwordAnswer))
            {
                if (passwordAnswer.Length > 0x80)
                {
                    status = MembershipCreateStatus.InvalidAnswer;
                    return null;
                }
                str3 = EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), (int)_PasswordFormat, salt);
            }
            else
            {
                str3 = passwordAnswer;
            }
            if (!SecUtility.ValidateParameter(ref str3, RequiresQuestionAndAnswer, true, false, 0x80))
            {
                status = MembershipCreateStatus.InvalidAnswer;
                return null;
            }
            if (!SecUtility.ValidateParameter(ref username, true, true, true, 0x100))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }
            if (!SecUtility.ValidateParameter(ref email, RequiresUniqueEmail, RequiresUniqueEmail, false, 0x100))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }
            if (!SecUtility.ValidateParameter(ref passwordQuestion, RequiresQuestionAndAnswer, true, false, 0x100))
            {
                status = MembershipCreateStatus.InvalidQuestion;
                return null;
            }
            if (password.Length < MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            int num = 0;
            for (int i = 0; i < password.Length; i++)
            {
                if (!char.IsLetterOrDigit(password, i))
                {
                    num++;
                }
            }
            if (num < MinRequiredNonAlphanumericCharacters)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if ((PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(password, PasswordStrengthRegularExpression))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(e);
            if (e.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    DateTime time = RoundToSeconds(DateTime.Now);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_CreateUser", connection.Connection)
                    {
                        CommandTimeout = CommandTimeout,
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(CreateInputParam("@Password", SqlDbType.NVarChar, objValue));
                    command.Parameters.Add(CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, salt));
                    command.Parameters.Add(CreateInputParam("@Email", SqlDbType.NVarChar, email));
                    command.Parameters.Add(CreateInputParam("@PasswordQuestion", SqlDbType.NVarChar, passwordQuestion));
                    command.Parameters.Add(CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, str3));
                    command.Parameters.Add(CreateInputParam("@IsApproved", SqlDbType.Bit, isApproved));
                    command.Parameters.Add(CreateInputParam("@UniqueEmail", SqlDbType.Int, RequiresUniqueEmail ? 1 : 0));
                    command.Parameters.Add(CreateInputParam("@PasswordFormat", SqlDbType.Int, (int)PasswordFormat));
                    command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, time));
                    SqlParameter parameter = CreateInputParam("@UserId", SqlDbType.Int, providerUserKey);
                    parameter.Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int num3 = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                    if ((num3 < 0) || (num3 > 11))
                    {
                        num3 = 11;
                    }
                    status = (MembershipCreateStatus)num3;
                    if (num3 != 0)
                    {
                        return null;
                    }
                    providerUserKey = (int)command.Parameters["@UserId"].Value;
                    user = new MembershipUser(Name, username, providerUserKey, email, passwordQuestion, null, isApproved, false, time, time, time, time, new DateTime(0x6da, 1, 1));
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return user;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("aspnet_Membership_DeleteUser", connection.Connection)
                    {
                        CommandTimeout = CommandTimeout,
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    SqlParameter parameter = new SqlParameter("@NumTablesDeletedFrom", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    int num = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                    flag = num > 0;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return flag;
        }

        internal string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0)
            {
                return pass;
            }
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Convert.FromBase64String(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            byte[] inArray = null;
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            if (passwordFormat == 1)
            {
                inArray = HashAlgorithm.Create(System.Web.Security.Membership.HashAlgorithmType).ComputeHash(dst);
            }
            else
            {
                inArray = EncryptPassword(dst);
            }
            return Convert.ToBase64String(inArray);
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users2;
            SecUtility.CheckParameter(ref emailToMatch, false, false, false, 0x100, "emailToMatch");
            if (pageIndex < 0)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageSize must be greater than zero."), "pageSize");
            }
            long num = ((pageIndex * pageSize) + pageSize) - 1L;
            if (num > 0x7fffffffL)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
            }
            try
            {
                SqlConnectionHolder connection = null;
                totalRecords = 0;
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_FindUsersByEmail", connection.Connection);
                    MembershipUserCollection users = new MembershipUserCollection();
                    SqlDataReader reader = null;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(CreateInputParam("@EmailToMatch", SqlDbType.NVarChar, emailToMatch));
                    command.Parameters.Add(CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
                    command.Parameters.Add(CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (null != reader && reader.Read())
                        {
                            string nullableString = GetNullableString(reader, 0);
                            string email = GetNullableString(reader, 1);
                            string passwordQuestion = GetNullableString(reader, 2);
                            string comment = GetNullableString(reader, 3);
                            bool boolean = reader.GetBoolean(4);
                            DateTime dateTime = reader.GetDateTime(5);
                            DateTime lastLoginDate = reader.GetDateTime(6);
                            DateTime lastActivityDate = reader.GetDateTime(7);
                            DateTime lastPasswordChangedDate = reader.GetDateTime(8);
                            int providerUserKey = reader.GetInt32(9);
                            bool isLockedOut = reader.GetBoolean(10);
                            DateTime lastLockoutDate = reader.GetDateTime(11);
                            users.Add(new MembershipUser(Name, nullableString, providerUserKey, email, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate));
                        }
                        users2 = users;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        if ((parameter.Value != null) && (parameter.Value is int))
                        {
                            totalRecords = (int)parameter.Value;
                        }
                    }
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return users2;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users2;
            SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 0x100, "usernameToMatch");
            if (pageIndex < 0)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageSize must be greater than zero."), "pageSize");
            }
            long num = ((pageIndex * pageSize) + pageSize) - 1L;
            if (num > 0x7fffffffL)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
            }
            try
            {
                SqlConnectionHolder connection = null;
                totalRecords = 0;
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_FindUsersByName", connection.Connection);
                    MembershipUserCollection users = new MembershipUserCollection();
                    SqlDataReader reader = null;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch));
                    command.Parameters.Add(CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
                    command.Parameters.Add(CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (null != reader && reader.Read())
                        {
                            string nullableString = GetNullableString(reader, 0);
                            string email = GetNullableString(reader, 1);
                            string passwordQuestion = GetNullableString(reader, 2);
                            string comment = GetNullableString(reader, 3);
                            bool boolean = reader.GetBoolean(4);
                            DateTime dateTime = reader.GetDateTime(5);
                            DateTime lastLoginDate = reader.GetDateTime(6);
                            DateTime lastActivityDate = reader.GetDateTime(7);
                            DateTime lastPasswordChangedDate = reader.GetDateTime(8);
                            int providerUserKey = reader.GetInt32(9);
                            bool isLockedOut = reader.GetBoolean(10);
                            DateTime lastLockoutDate = reader.GetDateTime(11);
                            users.Add(new MembershipUser(Name, nullableString, providerUserKey, email, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate));
                        }
                        users2 = users;
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        if ((parameter.Value != null) && (parameter.Value is int))
                        {
                            totalRecords = (int)parameter.Value;
                        }
                    }
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            return users2;
        }

        public virtual string GeneratePassword()
        {
            return System.Web.Security.Membership.GeneratePassword((MinRequiredPasswordLength < 14) ? 14 : MinRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);
        }

        internal string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
            }
            if (pageSize < 1)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The pageSize must be greater than zero."), "pageSize");
            }
            long num = ((pageIndex * pageSize) + pageSize) - 1L;
            if (num > 0x7fffffffL)
            {
                throw new ArgumentException(Hidistro.Membership.ASPNETProvider.SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
            }
            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;
            try
            {
                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetAllUsers", connection.Connection);
                    SqlDataReader reader = null;
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                    command.CommandTimeout = CommandTimeout;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
                    command.Parameters.Add(CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
                    parameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(parameter);
                    try
                    {
                        reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                        while (null != reader && reader.Read())
                        {
                            string nullableString = GetNullableString(reader, 0);
                            string email = GetNullableString(reader, 1);
                            string passwordQuestion = GetNullableString(reader, 2);
                            string comment = GetNullableString(reader, 3);
                            bool boolean = reader.GetBoolean(4);
                            DateTime dateTime = reader.GetDateTime(5);
                            DateTime lastLoginDate = reader.GetDateTime(6);
                            DateTime lastActivityDate = reader.GetDateTime(7);
                            DateTime lastPasswordChangedDate = reader.GetDateTime(8);
                            int providerUserKey = reader.GetInt32(9);
                            bool isLockedOut = reader.GetBoolean(10);
                            DateTime lastLockoutDate = reader.GetDateTime(11);
                            users.Add(new MembershipUser(Name, nullableString, providerUserKey, email, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate));
                        }
                    }
                    finally
                    {
                        if (reader != null)
                        {
                            reader.Close();
                        }
                        if ((parameter.Value != null) && (parameter.Value is int))
                        {
                            totalRecords = (int)parameter.Value;
                        }
                    }
                    return users;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }
            }
            catch
            {
                throw;
            }
            //return users;
        }

       string GetEncodedPasswordAnswer(string username, string passwordAnswer)
        {
            int num;
            int num2;
            int num3;
            int num4;
            string str;
            string str2;
            bool flag;
            DateTime time;
            DateTime time2;
            if (passwordAnswer != null)
            {
                passwordAnswer = passwordAnswer.Trim();
            }
            if (string.IsNullOrEmpty(passwordAnswer))
            {
                return passwordAnswer;
            }
            GetPasswordWithFormat(username, false, out num, out str, out num2, out str2, out num3, out num4, out flag, out time, out time2);
            if (num != 0)
            {
                throw new ProviderException(GetExceptionText(num));
            }
            return EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), num2, str2);
        }

       string GetExceptionText(int status)
        {
            string str;
            switch (status)
            {
                case 0:
                    return string.Empty;

                case 1:
                    str = "The user was not found.";
                    break;

                case 2:
                    str = "The password supplied is wrong.";
                    break;

                case 3:
                    str = "The password-answer supplied is wrong.";
                    break;

                case 4:
                    str = "The password supplied is invalid.  Passwords must conform to the password strength requirements configured for the default provider.";
                    break;

                case 5:
                    str = "The password-question supplied is invalid.  Note that the current provider configuration requires a valid password question and answer.  As a result, a CreateUser overload that accepts question and answer parameters must also be used.";
                    break;

                case 6:
                    str = "The password-answer supplied is invalid.";
                    break;

                case 7:
                    str = "The E-mail supplied is invalid.";
                    break;

                case 0x63:
                    str = "The user account has been locked out.";
                    break;

                default:
                    str = "The Provider encountered an unknown error.";
                    break;
            }
            return Hidistro.Membership.ASPNETProvider.SR.GetString(str);
        }

       string GetNullableString(SqlDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetString(col);
            }
            return null;
        }

        public override int GetNumberOfUsersOnline()
        {
            int num = 0;

            SqlConnectionHolder connection = null;

            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetNumberOfUsersOnline", connection.Connection);
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(CreateInputParam("@MinutesSinceLastInActive", SqlDbType.Int, System.Web.Security.Membership.UserIsOnlineTimeWindow));
                command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                parameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                num = (parameter.Value != null) ? ((int)parameter.Value) : -1;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return num;
        }

        public override string GetPassword(string username, string passwordAnswer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new NotSupportedException(Hidistro.Membership.ASPNETProvider.SR.GetString("This Membership Provider has not been configured to support password retrieval."));
            }
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            string encodedPasswordAnswer = GetEncodedPasswordAnswer(username, passwordAnswer);
            SecUtility.CheckParameter(ref encodedPasswordAnswer, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 0x80, "passwordAnswer");
            int passwordFormat = 0;
            int status = 0;
            string pass = GetPasswordFromDB(username, encodedPasswordAnswer, RequiresQuestionAndAnswer, out passwordFormat, out status);
            if (pass != null)
            {
                return UnEncodePassword(pass, passwordFormat);
            }
            string exceptionText = GetExceptionText(status);
            if (IsStatusDueToBadPassword(status))
            {
                throw new MembershipPasswordException(exceptionText);
            }
            throw new ProviderException(exceptionText);
        }

       string GetPasswordFromDB(string username, string passwordAnswer, bool requiresQuestionAndAnswer, out int passwordFormat, out int status)
        {
            string str2;

            SqlConnectionHolder connection = null;
            SqlDataReader reader = null;
            SqlParameter parameter = null;
            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetPassword", connection.Connection)
                {
                    CommandTimeout = CommandTimeout,
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                command.Parameters.Add(CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, MaxInvalidPasswordAttempts));
                command.Parameters.Add(CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, PasswordAttemptWindow));
                command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                if (requiresQuestionAndAnswer)
                {
                    command.Parameters.Add(CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, passwordAnswer));
                }
                parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(parameter);
                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                string str = null;
                status = -1;
                if (reader.Read())
                {
                    str = reader.GetString(0);
                    passwordFormat = reader.GetInt32(1);
                }
                else
                {
                    str = null;
                    passwordFormat = 0;
                }
                str2 = str;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                    status = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return str2;
        }

       void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password, out int passwordFormat, out string passwordSalt, out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
        {

            SqlConnectionHolder connection = null;
            SqlDataReader reader = null;
            SqlParameter parameter = null;
            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetPasswordWithFormat", connection.Connection)
                {
                    CommandTimeout = CommandTimeout,
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                command.Parameters.Add(CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
                command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(parameter);
                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                status = -1;
                if (reader.Read())
                {
                    password = reader.GetString(0);
                    passwordFormat = reader.GetInt32(1);
                    passwordSalt = reader.GetString(2);
                    failedPasswordAttemptCount = reader.GetInt32(3);
                    failedPasswordAnswerAttemptCount = reader.GetInt32(4);
                    isApproved = reader.GetBoolean(5);
                    lastLoginDate = reader.GetDateTime(6);
                    lastActivityDate = reader.GetDateTime(7);
                }
                else
                {
                    password = null;
                    passwordFormat = 0;
                    passwordSalt = null;
                    failedPasswordAttemptCount = 0;
                    failedPasswordAnswerAttemptCount = 0;
                    isApproved = false;
                    lastLoginDate = DateTime.Now;
                    lastActivityDate = DateTime.Now;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                    status = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            MembershipUser user;
            if (providerUserKey == null)
            {
                throw new ArgumentNullException("providerUserKey");
            }
            SqlDataReader reader = null;

            SqlConnectionHolder connection = null;
            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetUserByUserId", connection.Connection)
                {
                    CommandTimeout = CommandTimeout,
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(CreateInputParam("@UserId", SqlDbType.Int, providerUserKey));
                command.Parameters.Add(CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
                command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(parameter);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string nullableString = GetNullableString(reader, 0);
                    string passwordQuestion = GetNullableString(reader, 1);
                    string comment = GetNullableString(reader, 2);
                    bool boolean = reader.GetBoolean(3);
                    DateTime dateTime = reader.GetDateTime(4);
                    DateTime lastLoginDate = reader.GetDateTime(5);
                    DateTime lastActivityDate = reader.GetDateTime(6);
                    DateTime lastPasswordChangedDate = reader.GetDateTime(7);
                    string name = GetNullableString(reader, 8);
                    bool isLockedOut = reader.GetBoolean(9);
                    return new MembershipUser(Name, name, providerUserKey, nullableString, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, reader.GetDateTime(10));
                }
                user = null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return user;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser user;
            SecUtility.CheckParameter(ref username, true, false, true, 0x100, "username");
            SqlDataReader reader = null;
            SqlConnectionHolder connection = null;
            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetUserByName", connection.Connection)
                {
                    CommandTimeout = CommandTimeout,
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                command.Parameters.Add(CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
                command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(parameter);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string nullableString = GetNullableString(reader, 0);
                    string passwordQuestion = GetNullableString(reader, 1);
                    string comment = GetNullableString(reader, 2);
                    bool boolean = reader.GetBoolean(3);
                    DateTime dateTime = reader.GetDateTime(4);
                    DateTime lastLoginDate = reader.GetDateTime(5);
                    DateTime lastActivityDate = reader.GetDateTime(6);
                    DateTime lastPasswordChangedDate = reader.GetDateTime(7);
                    int providerUserKey = reader.GetInt32(8);
                    bool isLockedOut = reader.GetBoolean(9);
                    return new MembershipUser(Name, username, providerUserKey, nullableString, passwordQuestion, comment, boolean, isLockedOut, dateTime, lastLoginDate, lastActivityDate, lastPasswordChangedDate, reader.GetDateTime(10));
                }
                user = null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return user;
        }

        public override string GetUserNameByEmail(string email)
        {
            string str2;
            SecUtility.CheckParameter(ref email, false, false, false, 0x100, "email");

            SqlConnectionHolder connection = null;
            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_GetUserByEmail", connection.Connection);
                string nullableString = null;
                SqlDataReader reader = null;
                command.CommandTimeout = CommandTimeout;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(CreateInputParam("@Email", SqlDbType.NVarChar, email));
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(parameter);
                try
                {
                    reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
                    if (reader.Read())
                    {
                        nullableString = GetNullableString(reader, 0);
                        if (RequiresUniqueEmail && reader.Read())
                        {
                            throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("More than one user has the specified e-mail address."));
                        }
                    }
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                str2 = nullableString;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return str2;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.IsNullOrEmpty(name))
            {
                name = "SqlMembershipProvider";
            }
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", Hidistro.Membership.ASPNETProvider.SR.GetString("SQL membership provider."));
            }
            base.Initialize(name, config);
            _EnablePasswordRetrieval = SecUtility.GetBooleanValue(config, "enablePasswordRetrieval", false);
            _EnablePasswordReset = SecUtility.GetBooleanValue(config, "enablePasswordReset", true);
            _RequiresQuestionAndAnswer = SecUtility.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
            _RequiresUniqueEmail = SecUtility.GetBooleanValue(config, "requiresUniqueEmail", true);
            _MaxInvalidPasswordAttempts = SecUtility.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
            _PasswordAttemptWindow = SecUtility.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
            _MinRequiredPasswordLength = SecUtility.GetIntValue(config, "minRequiredPasswordLength", 7, false, 0x80);
            _MinRequiredNonalphanumericCharacters = SecUtility.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 0x80);
            _PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
            if (_PasswordStrengthRegularExpression != null)
            {
                _PasswordStrengthRegularExpression = _PasswordStrengthRegularExpression.Trim();
                if (_PasswordStrengthRegularExpression.Length == 0)
                {
                    goto Label_0156;
                }
                try
                {
                    new Regex(_PasswordStrengthRegularExpression);
                    goto Label_0156;
                }
                catch (ArgumentException exception)
                {
                    throw new ProviderException(exception.Message, exception);
                }
            }
            _PasswordStrengthRegularExpression = string.Empty;
        Label_0156:
            if (_MinRequiredNonalphanumericCharacters > _MinRequiredPasswordLength)
            {
                throw new HttpException(Hidistro.Membership.ASPNETProvider.SR.GetString("The minRequiredNonalphanumericCharacters can not be greater than minRequiredPasswordLength."));
            }
            _CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
            _AppName = config["applicationName"];
            if (string.IsNullOrEmpty(_AppName))
            {
                _AppName = SecUtility.GetDefaultAppName();
            }
            if (_AppName.Length > 0x100)
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The application name is too long."));
            }
            string str = config["passwordFormat"];
            if (str == null)
            {
                str = "Hashed";
            }
            string str4 = str;
            if (str4 != null)
            {
                if (!(str4 == "Clear"))
                {
                    if (str4 == "Encrypted")
                    {
                        _PasswordFormat = MembershipPasswordFormat.Encrypted;
                        goto Label_0246;
                    }
                    if (str4 == "Hashed")
                    {
                        _PasswordFormat = MembershipPasswordFormat.Hashed;
                        goto Label_0246;
                    }
                }
                else
                {
                    _PasswordFormat = MembershipPasswordFormat.Clear;
                    goto Label_0246;
                }
            }
            throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Password format specified is invalid."));
        Label_0246:
            if ((PasswordFormat == MembershipPasswordFormat.Hashed) && EnablePasswordRetrieval)
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set supportsPasswordRetrieval to false."));
            }
            string specifiedConnectionString = config["connectionStringName"];
            if ((specifiedConnectionString == null) || (specifiedConnectionString.Length < 1))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The attribute 'connectionStringName' is missing or empty."));
            }
            _sqlConnectionString = SqlConnectionHelper.GetConnectionString(specifiedConnectionString, true, true);
            if ((_sqlConnectionString == null) || (_sqlConnectionString.Length < 1))
            {
                throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The connection name '{0}' was not found in the applications configuration or the connection string is empty.", specifiedConnectionString));
            }
            config.Remove("connectionStringName");
            config.Remove("enablePasswordRetrieval");
            config.Remove("enablePasswordReset");
            config.Remove("requiresQuestionAndAnswer");
            config.Remove("applicationName");
            config.Remove("requiresUniqueEmail");
            config.Remove("maxInvalidPasswordAttempts");
            config.Remove("passwordAttemptWindow");
            config.Remove("commandTimeout");
            config.Remove("passwordFormat");
            config.Remove("name");
            config.Remove("minRequiredPasswordLength");
            config.Remove("minRequiredNonalphanumericCharacters");
            config.Remove("passwordStrengthRegularExpression");
            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Attribute not recognized '{0}'", key));
                }
            }
        }

       bool IsStatusDueToBadPassword(int status)
        {
            return (((status >= 2) && (status <= 6)) || (status == 0x63));
        }

        public override string ResetPassword(string username, string passwordAnswer)
        {
            string str;
            int num;
            string str2;
            int num2;
            int num3;
            int num4;
            bool flag;
            DateTime time;
            DateTime time2;
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException(Hidistro.Membership.ASPNETProvider.SR.GetString("This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file."));
            }
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");
            GetPasswordWithFormat(username, false, out num2, out str2, out num, out str, out num3, out num4, out flag, out time, out time2);
            if (num2 == 0)
            {
                string str3;
                string str6;
                if (passwordAnswer != null)
                {
                    passwordAnswer = passwordAnswer.Trim();
                }
                if (!string.IsNullOrEmpty(passwordAnswer))
                {
                    str3 = EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), num, str);
                }
                else
                {
                    str3 = passwordAnswer;
                }
                SecUtility.CheckParameter(ref str3, RequiresQuestionAndAnswer, RequiresQuestionAndAnswer, false, 0x80, "passwordAnswer");
                string password = GeneratePassword();
                ValidatePasswordEventArgs e = new ValidatePasswordEventArgs(username, password, false);
                OnValidatingPassword(e);
                if (e.Cancel)
                {
                    if (e.FailureInformation != null)
                    {
                        throw e.FailureInformation;
                    }
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The custom password validation failed."));
                }

                SqlConnectionHolder connection = null;
                try
                {
                    connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                    SqlCommand command = new SqlCommand("dbo.aspnet_Membership_ResetPassword", connection.Connection)
                    {
                        CommandTimeout = CommandTimeout,
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                    command.Parameters.Add(CreateInputParam("@NewPassword", SqlDbType.NVarChar, EncodePassword(password, num, str)));
                    command.Parameters.Add(CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, MaxInvalidPasswordAttempts));
                    command.Parameters.Add(CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, PasswordAttemptWindow));
                    command.Parameters.Add(CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, str));
                    command.Parameters.Add(CreateInputParam("@PasswordFormat", SqlDbType.Int, num));
                    command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                    if (RequiresQuestionAndAnswer)
                    {
                        command.Parameters.Add(CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, str3));
                    }
                    SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                    num2 = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                    if (num2 != 0)
                    {
                        string exceptionText = GetExceptionText(num2);
                        if (IsStatusDueToBadPassword(num2))
                        {
                            throw new MembershipPasswordException(exceptionText);
                        }
                        throw new ProviderException(exceptionText);
                    }
                    str6 = password;
                }
                finally
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection = null;
                    }
                }

                return str6;
            }
            if (IsStatusDueToBadPassword(num2))
            {
                throw new MembershipPasswordException(GetExceptionText(num2));
            }
            throw new ProviderException(GetExceptionText(num2));
        }

       DateTime RoundToSeconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        internal string UnEncodePassword(string pass, int passwordFormat)
        {
            switch (passwordFormat)
            {
                case 0:
                    return pass;

                case 1:
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("Hashed passwords cannot be decoded."));
            }
            byte[] encodedPassword = Convert.FromBase64String(pass);
            byte[] bytes = DecryptPassword(encodedPassword);
            if (bytes == null)
            {
                return null;
            }
            return Encoding.Unicode.GetString(bytes, 0x10, bytes.Length - 0x10);
        }

        public override bool UnlockUser(string username)
        {
            bool flag;
            SecUtility.CheckParameter(ref username, true, true, true, 0x100, "username");

            SqlConnectionHolder connection = null;
            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_UnlockUser", connection.Connection)
                {
                    CommandTimeout = CommandTimeout,
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, username));
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                if (((parameter.Value != null) ? ((int)parameter.Value) : -1) == 0)
                {
                    return true;
                }
                flag = false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

            return flag;
        }

        public override void UpdateUser(MembershipUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            string userName = user.UserName;
            SecUtility.CheckParameter(ref userName, true, true, true, 0x100, "UserName");
            string email = user.Email;
            SecUtility.CheckParameter(ref email, RequiresUniqueEmail, RequiresUniqueEmail, false, 0x100, "Email");
            user.Email = email;

            SqlConnectionHolder connection = null;
            try
            {
                connection = SqlConnectionHelper.GetConnection(_sqlConnectionString, true);
                SqlCommand command = new SqlCommand("dbo.aspnet_Membership_UpdateUser", connection.Connection)
                {
                    CommandTimeout = CommandTimeout,
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(CreateInputParam("@UserName", SqlDbType.NVarChar, user.UserName));
                command.Parameters.Add(CreateInputParam("@Email", SqlDbType.NVarChar, user.Email));
                command.Parameters.Add(CreateInputParam("@Comment", SqlDbType.NText, user.Comment));
                command.Parameters.Add(CreateInputParam("@IsApproved", SqlDbType.Bit, user.IsApproved ? 1 : 0));
                command.Parameters.Add(CreateInputParam("@LastLoginDate", SqlDbType.DateTime, user.LastLoginDate));
                command.Parameters.Add(CreateInputParam("@LastActivityDate", SqlDbType.DateTime, user.LastActivityDate));
                command.Parameters.Add(CreateInputParam("@UniqueEmail", SqlDbType.Int, RequiresUniqueEmail ? 1 : 0));
                command.Parameters.Add(CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
                SqlParameter parameter = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                int status = (parameter.Value != null) ? ((int)parameter.Value) : -1;
                if (status != 0)
                {
                    throw new ProviderException(GetExceptionText(status));
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }

        }

        public override bool ValidateUser(string username, string password)
        {
            return ((SecUtility.ValidateParameter(ref username, true, true, true, 0x100) && SecUtility.ValidateParameter(ref password, true, true, false, 0x80)) && CheckPassword(username, password, true, true));
        }

        public override string ApplicationName
        {
            get
            {
                return _AppName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Length > 0x100)
                {
                    throw new ProviderException(Hidistro.Membership.ASPNETProvider.SR.GetString("The application name is too long."));
                }
                _AppName = value;
            }
        }

       int CommandTimeout
        {
            get
            {
                return _CommandTimeout;
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return _EnablePasswordReset;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return _EnablePasswordRetrieval;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return _MaxInvalidPasswordAttempts;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return _MinRequiredNonalphanumericCharacters;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return _MinRequiredPasswordLength;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return _PasswordAttemptWindow;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return _PasswordFormat;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return _PasswordStrengthRegularExpression;
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return _RequiresQuestionAndAnswer;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return _RequiresUniqueEmail;
            }
        }
    }
}

