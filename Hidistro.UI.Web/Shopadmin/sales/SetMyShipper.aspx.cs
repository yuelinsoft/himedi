using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.ShopAdmin
{
    public partial class SetMyShipper : DistributorPage
    {
        private void btnEditShipper_Click(object sender, EventArgs e)
        {
            ShippersInfo shipper = new ShippersInfo();
            shipper.ShipperTag = txtShipperTag.Text.Trim();
            shipper.ShipperName = txtShipperName.Text.Trim();
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
                shipper.Remark = txtRemark.Text.Trim();
                if (ValidationShipper(shipper))
                {
                    if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
                    {
                        ShowMsg("手机号码和电话号码必填其一", false);
                    }
                    else if (SubsiteSalesHelper.SetMyShipper(shipper))
                    {
                        ShowMsg("成功保存了发货信息", true);
                    }
                    else
                    {
                        ShowMsg("保存发货信息失败", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnEditShipper.Click += new EventHandler(btnEditShipper_Click);
            if (!Page.IsPostBack)
            {
                ShippersInfo myShipper = SubsiteSalesHelper.GetMyShipper();
                if (myShipper != null)
                {
                    Globals.EntityCoding(myShipper, false);
                    txtShipperTag.Text = myShipper.ShipperTag;
                    txtShipperName.Text = myShipper.ShipperName;
                    ddlReggion.SetSelectedRegionId(new int?(myShipper.RegionId));
                    txtAddress.Text = myShipper.Address;
                    txtCellPhone.Text = myShipper.CellPhone;
                    txtTelPhone.Text = myShipper.TelPhone;
                    txtZipcode.Text = myShipper.Zipcode;
                    txtRemark.Text = myShipper.Remark;
                }
            }
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

