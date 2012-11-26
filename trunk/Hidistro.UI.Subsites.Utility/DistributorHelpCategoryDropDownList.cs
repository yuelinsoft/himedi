using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorHelpCategoryDropDownList : DropDownList
    {
        bool allowNull = true;
        string nullToDisplay = "";

        public override void DataBind()
        {
            Items.Clear();
            base.Items.Add(new ListItem(NullToDisplay, string.Empty));
            foreach (HelpCategoryInfo info in SubsiteCommentsHelper.GetHelpCategorys())
            {
                Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.CategoryId.Value.ToString()));
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
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}

