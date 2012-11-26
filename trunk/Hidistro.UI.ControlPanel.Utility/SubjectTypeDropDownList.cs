namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Commodities;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class SubjectTypeDropDownList : DropDownList
    {
        public SubjectTypeDropDownList()
        {
            this.Items.Clear();
            int num = 4;
            this.Items.Add(new ListItem("最新商品", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 1;
            this.Items.Add(new ListItem("热卖商品", num2.ToString(CultureInfo.InvariantCulture)));
            int num3 = 3;
            this.Items.Add(new ListItem("推荐商品", num3.ToString(CultureInfo.InvariantCulture)));
            int num4 = 2;
            this.Items.Add(new ListItem("特价商品", num4.ToString(CultureInfo.InvariantCulture)));
        }

        public SubjectType SelectedValue
        {
            get
            {
                return (SubjectType) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

