namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class ProductConsultations : HtmlTemplatedWebControl
    {
       IButton btnRefer;
       ProductDetailsLink prodetailsLink;
       int productId;
       HtmlInputText txtConsultationCode;
       TextBox txtContent;
       TextBox txtEmail;
       TextBox txtUserName;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
            {
                base.GotoResourceNotFound();
            }
            this.txtEmail = (TextBox) this.FindControl("txtEmail");
            this.txtUserName = (TextBox) this.FindControl("txtUserName");
            this.txtContent = (TextBox) this.FindControl("txtContent");
            this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
            this.txtConsultationCode = (HtmlInputText) this.FindControl("txtConsultationCode");
            this.prodetailsLink = (ProductDetailsLink) this.FindControl("ProductDetailsLink1");
            this.btnRefer.Click += new EventHandler(this.btnRefer_Click);
            if (!this.Page.IsPostBack)
            {
                PageTitle.AddSiteNameTitle("商品咨询", HiContext.Current.Context);
                if ((HiContext.Current.User.UserRole == UserRole.Member) || (HiContext.Current.User.UserRole == UserRole.Underling))
                {
                    this.txtUserName.Text = HiContext.Current.User.Username;
                    this.txtEmail.Text = HiContext.Current.User.Email;
                    this.btnRefer.Text = "咨询";
                }
                ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
                if (productSimpleInfo != null)
                {
                    this.prodetailsLink.ProductId = this.productId;
                    this.prodetailsLink.ProductName = productSimpleInfo.ProductName;
                }
                this.txtConsultationCode.Value = string.Empty;
            }
        }

        public void btnRefer_Click(object sender, EventArgs e)
        {
            ProductConsultationInfo target = new ProductConsultationInfo();
            target.ConsultationDate = DateTime.Now;
            target.ProductId = this.productId;
            target.UserId = HiContext.Current.User.UserId;
            target.UserName = this.txtUserName.Text;
            target.UserEmail = this.txtEmail.Text;
            target.ConsultationText = this.txtContent.Text;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductConsultationInfo>(target, new string[] { "Refer" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMessage(msg, false);
            }
            else if (string.IsNullOrEmpty(this.txtConsultationCode.Value))
            {
                this.ShowMessage("请输入验证码", false);
            }
            else if (!HiContext.Current.CheckVerifyCode(this.txtConsultationCode.Value.Trim()))
            {
                this.ShowMessage("验证码不正确", false);
            }
            else if (ProductProcessor.InsertProductConsultation(target))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "咨询成功，管理员回复即可显示", Globals.GetSiteUrls().UrlData.FormatUrl("productConsultations", new object[] { this.productId })));
            }
            else
            {
                this.ShowMessage("咨询失败，请重试", false);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ProductConsultations.html";
            }
            base.OnInit(e);
        }
    }
}

