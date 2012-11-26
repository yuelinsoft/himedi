namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Promotions;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class GroupBuyStatusLabel : Label
    {
        protected override void Render(HtmlTextWriter writer)
        {
            switch (((GroupBuyStatus) GroupBuyStatusCode))
            {
                case GroupBuyStatus.UnderWay:
                    base.Text = "正在进行中";
                    break;

                case GroupBuyStatus.EndUntreated:
                    base.Text = "结束未处理";
                    break;

                case GroupBuyStatus.Success:
                    base.Text = "成功结束";
                    break;

                case GroupBuyStatus.Failed:
                    base.Text = "失败结束";
                    break;

                default:
                    base.Text = "-";
                    break;
            }
            base.Render(writer);
        }

        public object GroupBuyStatusCode
        {
            get
            {
                return ViewState["GroupBuyStatusCode"];
            }
            set
            {
                ViewState["GroupBuyStatusCode"] = value;
            }
        }
    }
}

