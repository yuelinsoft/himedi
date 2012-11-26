namespace Hidistro.UI.ControlPanel.Utility
{
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class CalculateModeRadioButtonList : RadioButtonList
    {
        public CalculateModeRadioButtonList()
        {
            int num = 1;
            this.Items.Add(new ListItem("固定金额", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 2;
            this.Items.Add(new ListItem("购物车金额百分比", num2.ToString(CultureInfo.InvariantCulture)));
        }

        public int SelectedValue
        {
            get
            {
                return int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

