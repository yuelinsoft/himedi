using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorProductCategoriesDropDownList : DropDownList
    {
        bool isTopCategory;
        bool m_AutoDataBind;
        string m_NullToDisplay = "";
        string strDepth = "　　";

        public override void DataBind()
        {
            Items.Clear();
            Items.Add(new ListItem(NullToDisplay, string.Empty));
            if (IsTopCategory)
            {
                foreach (CategoryInfo info in SubsiteCatalogHelper.GetMainCategories())
                {
                    Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.CategoryId.ToString()));
                }
            }
            else
            {
                DataTable categories = SubsiteCatalogHelper.GetCategories();
                string cateid = "";
                for (int i = 0; i < categories.Rows.Count; i++)
                {
                     cateid = categories.Rows[i]["CategoryId"].ToString();
                     Items.Add(new ListItem(FormatDepth((int)categories.Rows[i]["Depth"], Globals.HtmlDecode((string)categories.Rows[i]["Name"])), cateid));
                }
            }
        }

        string FormatDepth(int depth, string categoryName)
        {
            for (int i = 1; i < depth; i++)
            {
                categoryName = strDepth + categoryName;
            }
            return categoryName;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (AutoDataBind && !Page.IsPostBack)
            {
                DataBind();
            }
        }

        public bool AutoDataBind
        {
            get
            {
                return m_AutoDataBind;
            }
            set
            {
                m_AutoDataBind = value;
            }
        }

        public bool IsTopCategory
        {
            get
            {
                return isTopCategory;
            }
            set
            {
                isTopCategory = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return m_NullToDisplay;
            }
            set
            {
                m_NullToDisplay = value;
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

