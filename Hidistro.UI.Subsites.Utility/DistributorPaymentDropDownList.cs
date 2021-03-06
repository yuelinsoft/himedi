﻿using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorPaymentDropDownList : DropDownList
    {
        bool allowNull = true;

        public override void DataBind()
        {
            base.Items.Clear();
            if (AllowNull)
            {
                base.Items.Add(new ListItem(string.Empty, string.Empty));
            }
            foreach (PaymentModeInfo info in SubsiteSalesHelper.GetPaymentModes())
            {
                base.Items.Add(new ListItem(Globals.HtmlDecode(info.Name), info.ModeId.ToString()));
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

