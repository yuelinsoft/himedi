using Hidistro.Entities.Commodities;
using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    //类型下拉控件
    public class SubjectTypeDropDownList : DropDownList
    {

        public SubjectTypeDropDownList()
        {
            Items.Clear();
            Items.Add(new ListItem("热卖商品", "1"));
            Items.Add(new ListItem("特价商品", "2")); 
            Items.Add(new ListItem("推荐商品", "3"));
            Items.Add(new ListItem("最新商品", "4"));
        }


        public SubjectType SelectedValue
        {
            get
            {

                return (SubjectType)int.Parse(base.SelectedValue, CultureInfo.InvariantCulture);

            }
            set
            {

                int num = (int)value;

                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString(CultureInfo.InvariantCulture)));

            }

        }

    }

}

