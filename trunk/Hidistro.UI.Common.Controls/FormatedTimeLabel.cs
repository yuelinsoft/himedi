namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class FormatedTimeLabel : Literal
    {
       string dataField;
       string formatDateTime = string.Empty;
       string nullToDisplay = "-";
       bool showTime = true;

        public override void DataBind()
        {
            if (DataField != null)
            {
                Time = DataBinder.Eval(Page.GetDataItem(), DataField);
            }
            else
            {
                base.DataBind();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (((Time == null) || (Time == DBNull.Value)) || (Convert.ToDateTime((DateTime) Time, CultureInfo.InvariantCulture) == DateTime.MinValue))
            {
                base.Text = NullToDisplay;
            }
            else
            {
                DateTime time = (DateTime) Time;
                if (!string.IsNullOrEmpty(FormatDateTime))
                {
                    base.Text = time.ToString(FormatDateTime, CultureInfo.InvariantCulture);
                }
                else if (ShopTime)
                {
                    base.Text = time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    base.Text = time.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                base.Render(writer);
            }
        }

        public string DataField
        {
            get
            {
                return dataField;
            }
            set
            {
                dataField = value;
            }
        }

        public string FormatDateTime
        {
            get
            {
                return formatDateTime;
            }
            set
            {
                formatDateTime = value;
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

        public bool ShopTime
        {
            get
            {
                return showTime;
            }
            set
            {
                showTime = value;
            }
        }

        public object Time
        {
            get
            {
                if (ViewState["Time"] == null)
                {
                    return null;
                }
                return ViewState["Time"];
            }
            set
            {
                if ((value == null) || (value == DBNull.Value))
                {
                    ViewState["Time"] = null;
                }
                else
                {
                    ViewState["Time"] = value;
                }
            }
        }
    }
}

