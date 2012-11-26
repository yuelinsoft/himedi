namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI.WebControls;

    public class ExportFormatRadioButtonList : RadioButtonList
    {
       System.Web.UI.WebControls.RepeatDirection repeatDirection;

        public ExportFormatRadioButtonList()
        {
            Items.Clear();
            Items.Add(new ListItem("CSV格式", "csv"));
            Items.Add(new ListItem("TXT格式", "txt"));
            base.SelectedIndex = 0;
        }

        public override System.Web.UI.WebControls.RepeatDirection RepeatDirection
        {
            get
            {
                return repeatDirection;
            }
            set
            {
                repeatDirection = value;
            }
        }
    }
}

