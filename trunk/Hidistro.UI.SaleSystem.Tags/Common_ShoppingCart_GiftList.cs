namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_ShoppingCart_GiftList : AscxTemplatedWebControl
    {
       DataList dataListGiftShoppingCrat;
       Panel pnlShopGiftCart;
        public const string TagID = "Common_ShoppingCart_GiftList";

        public event DataListCommandEventHandler ItemCommand;

        public Common_ShoppingCart_GiftList()
        {
            base.ID = "Common_ShoppingCart_GiftList";
        }

        protected override void AttachChildControls()
        {
            this.dataListGiftShoppingCrat = (DataList) this.FindControl("dataListGiftShoppingCrat");
            this.pnlShopGiftCart = (Panel) this.FindControl("pnlShopGiftCart");
            this.pnlShopGiftCart.Visible = false;
            this.dataListGiftShoppingCrat.ItemCommand += new DataListCommandEventHandler(this.dataListGiftShoppingCrat_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListGiftShoppingCrat.DataSource != null)
            {
                this.dataListGiftShoppingCrat.DataBind();
            }
        }

       void dataListGiftShoppingCrat_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_GiftList.ascx";
            }
            base.OnInit(e);
        }

        public void ShowGiftCart()
        {
            if (this.DataSource == null)
            {
                this.pnlShopGiftCart.Visible = false;
            }
            else
            {
                this.pnlShopGiftCart.Visible = true;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListGiftShoppingCrat.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListGiftShoppingCrat.DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }
    }
}

