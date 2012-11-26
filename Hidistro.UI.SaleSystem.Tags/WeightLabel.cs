namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class WeightLabel : WebControl
    {
        public WeightLabel() : base(HtmlTextWriterTag.Span)
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(base.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, base.CssClass);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "SubmitOrder_Weight");
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
                if (this.ViewState["WeightText"] == null)
                {
                    return null;
                }
                return (string) this.ViewState["WeightText"];
            }
            set
            {
                this.ViewState["WeightText"] = value;
            }
        }
    }
}

