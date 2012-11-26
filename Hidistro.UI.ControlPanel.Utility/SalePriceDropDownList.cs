namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Web.UI.WebControls;

    public class SalePriceDropDownList : DropDownList
    {
       bool allowNull = true;
       string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            base.Items.Add(new ListItem("成本价", "CostPrice"));
            base.Items.Add(new ListItem("一口价", "SalePrice"));
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }
    }
}

