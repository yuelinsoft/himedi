
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Members)]
    public partial class MemberDetails : AdminPage
    {

        int currentUserId;

        private void btnEdit_Click(object sender, EventArgs e)
        {
            base.Response.Redirect(Globals.GetAdminAbsolutePath("/member/EditMember.aspx?userId=" + Page.Request.QueryString["userId"]), true);
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
                Uri url = HttpContext.Current.Request.Url;
                string str = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(CultureInfo.InvariantCulture));
                lblUserLink.Text = string.Concat(new object[] { string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { url.Scheme, HiContext.Current.SiteSettings.SiteUrl, str }), Globals.ApplicationPath, "/?ReferralUserId=", member.UserId });
                litUserName.Text = member.Username;
                litIsApproved.Text = member.IsApproved ? "通过" : "禁止";
                litGrade.Text = MemberHelper.GetMemberGrade(member.GradeId).Name;
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

