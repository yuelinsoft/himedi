using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
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
    [PrivilegeCheck(Privilege.EditDistributorGrade)]
    public partial class EditDistributorGrades : AdminPage
    {
        int gradeId;

        private void btnEditDistrbutor_Click(object sender, EventArgs e)
        {
            int num;
            DistributorGradeInfo distributorGrade = new DistributorGradeInfo();
            distributorGrade.Name = txtRankName.Text.Trim();
            distributorGrade.Description = txtRankDesc.Text.Trim();
            distributorGrade.GradeId = gradeId;
            if (!(!int.TryParse(txtValue.Text, out num) || txtValue.Text.Contains(".")))
            {
                distributorGrade.Discount = num;
            }
            else
            {
                ShowMsg("等级折扣必须为正整数", false);
                return;
            }
            if (ValidationMemberGrade(distributorGrade))
            {
                if (DistributorHelper.UpdateDistributorGrade(distributorGrade))
                {
                    ShowMsg("修改分销商等级成功", true);
                }
                else
                {
                    ShowMsg("修改分销商等级失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnEditDistrbutor.Click += new EventHandler(btnEditDistrbutor_Click);
            if (!int.TryParse(base.Request.QueryString["GradeId"], out gradeId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                DistributorGradeInfo distributorGradeInfo = DistributorHelper.GetDistributorGradeInfo(gradeId);
                if (distributorGradeInfo == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(distributorGradeInfo, false);
                    txtRankName.Text = distributorGradeInfo.Name;
                    txtValue.Text = distributorGradeInfo.Discount.ToString();
                    txtRankDesc.Text = distributorGradeInfo.Description;
                }
            }
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

