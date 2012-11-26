namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class BrandCategoriesDropDownList : DropDownList
    {
       bool allowNull = true;
       string nullToDisplay = "";
       int? productTypeId;

        public override void DataBind()
        {
            Items.Clear();
            if (AllowNull)
            {
                base.Items.Add(new ListItem(NullToDisplay, string.Empty));
            }
            DataTable brandCategoriesByTypeId = new DataTable();
            if (ProductTypeId.HasValue)
            {
                brandCategoriesByTypeId = ControlProvider.Instance().GetBrandCategoriesByTypeId(productTypeId.Value);
            }
            else
            {
                brandCategoriesByTypeId = ControlProvider.Instance().GetBrandCategories();
            }
            foreach (DataRow row in brandCategoriesByTypeId.Rows)
            {
                int num = (int) row["BrandId"];
                Items.Add(new ListItem((string) row["BrandName"], num.ToString(CultureInfo.InvariantCulture)));
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

        public int? ProductTypeId
        {
            get
            {
                return productTypeId;
            }
            set
            {
                productTypeId = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (!string.IsNullOrEmpty(base.SelectedValue))
                {
                    return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
                }
                return null;
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

