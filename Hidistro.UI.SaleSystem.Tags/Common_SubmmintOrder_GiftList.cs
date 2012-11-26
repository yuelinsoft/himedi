namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;

    public class Common_SubmmintOrder_GiftList : AscxTemplatedWebControl
    {
       DataList dataListShoppingCrat;
       Panel pnlShopGiftCart;
        public const string TagID = "Common_SubmmintOrder_GiftList";

        public Common_SubmmintOrder_GiftList()
        {
            base.ID = "Common_SubmmintOrder_GiftList";
        }

        protected override void AttachChildControls()
        {
            this.dataListShoppingCrat = (DataList) this.FindControl("dataListShoppingCrat");
            this.pnlShopGiftCart = (Panel) this.FindControl("pnlShopGiftCart");
            this.pnlShopGiftCart.Visible = false;
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListShoppingCrat.DataSource != null)
            {
                this.dataListShoppingCrat.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_SubmmintOrder/Skin-Common_SubmmintOrder_GiftList.ascx";
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

