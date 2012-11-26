namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Shopping;
    using System;
    using System.Collections.Generic;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Private_OrderOptionItems : CompositeControl
    {
       const int CALCULATEMODE_PERCENTAGE = 2;
       ListControl listItems;
        public const string TagID = "list_Private_OrderOptionItems";

        public Private_OrderOptionItems()
        {
            base.ID = "list_Private_OrderOptionItems";
        }

       decimal CalculateOrderOptionPrice(OrderLookupItemInfo item)
        {
            if (item.CalculateMode.Value == 2)
            {
                return ((this.GetCartTotalPrice() * decimal.Parse(item.AppendMoney.ToString())) / 100M);
            }
            return decimal.Parse(item.AppendMoney.ToString());
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            IList<OrderLookupItemInfo> orderLookupItems = ShoppingProcessor.GetOrderLookupItems(this.LookupListId);
            if ((orderLookupItems != null) && (orderLookupItems.Count != 0))
            {
                if (this.SelectMode == SelectModeTypes.DropDownList)
                {
                    this.listItems = new DropDownList();
                    this.listItems.Items.Add(new ListItem(string.Empty, string.Empty));
                    this.listItems.Attributes.Add("onchange", "$.myfn.OrderOptionSelectForDropDownList(this);");
                }
                else
                {
                    this.listItems = new RadioButtonList();
                }
                foreach (OrderLookupItemInfo info in orderLookupItems)
                {
                    string str;
                    if (info.AppendMoney.HasValue)
                    {
                        decimal money = this.CalculateOrderOptionPrice(info);
                        str = this.Page.Server.HtmlDecode(info.Name) + "(" + Globals.FormatMoney(money).ToString() + ")";
                    }
                    else
                    {
                        str = this.Page.Server.HtmlDecode(info.Name);
                    }
                    this.listItems.Items.Add(new ListItem(Globals.HtmlDecode(str), info.LookupItemId.ToString()));
                }
                this.Controls.Add(this.listItems);
            }
        }

       decimal GetCartTotalPrice()
        {
            ShoppingCartInfo groupBuyShoppingCart;
            int num;
            string str;
            if ((int.TryParse(this.Page.Request.QueryString["buyAmount"], out num) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "groupBuy")))
            {
                str = this.Page.Request.QueryString["productSku"];
                groupBuyShoppingCart = ShoppingCartProcessor.GetGroupBuyShoppingCart(str, num);
            }
            else if ((int.TryParse(this.Page.Request.QueryString["buyAmount"], out num) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "countDown")))
            {
                str = this.Page.Request.QueryString["productSku"];
                groupBuyShoppingCart = ShoppingCartProcessor.GetCountDownShoppingCart(str, num);
            }
            else if ((int.TryParse(this.Page.Request.QueryString["buyAmount"], out num) && !string.IsNullOrEmpty(this.Page.Request.QueryString["productSku"])) && (!string.IsNullOrEmpty(this.Page.Request.QueryString["from"]) && (this.Page.Request.QueryString["from"] == "signBuy")))
            {
                str = this.Page.Request.QueryString["productSku"];
                groupBuyShoppingCart = ShoppingCartProcessor.GetShoppingCart(str, num);
            }
            else
            {
                groupBuyShoppingCart = ShoppingCartProcessor.GetShoppingCart();
            }
            decimal total = 0M;
            if (groupBuyShoppingCart != null)
            {
                total = groupBuyShoppingCart.GetTotal();
            }
            return total;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.HasControls())
            {
                foreach (Control control in this.Controls)
                {
                    control.RenderControl(writer);
                }
            }
        }

        public int LookupListId
        {
            get
            {
                if (this.ViewState["LookupListId"] == null)
                {
                    return 0;
                }
                return (int) this.ViewState["LookupListId"];
            }
            set
            {
                this.ViewState["LookupListId"] = value;
            }
        }

        public OrderLookupItemInfo SelectedItem
        {
            get
            {
                if (((this.listItems != null) && (this.listItems.Items.Count != 0)) && ((this.listItems.SelectedItem != null) && !string.IsNullOrEmpty(this.listItems.SelectedValue)))
                {
                    return ShoppingProcessor.GetOrderLookupItem(int.Parse(this.listItems.SelectedItem.Value), string.Empty);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.EnsureChildControls();
                    if (this.listItems.Items.FindByValue(value.LookupItemId.ToString()) != null)
                    {
                        this.listItems.Items.FindByValue(value.LookupItemId.ToString()).Selected = true;
                    }
                }
            }
        }

        public SelectModeTypes SelectMode
        {
            get
            {
                if (this.ViewState["SelectMode"] == null)
                {
                    return SelectModeTypes.DropDownList;
                }
                return (SelectModeTypes) this.ViewState["SelectMode"];
            }
            set
            {
                this.ViewState["SelectMode"] = value;
            }
        }
    }
}

