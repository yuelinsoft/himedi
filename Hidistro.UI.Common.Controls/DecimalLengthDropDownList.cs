namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class DecimalLengthDropDownList : DropDownList
    {
        public DecimalLengthDropDownList()
        {
            Items.Clear();
            Items.Add(new ListItem("2位", "2"));
            Items.Add(new ListItem("1位", "1"));
            Items.Add(new ListItem("0位", "0"));
        }

        public int SelectedValue
        {
            get
            {
                int num;
                if (int.TryParse(base.SelectedValue, out num))
                {
                    return num;
                }
                return 2;
            }
            set
            {
                base.SelectedValue = value.ToString();
            }
        }
    }
}

