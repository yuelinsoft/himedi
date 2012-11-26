using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Shippers)]
    public partial class AddShipper : AdminPage
    {
        private void btnAddShipper_Click(object sender, EventArgs e)
        {
            ShippersInfo info2 = new ShippersInfo();
            info2.ShipperTag = txtShipperTag.Text.Trim();
            info2.ShipperName = txtShipperName.Text.Trim();
            ShippersInfo shipper = info2;
            if (!ddlReggion.GetSelectedRegionId().HasValue)
            {
                ShowMsg("请选择地区", false);
            }
            else
            {
                shipper.RegionId = ddlReggion.GetSelectedRegionId().Value;
                shipper.Address = txtAddress.Text.Trim();
                shipper.CellPhone = txtCellPhone.Text.Trim();
                shipper.TelPhone = txtTelPhone.Text.Trim();
                shipper.Zipcode = txtZipcode.Text.Trim();
                shipper.IsDefault = chkIsDefault.SelectedValue;
                shipper.Remark = txtRemark.Text.Trim();
                if (ValidationShipper(shipper))
                {
                    if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
                    {
                        ShowMsg("手机号码和电话号码必填其一", false);
                    }
                    else if (SalesHelper.AddShipper(shipper))
                    {
                        ShowMsg("成功添加了一个发货信息", true);
                    }
                    else
                    {
                        ShowMsg("添加发货信息失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            base.OnInitComplete(e);
            btnAddShipper.Click += new EventHandler(btnAddShipper_Click);
        }

        private bool ValidationShipper(ShippersInfo shipper)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ShippersInfo>(shipper, new string[] { "Valshipper" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

