namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Commodities;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class ProductTypeDownList : DropDownList
    {
       bool allowNull = true;
       string nullToDisplay = "全部";

        public override void DataBind()
        {
            Items.Clear();
            IList<ProductTypeInfo> productTypes = ControlProvider.Instance().GetProductTypes();
            if (AllowNull)
            {
                base.Items.Add(new ListItem(NullToDisplay, string.Empty));
            }
            foreach (ProductTypeInfo info in productTypes)
            {
                base.Items.Add(new ListItem(info.TypeName, info.TypeId.ToString()));
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
                if (value.HasValue && (value > 0))
                {
                    base.SelectedValue = value.Value.ToString();
                }
                else
                {
                    base.SelectedValue = string.Empty;
                }
            }
        }
    }
}

