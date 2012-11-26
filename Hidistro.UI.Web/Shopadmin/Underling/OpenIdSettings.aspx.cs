    using Hidistro.Core.Cryptography;
    using Hidistro.Entities.Members;
    using Hidistro.Subsites.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Plugins;
    using kindeditor.Net;
    using System;
    using System.Runtime.InteropServices;
    using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class OpenIdSettings : DistributorPage
    {

         string openIdType;


        private void btnSave_Click(object sender, EventArgs e)
        {
            ConfigData data;
            if (ValidateValues(out data))
            {
                OpenIdSettingsInfo settings = new OpenIdSettingsInfo();
                settings.Name = txtName.Text.Trim();
                settings.Description = fcContent.Text;
                settings.OpenIdType = openIdType;
                settings.Settings = Cryptographer.Encrypt(data.SettingsXml);
                SubSiteOpenIdHelper.SaveSettings(settings);
                Response.Redirect("openidservices.aspx");
            }
        }

        private ConfigData LoadConfig()
        {
            txtSelectedName.Value = openIdType;
            txtConfigData.Value = "";
            ConfigablePlugin plugin = OpenIdService.CreateInstance(openIdType);
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
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            openIdType = base.Request.QueryString["t"];
            if (string.IsNullOrEmpty(openIdType) || (openIdType.Trim().Length == 0))
            {
                base.GotoResourceNotFound();
            }
            PluginItem pluginItem = OpenIdPlugins.Instance().GetPluginItem(openIdType);
            if (pluginItem == null)
            {
                base.GotoResourceNotFound();
            }
            if (!Page.IsPostBack)
            {
                lblDisplayName.Text = pluginItem.DisplayName;
                txtSelectedName.Value = openIdType;
                OpenIdSettingsInfo openIdSettings = SubSiteOpenIdHelper.GetOpenIdSettings(openIdType);
                if (openIdSettings != null)
                {
                    ConfigData data = new ConfigData(Cryptographer.Decrypt(openIdSettings.Settings));
                    txtConfigData.Value = data.SettingsXml;
                    txtName.Text = openIdSettings.Name;
                    fcContent.Text = openIdSettings.Description;
                }
            }
        }

        private bool ValidateValues(out ConfigData data)
        {
            string str = string.Empty;
            data = LoadConfig();
            if (!data.IsValid)
            {
                foreach (string str2 in data.ErrorMsgs)
                {
                    str = str + Formatter.FormatErrorMessage(str2);
                }
            }
            if ((string.IsNullOrEmpty(txtName.Text) || (txtName.Text.Trim().Length == 0)) || (txtName.Text.Length > 50))
            {
                str = str + Formatter.FormatErrorMessage("显示名称不能为空，长度限制在50个字符以内");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

