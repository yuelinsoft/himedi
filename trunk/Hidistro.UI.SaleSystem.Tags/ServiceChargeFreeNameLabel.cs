namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ServiceChargeFreeNameLabel : WebControl
    {
        public ServiceChargeFreeNameLabel() : base(HtmlTextWriterTag.Span)
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "SubmitOrder_ServiceChargeFreeName");
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(this.Text));
        }

        public override ControlCollection Controls
        {
            get
            {
                base.EnsureChildControls();
                return base.Controls;
            }
        }

        public string Text
        {
            get
            {
                if (this.ViewState["ServiceChargeFreeNameText"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["ServiceChargeFreeNameText"];
            }
            set
            {
                this.ViewState["ServiceChargeFreeNameText"] = value;
            }
        }
    }
}

