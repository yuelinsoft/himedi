using Hidistro.Core;
using Hidistro.Subsites.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class AuthorizeProductLineCheckBoxList : CheckBoxList
    {
        public override void DataBind()
        {

            Items.Clear();

            foreach (DataRow row in SubSiteProducthelper.GetAuthorizeProductLines().Rows)
            {
                Items.Add(new ListItem(string.Concat(new object[] { Globals.HtmlDecode((string)row["Name"]), "（", row["ProductCount"], "）" }), row["LineId"].ToString()));
            }

        }

        public IList<int> SelectedValue
        {
            get
            {

                IList<int> list = new List<int>();

                for (int i = 0; i < Items.Count; i++)
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

                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].Selected = false;
                }

                //IList<int> list = value;
                foreach (int item in value)
                {
                    for (int j = 0; j < Items.Count; j++)
                    {

                        if (this.Items[j].Value == item.ToString())
                        {

                            Items[j].Selected = true;

                        }

                    }

                }

            }

        }

    }

}

