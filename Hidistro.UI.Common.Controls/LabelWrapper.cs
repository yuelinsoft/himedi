namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class LabelWrapper : IText
    {
       Label _label;

        internal LabelWrapper(Label label)
        {
            _label = label;
        }

        public System.Web.UI.Control Control
        {
            get
            {
                return _label;
            }
        }

        public string Text
        {
            get
            {
                return _label.Text;
            }
            set
            {
                _label.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _label.Visible;
            }
            set
            {
                _label.Visible = value;
            }
        }
    }
}

