namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;

    public class CartTotalPriceLabel : PriceLabel
    {
        public CartTotalPriceLabel() : base("SubmitOrder_CartTotalPriceLabel", "SubmitOrder_CartTotalPriceLabel_v")
        {
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
            base.AddAttributesToRender(writer);
        }
    }
}

