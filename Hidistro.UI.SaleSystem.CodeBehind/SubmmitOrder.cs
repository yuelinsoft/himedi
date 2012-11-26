using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    /// <summary>
    /// 提交订单
    /// </summary>
    public class SubmmitOrder : HtmlTemplatedWebControl
    {
        #region 字段
        IButton btnCreateOrder;
        int buyAmount;
        string buytype = "";
        Common_SubmmintOrder_GiftList cartGiftList;
        Common_SubmmintOrder_ProductList cartProductList;
        HtmlSelect CmbCoupCode;
        //HtmlGenericControl divcopue;
        RegionSelector dropRegions;
        HtmlInputHidden hdbuytype;
        HyperLink hlkDiscountName;
        HyperLink hlkFeeFreeName;
        HtmlInputText htmlCouponCode;
        HtmlInputHidden inputPaymentModeId;
        HtmlInputHidden inputShippingModeId;
        bool isCountDown;
        bool isGroupBuy;
        bool isSignBuy;
        CartTotalPriceLabel lblCartTotalPrice;
        OrderOptionPriceLabel lblOrderOptionPrice;
        OrderTotalPriceLabel lblOrderTotal;
        PackingChargeFreeNameLabel lblPackingChargeFree;
        PaymentPriceLabel lblPaymentPrice;
        ServiceChargeFreeNameLabel lblServiceChargeFree;
        ShipChargeFeeNameLabel lblShipChargeFee;
        ShippingModePriceLabel lblShippModePrice;
        WeightLabel litAllWeight;
        CouponPriceLabel litCouponAmout;
        Literal litDiscountPrice;
        PointTotalLabel litPoint;
        Literal litProductAmount;
        Common_OrderOptionList orderOptionList;
        Common_PaymentModeList paymentModeList;
        string productSku;
        Common_ShippingAddressRadioButtonList radlAddress;
        Common_ShippingModeList shippingModeList;
        ShoppingCartInfo shoppingCart;
        TextBox txtAddress;
        TextBox txtCellPhone;
        TextBox txtMessage;
        TextBox txtShipTo;
        TextBox txtTelPhone;
        TextBox txtZipcode;
        #endregion

        protected override void AttachChildControls()
        {
            radlAddress = (Common_ShippingAddressRadioButtonList)FindControl("Common_ShippingAddressesRadioButtonList");
            dropRegions = (RegionSelector)FindControl("dropRegions");
            txtShipTo = (TextBox)FindControl("txtShipTo");
            txtAddress = (TextBox)FindControl("txtAddress");
            txtZipcode = (TextBox)FindControl("txtZipcode");
            txtCellPhone = (TextBox)FindControl("txtCellPhone");
            txtTelPhone = (TextBox)FindControl("txtTelPhone");
            orderOptionList = (Common_OrderOptionList)FindControl("Common_OrderOptions");
            shippingModeList = (Common_ShippingModeList)FindControl("Common_ShippingModeList");
            paymentModeList = (Common_PaymentModeList)FindControl("grd_Common_PaymentModeList");
            inputPaymentModeId = (HtmlInputHidden)FindControl("inputPaymentModeId");
            inputShippingModeId = (HtmlInputHidden)FindControl("inputShippingModeId");
            hdbuytype = (HtmlInputHidden)FindControl("hdbuytype");
            lblPaymentPrice = (PaymentPriceLabel)FindControl("lblPaymentPrice");
            lblShippModePrice = (ShippingModePriceLabel)FindControl("lblShippModePrice");
            cartProductList = (Common_SubmmintOrder_ProductList)FindControl("Common_SubmmintOrder_ProductList");
            cartGiftList = (Common_SubmmintOrder_GiftList)FindControl("Common_SubmmintOrder_GiftList");
            lblOrderOptionPrice = (OrderOptionPriceLabel)FindControl("lblOrderOptionPrice");
            litProductAmount = (Literal)FindControl("litProductAmount");
            litAllWeight = (WeightLabel)FindControl("litAllWeight");
            litPoint = (PointTotalLabel)FindControl("litPoint");
            lblOrderTotal = (OrderTotalPriceLabel)FindControl("lblOrderTotal");
            lblCartTotalPrice = (CartTotalPriceLabel)FindControl("lblCartTotalPrice");
            txtMessage = (TextBox)FindControl("txtMessage");
            hlkFeeFreeName = (HyperLink)FindControl("hlkFeeFreeName");
            lblServiceChargeFree = (ServiceChargeFreeNameLabel)FindControl("lblServiceChargeFree");
            lblShipChargeFee = (ShipChargeFeeNameLabel)FindControl("lblShipChargeFee");
            lblPackingChargeFree = (PackingChargeFreeNameLabel)FindControl("lblPackingChargeFree");
            hlkDiscountName = (HyperLink)FindControl("hlkDiscountName");
            litDiscountPrice = (Literal)FindControl("litDiscountPrice");
            htmlCouponCode = (HtmlInputText)FindControl("htmlCouponCode");
            CmbCoupCode = (HtmlSelect)FindControl("CmbCoupCode");
            litCouponAmout = (CouponPriceLabel)FindControl("litCouponAmout");
            btnCreateOrder = ButtonManager.Create(FindControl("btnCreateOrder"));
            btnCreateOrder.Click += new EventHandler(btnCreateOrder_Click);

            if (!Page.IsPostBack)
            {
                //绑定地址
                BindUserAddress();

                //绑定订单
                orderOptionList.DataSource = ShoppingProcessor.GetUsableOrderLookupLists();
                orderOptionList.DataBind();

                //绑定配送方式
                shippingModeList.DataSource = ShoppingProcessor.GetShippingModes();
                shippingModeList.DataBind();

                //绑定支付方式
                ReBindPayment();

                if (shoppingCart != null)
                {
                    BindPromote(shoppingCart);

                    BindShoppingCartInfo(shoppingCart);

                    CmbCoupCode.DataTextField = "DisplayText";
                    CmbCoupCode.DataValueField = "ClaimCode";
                    CmbCoupCode.DataSource = ShoppingProcessor.GetCoupon(shoppingCart.GetTotal());
                    CmbCoupCode.DataBind();

                    ListItem item = new ListItem("", "0");
                    CmbCoupCode.Items.Insert(0, item);

                    hdbuytype.Value = buytype;
                }

            }

        }

        void BindPromote(ShoppingCartInfo shoppingCart)
        {
            if (!string.IsNullOrEmpty(shoppingCart.DiscountName))
            {

                hlkDiscountName.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), shoppingCart.DiscountActivityId);

                hlkDiscountName.Text = shoppingCart.DiscountName;

                switch (shoppingCart.DiscountValueType)
                {
                    case DiscountValueType.Amount:
                        {
                            litDiscountPrice.Text = "-" + Globals.FormatMoney(shoppingCart.DiscountValue);
                            break;//增加的
                            //goto Label_00F0;
                        }
                    case DiscountValueType.Percent:
                        {
                            litDiscountPrice.Text = "-" + Globals.FormatMoney(shoppingCart.GetAmount() - ((shoppingCart.DiscountValue / 100M) * shoppingCart.GetAmount()));
                            break;//增加的
                            //goto Label_00F0;
                        }
                }
                
                //以下是从Label_00F0复制过来

                if (!string.IsNullOrEmpty(shoppingCart.FeeFreeName))
                {

                    hlkFeeFreeName.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), shoppingCart.FeeFreeActivityId);

                    hlkFeeFreeName.Text = shoppingCart.FeeFreeName;

                    if (shoppingCart.EightFree)
                    {
                        lblShipChargeFee.Text = "免运费 -";
                    }

                    if (shoppingCart.OrderOptionFree)
                    {
                        lblPackingChargeFree.Text = "免订单可选项费 -";
                    }

                    if (shoppingCart.ProcedureFeeFree)
                    {
                        lblServiceChargeFree.Text = "免支付手续费 -";
                        //return;
                    }

                }
                else
                {
                    hlkFeeFreeName.Text = "暂无";
                }


            }
            else
            {
                hlkDiscountName.Text = "暂无";
                litDiscountPrice.Text = string.Empty;
            }
        //Label_00F0:
        //    if (!string.IsNullOrEmpty(shoppingCart.FeeFreeName))
        //    {
        //        hlkFeeFreeName.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), shoppingCart.FeeFreeActivityId);
        //        hlkFeeFreeName.Text = shoppingCart.FeeFreeName;
        //        if (shoppingCart.EightFree)
        //        {
        //            lblShipChargeFee.Text = "免运费 -";
        //        }
        //        if (shoppingCart.OrderOptionFree)
        //        {
        //            lblPackingChargeFree.Text = "免订单可选项费 -";
        //        }
        //        if (shoppingCart.ProcedureFeeFree)
        //        {
        //            lblServiceChargeFree.Text = "免支付手续费 -";
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        hlkFeeFreeName.Text = "暂无";
        //    }
        }

        void BindShoppingCartInfo(ShoppingCartInfo shoppingCart)
        {
            if (shoppingCart.LineItems.Values.Count > 0)
            {
                cartProductList.DataSource = shoppingCart.LineItems.Values;
                cartProductList.DataBind();
                cartProductList.ShowProductCart();
            }
            if (shoppingCart.LineGifts.Count > 0)
            {
                cartGiftList.DataSource = shoppingCart.LineGifts;
                cartGiftList.DataBind();
                cartGiftList.ShowGiftCart();
            }
            litProductAmount.Text = Globals.FormatMoney(shoppingCart.GetAmount());
            lblCartTotalPrice.Value = new decimal?(shoppingCart.GetTotal());
            lblOrderTotal.Value = new decimal?(shoppingCart.GetTotal());
            litAllWeight.Text = shoppingCart.Weight.ToString();
            decimal num = 0M;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if ((shoppingCart.GetTotal() / masterSettings.PointsRate) > 2147483647M)
            {
                num = 2147483647M;
            }
            else if (masterSettings.PointsRate != 0M)
            {
                num = Math.Round((decimal)(shoppingCart.GetTotal() / masterSettings.PointsRate), 0);
            }
            litPoint.Text = num.ToString();
        }

        void BindUserAddress()
        {
            radlAddress.DataBind();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["shippingId"]))
            {
                int result = 0;
                int.TryParse(Page.Request.QueryString["shippingId"], out result);
                if (result > 0)
                {
                    radlAddress.SelectedValue = result;
                }
            }
            if (!HiContext.Current.User.IsAnonymous)
            {
                Member user = HiContext.Current.User as Member;
                txtShipTo.Text = user.RealName;
                dropRegions.SetSelectedRegionId(new int?(user.RegionId));
                dropRegions.DataBind();
                txtAddress.Text = user.Address;
                txtTelPhone.Text = user.TelPhone;
                txtCellPhone.Text = user.CellPhone;
            }
        }

        public void btnCreateOrder_Click(object sender, EventArgs e)
        {
            if (ValidateCreateOrder())
            {
                OrderInfo orderInfo = GetOrderInfo(shoppingCart);

                if (shoppingCart.Quantity > 1)
                {
                    isSignBuy = false;
                }

                if (orderInfo == null)
                {
                    ShowMessage("购物车中已经没有任何商品", false);
                }
                else if (!HiContext.Current.User.IsAnonymous && (shoppingCart.GetTotalNeedPoint() > ((Member)HiContext.Current.User).Points))
                {
                    ShowMessage("您当前的积分不够兑换所需积分！", false);
                }
                else if (isCountDown && (ProductBrowser.GetCountDownInfo(shoppingCart.LineItems[productSku].ProductId).EndDate < DateTime.Now))
                {
                    ShowMessage("此订单为抢购订单，但抢购时间已到！", false);
                }
                else
                {
                    try
                    {
                        if (ShoppingProcessor.CreatOrder(orderInfo))
                        {
                            Messenger.OrderCreated(orderInfo, HiContext.Current.User);
                            orderInfo.OnCreated();
                            if (shoppingCart.GetTotalNeedPoint() > 0)
                            {
                                ShoppingProcessor.CutNeedPoint(shoppingCart.GetTotalNeedPoint());
                            }
                            if ((!isCountDown && !isGroupBuy) && !isSignBuy)
                            {
                                ShoppingCartProcessor.ClearShoppingCart();
                            }
                            if (!HiContext.Current.User.IsAnonymous && (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) == 0))
                            {
                                ShippingAddressInfo shippingAddress = new ShippingAddressInfo();
                                shippingAddress.UserId = HiContext.Current.User.UserId;
                                shippingAddress.ShipTo = Globals.HtmlEncode(txtShipTo.Text);
                                shippingAddress.RegionId = dropRegions.GetSelectedRegionId().Value;
                                shippingAddress.Address = Globals.HtmlEncode(txtAddress.Text);
                                shippingAddress.Zipcode = txtZipcode.Text;
                                shippingAddress.CellPhone = txtCellPhone.Text;
                                shippingAddress.TelPhone = txtTelPhone.Text;
                                PersonalHelper.CreateShippingAddress(shippingAddress);
                            }
                            Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("FinishOrder", new object[] { orderInfo.OrderId }));
                        }
                    }
                    catch (Exception exception)
                    {
                        ShowMessage(exception.ToString(), false);
                    }
                }
            }
        }

        void FillOrderCoupon(OrderInfo orderInfo)
        {
            if ((!isGroupBuy && !isCountDown) && (!string.IsNullOrEmpty(htmlCouponCode.Value) && lblOrderTotal.Value.HasValue))
            {
                CouponInfo info = ShoppingProcessor.UseCoupon(lblOrderTotal.Value.Value, htmlCouponCode.Value);
                orderInfo.CouponName = info.Name;
                if (info.Amount.HasValue)
                {
                    orderInfo.CouponAmount = info.Amount.Value;
                }
                orderInfo.CouponCode = htmlCouponCode.Value;
                orderInfo.CouponValue = info.DiscountValue;
            }
        }

        void FillOrderOptions(OrderInfo orderInfo)
        {
            orderInfo.OrderOptions.Clear();
            IList<OrderLookupItemInfo> selectedOptions = orderOptionList.SelectedOptions;
            if ((selectedOptions != null) && (selectedOptions.Count > 0))
            {
                foreach (OrderLookupItemInfo info in selectedOptions)
                {
                    OrderOptionInfo item = new OrderOptionInfo();
                    item.AdjustedPrice = 0M;
                    if (info.AppendMoney.HasValue)
                    {
                        if (info.CalculateMode.Value == 1)
                        {
                            item.AdjustedPrice = info.AppendMoney.Value;
                        }
                        else
                        {
                            item.AdjustedPrice = orderInfo.GetDiscountedAmount() * (info.AppendMoney.Value / 100M);
                        }
                    }
                    item.CustomerDescription = info.UserInputContent;
                    item.CustomerTitle = info.UserInputTitle;
                    item.ItemDescription = info.Name;
                    OrderLookupListInfo orderLookupList = ShoppingProcessor.GetOrderLookupList(info.LookupListId);
                    if (orderLookupList != null)
                    {
                        item.ListDescription = orderLookupList.Name;
                    }
                    item.LookupItemId = info.LookupItemId;
                    item.LookupListId = info.LookupListId;
                    item.OrderId = orderInfo.OrderId;
                    orderInfo.OrderOptions.Add(item);
                }
            }
        }

        void FillOrderPaymentMode(OrderInfo orderInfo)
        {
            orderInfo.PaymentTypeId = int.Parse(inputPaymentModeId.Value);
            PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(orderInfo.PaymentTypeId);
            if (paymentMode != null)
            {
                orderInfo.PaymentType = Globals.HtmlEncode(paymentMode.Name);
                orderInfo.AdjustedPayCharge = orderInfo.PayCharge = ShoppingProcessor.CalcPayCharge(orderInfo.GetTotal(), paymentMode);
            }
        }

        void FillOrderShippingMode(OrderInfo orderInfo, ShoppingCartInfo shoppingCartInfo)
        {
            orderInfo.ShippingRegion = dropRegions.SelectedRegions;
            orderInfo.Address = Globals.HtmlEncode(txtAddress.Text);
            orderInfo.ZipCode = txtZipcode.Text;
            orderInfo.ShipTo = Globals.HtmlEncode(txtShipTo.Text);
            orderInfo.TelPhone = txtTelPhone.Text;
            orderInfo.CellPhone = txtCellPhone.Text;
            if (!string.IsNullOrEmpty(inputShippingModeId.Value))
            {
                orderInfo.ShippingModeId = int.Parse(inputShippingModeId.Value, NumberStyles.None);
            }
            if (dropRegions.GetSelectedRegionId().HasValue)
            {
                orderInfo.RegionId = dropRegions.GetSelectedRegionId().Value;
            }
            ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(orderInfo.ShippingModeId, true);
            if (shippingMode != null)
            {
                orderInfo.ModeName = shippingMode.Name;
                orderInfo.Freight = ShoppingProcessor.CalcFreight(orderInfo.RegionId, shoppingCartInfo.Weight, shippingMode);
                orderInfo.AdjustedFreight = orderInfo.Freight;
            }
        }

        string GenerateOrderId()
        {
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return (DateTime.Now.ToString("yyyyMMdd") + str);
        }

        OrderInfo GetOrderInfo(ShoppingCartInfo shoppingCartInfo)
        {
            OrderInfo orderInfo = ShoppingProcessor.ConvertShoppingCartToOrder(shoppingCartInfo, isGroupBuy, isCountDown, isSignBuy);
            if (orderInfo == null)
            {
                return null;
            }
            if (isGroupBuy)
            {
                GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(shoppingCartInfo.LineItems[productSku].ProductId);
                orderInfo.GroupBuyId = productGroupBuyInfo.GroupBuyId;
                orderInfo.NeedPrice = productGroupBuyInfo.NeedPrice;
            }
            orderInfo.OrderId = GenerateOrderId();
            orderInfo.OrderDate = DateTime.Now;
            IUser user = HiContext.Current.User;
            orderInfo.UserId = user.UserId;
            orderInfo.Username = user.Username;
            if (!user.IsAnonymous)
            {
                Member member = user as Member;
                orderInfo.EmailAddress = member.Email;
                orderInfo.RealName = member.RealName;
                orderInfo.QQ = member.QQ;
                orderInfo.Wangwang = member.Wangwang;
                orderInfo.MSN = member.MSN;
            }
            orderInfo.Remark = Globals.HtmlEncode(txtMessage.Text);
            orderInfo.OrderStatus = OrderStatus.WaitBuyerPay;
            orderInfo.RefundStatus = RefundStatus.None;
            FillOrderCoupon(orderInfo);
            FillOrderShippingMode(orderInfo, shoppingCartInfo);
            FillOrderOptions(orderInfo);
            FillOrderPaymentMode(orderInfo);
            return orderInfo;
        }

        protected override void OnInit(EventArgs e)
        {
            if ((int.TryParse(Page.Request.QueryString["buyAmount"], out buyAmount) && !string.IsNullOrEmpty(Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(Page.Request.QueryString["from"]) && (Page.Request.QueryString["from"] == "groupBuy")))
            {
                productSku = Page.Request.QueryString["productSku"];
                isGroupBuy = true;
                shoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(productSku, buyAmount);
                buytype = "GroupBuy";
            }
            else if ((int.TryParse(Page.Request.QueryString["buyAmount"], out buyAmount) && !string.IsNullOrEmpty(Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(Page.Request.QueryString["from"]) && (Page.Request.QueryString["from"] == "countDown")))
            {
                productSku = Page.Request.QueryString["productSku"];
                isCountDown = true;
                shoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(productSku, buyAmount);
                buytype = "CountDown";
            }
            else if ((int.TryParse(Page.Request.QueryString["buyAmount"], out buyAmount) && !string.IsNullOrEmpty(Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(Page.Request.QueryString["from"]) && (Page.Request.QueryString["from"] == "signBuy")))
            {
                productSku = Page.Request.QueryString["productSku"];
                isSignBuy = true;
                shoppingCart = ShoppingCartProcessor.GetShoppingCart(productSku, buyAmount);
            }
            else
            {
                shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                if ((shoppingCart != null) && (shoppingCart.Quantity == 0))
                {
                    buytype = "0";
                }
            }
            if (shoppingCart == null)
            {
                Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该件商品已经被管理员删除"));
            }
            else
            {
                if (SkinName == null)
                {
                    SkinName = "Skin-SubmmitOrder.html";
                }
                base.OnInit(e);
            }
        }

        void ReBindPayment()
        {
            IList<PaymentModeInfo> paymentModes = ShoppingProcessor.GetPaymentModes();
            IList<PaymentModeInfo> list2 = new List<PaymentModeInfo>();
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
            if ((cookie != null) && !string.IsNullOrEmpty(cookie.Value))
            {
                foreach (PaymentModeInfo info in paymentModes)
                {
                    if ((((string.Compare(info.Gateway, "hishop.plugins.payment.alipay_shortcut.shortcutrequest", true) == 0) || (string.Compare(info.Gateway, "hishop.plugins.payment.alipaydirect.directrequest", true) == 0)) || ((string.Compare(info.Gateway, "hishop.plugins.payment.alipayassure.assurerequest", true) == 0) || (string.Compare(info.Gateway, "hishop.plugins.payment.alipay.standardrequest", true) == 0))) || ((string.Compare(info.Gateway, "hishop.plugins.payment.advancerequest", true) != 0) || !HiContext.Current.User.IsAnonymous))
                    {
                        list2.Add(info);
                    }
                }
            }
            else
            {
                foreach (PaymentModeInfo info2 in paymentModes)
                {
                    if (string.Compare(info2.Gateway, "hishop.plugins.payment.alipay_shortcut.shortcutrequest", true) != 0)
                    {
                        list2.Add(info2);
                    }
                    if ((string.Compare(info2.Gateway, "hishop.plugins.payment.advancerequest", true) == 0) && HiContext.Current.User.IsAnonymous)
                    {
                        list2.Remove(info2);
                    }
                }
            }
            paymentModeList.DataSource = list2;
            paymentModeList.DataBind();
        }

        bool ValidateCreateOrder()
        {
            if (!dropRegions.GetSelectedRegionId().HasValue || (dropRegions.GetSelectedRegionId().Value == 0))
            {
                ShowMessage("请选择收货地址", false);
                return false;
            }
            string pattern = @"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*";
            Regex regex = new Regex(pattern);
            if (string.IsNullOrEmpty(txtShipTo.Text) || !regex.IsMatch(txtShipTo.Text.Trim()))
            {
                ShowMessage("请输入正确的收货人姓名", false);
                return false;
            }
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                ShowMessage("请输入收货人详细地址", false);
                return false;
            }
            if (string.IsNullOrEmpty(inputShippingModeId.Value))
            {
                ShowMessage("请选择配送方式", false);
                return false;
            }
            if (string.IsNullOrEmpty(inputPaymentModeId.Value))
            {
                ShowMessage("请选择支付方式", false);
                return false;
            }
            if (string.IsNullOrEmpty(txtTelPhone.Text.Trim()) && string.IsNullOrEmpty(txtCellPhone.Text.Trim()))
            {
                ShowMessage("电话号码和手机号码必填其一", false);
                return false;
            }
            return true;
        }
    }
}

