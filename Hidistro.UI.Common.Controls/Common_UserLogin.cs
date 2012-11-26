namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using System;
    using System.Web.UI.WebControls;

    public class Common_UserLogin : AscxTemplatedWebControl
    {
       Literal litAccount;
       Literal litMemberGrade;
       Literal litNum;
       Literal litPoint;
       Panel pnlLogin;
       Panel pnlLogout;

        protected override void AttachChildControls()
        {
            pnlLogin = (Panel) FindControl("pnlLogin");
            pnlLogout = (Panel) FindControl("pnlLogout");
            litAccount = (Literal) FindControl("litAccount");
            litMemberGrade = (Literal) FindControl("litMemberGrade");
            litPoint = (Literal) FindControl("litPoint");
            litNum = (Literal) FindControl("litNum");
            pnlLogout.Visible = !HiContext.Current.User.IsAnonymous;
            pnlLogin.Visible = HiContext.Current.User.IsAnonymous;
            if (!Page.IsPostBack && ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling)))
            {
                string str;
                int num;
                Member user = HiContext.Current.User as Member;
                litAccount.Text = Globals.FormatMoney(user.Balance);
                litPoint.Text = user.Points.ToString();
                ControlProvider.Instance().GetMemberExpandInfo(user.GradeId, user.Username, out str, out num);
                litMemberGrade.Text = str;
                litNum.Text = num.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (SkinName == null)
            {
                SkinName = "/ascx/tags/Skin-Common_UserLogin.ascx";
            }
            base.OnInit(e);
        }
    }
}

