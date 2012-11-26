using Hidistro.Entities.Promotions;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorDiscountValueTypeRadioButtonList : RadioButtonList
    {
        public DistributorDiscountValueTypeRadioButtonList()
        {
            Items.Clear();
            Items.Add(new ListItem("优惠金额","0"));
            Items.Add(new ListItem("折扣率", "1"));
            RepeatDirection = RepeatDirection.Horizontal;
            SelectedIndex = 0;
        }

        public DiscountValueType SelectedValue
        {
            get
            {
                return (DiscountValueType)int.Parse(base.SelectedValue);
            }
            set
            {
                SelectedIndex = Items.IndexOf(Items.FindByValue(((int)value).ToString()));
            }
        }
    }
}

