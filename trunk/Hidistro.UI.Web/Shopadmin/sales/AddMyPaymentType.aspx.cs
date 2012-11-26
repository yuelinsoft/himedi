using Hidistro.Core.Cryptography;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using kindeditor.Net;
using System;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
    public partial class AddMyPaymentType : DistributorPage
    {

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string str;
            ConfigData data;
            decimal num;
            if (ValidateValues(out str, out data, out num))
            {
                PaymentModeInfo paymentMode = new PaymentModeInfo();
                paymentMode.Name = txtName.Text;
                paymentMode.Description = fcContent.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                paymentMode.Gateway = str;
                paymentMode.IsUseInpour = radiIsUseInpour.SelectedValue;
                paymentMode.Charge = num;
                paymentMode.IsPercent = chkIsPercent.Checked;
                paymentMode.Settings = Cryptographer.Encrypt(data.SettingsXml);

                switch (SubsiteSalesHelper.CreatePaymentMode(paymentMode))
                {
                    case PaymentModeActionStatus.Success:
                        {
                            Response.Redirect("MyPaymentTypes.aspx");
                            return;
                        }
                    case PaymentModeActionStatus.DuplicateName:
                        {
                            ShowMsg("已经存在一个相同的支付方式名称", false);
                            return;
                        }
                    case PaymentModeActionStatus.OutofNumber:
                        {
                            ShowMsg("支付方式的数目已经超出系统设置的数目", false);
                            return;
                        }

                    case PaymentModeActionStatus.DuplicateGateway:
                        {
                            ShowMsg("已经添加了一个相同类型的支付接口", false);
                            return;
                        }
                }
                ShowMsg("未知错误", false);
            }
        }

        private ConfigData LoadConfig(out string selectedName)
        {
            selectedName = base.Request.Form["ddlPayments"];
            txtSelectedName.Value = selectedName;
            txtConfigData.Value = "";
            if (string.IsNullOrEmpty(selectedName) || (selectedName.Length == 0))
            {
                return null;
            }
            ConfigablePlugin plugin = PaymentRequest.CreateInstance(selectedName);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnCreate.Click += new EventHandler(btnCreate_Click);
            if (!Page.IsPostBack)
            {
                txtCharge.Text = "0";
            }
        }

        private bool ValidateValues(out string selectedPlugin, out ConfigData data, out decimal payCharge)
        {
            string str = string.Empty;
            data = LoadConfig(out selectedPlugin);
            payCharge = 0M;
            if (string.IsNullOrEmpty(selectedPlugin))
            {
                ShowMsg("请先选择一个支付接口类型", false);
                return false;
            }
            if (!data.IsValid)
            {
                foreach (string str2 in data.ErrorMsgs)
                {
                    str = str + Formatter.FormatErrorMessage(str2);
                }
            }
            if (!decimal.TryParse(txtCharge.Text, out payCharge))
            {
                str = str + Formatter.FormatErrorMessage("支付手续费无效,大小在0-10000000之间");
            }
            if ((payCharge < 0M) || (payCharge > 10000000M))
            {
                str = str + Formatter.FormatErrorMessage("支付手续费大小1-10000000之间");
            }
            if (string.IsNullOrEmpty(txtName.Text) || (txtName.Text.Length > 60))
            {
                str = str + Formatter.FormatErrorMessage("支付方式名称不能为空，长度限制在1-60个字符之间");
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

