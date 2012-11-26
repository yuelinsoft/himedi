using Hidistro.Core;
using Hidistro.Subsites.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorProductCategoriesListBox : ListBox
    {
        string strDepth = "　　";

        public override void DataBind()
        {
            Items.Clear();
            DataTable categories = SubsiteCatalogHelper.GetCategories();
            string cateid = "";
            for (int i = 0; i < categories.Rows.Count; i++)
            {
                cateid = categories.Rows[i]["CategoryId"].ToString();
                Items.Add(new ListItem(FormatDepth((int)categories.Rows[i]["Depth"], Globals.HtmlDecode((string)categories.Rows[i]["Name"])), cateid));
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

        public IList<int> SelectedValue
        {
            get
            {
                IList<int> list = new List<int>();
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Selected)
                    {
                        list.Add(int.Parse(Items[i].Value));
                    }
                }
                return list;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Selected = false;
                }
                foreach (int item in value)
                {
                    for (int j = 0; j < Items.Count; j++)
                    {
                        if (Items[j].Value == item.ToString())
                        {
                            Items[j].Selected = true;
                        }
                    }
                }
            }


        }


    }


}

