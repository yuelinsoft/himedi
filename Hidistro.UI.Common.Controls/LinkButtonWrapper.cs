namespace Hidistro.UI.Common.Controls
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class LinkButtonWrapper : IButton, IText
    {
       LinkButton _button;

        public event EventHandler Click
        {
            add
            {
                _button.Click += value;
            }
            remove
            {
                _button.Click -= value;
            }
        }

        public event CommandEventHandler Command
        {
            add
            {
                _button.Command += value;
            }
            remove
            {
                _button.Command -= value;
            }
        }

        internal LinkButtonWrapper(LinkButton button)
        {
            _button = button;
        }

        public AttributeCollection Attributes
        {
            get
            {
                return _button.Attributes;
            }
        }

        public bool CausesValidation
        {
            get
            {
                return _button.CausesValidation;
            }
            set
            {
                _button.CausesValidation = value;
            }
        }

        public string CommandArgument
        {
            get
            {
                return _button.CommandArgument;
            }
            set
            {
                _button.CommandArgument = value;
            }
        }

        public string CommandName
        {
            get
            {
                return _button.CommandName;
            }
            set
            {
                _button.CommandName = value;
            }
        }

        public System.Web.UI.Control Control
        {
            get
            {
                return _button;
            }
        }

        public string Text
        {
            get
            {
                return _button.Text;
            }
            set
            {
                _button.Text = value;
            }
        }

        public bool Visible
        {
            get
            {
                return _button.Visible;
            }
            set
            {
                _button.Visible = value;
            }
        }
    }
}

