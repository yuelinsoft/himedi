using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class DistributorProfile : DistributorPage
    {


        private void BindData(Distributor distributor)
        {
            txtRealName.Text = distributor.RealName;
            txtCompanyName.Text = distributor.CompanyName;
            txtprivateEmail.Text = distributor.Email;
            txtAddress.Text = distributor.Address;
            txtZipcode.Text = distributor.Zipcode;
            txtQQ.Text = distributor.QQ;
            txtWangwang.Text = distributor.Wangwang;
            txtMSN.Text = distributor.MSN;
            txtTel.Text = distributor.TelPhone;
            txtCellPhone.Text = distributor.CellPhone;
            rsddlRegion.SetSelectedRegionId(new int?(distributor.RegionId));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidationInput())
            {
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                distributor.RealName = txtRealName.Text.Trim();
                distributor.CompanyName = txtCompanyName.Text.Trim();
                if (rsddlRegion.GetSelectedRegionId().HasValue)
                {
                    distributor.RegionId = rsddlRegion.GetSelectedRegionId().Value;
                    distributor.TopRegionId = RegionHelper.GetTopRegionId(distributor.RegionId);
                }
                distributor.Email = txtprivateEmail.Text.Trim();
                distributor.Address = txtAddress.Text.Trim();
                distributor.Zipcode = txtZipcode.Text.Trim();
                distributor.QQ = txtQQ.Text.Trim();
                distributor.Wangwang = txtWangwang.Text.Trim();
                distributor.MSN = txtMSN.Text.Trim();
                distributor.TelPhone = txtTel.Text.Trim();
                distributor.CellPhone = txtCellPhone.Text.Trim();
                distributor.IsCreate = false;
                if (ValidationDistributorRequest(distributor))
                {
                    if (SubsiteStoreHelper.UpdateDistributor(distributor))
                    {
                        ShowMsg("成功的修改了信息", true);
                    }
                    else
                    {
                        ShowMsg("修改失败", false);
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSave.Click += new EventHandler(btnSave_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                Distributor distributor = SubsiteStoreHelper.GetDistributor();
                if ((distributor != null) && (distributor.UserRole == UserRole.Distributor))
                {
                    BindData(distributor);
                }
            }
        }

        private bool ValidationDistributorRequest(Distributor distributor)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Distributor>(distributor, new string[] { "ValDistributor" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }

        public bool ValidationInput()
        {
            string str = string.Empty;
            if ((string.IsNullOrEmpty(txtQQ.Text) && string.IsNullOrEmpty(txtWangwang.Text)) && string.IsNullOrEmpty(txtMSN.Text))
            {
                str = str + "QQ,旺旺,MSN,三者必填其一";
            }
            if (string.IsNullOrEmpty(txtTel.Text) && string.IsNullOrEmpty(txtCellPhone.Text))
            {
                str = str + "<br/>固定电话和手机,二者必填其一";
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

