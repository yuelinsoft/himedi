using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Cryptography;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using System;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EmailSettings)]
    public partial class EmailSettings : AdminPage
    {
        private void btnChangeEmailSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = LoadConfig(out str);
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                masterSettings.EmailSender = string.Empty;
                masterSettings.EmailSettings = string.Empty;
            }
            else
            {
                if (!data.IsValid)
                {
                    string msg = "";
                    foreach (string str3 in data.ErrorMsgs)
                    {
                        msg = msg + Formatter.FormatErrorMessage(str3);
                    }
                    ShowMsg(msg, false);
                    return;
                }
                masterSettings.EmailSender = str;
                masterSettings.EmailSettings = Cryptographer.Encrypt(data.SettingsXml);
            }
            SettingsManager.Save(masterSettings);
            Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
        }

        private void btnTestEmailSettings_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data = LoadConfig(out str);
            if (string.IsNullOrEmpty(str) || (data == null))
            {
                ShowMsg("请先选择发送方式并填写配置信息", false);
            }
            else if (!data.IsValid)
            {
                string msg = "";
                foreach (string str3 in data.ErrorMsgs)
                {
                    msg = msg + Formatter.FormatErrorMessage(str3);
                }
                ShowMsg(msg, false);
            }
            else if (string.IsNullOrEmpty(txtTestEmail.Text) || (txtTestEmail.Text.Trim().Length == 0))
            {
                ShowMsg("请填写接收测试邮件的邮箱地址", false);
            }
            else if (!Regex.IsMatch(txtTestEmail.Text.Trim(), @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                ShowMsg("请填写正确的邮箱地址", false);
            }
            else
            {
                MailMessage message2 = new MailMessage();
                message2.IsBodyHtml = true;
                message2.Priority = MailPriority.High;
                message2.Body = "Success";
                message2.Subject = "This is a test mail";
                MailMessage mail = message2;
                mail.To.Add(txtTestEmail.Text.Trim());
                EmailSender sender2 = EmailSender.CreateInstance(str, data.SettingsXml);
                try
                {
                    if (sender2.Send(mail, Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
                    {
                        ShowMsg("发送测试邮件成功", true);
                    }
                    else
                    {
                        ShowMsg("发送测试邮件失败", false);
                    }
                }
                catch (Exception exception)
                {
                    ShowMsg(exception.Message, false);
                }
            }
        }

        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlEmails"];
            txtSelectedName.Value = selectedName;
            txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || (selectedName.Length == 0))
            {
                return null;
            }
            ConfigablePlugin plugin = EmailSender.CreateInstance(selectedName);
            if (plugin == null)
            {
                return null;
            }
            ConfigData configData = plugin.GetConfigData(base.Request.Form);
            if (configData != null)
            {
                txtConfigData.Value = configData.SettingsXml;
            }
            return configData;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnChangeEmailSettings.Click += new EventHandler(btnChangeEmailSettings_Click);
            btnTestEmailSettings.Click += new EventHandler(btnTestEmailSettings_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                if (masterSettings.EmailEnabled)
                {
                    txtSelectedName.Value = masterSettings.EmailSender.ToLower();
                    ConfigData data = new ConfigData(Cryptographer.Decrypt(masterSettings.EmailSettings));
                    txtConfigData.Value = data.SettingsXml;
                }
            }
        }
    }
}

