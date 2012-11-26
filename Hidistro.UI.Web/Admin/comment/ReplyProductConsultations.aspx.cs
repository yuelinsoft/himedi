using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductConsultationsManage)]
    public partial class ReplyProductConsultations : AdminPage
    {

        int consultationId;


        protected void btnReplyProductConsultation_Click(object sender, EventArgs e)
        {
            ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(consultationId);
            if (string.IsNullOrEmpty(fckReplyText.Text))
            {
                productConsultation.ReplyText = null;
            }
            else
            {
                productConsultation.ReplyText = fckReplyText.Text;
            }
            productConsultation.ReplyUserId = new int?(HiContext.Current.User.UserId);
            productConsultation.ReplyDate = new DateTime?(DateTime.Now);
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductConsultationInfo>(productConsultation, new string[] { "Reply" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            else if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
            {
                fckReplyText.Text = string.Empty;
                ShowMsg("成功回复了选择的商品咨询", true);
            }
            else
            {
                ShowMsg("回复商品咨询失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["ConsultationId"], out consultationId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnReplyProductConsultation.Click += new EventHandler(btnReplyProductConsultation_Click);
                if (!Page.IsPostBack)
                {
                    ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(consultationId);
                    if (productConsultation == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        litUserName.Text = productConsultation.UserName;
                        litConsultationText.Text = productConsultation.ConsultationText;
                        lblTime.Time = productConsultation.ConsultationDate;
                    }
                }
            }
        }
    }
}

