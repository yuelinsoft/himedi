namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Members;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class TradeTypeNameLabel : Label
    {
       bool isDistributor;
       string tradeType;
       TradeTypes type;

        protected override void OnDataBinding(EventArgs e)
        {
            if (!string.IsNullOrEmpty(tradeType))
            {
                type = (TradeTypes) Enum.Parse(typeof(TradeTypes), DataBinder.Eval(Page.GetDataItem(), TradeType).ToString());
            }
            else
            {
                base.OnDataBinding(e);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            switch (type)
            {
                case TradeTypes.SelfhelpInpour:
                    base.Text = "自助充值";
                    break;

                case TradeTypes.BackgroundAddmoney:
                    base.Text = "后台加款";
                    break;

                case TradeTypes.Consume:
                    base.Text = "消费";
                    break;

                case TradeTypes.DrawRequest:
                    base.Text = "提现";
                    break;

                case TradeTypes.RefundOrder:
                    if (!isDistributor)
                    {
                        base.Text = "订单退款";
                        break;
                    }
                    base.Text = "采购单退款";
                    break;

                case TradeTypes.ReferralDeduct:
                    base.Text = "推荐人提成";
                    break;

                default:
                    base.Text = "其他";
                    break;
            }
            base.Render(writer);
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

        public string TradeType
        {
            get
            {
                return tradeType;
            }
            set
            {
                tradeType = value;
            }
        }
    }
}

