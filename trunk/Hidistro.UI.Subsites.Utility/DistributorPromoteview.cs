using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using kindeditor.Net;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorPromoteview : TemplatedWebControl
    {
        string errors;
        KindeditorControl fckDescription;
        bool isValid = true;
        PromotionInfo promotion;
        TextBox txtPromoteSalesName;

        protected override void AttachChildControls()
        {
            txtPromoteSalesName = (TextBox)FindControl("txtPromoteSalesName");
            fckDescription = (KindeditorControl)FindControl("fckDescription");
            if (!Page.IsPostBack && (promotion != null))
            {
                txtPromoteSalesName.Text = promotion.Name;
                fckDescription.Text = promotion.Description;
            }
        }

        public void Reset()
        {
            txtPromoteSalesName.Text = string.Empty;
            fckDescription.Text = string.Empty;
        }

        public string CurrentErrors
        {
            get
            {
                return errors;
            }
        }

        public bool IsValid
        {
            get
            {
                return isValid;
            }
        }

        public PromotionInfo Item
        {
            get
            {
                errors = string.Empty;
                PromotionInfo info = new PromotionInfo();
                info.Name = txtPromoteSalesName.Text;
                info.Description = fckDescription.Text;
                return info;
            }
            set
            {
                promotion = value;
            }
        }
    }
}

