namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Text;
    using System.Web.UI;

    public class LeaveListTime : Control
    {
       string auto = "productId";
       string bindData = "EndDate";
       string outStr = string.Empty;

        public override void DataBind()
        {
            int num = 1;
            int num2 = (int) DataBinder.Eval(Page.GetDataItem(), Auto);
            DateTime time = (DateTime) DataBinder.Eval(Page.GetDataItem(), BindData);
            if (time < DateTime.Now)
            {
                num = 0;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(" <script type=\"text/javascript\"> ");
            builder.AppendFormat(" function LimitTimeBuyTimeShow_{0}()", num2.ToString());
            builder.Append(" { ");
            builder.AppendFormat(" showTimeList(\"{0}\",\"htmlspan{1}\",\"{2}\");", time.ToString("yyyy-MM-dd HH:mm:ss"), num2.ToString(), num);
            builder.AppendFormat(" setTimeout(\"LimitTimeBuyTimeShow_{0}()\", 1000);", num2.ToString());
            builder.Append(" }");
            builder.AppendFormat(" LimitTimeBuyTimeShow_{0}(); ", num2.ToString());
            builder.Append(" </script>");
            outStr = builder.ToString();
            base.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(outStr.ToString());
        }

        public string Auto
        {
            get
            {
                return auto;
            }
            set
            {
                auto = value;
            }
        }

        public string BindData
        {
            get
            {
                return bindData;
            }
            set
            {
                bindData = value;
            }
        }
    }
}

