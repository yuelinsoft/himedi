namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Web.UI.WebControls;

    public class PurchaseDropDownList : DropDownList
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
            base.Items.Add(new ListItem("采购价", "PurchasePrice"));
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

