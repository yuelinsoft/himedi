namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ProductCategoriesDropDownList : DropDownList
    {
       bool isTopCategory;
       bool m_AutoDataBind;
       string m_NullToDisplay = "";
       string strDepth = "　　";

        public override void DataBind()
        {
            this.Items.Clear();
            this.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            if (this.IsTopCategory)
            {
                foreach (CategoryInfo info in CatalogHelper.GetMainCategories())
                {
                    this.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.CategoryId.ToString()));
                }
            }
            else
            {
                DataTable categories = CatalogHelper.GetCategories();
                for (int i = 0; i < categories.Rows.Count; i++)
                {
                    int num3 = (int) categories.Rows[i]["CategoryId"];
                    this.Items.Add(new ListItem(this.FormatDepth((int) categories.Rows[i]["Depth"], Globals.HtmlDecode((string) categories.Rows[i]["Name"])), num3.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }

       string FormatDepth(int depth, string categoryName)
        {
            for (int i = 1; i < depth; i++)
            {
                categoryName = this.strDepth + categoryName;
            }
            return categoryName;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.AutoDataBind && !this.Page.IsPostBack)
            {
                this.DataBind();
            }
        }

        public bool AutoDataBind
        {
            get
            {
                return this.m_AutoDataBind;
            }
            set
            {
                this.m_AutoDataBind = value;
            }
        }

        public bool IsTopCategory
        {
            get
            {
                return this.isTopCategory;
            }
            set
            {
                this.isTopCategory = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.m_NullToDisplay;
            }
            set
            {
                this.m_NullToDisplay = value;
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

