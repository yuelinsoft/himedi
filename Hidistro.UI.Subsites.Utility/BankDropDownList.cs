using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    /// <summary>
    /// 银行选择下拉控件
    /// </summary>
    public class BankDropDownList : DropDownList
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public BankDropDownList()
        {
            Items.Clear();
            Items.Add(new ListItem("请选择", "请选择"));
            Items.Add(new ListItem("中国工商银行", "中国工商银行"));
            Items.Add(new ListItem("中国建设银行", "中国建设银行"));
            Items.Add(new ListItem("中国农业银行", "中国农业银行"));
        }

    }
}

