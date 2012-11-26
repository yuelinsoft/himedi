namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Members;
    using System;
    using System.Web.UI.WebControls;

    public class TradeTypeDropDownList : DropDownList
    {
       bool allowNull = true;
       bool isDistributor;
       string nullToDisplay;

        public override void DataBind()
        {
            Items.Clear();
            if (AllowNull)
            {
                Items.Add(new ListItem(NullToDisplay, string.Empty));
            }
            Items.Add(new ListItem("自助充值", "1"));
            Items.Add(new ListItem("后台加款", "2"));
            Items.Add(new ListItem("消费", "3"));
            Items.Add(new ListItem("提现", "4"));
            if (IsDistributor)
            {
                Items.Add(new ListItem("采购单退款", "5"));
            }
            else
            {
                Items.Add(new ListItem("订单退款", "5"));
            }
            Items.Add(new ListItem("推荐人提成", "6"));
        }

        public bool AllowNull
        {
            get
            {
                return allowNull;
            }
            set
            {
                allowNull = value;
            }
        }

        public bool IsDistributor
        {
            get
            {
                return isDistributor;
            }
            set
            {
                isDistributor = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return nullToDisplay;
            }
            set
            {
                nullToDisplay = value;
            }
        }

        public TradeTypes SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return TradeTypes.NotSet;
                }
                return (TradeTypes) int.Parse(base.SelectedValue);
            }
            set
            {
                int num = (int) value;
                base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(num.ToString()));
            }
        }
    }
}

