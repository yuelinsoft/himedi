using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.Messages;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorsRegister : HtmlTemplatedWebControl
    {
        Button btnOK;
        RegionSelector dropRegion;
        TextBox txtAddress;
        TextBox txtCellPhone;
        TextBox txtCompanyName;
        TextBox txtEmail;
        TextBox txtMSN;
        TextBox txtPassword;
        TextBox txtPasswordAnswer;
        TextBox txtPasswordCompare;
        TextBox txtPasswordQuestion;
        TextBox txtQQ;
        TextBox txtRealName;
        TextBox txtTelPhone;
        TextBox txtTransactionPassword;
        TextBox txtTransactionPasswordCompare;
        TextBox txtUserName;
        TextBox txtWangwang;
        TextBox txtZipcode;

        protected override void AttachChildControls()
        {
            txtUserName = (TextBox)FindControl("txtUserName");
            txtEmail = (TextBox)FindControl("txtEmail");
            txtPassword = (TextBox)FindControl("txtPassword");
            txtPasswordCompare = (TextBox)FindControl("txtPasswordCompare");
            txtTransactionPassword = (TextBox)FindControl("txtTransactionPassword");
            txtTransactionPasswordCompare = (TextBox)FindControl("txtTransactionPasswordCompare");
            txtRealName = (TextBox)FindControl("txtRealName");
            txtCompanyName = (TextBox)FindControl("txtCompanyName");
            dropRegion = (RegionSelector)FindControl("dropRegion");
            txtAddress = (TextBox)FindControl("txtAddress");
            txtZipcode = (TextBox)FindControl("txtZipcode");
            txtQQ = (TextBox)FindControl("txtQQ");
            txtWangwang = (TextBox)FindControl("txtWangwang");
            txtMSN = (TextBox)FindControl("txtMSN");
            txtTelPhone = (TextBox)FindControl("txtTelPhone");
            txtCellPhone = (TextBox)FindControl("txtCellPhone");
            txtPasswordQuestion = (TextBox)FindControl("txtPasswordQuestion");
            txtPasswordAnswer = (TextBox)FindControl("txtPasswordAnswer");
            btnOK = (Button)FindControl("btnOK");
            btnOK.Click += new EventHandler(btnOK_Click);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidationInput())
            {
                int? selectedRegionId = dropRegion.GetSelectedRegionId();
                HiMembershipUser membershipUser = new HiMembershipUser(false, UserRole.Distributor);
                Distributor distributor = new Distributor(membershipUser);
                distributor.IsApproved = false;
                distributor.Username = txtUserName.Text;
                distributor.Email = txtEmail.Text;
                distributor.Password = txtPasswordCompare.Text;
                if (!string.IsNullOrEmpty(txtTransactionPasswordCompare.Text))
                {
                    distributor.TradePassword = txtTransactionPasswordCompare.Text;
                }
                else
                {
                    distributor.TradePassword = distributor.Password;
                }
                distributor.RealName = txtRealName.Text;
                distributor.CompanyName = txtCompanyName.Text;
                if (selectedRegionId.HasValue)
                {
                    distributor.RegionId = selectedRegionId.Value;
                    distributor.TopRegionId = RegionHelper.GetTopRegionId(distributor.RegionId);
                }
                distributor.Address = txtAddress.Text;
                distributor.Zipcode = txtZipcode.Text;
                distributor.QQ = txtQQ.Text;
                distributor.Wangwang = txtWangwang.Text;
                distributor.MSN = txtMSN.Text;
                distributor.TelPhone = txtTelPhone.Text;
                distributor.CellPhone = txtCellPhone.Text;
                distributor.Remark = string.Empty;
                if (ValidationDistributorRequest(distributor))
                {
                    switch (SubsiteStoreHelper.CreateDistributor(distributor))
                    {
                        case CreateUserStatus.UnknownFailure:
                            ShowMessage("未知错误", false);
                            return;

                        case CreateUserStatus.Created:
                            distributor.ChangePasswordQuestionAndAnswer(null, txtPasswordQuestion.Text, txtPasswordAnswer.Text);
                            Messenger.UserRegister(distributor, txtPasswordCompare.Text);
                            distributor.OnRegister(new UserEventArgs(distributor.Username, txtPasswordCompare.Text, null));
                            Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/DistributorsRegisterComplete.aspx");
                            return;

                        case CreateUserStatus.DuplicateUsername:
                            ShowMessage("您输入的用户名已经被注册使用", false);
                            return;

                        case CreateUserStatus.DuplicateEmailAddress:
                            ShowMessage("您输入的电子邮件地址已经被注册使用", false);
                            return;

                        case CreateUserStatus.InvalidFirstCharacter:
                        case CreateUserStatus.Updated:
                        case CreateUserStatus.Deleted:
                        case CreateUserStatus.InvalidQuestionAnswer:
                            return;

                        case CreateUserStatus.DisallowedUsername:
                            ShowMessage("用户名被禁止注册", false);
                            return;

                        case CreateUserStatus.InvalidPassword:
                            ShowMessage("无效的密码", false);
                            return;

                        case CreateUserStatus.InvalidEmail:
                            ShowMessage("无效的电子邮件地址", false);
                            return;
                    }
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
            }
            if (SkinName == null)
            {
                SkinName = "Skin-DistributorsRegister.html";
            }
            base.OnInit(e);
        }

        bool ValidationDistributorRequest(Distributor distributor)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Distributor>(distributor, new string[] { "ValDistributor" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMessage(msg, false);
            }
            return results.IsValid;
        }

        bool ValidationInput()
        {
            string str = string.Empty;
            if (txtUserName.Text.Trim().Length <= 1)
            {
                str = str + "请输入至少两位长度的字符";
            }
            if (string.Compare(txtPassword.Text, txtPasswordCompare.Text) != 0)
            {
                str = str + "请确定两次输入的登录密码相同";
            }
            if (string.IsNullOrEmpty(txtTransactionPassword.Text.Trim()))
            {
                str = str + "<br/>交易密码不允许为空！";
            }
            if (string.IsNullOrEmpty(txtTransactionPasswordCompare.Text.Trim()))
            {
                str = str + "<br/>重复交易密码不允许为空！";
            }
            if (!string.IsNullOrEmpty(txtTransactionPassword.Text) && (string.Compare(txtTransactionPassword.Text, txtTransactionPasswordCompare.Text) != 0))
            {
                str = str + "<br/>请确定两次输入的交易密码相同";
            }
            if ((string.IsNullOrEmpty(txtQQ.Text) && string.IsNullOrEmpty(txtWangwang.Text)) && string.IsNullOrEmpty(txtMSN.Text))
            {
                str = str + "<br/>QQ,旺旺,MSN,三者必填其一";
            }
            if (string.IsNullOrEmpty(txtTelPhone.Text) && string.IsNullOrEmpty(txtCellPhone.Text))
            {
                str = str + "<br/>固定电话和手机,二者必填其一";
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMessage(str, false);
                return false;
            }
            return true;
        }
    }
}

