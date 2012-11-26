using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Subsites.Members;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class UnderlingGradeCheckBoxList : CheckBoxList
    {
        public override void DataBind()
        {
            Items.Clear();
            IList<MemberGradeInfo> underlingGrades = UnderlingHelper.GetUnderlingGrades();
            int index = 0;
            foreach (MemberGradeInfo info in underlingGrades)
            {
                Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.GradeId.ToString()));
                Items[index++].Selected = true;
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

