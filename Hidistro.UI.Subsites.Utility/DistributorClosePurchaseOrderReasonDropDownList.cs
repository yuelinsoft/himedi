using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorClosePurchaseOrderReasonDropDownList : DropDownList
    {
        public DistributorClosePurchaseOrderReasonDropDownList()
        {
            Items.Clear();
            Items.Add(new ListItem("请选择取消的理由", "请选择取消的理由"));
            Items.Add(new ListItem("分销商的会员不想买了", "分销商的会员不想买了"));
            Items.Add(new ListItem("暂时缺货", "暂时缺货"));
            Items.Add(new ListItem("其他原因", "其他原因"));
        }
    }
}

