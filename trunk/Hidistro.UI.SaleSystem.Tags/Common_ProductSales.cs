using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{

    public class Common_ProductSales : AscxTemplatedWebControl
    {

        public int maxNum = 6;

        Repeater rp_productsales;


        protected override void AttachChildControls()
        {
            rp_productsales = (Repeater)this.FindControl("rp_productsales");

            DataTable lineItems = ProductBrowser.GetLineItems(Convert.ToInt32(this.Page.Request.QueryString["productId"]), this.maxNum);

            rp_productsales.DataSource = lineItems;
            rp_productsales.DataBind();

        }

        protected override void OnInit(EventArgs e)
        {
            if (SkinName == null)
            {
                SkinName = "/ascx/tags/Common_ViewProduct/Skin-Common_ProductSales.ascx";
            }
            base.OnInit(e);
        }

        public int MaxNum
        {
            get
            {
                return maxNum;
            }
            set
            {
                maxNum = value;
            }
        }

    }

}

