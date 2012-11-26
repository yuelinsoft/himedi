using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.sales
{
    public partial class EditShippingTemplate : AdminPage
    {

        int templateId;


        private void BindControl(ShippingModeInfo modeItem)
        {
            txtModeName.Text = Globals.HtmlDecode(modeItem.Name);
            txtWeight.Text = modeItem.Weight.ToString();
            txtAddWeight.Text = modeItem.AddWeight.ToString();
            if (modeItem.AddPrice.HasValue)
            {
                txtAddPrice.Text = modeItem.AddPrice.Value.ToString("F2");
            }
            txtPrice.Text = modeItem.Price.ToString("F2");
            RegionList.Clear();
            if ((modeItem.ModeGroup != null) && (modeItem.ModeGroup.Count > 0))
            {
                foreach (ShippingModeGroupInfo info in modeItem.ModeGroup)
                {
                    Region region2 = new Region();
                    region2.RegionPrice = decimal.Parse(info.Price.ToString("F2"));
                    region2.RegionAddPrice = decimal.Parse(info.AddPrice.ToString("F2"));
                    Region item = region2;
                    StringBuilder builder = new StringBuilder();
                    StringBuilder builder2 = new StringBuilder();
                    foreach (ShippingRegionInfo info2 in info.ModeRegions)
                    {
                        builder.Append(info2.RegionId + ",");
                        builder2.Append(RegionHelper.GetFullRegion(info2.RegionId, ",") + ",");
                    }
                    if (!string.IsNullOrEmpty(builder.ToString()))
                    {
                        item.RegionsId = builder.ToString().Substring(0, builder.ToString().Length - 1);
                    }
                    if (!string.IsNullOrEmpty(builder2.ToString()))
                    {
                        item.Regions = builder2.ToString().Substring(0, builder2.ToString().Length - 1);
                    }
                    RegionList.Add(item);
                }
            }
        }

        private void BindRegion()
        {
            grdRegion.DataSource = RegionList;
            grdRegion.DataBind();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            decimal num;
            decimal num2;
            if (ValidateValues(out num, out num2))
            {
                Region region2 = new Region();
                region2.RegionsId = txtRegion_Id.Text;
                region2.Regions = txtRegion.Value;
                region2.RegionPrice = num;
                region2.RegionAddPrice = num2;
                Region item = region2;
                RegionList.Add(item);
                BindRegion();
                txtRegion_Id.Text = string.Empty;
                txtRegion.Value = string.Empty;
                txtRegionPrice.Text = "0";
                txtAddRegionPrice.Text = "0";
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int num;
            int? nullable;
            decimal num2;
            decimal? nullable2;
            if (ValidateRegionValues(out num, out nullable, out num2, out nullable2))
            {
                new List<ShippingModeGroupInfo>();
                ShippingModeInfo info6 = new ShippingModeInfo();
                info6.Name = Globals.HtmlEncode(txtModeName.Text.Trim());
                info6.Weight = num;
                info6.AddWeight = nullable;
                info6.Price = num2;
                info6.AddPrice = nullable2;
                info6.TemplateId = templateId;
                ShippingModeInfo target = info6;
                foreach (GridViewRow row in grdRegion.Rows)
                {
                    ShippingModeGroupInfo info5 = new ShippingModeGroupInfo();
                    info5.Price = decimal.Parse(((TextBox)row.FindControl("txtModeRegionPrice")).Text);
                    info5.AddPrice = decimal.Parse(((TextBox)row.FindControl("txtModeRegionAddPrice")).Text);
                    ShippingModeGroupInfo item = info5;
                    TextBox box = (TextBox)grdRegion.Rows[row.RowIndex].FindControl("txtRegionvalue_Id");
                    foreach (string str in box.Text.Split(new char[] { ',' }))
                    {
                        ShippingRegionInfo info4 = new ShippingRegionInfo();
                        info4.RegionId = Convert.ToInt32(str.Trim());
                        ShippingRegionInfo info3 = info4;
                        item.ModeRegions.Add(info3);
                    }
                    target.ModeGroup.Add(item);
                }
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<ShippingModeInfo>(target, new string[] { "ValShippingModeInfo" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    ShowMsg(msg, false);
                }
                else if (SalesHelper.UpdateShippingTemplate(target))
                {
                    Page.Response.Redirect("EditShippingTemplate.aspx?TemplateId=" + target.TemplateId + "&isUpdate=true");
                }
                else
                {
                    ShowMsg("您添加的地区有重复", false);
                }
            }
        }

        private void grdRegion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            RegionList.RemoveAt(e.RowIndex);
            BindRegion();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["TemplateId"], out templateId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnUpdate.Click += new EventHandler(btnUpdate_Click);
                btnAdd.Click += new EventHandler(btnAdd_Click);
                grdRegion.RowDeleting += new GridViewDeleteEventHandler(grdRegion_RowDeleting);
                if (!Page.IsPostBack)
                {
                    if ((Page.Request.QueryString["isUpdate"] != null) && (Page.Request.QueryString["isUpdate"] == "true"))
                    {
                        ShowMsg("成功修改了一个配送方式", true);
                    }
                    ShippingModeInfo shippingTemplate = SalesHelper.GetShippingTemplate(templateId, true);
                    if (shippingTemplate == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        BindControl(shippingTemplate);
                        BindRegion();
                    }
                }
            }
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
                str = str + Formatter.FormatErrorMessage("默认起步价不能为空,必须为非负数字,限制在1000万以内");
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
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
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

