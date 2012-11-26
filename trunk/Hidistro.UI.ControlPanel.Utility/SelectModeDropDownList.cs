namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class SelectModeDropDownList : DropDownList
    {
        public SelectModeDropDownList()
        {
            this.Items.Clear();
            int num = 1;
            this.Items.Add(new ListItem("下拉列表", num.ToString(CultureInfo.InvariantCulture)));
            int num2 = 2;
            this.Items.Add(new ListItem("单选按钮", num2.ToString(CultureInfo.InvariantCulture)));
        }

        public SelectModeTypes SelectedValue
        {
            get
            {
                return (SelectModeTypes) int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));
            }
        }
    }
}

