namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Shopping;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class ShoppingCart : HtmlTemplatedWebControl
    {
       Button btnCheckout;
       ImageLinkButton btnClearCart;
       Button btnShopping;
       Button btnSKU;
       HiddenField hfdIsLogin;
       HyperLink hlkDiscountName;
       FormatedMoneyLabel lblAmoutPrice;
       FormatedMoneyLabel lblTotalPrice;
       Literal litlDiscountPrice;
       Literal litNoProduct;
       Panel pnlShopCart;
       Common_ShoppingCart_GiftList shoppingCartGiftList;
       Common_ShoppingCart_ProductList shoppingCartProductList;
       TextBox txtSKU;

        protected override void AttachChildControls()
        {
            this.txtSKU = (TextBox) this.FindControl("txtSKU");
            this.btnSKU = (Button) this.FindControl("btnSKU");
            this.btnClearCart = (ImageLinkButton) this.FindControl("btnClearCart");
            this.shoppingCartProductList = (Common_ShoppingCart_ProductList) this.FindControl("Common_ShoppingCart_ProductList");
            this.shoppingCartGiftList = (Common_ShoppingCart_GiftList) this.FindControl("Common_ShoppingCart_GiftList");
            this.lblTotalPrice = (FormatedMoneyLabel) this.FindControl("lblTotalPrice");
            this.lblAmoutPrice = (FormatedMoneyLabel) this.FindControl("lblAmoutPrice");
            this.litlDiscountPrice = (Literal) this.FindControl("litlDiscountPrice");
            this.hlkDiscountName = (HyperLink) this.FindControl("hlkDiscountName");
            this.btnCheckout = (Button) this.FindControl("btnCheckout");
            this.btnShopping = (Button) this.FindControl("btnShopping");
            this.pnlShopCart = (Panel) this.FindControl("pnlShopCart");
            this.litNoProduct = (Literal) this.FindControl("litNoProduct");
            this.hfdIsLogin = (HiddenField) this.FindControl("hfdIsLogin");
            this.btnSKU.Click += new EventHandler(this.btnSKU_Click);
            this.btnClearCart.Click += new EventHandler(this.btnClearCart_Click);
            this.shoppingCartProductList.ItemCommand += new DataListCommandEventHandler(this.shoppingCartProductList_ItemCommand);
            this.shoppingCartGiftList.ItemCommand += new DataListCommandEventHandler(this.shoppingCartGiftList_ItemCommand);
            this.btnCheckout.Click += new EventHandler(this.btnCheckout_Click);
            this.btnShopping.Click += new EventHandler(this.btnShopping_Click);
            if (!HiContext.Current.SiteSettings.IsOpenSiteSale && !HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                this.btnSKU.Visible = false;
                this.btnCheckout.Visible = false;
                this.btnShopping.Visible = false;
            }
            if (!this.Page.IsPostBack)
            {
                this.BindShoppingCart();
            }
            if (!HiContext.Current.User.IsAnonymous)
            {
                this.hfdIsLogin.Value = "logined";
            }
        }

       void BindDiscountName(ShoppingCartInfo cartInfo)
        {
            if (!string.IsNullOrEmpty(cartInfo.DiscountName))
            {
                this.hlkDiscountName.NavigateUrl = string.Format(Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails"), cartInfo.DiscountActivityId);
                this.hlkDiscountName.Text = cartInfo.DiscountName;
                switch (cartInfo.DiscountValueType)
                {
                    case DiscountValueType.Amount:
                        this.litlDiscountPrice.Text = "-" + Globals.FormatMoney(cartInfo.DiscountValue);
                        return;

                    case DiscountValueType.Percent:
                        this.litlDiscountPrice.Text = "-" + Globals.FormatMoney(cartInfo.GetAmount() - ((cartInfo.DiscountValue / 100M) * cartInfo.GetAmount()));
                        return;
                }
            }
            else
            {
                this.litlDiscountPrice.Text = "-" + Globals.FormatMoney(0M);
                this.hlkDiscountName.Text = string.Empty;
            }
        }

       void BindShoppingCart()
        {
            ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
            if (shoppingCart == null)
            {
                this.pnlShopCart.Visible = false;
                this.litNoProduct.Visible = true;
                ShoppingCartProcessor.ClearShoppingCart();
            }
            else
            {
                this.pnlShopCart.Visible = true;
                this.litNoProduct.Visible = false;
                this.BindDiscountName(shoppingCart);
                if (shoppingCart.LineItems.Values.Count > 0)
                {
                    this.shoppingCartProductList.DataSource = shoppingCart.LineItems.Values;
                    this.shoppingCartProductList.DataBind();
                    this.shoppingCartProductList.ShowProductCart();
                }
                if (shoppingCart.LineGifts.Count > 0)
                {
                    this.shoppingCartGiftList.DataSource = shoppingCart.LineGifts;
                    this.shoppingCartGiftList.DataBind();
                    this.shoppingCartGiftList.ShowGiftCart();
                }
                this.lblAmoutPrice.Money = shoppingCart.GetAmount();
                this.lblTotalPrice.Money = shoppingCart.GetTotal();
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            HiContext.Current.Context.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
        }

        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            ShoppingCartProcessor.ClearShoppingCart();
            this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
        }

        protected void btnShopping_Click(object sender, EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "CloseWindow", "<script>var ref = window.open(\"about:blank\", \"_self\");ref.close();</script>", false);
        }

        protected void btnSKU_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSKU.Text.Trim()))
            {
                this.ShowMessage("请输入货号", false);
            }
            else
            {
                IList<string> skuIdsBysku = ShoppingProcessor.GetSkuIdsBysku(this.txtSKU.Text.Trim());
                if ((skuIdsBysku == null) || (skuIdsBysku.Count == 0))
                {
                    this.ShowMessage("货号无效，请确认后重试", false);
                }
                else
                {
                    foreach (string str in skuIdsBysku)
                    {
                        DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(str);
                        if ((productInfoBySku != null) && (productInfoBySku.Rows.Count > 0))
                        {
                            string skuContent = string.Empty;
                            foreach (DataRow row in productInfoBySku.Rows)
                            {
                                if (!string.IsNullOrEmpty(row["AttributeName"].ToString()) && !string.IsNullOrEmpty(row["ValueStr"].ToString()))
                                {
                                    object obj2 = skuContent;
                                    skuContent = string.Concat(new object[] { obj2, row["AttributeName"], "：", row["ValueStr"], "; " });
                                }
                            }
                            ShoppingCartProcessor.AddLineItem((int) productInfoBySku.Rows[0]["ProductId"], str, skuContent, 1);
                        }
                    }
                    this.BindShoppingCart();
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ShoppingCart.html";
            }
            base.OnInit(e);
        }

        protected void shoppingCartGiftList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int num;
            Control control = e.Item.Controls[0];
            TextBox box = (TextBox) control.FindControl("txtBuyNum");
            Literal literal = (Literal) control.FindControl("litGiftId");
            if (!int.TryParse(box.Text, out num) || (box.Text.IndexOf(".") != -1))
            {
                this.ShowMessage("兑换数量必须为整数", false);
            }
            else if (num <= 0)
            {
                this.ShowMessage("兑换数量必须为大于0的整数", false);
            }
            else
            {
                if (e.CommandName == "updateBuyNum")
                {
                    ShoppingCartProcessor.UpdateGiftItemQuantity(Convert.ToInt32(literal.Text), num);
                }
                if (e.CommandName == "delete")
                {
                    ShoppingCartProcessor.RemoveGiftItem(Convert.ToInt32(literal.Text));
                }
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
            }
        }

        protected void shoppingCartProductList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int num;
            Control control = e.Item.Controls[0];
            TextBox box = (TextBox) control.FindControl("txtBuyNum");
            Literal literal = (Literal) control.FindControl("litProductId");
            Literal literal2 = (Literal) control.FindControl("litSkuId");
            if (!int.TryParse(box.Text, out num) || (box.Text.IndexOf(".") != -1))
            {
                this.ShowMessage("购买数量必须为整数", false);
            }
            else if (num <= 0)
            {
                this.ShowMessage("购买数量必须为大于0的整数", false);
            }
            else
            {
                if (e.CommandName == "updateBuyNum")
                {
                    if (ShoppingCartProcessor.GetSkuStock(literal2.Text.Trim()) < num)
                    {
                        this.ShowMessage("该商品库存不够", false);
                        return;
                    }
                    ShoppingCartProcessor.UpdateLineItemQuantity(Convert.ToInt32(literal.Text), literal2.Text, num);
                }
                if (e.CommandName == "delete")
                {
                    ShoppingCartProcessor.RemoveLineItem(literal2.Text);
                }
                this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("shoppingCart"), true);
            }
        }
    }
}

