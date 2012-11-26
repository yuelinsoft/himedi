namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Promotions;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class PromoteTypeDropDownList : DropDownList
    {
       bool allowNull = true;
       bool nullItemCreated;
       string nullToDisplay = "";

        public PromoteTypeDropDownList()
        {
            Items.Clear();
            int num = 2;
            Items.Add(new ListItem("满额打折", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 4;
            Items.Add(new ListItem("买几送几", num2.ToString(CultureInfo.InvariantCulture)));
            int num3 = 3;
            Items.Add(new ListItem("满额免费用", num3.ToString(CultureInfo.InvariantCulture)));
            int num4 = 5;
            Items.Add(new ListItem("批发打折", num4.ToString(CultureInfo.InvariantCulture)));
        }

       void CreateNullItem()
        {
            if (AllowNull)
            {
                int num = 0;
                Items.Insert(0, new ListItem(NullToDisplay, num.ToString()));
                nullItemCreated = true;
            }
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
                if (allowNull && !nullItemCreated)
                {
                    CreateNullItem();
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
                if (!nullItemCreated && AllowNull)
                {
                    CreateNullItem();
                }
                else if (nullItemCreated)
                {
                    Items[0].Text = nullToDisplay;
                }
            }
        }

        public PromoteType SelectedValue
        {
            get
            {
                return (PromoteType) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

