namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class FormatedSiteUrl : HyperLink
    {
       object data;
       string dataField;
       string urlName;

        protected override void OnDataBinding(EventArgs e)
        {
            data = DataBinder.Eval(Page.GetDataItem(), DataField);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((string.IsNullOrEmpty(base.NavigateUrl) && !string.IsNullOrEmpty(UrlName)) && (data != null))
            {
                base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(UrlName, new object[] { data });
            }
            base.Render(writer);
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

        public string UrlName
        {
            get
            {
                return urlName;
            }
            set
            {
                urlName = value;
            }
        }
    }
}

