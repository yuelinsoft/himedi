using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
    public class HiImageButtonWrapper : IButton, IText
    {
        HiImageButton _button;

        public event EventHandler Click;

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

        internal HiImageButtonWrapper(HiImageButton button)
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
                return string.Empty;
            }
            set
            {
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

