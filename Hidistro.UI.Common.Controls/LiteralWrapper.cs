namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class LiteralWrapper : IText
    {
       Literal _literal;

        internal LiteralWrapper(Literal literal)
        {
            _literal = literal;
        }

        public System.Web.UI.Control Control
        {
            get
            {
                return _literal;
            }
        }

        public string Text
        {
            get
            {
                return _literal.Text;
            }
            set
            {
                _literal.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _literal.Visible;
            }
            set
            {
                _literal.Visible = value;
            }
        }
    }
}

