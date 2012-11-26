namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class PackingChargeFreeNameLabel : WebControl
    {
        public PackingChargeFreeNameLabel() : base(HtmlTextWriterTag.Span)
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "SubmitOrder_PackingChargeFreeName");
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
                if (this.ViewState["PackingChargeFreeNameText"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["PackingChargeFreeNameText"];
            }
            set
            {
                this.ViewState["PackingChargeFreeNameText"] = value;
            }
        }
    }
}

