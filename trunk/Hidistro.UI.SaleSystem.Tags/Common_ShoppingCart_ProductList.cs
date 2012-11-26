namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_ShoppingCart_ProductList : AscxTemplatedWebControl
    {
       DataList dataListShoppingCrat;
       Panel pnlShopProductCart;
        public const string TagID = "Common_ShoppingCart_ProductList";

        public event DataListCommandEventHandler ItemCommand;

        public Common_ShoppingCart_ProductList()
        {
            base.ID = "Common_ShoppingCart_ProductList";
        }

        protected override void AttachChildControls()
        {
            this.dataListShoppingCrat = (DataList) this.FindControl("dataListShoppingCrat");
            this.pnlShopProductCart = (Panel) this.FindControl("pnlShopProductCart");
            this.pnlShopProductCart.Visible = false;
            this.dataListShoppingCrat.ItemCommand += new DataListCommandEventHandler(this.dataListShoppingCrat_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListShoppingCrat.DataSource != null)
            {
                this.dataListShoppingCrat.DataBind();
            }
        }

       void dataListShoppingCrat_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ShoppingCart/Skin-Common_ShoppingCart_ProductList.ascx";
            }
            base.OnInit(e);
        }

        public void ShowProductCart()
        {
            if (this.DataSource == null)
            {
                this.pnlShopProductCart.Visible = false;
            }
            else
            {
                this.pnlShopProductCart.Visible = true;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListShoppingCrat.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListShoppingCrat.DataSource = value;
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

