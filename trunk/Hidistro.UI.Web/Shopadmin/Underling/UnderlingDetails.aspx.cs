using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnderlingDetails : DistributorPage
    {
        int currentUserId = 0;

        private void btnEdit_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("EditUnderling.aspx?userId=" + Page.Request.QueryString["userId"]);
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
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                lblUserLink.Text = string.Concat(new object[] { "http://", siteSettings.SiteUrl, Globals.ApplicationPath, "/?ReferralUserId=", member.UserId });
                litUserName.Text = member.Username;
                litIsApproved.Text = member.IsApproved ? "通过" : "禁止";
                litGrade.Text = UnderlingHelper.GetMemberGrade(member.GradeId).Name;
                litCreateDate.Text = member.CreateDate.ToString();
                litLastLoginDate.Text = member.LastLoginDate.ToString();
                litRealName.Text = member.RealName;
                litBirthDate.Text = member.BirthDate.ToString();
                litAddress.Text = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
                litQQ.Text = member.QQ;
                litMSN.Text = member.MSN;
                litTelPhone.Text = member.TelPhone;
                litCellPhone.Text = member.CellPhone;
                litEmail.Text = member.Email;
                if (member.Gender == Gender.Female)
                {
                    litGender.Text = "女";
                }
                else if (member.Gender == Gender.Male)
                {
                    litGender.Text = "男";
                }
                else
                {
                    litGender.Text = "保密";
                }
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
                btnEdit.Click += new EventHandler(btnEdit_Click);
                if (!Page.IsPostBack)
                {
                    LoadMemberInfo();
                }
            }
        }
    }
}

