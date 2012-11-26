using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Cryptography;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Security;

namespace Hidistro.UI.Web.Installer
{
    /// <summary>
    ///易分销-用户安装向导页
    /// </summary>
    public partial class Install : Page
    {

        #region 字段定义
        IList<string> errorMsgs;
        bool isAddDemo = false;
        bool testSuccessed = false;
        string action;
        string dbName;
        string dbPassword;
        string dbServer;
        string dbUsername;
        string email;
        string password;
        string password2;
        string siteDescription;
        string siteName;
        string username;
        #endregion

        /// <summary>
        ///页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadParameters();

            if (!string.IsNullOrEmpty(Request["isCallback"]) && (Request["isCallback"] == "true"))
            {

                bool flag = false;
                string ljson = "";

                if (action == "Test")
                {
                    flag = ExecuteTest();
                }

                if (flag)
                {

                    ljson = "{\"Status\":\"OK\"}";

                }
                else
                {

                    StringBuilder builder = new StringBuilder();

                    if ((errorMsgs != null) && (errorMsgs.Count > 0))
                    {

                        foreach (string item in errorMsgs)
                        {
                            builder.Append("{\"Text\":\"" + item + "\"},");
                        }

                        if (builder.Length != 0)
                        {
                            builder.Remove(builder.Length - 1, 1);
                        }

                        errorMsgs.Clear();

                    }
                    else
                    {

                        builder.Append("{\"Text\":\"无效的操作类型：" + action + "\"}");

                    }

                    ljson = "{\"Status\":\"Fail\",\"ErrorMsgs\":[" + builder.ToString() + "]}";

                }

                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write(ljson);
                Response.End();

            }

        }

        /// <summary>
        /// 初始化权限
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool AddBuiltInRoles(out string errorMsg)
        {

            bool flag = false;
            DbConnection connection = null;
            DbTransaction transaction = null;

            try
            {

                using (connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();

                    DbCommand command = connection.CreateCommand();

                    transaction = connection.BeginTransaction();

                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO aspnet_Roles(RoleName, LoweredRoleName) VALUES(@RoleName, LOWER(@RoleName))";

                    DbParameter parameter = new SqlParameter("@RoleName", SqlDbType.NVarChar, 256);
                    command.Parameters.Add(parameter);

                    RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;

                    command.Parameters["@RoleName"].Value = rolesConfiguration.Distributor;
                    command.ExecuteNonQuery();

                    command.Parameters["@RoleName"].Value = rolesConfiguration.Manager;
                    command.ExecuteNonQuery();

                    command.Parameters["@RoleName"].Value = rolesConfiguration.Member;
                    command.ExecuteNonQuery();

                    command.Parameters["@RoleName"].Value = rolesConfiguration.SystemAdministrator;
                    command.ExecuteNonQuery();

                    command.Parameters["@RoleName"].Value = rolesConfiguration.Underling;
                    command.ExecuteNonQuery();

                    transaction.Commit();

                    connection.Close();
                }

                errorMsg = "";

                flag = true;

            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exception2)
                    {
                        errorMsg = exception2.Message;
                    }
                }

                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }

            }
            return flag;

        }

        /// <summary>
        /// 添加测试数据
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool AddDemoData(out string errorMsg)
        {
            bool flag = false;
            string sqlpath = Request.MapPath("SqlScripts/SiteDemo.zh-CN.sql");
            if (!File.Exists(sqlpath))
            {
                errorMsg = "没有找到演示数据文件-SiteDemo.Sql";
            }
            else
            {
                flag = ExecuteScriptFile(sqlpath, out errorMsg);
            }
            return flag;
        }

        /// <summary>
        /// 添加初始数据
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool AddInitData(out string errorMsg)
        {
            bool flag = false;
            string scriptPath = Request.MapPath("SqlScripts/SiteInitData.zh-CN.Sql");
            if (!File.Exists(scriptPath))
            {
                errorMsg = "没有找到初始化数据文件-SiteInitData.Sql";
            }
            else
            {
                flag = ExecuteScriptFile(scriptPath, out errorMsg);
            }
            return flag;
        }

        /// <summary>
        /// 安装按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInstall_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            if (!ValidateUser(out msg))
            {
                ShowMsg(msg, false);
            }
            else if (!(testSuccessed || ExecuteTest()))
            {
                ShowMsg("数据库链接信息有误", false);
            }
            else if (!CreateDataSchema(out msg))
            {
                ShowMsg(msg, false);
            }
            else if (!AddBuiltInRoles(out msg))
            {
                ShowMsg(msg, false);
            }
            else if (!CreateAnonymous(out msg))
            {
                ShowMsg(msg, false);
            }
            else
            {
                int newUserid = 0;

                if (!CreateAdministrator(out newUserid, out msg))//创建管理员
                {
                    ShowMsg(msg, false);
                }
                else if (!CreateKey(newUserid, out msg))
                {
                    ShowMsg(msg, false);
                }
                else if (!AddInitData(out msg))
                {
                    ShowMsg(msg, false);
                }
                else if (!isAddDemo || AddDemoData(out msg))
                {
                    if (!SaveSiteSettings(out msg))
                    {
                        ShowMsg(msg, false);
                    }
                    else if (!SaveConfig(out msg))
                    {
                        ShowMsg(msg, false);
                    }
                    else
                    {
                        Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
                    }
                }
            }
        }

        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="newUserId"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool CreateAdministrator(out int newUserId, out string errorMsg)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            bool flag = false;
            try
            {
                using (connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                    RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;
                    DbCommand command = connection.CreateCommand();
                    transaction = connection.BeginTransaction();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT RoleId FROM aspnet_Roles WHERE [LoweredRoleName] = LOWER(@RoleName)";
                    command.Parameters.Add(new SqlParameter("@RoleName", rolesConfiguration.SystemAdministrator));
                    Guid guid = (Guid)command.ExecuteScalar();
                    command.Parameters["@RoleName"].Value = rolesConfiguration.Manager;
                    Guid guid2 = (Guid)command.ExecuteScalar();
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO aspnet_Users  (UserName, LoweredUserName, IsAnonymous, UserRole, LastActivityDate, Password, PasswordFormat, PasswordSalt, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, Email, LoweredEmail) VALUES (@Username, LOWER(@Username), 0, @UserRole, @CreateDate, @Password, @PasswordFormat, @PasswordSalt, 1, 0, @CreateDate, @CreateDate, @CreateDate, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), @Email, LOWER(@Email));SELECT @@IDENTITY";
                    command.Parameters.Add(new SqlParameter("@Username", username));
                    command.Parameters.Add(new SqlParameter("@UserRole", UserRole.SiteManager));
                    command.Parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    command.Parameters.Add(new SqlParameter("@Password", password));
                    command.Parameters.Add(new SqlParameter("@PasswordFormat", MembershipPasswordFormat.Clear));
                    command.Parameters.Add(new SqlParameter("@PasswordSalt", ""));
                    command.Parameters.Add(new SqlParameter("@Email", email));
                    newUserId = Convert.ToInt32(command.ExecuteScalar());
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO aspnet_Managers(UserId) VALUES(@UserId)";
                    command.Parameters.Add(new SqlParameter("@UserId", (int)newUserId));
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO aspnet_UsersInRoles(UserId, RoleId) VALUES(@UserId, @RoleId)";
                    command.Parameters.Add(new SqlParameter("@RoleId", guid2));
                    command.ExecuteNonQuery();
                    command.Parameters["@RoleId"].Value = guid;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    connection.Close();
                }
                errorMsg = "";
                flag = true;
            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
                newUserId = 0;
                if (transaction != null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exception2)
                    {
                        errorMsg = exception2.Message;
                    }
                }
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return flag;

        }

        /// <summary>
        /// 创建匿名用户
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool CreateAnonymous(out string errorMsg)
        {
            DbConnection connection = null;
            bool flag = false;

            try
            {
                using (connection = new SqlConnection(GetConnectionString()))
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = (SqlConnection)connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT INTO aspnet_Users  (UserName, LoweredUserName, IsAnonymous, UserRole, LastActivityDate, Password, PasswordFormat, PasswordSalt, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart) VALUES ('Anonymous', LOWER('Anonymous'), 1, @UserRole, @CreateDate, 'DVZTktxeMzDtXR7eik7Cdw==', 0, '', 1, 0, @CreateDate, @CreateDate, @CreateDate, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ))";
                    command.Parameters.Add(new SqlParameter("@UserRole", UserRole.Anonymous));
                    command.Parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                errorMsg = null;
                flag = true;

            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;
                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return flag;

        }

        //创建数据架构
        bool CreateDataSchema(out string errorMsg)
        {
            string path = Request.MapPath("SqlScripts/Schema.sql");
            if (!File.Exists(path))
            {
                errorMsg = "没有找到数据库架构文件-Schema.sql";
                return false;
            }
            return ExecuteScriptFile(path, out errorMsg);
        }

        //创建key
        static string CreateKey(int len)
        {

            byte[] data = new byte[len];

            new RNGCryptoServiceProvider().GetBytes(data);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(string.Format("{0:X2}", data[i]));//转成16进制
            }

            return builder.ToString();

        }

        //创建key
        bool CreateKey(int userId, out string errorMsg)
        {
            bool flag = false;
            try
            {
                byte[] plaintext = KeyManager.GenerateSymmetricKey(typeof(RijndaelManaged));

                string filename = Request.MapPath(Globals.ApplicationPath + "/config/key.config");

                byte[] inArray = Cryptographer.EncryptWithPassword(plaintext, password);

                XmlDocument document = new XmlDocument();
                document.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<Settings><Token></Token><Key></Key></Settings>");
                document.SelectSingleNode("Settings/Token").InnerText = userId.ToString(CultureInfo.InvariantCulture);
                document.SelectSingleNode("Settings/Key").InnerText = Convert.ToBase64String(inArray);

                //保存文件
                document.Save(filename);
                document.Save(Request.MapPath(Globals.ApplicationPath + "/config/key.config.bak"));

                CryptographyUtility.ZeroOutBytes(inArray);


                byte[] encryptedKey = System.Security.Cryptography.ProtectedData.Protect(plaintext, null, System.Security.Cryptography.DataProtectionScope.LocalMachine);
                using (Stream stream = new FileStream(Request.MapPath(Globals.ApplicationPath + "/config/Hishop.key"), FileMode.Create))
                {
                    KeyManager.Write(stream, encryptedKey, DataProtectionScope.LocalMachine);
                }

                CryptographyUtility.ZeroOutBytes(encryptedKey);
                CryptographyUtility.ZeroOutBytes(plaintext);

                errorMsg = "";
                flag = true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
            }

            return flag;

        }

        //执行脚本文件
        bool ExecuteScriptFile(string pathToScriptFile, out string errorMsg)
        {

            StreamReader reader = null;
            SqlConnection connection = null;
            bool flag = false;

            try
            {
                string applicationPath = Globals.ApplicationPath;

                using (reader = new StreamReader(pathToScriptFile))
                {
                    using (connection = new SqlConnection(GetConnectionString()))
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 60;

                        connection.Open();

                        string sql = "";

                        while (!reader.EndOfStream)
                        {

                            sql = NextSqlFromStream(reader);

                            if (!string.IsNullOrEmpty(sql))
                            {
                                cmd.CommandText = sql.Replace("$VirsualPath$", applicationPath);
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }
                }

                errorMsg = "";
                flag = true;

            }
            catch (SqlException exception)
            {
                errorMsg = exception.Message;

                if ((connection != null) && (connection.State != ConnectionState.Closed))
                {
                    connection.Close();
                    connection.Dispose();
                }

                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

            }

            return flag;
        }

        /// <summary>
        /// 测试执行
        /// </summary>
        /// <returns></returns>
        bool ExecuteTest()
        {
            errorMsgs = new List<string>();
            DbTransaction transaction = null;
            DbConnection connection = null;
            string msg;

            if (ValidateConnectionStrings(out msg))
            {

                #region 数据库测试
                try
                {
                    using (connection = new SqlConnection(GetConnectionString()))
                    {


                        connection.Open();                              //   打开数据库连接
                        transaction = connection.BeginTransaction();    //   开始事务
                        DbCommand command = connection.CreateCommand(); //   创建CMD对象

                        command.Connection = connection;
                        command.Transaction = transaction;

                        //创建测试表
                        command.CommandText = "CREATE TABLE installTest(Test bit NULL)";
                        command.ExecuteNonQuery();

                        //删除测试表
                        command.CommandText = "DROP TABLE installTest";
                        command.ExecuteNonQuery();

                        transaction.Commit();
                        connection.Close();


                    }
                }
                catch (Exception ex)
                {
                    errorMsgs.Add(ex.Message);
                    if (transaction != null)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception exception1)
                        {
                            errorMsgs.Add(exception1.Message);
                        }
                    }
                    if ((connection != null) && (connection.State != ConnectionState.Closed))
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
                #endregion

                #region 配置测试
                try
                {
                    Configuration configuration = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

                    if (configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString == "none")
                    {
                        configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "required";
                    }
                    else
                    {
                        configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "none";
                    }
                    configuration.Save();
                }
                catch (Exception exception)
                {
                    errorMsgs.Add(exception.Message);
                }

                #endregion

                #region config文件夹测试
                if (!TestFolder(Request.MapPath(Globals.ApplicationPath + "/config/test.txt"), out msg))
                {
                    errorMsgs.Add(msg);
                }
                #endregion

                #region storage文件夹测试
                if (!TestFolder(Request.MapPath(Globals.ApplicationPath + "/storage/test.txt"), out msg))
                {
                    errorMsgs.Add(msg);
                }
                #endregion

            }
            else
            {
                errorMsgs.Add(msg);
            }
            return (errorMsgs.Count == 0);

        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns></returns>
        string GetConnectionString()
        {
            return string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3};", dbServer, dbUsername, dbPassword, dbName);
           // return string.Format("server={0};uid={1};pwd={2};database={3};", dbServer, dbUsername, dbPassword, dbName);
        }

        /// <summary>
        /// 加载参数
        /// </summary>
        void LoadParameters()
        {
            if (!(string.IsNullOrEmpty(Request["isCallback"]) || !(Request["isCallback"] == "true")))
            {
                action = Request["action"];
                dbServer = Request["DBServer"];
                dbName = Request["DBName"];
                dbUsername = Request["DBUsername"];
                dbPassword = Request["DBPassword"];
                username = Request["Username"];
                email = Request["Email"];
                password = Request["Password"];
                password2 = Request["Password2"];
                isAddDemo = !string.IsNullOrEmpty(Request["IsAddDemo"]) && (Request["IsAddDemo"] == "true");
                testSuccessed = !string.IsNullOrEmpty(Request["TestSuccessed"]) && (Request["TestSuccessed"] == "true");
                siteName = (string.IsNullOrEmpty(Request["SiteName"]) || (Request["SiteName"].Trim().Length == 0)) ? "Hishop" : Request["SiteName"];
                siteDescription = (string.IsNullOrEmpty(Request["SiteDescription"]) || (Request["SiteDescription"].Trim().Length == 0)) ? "最安全，最专业的网上商店系统" : Request["SiteDescription"];
            }
            else
            {
                dbServer = txtDbServer.Text;
                dbName = txtDbName.Text;
                dbUsername = txtDbUsername.Text;
                dbPassword = txtDbPassword.Text;
                username = txtUsername.Text;
                email = txtEmail.Text;
                password = txtPassword.Text;
                password2 = txtPassword2.Text;
                isAddDemo = chkIsAddDemo.Checked;
                siteName = (txtSiteName.Text.Trim().Length == 0) ? "Hishop" : txtSiteName.Text;
                siteDescription = (txtSiteDescription.Text.Trim().Length == 0) ? "最安全，最专业的网上商店系统" : txtSiteDescription.Text;
            }
        }


        /// <summary>
        /// 从只读流中读出SQL
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        static string NextSqlFromStream(StreamReader reader)
        {

            StringBuilder sqlBuilder = new StringBuilder();

            string strA = reader.ReadLine().Trim();

            //从流中循环读取数据
            while (!reader.EndOfStream && (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0))
            {

                sqlBuilder.Append(strA + Environment.NewLine);

                strA = reader.ReadLine();

            }

            if (string.Compare(strA, "GO", true, CultureInfo.InvariantCulture) != 0)
            {
                sqlBuilder.Append(strA + Environment.NewLine);
            }

            return sqlBuilder.ToString();

        }


        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool SaveConfig(out string errorMsg)
        {
            bool flag = false;

            try
            {
                //获取配置文件
                Configuration configuration = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

                //移去配置文件的Installer项
                configuration.AppSettings.Settings.Remove("Installer");
                MachineKeySection section = (MachineKeySection)configuration.GetSection("system.web/machineKey");

                //设置数据
                section.ValidationKey = CreateKey(20);
                section.DecryptionKey = CreateKey(24);
                section.Validation = MachineKeyValidation.SHA1;
                section.Decryption = "3DES";

                configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = GetConnectionString();
                configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");

                //保存到配置文件
                configuration.Save();

                errorMsg = null;
                flag = true;

            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
            }
            return flag;
        }

        /// <summary>
        /// 保存站点配置
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        bool SaveSiteSettings(out string errorMsg)
        {

            bool flag = false;
            errorMsg = null;

            if ((siteName.Length > 30) || (siteDescription.Length > 30))
            {
                errorMsg = "网店名称和简单介绍的长度不能超过30个字符";
            }

            try
            {
                //获取本站点配置路径
                string filename = Request.MapPath(Globals.ApplicationPath + "/config/SiteSettings.config");

                XmlDocument doc = new XmlDocument();
                SiteSettings settings = new SiteSettings(Request.Url.Host, null);
                doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<Settings></Settings>");

                settings.SiteName = siteName;
                settings.SiteDescription = siteDescription;
                settings.WriteToXml(doc);

                //保存站点配置信息
                doc.Save(filename);

                flag = true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
            }

            return flag;

        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <param name="seccess"></param>
        void ShowMsg(string errorMsg, bool seccess)
        {
            lblErrMessage.Text = errorMsg;
        }

        /// <summary>
        /// 写入权限测试
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        static bool TestFolder(string folderPath, out string errorMsg)
        {

            bool flag = false;

            try
            {
                File.WriteAllText(folderPath, "Hi");
                File.AppendAllText(folderPath, ",This is a test file.");
                File.Delete(folderPath);
                errorMsg = null;
                flag = true;
            }
            catch (Exception exception)
            {
                errorMsg = exception.Message;
            }

            return flag;

        }

        /// <summary>
        /// 验证连接字符串
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool ValidateConnectionStrings(out string msg)
        {
            bool flag = false;

            if (!(string.IsNullOrEmpty(dbServer) || string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(dbUsername)))
            {
                msg = "";
                flag = true;
            }
            else
            {
                msg = "数据库连接信息不完整";
            }

            return flag;

        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        bool ValidateUser(out string msg)
        {
            msg = null;
            if ((string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email)) || (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password2)))
            {
                msg = "管理员账号信息不完整";
                return false;
            }
            HiConfiguration config = HiConfiguration.GetConfig();
            if ((username.Length > config.UsernameMaxLength) || (username.Length < config.UsernameMinLength))
            {
                msg = string.Format("管理员用户名的长度只能在{0}和{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength);
                return false;
            }
            if (string.Compare(username, "anonymous", true) == 0)
            {
                msg = "不能使用anonymous作为管理员用户名";
                return false;
            }
            if (!Regex.IsMatch(username, config.UsernameRegex))
            {
                msg = "管理员用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头";
                return false;
            }
            if (email.Length > 0x100)
            {
                msg = "电子邮件的长度必须小于256个字符";
                return false;
            }
            if (!Regex.IsMatch(email, config.EmailRegex))
            {
                msg = "电子邮件的格式错误";
                return false;
            }
            if (password != password2)
            {
                msg = "管理员登录密码两次输入不一致";
                return false;
            }
            if ((password.Length >= System.Web.Security.Membership.Provider.MinRequiredPasswordLength) && (password.Length <= config.PasswordMaxLength))
            {
                return true;
            }
            msg = string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength);
            return false;
        }
    }
}

