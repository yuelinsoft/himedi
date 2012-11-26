using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class SubmitPurchaseOrder : DistributorPage
    {


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateCreateOrder())
            {
                PurchaseOrderInfo purchaseOrderInfo = GetPurchaseOrderInfo();
                if (purchaseOrderInfo.PurchaseOrderItems.Count == 0)
                {
                    ShowMsg("您暂时未选择您要添加的商品", false);
                }
                else if (SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
                {
                    SubsiteSalesHelper.ClearPurchaseShoppingCart();
                    Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/Pay.aspx?PurchaseOrderId=" + purchaseOrderInfo.PurchaseOrderId);
                }
                else
                {
                    ShowMsg("提交采购单失败", false);
                }
            }
        }

        private string GeneratePurchaseOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num2 = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num2 % 10)))).ToString();
            }
            return ("MPO" + DateTime.Now.ToString("yyyyMMdd") + str);
        }

        private PurchaseOrderInfo GetPurchaseOrderInfo()
        {
            PurchaseOrderInfo info = new PurchaseOrderInfo();
            Distributor user = Users.GetUser(HiContext.Current.User.UserId) as Distributor;
            string orderId = GeneratePurchaseOrderId();
            info.PurchaseOrderId = orderId;
            IList<PurchaseShoppingCartItemInfo> purchaseShoppingCartItemInfos = SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos();
            int totalWeight = 0;
            if (purchaseShoppingCartItemInfos.Count >= 1)
            {
                PurchaseOrderItemInfo item = null;
                foreach (PurchaseShoppingCartItemInfo info2 in purchaseShoppingCartItemInfos)
                {
                    item = new PurchaseOrderItemInfo();
                    item.PurchaseOrderId = orderId;
                    item.SkuId = info2.SkuId;
                    item.ThumbnailsUrl = info2.ThumbnailsUrl;
                    item.SKUContent = info2.SKUContent;
                    item.SKU = info2.SKU;
                    item.Quantity = info2.Quantity;
                    item.ProductId = info2.ProductId;
                    item.ItemWeight = info2.ItemWeight;
                    item.ItemCostPrice = info2.CostPrice;
                    item.ItemPurchasePrice = info2.ItemPurchasePrice;
                    item.ItemListPrice = info2.ItemListPrice;
                    item.ItemDescription = info2.ItemDescription;
                    item.ItemHomeSiteDescription = info2.ItemDescription;
                    totalWeight += info2.ItemWeight * info2.Quantity;
                    info.PurchaseOrderItems.Add(item);
                }
                ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(radioShippingMode.SelectedValue.Value, true);
                info.ShipTo = txtShipTo.Text.Trim();
                if (rsddlRegion.GetSelectedRegionId().HasValue)
                {
                    info.RegionId = rsddlRegion.GetSelectedRegionId().Value;
                }
                info.Address = Globals.HtmlEncode(txtAddress.Value.Trim());
                info.TelPhone = txtTel.Text.Trim();
                info.ZipCode = txtZipcode.Text.Trim();
                info.CellPhone = txtMobile.Text.Trim();
                info.OrderId = null;
                info.RealShippingModeId = radioShippingMode.SelectedValue.Value;
                info.RealModeName = shippingMode.Name;
                info.ShippingModeId = radioShippingMode.SelectedValue.Value;
                info.ModeName = shippingMode.Name;
                info.AdjustedFreight = SubsiteSalesHelper.CalcFreight(info.RegionId, totalWeight, shippingMode);
                info.Freight = info.AdjustedFreight;
                info.ShippingRegion = rsddlRegion.SelectedRegions;
                info.Remark = Globals.HtmlEncode(txtRemark.Text.Trim());
                info.PurchaseStatus = OrderStatus.WaitBuyerPay;
                info.DistributorId = user.UserId;
                info.Distributorname = user.Username;
                info.DistributorEmail = user.Email;
                info.DistributorRealName = user.RealName;
                info.DistributorQQ = user.QQ;
                info.DistributorWangwang = user.Wangwang;
                info.DistributorMSN = user.MSN;
                info.RefundStatus = RefundStatus.None;
                info.Weight = totalWeight;
                info.ExpressCompanyAbb = shippingMode.ExpressCompanyAbb;
                info.ExpressCompanyName = shippingMode.ExpressCompanyName;
            }
            return info;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            if (!base.IsPostBack)
            {
                radioShippingMode.DataBind();
            }
        }

        private bool ValidateCreateOrder()
        {
            string str = string.Empty;
            if (!(rsddlRegion.GetSelectedRegionId().HasValue && (rsddlRegion.GetSelectedRegionId().Value != 0)))
            {
                str = str + Formatter.FormatErrorMessage("请选择收货地址");
            }
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if (!(!string.IsNullOrEmpty(txtShipTo.Text) && regex.IsMatch(txtShipTo.Text.Trim())))
            {
                str = str + Formatter.FormatErrorMessage("请输入正确的收货人姓名");
            }
            if (!(string.IsNullOrEmpty(txtShipTo.Text) || ((txtShipTo.Text.Trim().Length >= 2) && (txtShipTo.Text.Trim().Length <= 20))))
            {
                str = str + Formatter.FormatErrorMessage("收货人姓名的长度限制在2-20个字符");
            }
            if (string.IsNullOrEmpty(txtAddress.Value.Trim()) || (txtAddress.Value.Trim().Length > 100))
            {
                str = str + Formatter.FormatErrorMessage("请输入收货人详细地址,在100个字符以内");
            }
            regex = new Regex("^[0-9]*$");
            if (!(string.IsNullOrEmpty(txtMobile.Text.Trim()) || ((regex.IsMatch(txtMobile.Text.Trim()) && (txtMobile.Text.Trim().Length <= 20)) && (txtMobile.Text.Trim().Length >= 3))))
            {
                str = str + Formatter.FormatErrorMessage("手机号码长度限制在3-20个字符之间,只能输入数字");
            }
            regex = new Regex("^[0-9-]*$");
            if (!(string.IsNullOrEmpty(txtTel.Text.Trim()) || ((regex.IsMatch(txtTel.Text.Trim()) && (txtTel.Text.Trim().Length <= 20)) && (txtTel.Text.Trim().Length >= 3))))
            {
                str = str + Formatter.FormatErrorMessage("电话号码长度限制在3-20个字符之间,只能输入数字和字符“-”");
            }
            if (!radioShippingMode.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择配送方式");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

