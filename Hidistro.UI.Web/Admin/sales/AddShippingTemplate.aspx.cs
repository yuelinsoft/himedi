using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class AddShippingTemplate : AdminPage
    {

        private void BindRegion()
        {
            grdRegion.DataSource = RegionList;
            grdRegion.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            decimal regionPrice = 0m;

            decimal addRegionPrice = 0m;

            if (ValidateValues(out regionPrice, out addRegionPrice))
            {
                Region item = new Region();
                item.RegionsId = txtRegion_Id.Text;
                item.Regions = txtRegion.Value;
                item.RegionPrice = regionPrice;
                item.RegionAddPrice = addRegionPrice;
                RegionList.Add(item);
                BindRegion();
                txtRegion_Id.Text = string.Empty;
                txtRegion.Value = string.Empty;
                txtRegionPrice.Text = "0";
                txtAddRegionPrice.Text = "0";
            }

        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

            int weight = 0;
            int? addWeight = 0;
            decimal price = 0m;
            decimal? addPrice = 0;

            if (ValidateRegionValues(out weight, out addWeight, out price, out addPrice))
            {
                new List<ShippingModeGroupInfo>();

                ShippingModeInfo shippingModeInfo = new ShippingModeInfo();

                shippingModeInfo.Name = Globals.HtmlEncode(txtModeName.Text.Trim());
                shippingModeInfo.Weight = weight;
                shippingModeInfo.AddWeight = addWeight;
                shippingModeInfo.Price = price;
                shippingModeInfo.AddPrice = addPrice;

                ShippingModeGroupInfo shippingModeGroupInfo = null;

                ShippingRegionInfo shippingRegionInfo = null;

                foreach (GridViewRow row in grdRegion.Rows)
                {
                    shippingModeGroupInfo = new ShippingModeGroupInfo();

                    shippingModeGroupInfo.Price = decimal.Parse(((TextBox)row.FindControl("txtModeRegionPrice")).Text);
                    shippingModeGroupInfo.AddPrice = decimal.Parse(((TextBox)row.FindControl("txtModeRegionAddPrice")).Text);

                    TextBox box = (TextBox)grdRegion.Rows[row.RowIndex].FindControl("txtRegionvalue_Id");

                    foreach (string str in box.Text.Split(new char[] { ',' }))
                    {
                        shippingRegionInfo = new ShippingRegionInfo();
                        shippingRegionInfo.RegionId = Convert.ToInt32(str.Trim());
                        shippingModeGroupInfo.ModeRegions.Add(shippingRegionInfo);
                    }

                    shippingModeInfo.ModeGroup.Add(shippingModeGroupInfo);

                }

                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ShippingModeInfo>(shippingModeInfo, "ValShippingModeInfo");

                string msg = string.Empty;

                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    ShowMsg(msg, false);
                }
                else if (SalesHelper.CreateShippingTemplate(shippingModeInfo))
                {
                    ClearControlValue();
                    ShowMsg("成功添加了一个配送方式模板", true);
                }
                else
                {
                    ShowMsg("您添加的地区有重复", false);
                }
            }
        }

        private void ClearControlValue()
        {
            txtAddPrice.Text = string.Empty;
            txtAddRegionPrice.Text = string.Empty;
            txtAddWeight.Text = string.Empty;
            txtModeName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtRegion.Value = string.Empty;
            txtRegion_Id.Text = string.Empty;
            txtRegionPrice.Text = string.Empty;
            txtWeight.Text = string.Empty;
        }

        private void grdRegion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            RegionList.RemoveAt(e.RowIndex);
            BindRegion();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnCreate.Click += new EventHandler(btnCreate_Click);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            grdRegion.RowDeleting += new GridViewDeleteEventHandler(grdRegion_RowDeleting);
        }

        private bool ValidateRegionValues(out int weight, out int? addWeight, out decimal price, out decimal? addPrice)
        {
            string str = string.Empty;
            addWeight = 0;
            addPrice = 0;
            if (!int.TryParse(txtWeight.Text.Trim(), out weight))
            {
                str = str + Formatter.FormatErrorMessage("起步重量不能为空,必须为正整数,限制在100千克以内");
            }
            if (!string.IsNullOrEmpty(txtAddWeight.Text.Trim()))
            {
                int num;
                if (int.TryParse(txtAddWeight.Text.Trim(), out num))
                {
                    addWeight = new int?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("加价重量不能为空,必须为正整数,限制在100千克以内");
                }
            }
            if (!decimal.TryParse(txtPrice.Text.Trim(), out price))
            {
                str = str + Formatter.FormatErrorMessage("默认起步价必须为非负数字,限制在1000万以内");
            }
            if (!string.IsNullOrEmpty(txtAddPrice.Text.Trim()))
            {
                decimal num2;
                if (decimal.TryParse(txtAddPrice.Text.Trim(), out num2))
                {
                    addPrice = new decimal?(num2);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("默认加价必须为非负数字,限制在1000万以内");
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }

        private bool ValidateValues(out decimal regionPrice, out decimal addRegionPrice)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(txtRegion_Id.Text))
            {
                str = str + Formatter.FormatErrorMessage("到达地不能为空");
            }
            if (string.IsNullOrEmpty(txtRegionPrice.Text))
            {
                str = str + Formatter.FormatErrorMessage("起步价不能为空");
                regionPrice = 0M;
            }
            else if (!decimal.TryParse(txtRegionPrice.Text.Trim(), out regionPrice))
            {
                str = str + Formatter.FormatErrorMessage("起步价只能为非负数字");
            }
            else if (decimal.Parse(txtRegionPrice.Text.Trim()) > 10000000M)
            {
                str = str + Formatter.FormatErrorMessage("起步价限制在1000万以内");
            }
            if (string.IsNullOrEmpty(txtAddRegionPrice.Text))
            {
                str = str + Formatter.FormatErrorMessage("加价不能为空");
                addRegionPrice = 0M;
            }
            else if (!decimal.TryParse(txtAddRegionPrice.Text.Trim(), out addRegionPrice))
            {
                str = str + Formatter.FormatErrorMessage("加价只能为非负数字");
            }
            else if (decimal.Parse(txtAddRegionPrice.Text.Trim()) > 10000000M)
            {
                str = str + Formatter.FormatErrorMessage("加价限制在1000万以内");
            }
            string.IsNullOrEmpty(str);
            return true;
        }

        private IList<Region> RegionList
        {
            get
            {
                if (ViewState["Region"] == null)
                {
                    ViewState["Region"] = new List<Region>();
                }
                return (IList<Region>)ViewState["Region"];
            }
        }

        [Serializable]
        public partial class Region
        {
            private decimal regionAddPrice;
            private decimal regionPrice;
            private string regions;
            private string regionsId;

            public decimal RegionAddPrice
            {
                get
                {
                    return regionAddPrice;
                }
                set
                {
                    regionAddPrice = value;
                }
            }

            public decimal RegionPrice
            {
                get
                {
                    return regionPrice;
                }
                set
                {
                    regionPrice = value;
                }
            }

            public string Regions
            {
                get
                {
                    return regions;
                }
                set
                {
                    regions = value;
                }
            }

            public string RegionsId
            {
                get
                {
                    return regionsId;
                }
                set
                {
                    regionsId = value;
                }
            }
        }
    }
}

