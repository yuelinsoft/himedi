using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
    /// <summary>
    /// 产品线
    /// </summary>
    public class ProductLineDropDownList : DropDownList
    {
        bool allowNull = true;
        string nullToDisplay = "全部";

        public override void DataBind()
        {
            base.Items.Clear();
            if (AllowNull)
            {
                base.Items.Add(new ListItem(NullToDisplay, string.Empty));
            }
            foreach (ProductLineInfo info in ControlProvider.Instance().GetProductLineList())
            {
                base.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.LineId.ToString()));
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
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (!value.HasValue)
                {
                    base.SelectedValue = string.Empty;
                }
                else
                {
                    base.SelectedValue = value.ToString();
                }
            }
        }
    }
}

