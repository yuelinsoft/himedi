
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.EditMember)]
    public partial class EditMember : AdminPage
    {

        int currentUserId;


        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            Member member = MemberHelper.GetMember(currentUserId);
            member.IsApproved = ddlApproved.SelectedValue.Value;
            member.Wangwang = Globals.HtmlEncode(txtWangwang.Text.Trim());
            member.GradeId = drpMemberRankList.SelectedValue.Value;
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
                if (MemberHelper.Update(member))
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
            Member member = MemberHelper.GetMember(currentUserId);
            if (member == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                ddlApproved.SelectedValue = new bool?(member.IsApproved);
                drpMemberRankList.SelectedValue = new int?(member.GradeId);
                lblLoginNameValue.Text = member.Username;
                lblRegsTimeValue.Time = member.CreateDate;
                lblLastLoginTimeValue.Time = member.LastLoginDate;
                lblTotalAmountValue.Text = Globals.FormatMoney(member.Expenditure);
                txtRealName.Text = member.RealName;
                calBirthday.SelectedDate = member.BirthDate;
                txtAddress.Text = Globals.HtmlDecode(member.Address);
                rsddlRegion.SetSelectedRegionId(new int?(member.RegionId));
                txtQQ.Text = member.QQ;
                txtMSN.Text = member.MSN;
                txtTel.Text = member.TelPhone;
                txtCellPhone.Text = member.CellPhone;
                txtprivateEmail.Text = member.Email;
                gender.SelectedValue = member.Gender;
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
                    drpMemberRankList.AllowNull = false;
                    drpMemberRankList.DataBind();
                    ddlApproved.AllowNull = false;
                    ddlApproved.DataBind();
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

