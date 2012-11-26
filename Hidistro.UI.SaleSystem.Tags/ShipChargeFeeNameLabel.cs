namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ShipChargeFeeNameLabel : WebControl
    {
        public ShipChargeFeeNameLabel() : base(HtmlTextWriterTag.Span)
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "SubmitOrder_ShipChargeFeeName");
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
                if (this.ViewState["ShipChargeFeeNameText"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["ShipChargeFeeNameText"];
            }
            set
            {
                this.ViewState["ShipChargeFeeNameText"] = value;
            }
        }
    }
}

