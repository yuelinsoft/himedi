namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class FormatedMoneyLabel : Label
    {
       string nullToDisplay = "-";

        protected override void Render(HtmlTextWriter writer)
        {
            if ((Money != null) && (Money != DBNull.Value))
            {
                base.Text = Globals.FormatMoney((decimal) Money);
            }
            if (string.IsNullOrEmpty(base.Text))
            {
                base.Text = NullToDisplay;
            }
            base.Render(writer);
        }

        public object Money
        {
            get
            {
                if (ViewState["Money"] == null)
                {
                    return null;
                }
                return ViewState["Money"];
            }
            set
            {
                if ((value == null) || (value == DBNull.Value))
                {
                    ViewState["Money"] = null;
                }
                else
                {
                    ViewState["Money"] = value;
                }
            }
        }

        public string NullToDisplay
        {
            get
            {
                return nullToDisplay;
            }
            set
            {
                nullToDisplay = value;
            }
        }
    }
}

