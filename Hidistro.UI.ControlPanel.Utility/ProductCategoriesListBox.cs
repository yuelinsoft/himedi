namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ProductCategoriesListBox : ListBox
    {
       string strDepth = "　　";

        public override void DataBind()
        {
            this.Items.Clear();
            DataTable categories = CatalogHelper.GetCategories();
            for (int i = 0; i < categories.Rows.Count; i++)
            {
                int num2 = (int) categories.Rows[i]["CategoryId"];
                this.Items.Add(new ListItem(this.FormatDepth((int) categories.Rows[i]["Depth"], Globals.HtmlDecode((string) categories.Rows[i]["Name"])), num2.ToString(CultureInfo.InvariantCulture)));
            }
            ListItem item = new ListItem("所有", "0");
            this.Items.Insert(0, item);
        }

       string FormatDepth(int depth, string categoryName)
        {
            for (int i = 1; i < depth; i++)
            {
                categoryName = this.strDepth + categoryName;
            }
            return categoryName;
        }

        public IList<int> SelectedValue
        {
            get
            {
                IList<int> list = new List<int>();
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        list.Add(int.Parse(this.Items[i].Value));
                    }
                }
                return list;
            }
            set
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    this.Items[i].Selected = false;
                }
                IList<int> list = value;
                foreach (int num2 in list)
                {
                    for (int j = 0; j < this.Items.Count; j++)
                    {
                        if (this.Items[j].Value == num2.ToString())
                        {
                            this.Items[j].Selected = true;
                        }
                    }
                }
            }
        }
    }
}

