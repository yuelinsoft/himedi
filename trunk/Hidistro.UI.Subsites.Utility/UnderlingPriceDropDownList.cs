using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Subsites.Members;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class UnderlingPriceDropDownList : DropDownList
    {
        bool allowNull = true;
        string nullToDisplay = "";

        public override void DataBind()
        {
            Items.Clear();
            if (AllowNull)
            {
                base.Items.Add(new ListItem(NullToDisplay, string.Empty));
            }
            base.Items.Add(new ListItem("一口价", "-3"));
            foreach (MemberGradeInfo info in UnderlingHelper.GetUnderlingGrades())
            {
                Items.Add(new ListItem(Globals.HtmlDecode(info.Name + "价"), info.GradeId.ToString()));
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
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString()));
                }
            }
        }
    }
}

