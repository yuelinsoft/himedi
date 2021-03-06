﻿namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Sales;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class ShippingModeDropDownList : DropDownList
    {
        
       string _NullToDisplay ;
       bool allowNull = true;

        public override void DataBind()
        {
            base.Items.Clear();
            if (AllowNull)
            {
                base.Items.Add(new ListItem(NullToDisplay, string.Empty));
            }
            foreach (ShippingModeInfo info in ControlProvider.Instance().GetShippingModes())
            {
                base.Items.Add(new ListItem(info.Name, info.ModeId.ToString()));
            }
        }

        public bool AllowNull
        {
            get
            {
                return allowNull;
            }
            set
            {
                allowNull = value;
            }
        }

        public string NullToDisplay
        {
            
            get
            {
                return _NullToDisplay ;
            }
            
            set
            {
                _NullToDisplay  = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (!value.HasValue)
                {
                    base.SelectedValue = string.Empty;
                }
                else
                {
                    base.SelectedValue = value.ToString();
                }
            }
        }
    }
}

