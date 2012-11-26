using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditUnderling : DistributorPage
    {

        int currentUserId = 0;

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            Member member = UnderlingHelper.GetMember(currentUserId);
            member.IsApproved = ddlApproved.SelectedValue.Value;
            member.GradeId = drpMemberRankList.SelectedValue.Value;
            member.Wangwang = Globals.HtmlEncode(txtWangwang.Text.Trim());
            member.RealName = txtRealName.Text.Trim();
            if (rsddlRegion.GetSelectedRegionId().HasValue)
            {
                member.RegionId = rsddlRegion.GetSelectedRegionId().Value;
                member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
            }
            member.Address = Globals.HtmlEncode(txtAddress.Text);
            member.QQ = txtQQ.Text;
            member.MSN = txtMSN.Text;
            member.TelPhone = txtTel.Text;
            member.CellPhone = txtCellPhone.Text;
            if (calBirthday.SelectedDate.HasValue)
            {
                member.BirthDate = new DateTime?(calBirthday.SelectedDate.Value);
            }
            member.Email = txtprivateEmail.Text;
            member.Gender = gender.SelectedValue;
            if (ValidationMember(member))
            {
                if (UnderlingHelper.Update(member))
                {
                    ShowMsg("成功修改了当前会员的个人资料", true);
                }
                else
                {
                    ShowMsg("当前会员的个人信息修改失败", false);
                }
            }
        }

        private void LoadMemberInfo()
        {
            Member member = UnderlingHelper.GetMember(currentUserId);
            if (member == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                drpMemberRankList.SelectedValue = new int?(member.GradeId);
                lblLoginNameValue.Text = member.Username;
                lblRegsTimeValue.Time = member.CreateDate;
                lblLastLoginTimeValue.Time = member.LastLoginDate;
                lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
                txtRealName.Text = member.RealName;
                calBirthday.SelectedDate = member.BirthDate;
                rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
                txtAddress.Text = Globals.HtmlDecode(member.Address);
                txtQQ.Text = member.QQ;
                txtMSN.Text = member.MSN;
                txtTel.Text = member.TelPhone;
                txtCellPhone.Text = member.CellPhone;
                txtprivateEmail.Text = member.Email;
                gender.SelectedValue = member.Gender;
                ddlApproved.SelectedValue = new bool?(member.IsApproved);
                txtWangwang.Text = Globals.HtmlDecode(member.Wangwang);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["userId"], out currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnEditUser.Click += new EventHandler(btnEditUser_Click);
                if (!Page.IsPostBack)
                {
                    ddlApproved.DataBind();
                    drpMemberRankList.AllowNull = false;
                    drpMemberRankList.DataBind();
                    LoadMemberInfo();
                }
            }
        }

        private bool ValidationMember(Member member)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<Member>(member, new string[] { "ValMember" });
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

