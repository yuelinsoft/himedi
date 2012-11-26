using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
    public class ApprovedDropDownList : DropDownList
    {
        bool allowNull = true;

        public override void DataBind()
        {
            Items.Clear();
            if (AllowNull)
            {
                base.Items.Add(new ListItem("全部", string.Empty));
            }
            Items.Add(new ListItem("通过", "True"));
            Items.Add(new ListItem("禁止", "False"));
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

        public bool? SelectedValue
        {
            get
            {
                bool result = true;
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                bool.TryParse(base.SelectedValue, out result);
                return new bool?(result);
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedValue = value.Value.ToString();
                }
            }
        }
    }
}

