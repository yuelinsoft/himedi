namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class Common_Register_Clause : HtmlContainerControl
    {
       string cssClass;
        public const string TagID = "div_Common_Register_Clause";

        public Common_Register_Clause() : base("div")
        {
            base.ID = "div_Common_Register_Clause";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.CssClass))
            {
                base.Attributes.Add("class", this.CssClass);
            }
            base.Render(writer);
        }

        public string CssClass
        {
            get
            {
                return this.cssClass;
            }
            set
            {
                this.cssClass = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
    }
}

