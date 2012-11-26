namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Promotions;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class DiscountValueTypeRadioButtonList : RadioButtonList
    {
        public DiscountValueTypeRadioButtonList()
        {
            this.Items.Clear();
            int num = 0;
            this.Items.Add(new ListItem("优惠金额", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 1;
            this.Items.Add(new ListItem("折扣率", num2.ToString(CultureInfo.InvariantCulture)));
            this.RepeatDirection = RepeatDirection.Horizontal;
            this.SelectedIndex = 0;
        }

        public DiscountValueType SelectedValue
        {
            get
            {
                return (DiscountValueType) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

