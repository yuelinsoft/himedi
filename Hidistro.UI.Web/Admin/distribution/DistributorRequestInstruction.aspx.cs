
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorRequestInstruction)]
    public partial class DistributorRequestInstruction : AdminPage
    {
        private void btnOK_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            masterSettings.DistributorRequestInstruction = fkFooter.Text;
            masterSettings.DistributorRequestProtocols = fkProtocols.Text;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteSettings>(masterSettings, new string[] { "ValRequestProtocols" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else
            {
                SettingsManager.Save(masterSettings);
                ShowMsg("保存成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            if (!Page.IsPostBack)
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                fkFooter.Text = masterSettings.DistributorRequestInstruction;
                fkProtocols.Text = masterSettings.DistributorRequestProtocols;
            }
        }
    }
}

