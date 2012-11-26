namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class OperationDropDownList : DropDownList
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
            base.Items.Add(new ListItem("+", "+"));
            base.Items.Add(new ListItem("*", "*"));
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

