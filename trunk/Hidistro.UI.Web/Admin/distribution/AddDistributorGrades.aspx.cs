using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AddDistributorGrade)]
    public partial class AddDistributorGrades : AdminPage
    {

        private void btnAddDistrbutor_Click(object sender, EventArgs e)
        {
            if (DistributorHelper.ExistGradeName(txtRankName.Text.Trim()))
            {
                ShowMsg("已经存在相同名称的分销商等级", false);
            }
            else
            {
                int num;
                DistributorGradeInfo distributorGrade = new DistributorGradeInfo();
                distributorGrade.Name = txtRankName.Text.Trim();
                distributorGrade.Description = txtRankDesc.Text.Trim();
                if (!(!int.TryParse(txtValue.Text, out num) || txtValue.Text.Contains(".")))
                {
                    distributorGrade.Discount = num;
                }
                else
                {
                    ShowMsg("等级折扣必须只能为正整数", false);
                    return;
                }
                if (ValidationMemberGrade(distributorGrade))
                {
                    if (DistributorHelper.AddDistributorGrade(distributorGrade))
                    {
                        ResetText();
                        ShowMsg("成功添加了一个分销商等级", true);
                    }
                    else
                    {
                        ShowMsg("添加分销商等级失败", false);
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnAddDistrbutor.Click += new EventHandler(btnAddDistrbutor_Click);
        }

        private void ResetText()
        {
            txtRankName.Text = string.Empty;
            txtRankDesc.Text = string.Empty;
            txtValue.Text = string.Empty;
        }

        private bool ValidationMemberGrade(DistributorGradeInfo distributorGrade)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<DistributorGradeInfo>(distributorGrade, new string[] { "ValDistributorGrade" });
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
    }
}

