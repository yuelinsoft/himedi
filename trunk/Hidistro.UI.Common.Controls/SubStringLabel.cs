namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SubStringLabel : Literal
    {
       int _strLength;
       string _strReplace = "...";
       string field;

        protected override void OnDataBinding(EventArgs e)
        {
            if (!string.IsNullOrEmpty(Field))
            {
                object obj2 = DataBinder.Eval(Page.GetDataItem(), Field);
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    base.Text = (string) obj2;
                }
            }
            base.OnDataBinding(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if ((StrLength > 0) && (StrLength < base.Text.Length))
            {
                base.Text = base.Text.Substring(0, StrLength) + StrReplace;
            }
            base.Render(writer);
        }

        public string Field
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
            }
        }

        public int StrLength
        {
            get
            {
                return _strLength;
            }
            set
            {
                _strLength = value;
            }
        }

        public string StrReplace
        {
            get
            {
                return _strReplace;
            }
            set
            {
                _strReplace = value;
            }
        }
    }
}

