using System;
using System.Runtime.CompilerServices;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
    public class YesNoRadioButtonList : RadioButtonList
    {

        string _NoText;

        string _YesText;

        public YesNoRadioButtonList()
        {
            NoText = "否";
            YesText = "是";
            Items.Clear();
            Items.Add(new ListItem(YesText, "True"));
            Items.Add(new ListItem(NoText, "False"));
            RepeatDirection = RepeatDirection.Horizontal;
            SelectedValue = true;
        }

        public string NoText
        {

            get
            {
                return _NoText;
            }

            set
            {
                _NoText = value;
            }
        }

        public bool SelectedValue
        {
            get
            {
                return bool.Parse(base.SelectedValue);
            }
            set
            {
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value ? "True" : "False"));
            }
        }

        public string YesText
        {

            get
            {
                return _YesText;
            }

            set
            {
                _YesText = value;
            }
        }
    }
}

