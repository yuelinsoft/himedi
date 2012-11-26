using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class SalePriceDropDownList : DropDownList
    {
        bool allowNull = true;
        string nullToDisplay = "";

        public override void DataBind()
        {
            Items.Clear();
            if (AllowNull)
            {
                base.Items.Add(new ListItem(NullToDisplay, string.Empty));
            }
            base.Items.Add(new ListItem("我的采购价", "PurchasePrice"));
            base.Items.Add(new ListItem("一口价", "SalePrice"));
        }

        public bool AllowNull
        {
            get
            {
                return allowNull;
            }
            set
            {
                allowNull = value;
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

