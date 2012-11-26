namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class IntroducedToFriend : HtmlTemplatedWebControl
    {
       Button btnRefer;
       HyperLink hlinkHome;
       HyperLink hlinkProductOfContext;
       HyperLink hlinkProductOfTitle;
       Literal litProductUrl;
       int productId;
       TextBox txtFriendEmail;
       TextBox txtFriendName;
       TextBox txtMessage;
       HtmlInputText txtTJCode;
       TextBox txtUserName;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.hlinkProductOfTitle = (HyperLink) this.FindControl("hlinkProductOfTitle");
            this.hlinkProductOfContext = (HyperLink) this.FindControl("hlinkProductOfContext");
            this.hlinkHome = (HyperLink) this.FindControl("hlinkHome");
            this.litProductUrl = (Literal) this.FindControl("litProductUrl");
            this.txtFriendEmail = (TextBox) this.FindControl("txtFriendEmail");
            this.txtFriendName = (TextBox) this.FindControl("txtFriendName");
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtMessage = (TextBox) this.FindControl("txtMessage");
            this.btnRefer = (Button) this.FindControl("btnRefer");
            this.txtTJCode = (HtmlInputText) this.FindControl("txtTJCode");
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            if (!this.Page.IsPostBack)
            {
                ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
                if (productSimpleInfo != null)
                {
                    IUser user = HiContext.Current.User;
                    this.txtUserName.Text = user.Username;
                    this.hlinkProductOfTitle.Text = productSimpleInfo.ProductName;
                    this.hlinkProductOfTitle.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { this.productId });
                    this.hlinkProductOfContext.Text = productSimpleInfo.ProductName;
                    this.hlinkProductOfContext.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { this.productId });
                    this.hlinkHome.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("home");
                    this.hlinkHome.Text = HiContext.Current.SiteSettings.SiteName;
                    this.txtTJCode.Value = string.Empty;
                    if ((user.UserRole == UserRole.Member) || (user.UserRole == UserRole.Underling))
                    {
                        this.litProductUrl.Text = Globals.FullPath(HttpContext.Current.Request.Url.PathAndQuery).Replace("IntroducedToFriend", "productDetails") + "&ReferralUserId=" + user.UserId;
                    }
                    else
                    {
                        this.litProductUrl.Text = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { this.productId }));
                    }
                    PageTitle.AddTitle("推荐给好友", HiContext.Current.Context);
                }
            }
        }

       void btnRefer_Click(object sender, EventArgs e)
        {
            if (!HiContext.Current.CheckVerifyCode(this.txtTJCode.Value))
            {
                this.ShowMessage("验证码不正确", false);
            }
            else if (!HiContext.Current.SiteSettings.EmailEnabled)
            {
                this.ShowMessage("系统还未设置电子邮件，暂时不能发送邮件", false);
            }
            else if (this.ValidateConvert())
            {
                string str3;
                string subject = string.Format("您的好友{0}给您推荐了一个好宝贝", this.txtUserName.Text);
                string body = string.Format("{0}，您好！<br/>我在{1}网站上看到{2}，快点出手吧 ，棒极了！你去看看吧！这个东东的网址是：<br/>{3}<br>{4}", new object[] { this.txtFriendName.Text, HiContext.Current.SiteSettings.SiteName, this.hlinkProductOfTitle.Text, this.litProductUrl.Text, this.txtMessage.Text });
                if (Messenger.SendMail(subject, body, this.txtFriendEmail.Text.Trim(), HiContext.Current.SiteSettings, out str3) != SendStatus.Success)
                {
                    this.ShowMessage(str3, false);
                }
                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "发送成功", Globals.GetSiteUrls().UrlData.FormatUrl("IntroducedToFriend", new object[] { this.productId })));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-IntroducedToFriend.html";
            }
            base.OnInit(e);
        }

       bool ValidateConvert()
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(this.txtMessage.Text) || (this.txtMessage.Text.Length > 30))
            {
                str = str + Formatter.FormatErrorMessage("请输入您的留言，不能为空长度限制在1-300之间");
            }
            if (string.IsNullOrEmpty(this.txtUserName.Text) || (this.txtUserName.Text.Length > 30))
            {
                str = str + Formatter.FormatErrorMessage("请输入您的名字，不能为空长度限制在1-30之间");
            }
            if (string.IsNullOrEmpty(this.txtFriendName.Text) || (this.txtFriendName.Text.Length > 30))
            {
                str = str + Formatter.FormatErrorMessage("请输入好友的名字，不能为空长度限制在1-30之间");
            }
            if ((string.IsNullOrEmpty(this.txtFriendEmail.Text) || (this.txtFriendEmail.Text.Length > 0x100)) || !Regex.IsMatch(this.txtFriendEmail.Text, @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,4}){1,2})"))
            {
                str = str + Formatter.FormatErrorMessage("请输入正确好友邮箱，不能为空长度限制在1-256之间");
            }
            if (!string.IsNullOrEmpty(str))
            {
                this.ShowMessage(str, false);
                return false;
            }
            return true;
        }
    }
}

