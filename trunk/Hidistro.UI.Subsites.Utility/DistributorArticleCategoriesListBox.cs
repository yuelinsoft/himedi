using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{

    public class DistributorArticleCategoriesListBox : ListBox
    {

        public override void DataBind()
        {
            Items.Clear();

            foreach (ArticleCategoryInfo info in SubsiteCommentsHelper.GetMainArticleCategories())
            {
                Items.Add(new ListItem(info.Name, info.CategoryId.ToString()));
            }

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


