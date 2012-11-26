namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Messages;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ForgotPassword : HtmlTemplatedWebControl
    {
       IButton btnCheckAnswer;
       IButton btnCheckUserName;
       IButton btnSetPassword;
       HtmlGenericControl htmDivPassword;
       HtmlGenericControl htmDivQuestionAndAnswer;
       HtmlGenericControl htmDivUserName;
       Literal litAnswerMessage;
       Literal litUserQuestion;
       TextBox txtPassword;
       TextBox txtRePassword;
       TextBox txtUserAnswer;
       TextBox txtUserName;

        protected override void AttachChildControls()
        {
            this.htmDivUserName = (HtmlGenericControl) this.FindControl("htmDivUserName");
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.btnCheckUserName = ButtonManager.Create(this.FindControl("btnCheckUserName"));
            this.htmDivQuestionAndAnswer = (HtmlGenericControl) this.FindControl("htmDivQuestionAndAnswer");
            this.litUserQuestion = (Literal) this.FindControl("litUserQuestion");
            this.txtUserAnswer = (TextBox) this.FindControl("txtUserAnswer");
            this.litAnswerMessage = (Literal) this.FindControl("litAnswerMessage");
            this.btnCheckAnswer = ButtonManager.Create(this.FindControl("btnCheckAnswer"));
            this.htmDivPassword = (HtmlGenericControl) this.FindControl("htmDivPassword");
            this.txtPassword = (TextBox) this.FindControl("txtPassword");
            this.txtRePassword = (TextBox) this.FindControl("txtRePassword");
            this.btnSetPassword = ButtonManager.Create(this.FindControl("btnSetPassword"));
            PageTitle.AddSiteNameTitle("找回密码", HiContext.Current.Context);
            this.btnCheckUserName.Click += new EventHandler(this.btnCheckUserName_Click);
            this.btnCheckAnswer.Click += new EventHandler(this.btnCheckAnswer_Click);
            this.btnSetPassword.Click += new EventHandler(this.btnSetPassword_Click);
            if (!this.Page.IsPostBack)
            {
                this.panelShow("InputUserName");
            }
        }

       void btnCheckAnswer_Click(object sender, EventArgs e)
        {
            if (Users.FindUserByUsername(this.txtUserName.Text.Trim()).MembershipUser.ValidatePasswordAnswer(this.txtUserAnswer.Text.Trim()))
            {
                this.panelShow("InputPassword");
            }
            else
            {
                this.litAnswerMessage.Visible = true;
            }
        }

       void btnCheckUserName_Click(object sender, EventArgs e)
        {
            IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
            if ((HiContext.Current.SiteSettings.IsDistributorSettings && (user.UserRole == UserRole.Member)) || (!HiContext.Current.SiteSettings.IsDistributorSettings && (user.UserRole == UserRole.Underling)))
            {
                this.ShowMessage("对不起，没有此用户的信息", false);
            }
            else if (((user != null) && (user.UserRole != UserRole.SiteManager)) && (user.UserRole != UserRole.Anonymous))
            {
                if (!string.IsNullOrEmpty(user.PasswordQuestion))
                {
                    if (this.litUserQuestion != null)
                    {
                        this.litUserQuestion.Text = user.PasswordQuestion.ToString();
                    }
                    this.panelShow("InputAnswer");
                }
                else
                {
                    this.ShowMessage("请自行联系后台管理员进行密码修改", false);
                }
            }
            else
            {
                this.ShowMessage("该用户不存在", false);
            }
        }

       void btnSetPassword_Click(object sender, EventArgs e)
        {
            IUser user = Users.FindUserByUsername(this.txtUserName.Text.Trim());
            if (this.txtPassword.Text.Trim() != this.txtRePassword.Text.Trim())
            {
                this.ShowMessage("两次输入的密码需一致", false);
            }
            else if (user.ChangePasswordWithAnswer(this.txtUserAnswer.Text, this.txtPassword.Text))
            {
                Messenger.UserPasswordForgotten(user, this.txtPassword.Text);
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("ForgotPasswordSuccess") + string.Format("?UserName={0}", user.Username));
            }
            else
            {
                this.ShowMessage("登录密码修改失败，请重试", false);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ForgotPassword.html";
            }
            base.OnInit(e);
        }

       void panelShow(string type)
        {
            this.litAnswerMessage.Visible = false;
            if (type == "InputUserName")
            {
                this.htmDivUserName.Visible = true;
                this.htmDivQuestionAndAnswer.Visible = false;
                this.htmDivPassword.Visible = false;
            }
            else if (type == "InputAnswer")
            {
                this.htmDivUserName.Visible = false;
                this.htmDivQuestionAndAnswer.Visible = true;
                this.htmDivPassword.Visible = false;
            }
            else if (type == "InputPassword")
            {
                this.htmDivUserName.Visible = false;
                this.htmDivQuestionAndAnswer.Visible = false;
                this.htmDivPassword.Visible = true;
            }
        }
    }
}

