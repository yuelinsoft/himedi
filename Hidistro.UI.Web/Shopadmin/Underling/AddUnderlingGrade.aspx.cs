using Hidistro.Entities.Members;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class AddUnderlingGrade : DistributorPage
    {


        private void btnSubmitMemberRanks_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (txtValue.Text.Trim().Contains("."))
            {
                ShowMsg("折扣必须为正整数", false);
            }
            else
            {
                int num;
                int num2;
                MemberGradeInfo memberGrade = new MemberGradeInfo();
                memberGrade.Name = txtRankName.Text.Trim();
                memberGrade.Description = txtRankDesc.Text.Trim();
                if (int.TryParse(txtPoint.Text.Trim(), out num))
                {
                    memberGrade.Points = num;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("积分设置无效或不能为空，必须大于等于0的整数");
                }
                memberGrade.IsDefault = chkIsDefault.SelectedValue;
                if (int.TryParse(txtValue.Text, out num2))
                {
                    memberGrade.Discount = num2;
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("等级折扣设置无效或不能为空，等级折扣必须在1-100之间");
                }
                if (!string.IsNullOrEmpty(str))
                {
                    ShowMsg(str, false);
                }
                else if (ValidationMemberGrade(memberGrade))
                {
                    if (UnderlingHelper.HasSamePointMemberGrade(memberGrade))
                    {
                        ShowMsg("已经存在相同积分的等级，每个会员等级的积分不能相同", false);
                    }
                    else if (UnderlingHelper.CreateUnderlingGrade(memberGrade))
                    {
                        ShowMsg("成功添加了一个会员等级", true);
                    }
                    else
                    {
                        ShowMsg("添加会员等级失败", false);
                    }
                }
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSubmitMemberRanks.Click += new EventHandler(btnSubmitMemberRanks_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private bool ValidationMemberGrade(MemberGradeInfo memberGrade)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<MemberGradeInfo>(memberGrade, new string[] { "ValMemberGrade" });
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

