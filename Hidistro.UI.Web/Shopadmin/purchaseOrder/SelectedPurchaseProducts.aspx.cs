using ASPNET.WebControls;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Hidistro.Entities.Sales;
namespace Hidistro.UI.Web.Shopadmin
{
    public partial class SelectedPurchaseProducts : DistributorPage
    {

        private void BindAddedData()
        {
            IList<PurchaseShoppingCartItemInfo> purchaseShoppingCartItemInfos = SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos();
            grdSelectedProducts.DataSource = purchaseShoppingCartItemInfos;
            grdSelectedProducts.DataBind();
        }

        private void grdSelectedProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem((string)grdSelectedProducts.DataKeys[e.RowIndex].Value))
            {
                BindAddedData();
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            grdSelectedProducts.RowDeleting += new GridViewDeleteEventHandler(grdSelectedProducts_RowDeleting);
            if (!IsPostBack)
            {
                BindAddedData();
            }
        }
    }
}

