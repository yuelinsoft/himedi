using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    public partial class ChangePurchaseOrderItems : DistributorPage
    {

        string isAdd = string.Empty;
        string productCode;
        int? productLineId;
        string productName;
        string purchaseOrderId = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdOrderItems.RowCommand += new GridViewCommandEventHandler(grdOrderItems_RowCommand);
            grdAuthorizeProducts.RowDataBound += new GridViewRowEventHandler(grdAuthorizeProducts_RowDataBound);
            if (!Page.IsPostBack)
            {
                ddlProductLine.DataBind();
                ddlProductLine.SelectedValue = productLineId;
                BindOrderItems();
                if (!string.IsNullOrEmpty(isAdd))
                {
                    BindData();
                }
            }
        }

        private void BindData()
        {
            ProductQuery entity = new ProductQuery();
            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.ProductCode = productCode;
            entity.Keywords = productName;
            entity.ProductLineId = productLineId;
            if (grdAuthorizeProducts.SortOrder.ToLower() == "desc")
            {
                entity.SortOrder = SortAction.Desc;
            }
            entity.SortBy = grdAuthorizeProducts.SortOrderBy;
            Globals.EntityCoding(entity, true);
            DbQueryResult submitPuchaseProducts = SubSiteProducthelper.GetSubmitPuchaseProducts(entity);
            grdAuthorizeProducts.DataSource = submitPuchaseProducts.Data;
            grdAuthorizeProducts.DataBind();
            pager.TotalRecords = submitPuchaseProducts.TotalRecords;
            pager1.TotalRecords = submitPuchaseProducts.TotalRecords;
        }

        private void BindOrderItems()
        {
            PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
            if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
            {
                pnlEmpty.Visible = true;
                pnlHasStatus.Visible = false;
            }
            else
            {
                pnlHasStatus.Visible = true;
                pnlEmpty.Visible = false;
                if ((purchaseOrder != null) && (purchaseOrder.PurchaseOrderItems.Count > 0))
                {
                    grdOrderItems.DataSource = purchaseOrder.PurchaseOrderItems;
                    grdOrderItems.DataBind();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBindData(true, false);
        }

        private void grdAuthorizeProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataTable skusByProductId = SubSiteProducthelper.GetSkusByProductId(Convert.ToInt32(grdAuthorizeProducts.DataKeys[e.Row.RowIndex].Value));
                Grid grid = (Grid)e.Row.FindControl("grdSkus");
                if (grid != null)
                {
                    grid.DataSource = skusByProductId;
                    grid.DataBind();
                }
            }
        }

        private void grdOrderItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string skuId = e.CommandArgument.ToString();
            if (e.CommandName == "UPDATE_ITEMS")
            {
                if (SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId).PurchaseOrderItems.Count <= 1)
                {
                    ShowMsg("采购单的最后一件商品不允许删除", false);
                    return;
                }
                SubsiteSalesHelper.DeletePurchaseOrderItem(purchaseOrderId, skuId);
            }
            else if (e.CommandName == "UPDATE_QUANTITY")
            {
                int num;
                Grid grid = (Grid)sender;
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                TextBox box = (TextBox)grdOrderItems.Rows[rowIndex].Cells[3].FindControl("txtItemNumber");
                if (!int.TryParse(box.Text.Trim(), out num))
                {
                    ShowMsg("商品数量填写错误", false);
                    return;
                }
                int skuStock = SubsiteSalesHelper.GetSkuStock(skuId);
                if (num > skuStock)
                {
                    ShowMsg("此商品库存不够", false);
                    return;
                }
                if (num <= 0)
                {
                    ShowMsg("商品购买数量不能小于等于0", false);
                    return;
                }
                SubsiteSalesHelper.UpdatePurchaseOrderItemQuantity(purchaseOrderId, skuId, num);
            }
            UpdatePurchaseOrder();
            BindOrderItems();
            ShowMsg("商品数量更新成功！", true);
            pager1.TotalRecords = pager.TotalRecords = 0;
            grdAuthorizeProducts.DataSource = null;
            grdAuthorizeProducts.DataBind();
        }

        public void grdSkus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int num;
            Grid grid = (Grid)sender;
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            string skuId = (string)grid.DataKeys[rowIndex].Value;
            TextBox box = (TextBox)grid.Rows[rowIndex].Cells[1].FindControl("txtNum");
            LinkButton button = (LinkButton)grid.Rows[rowIndex].Cells[2].FindControl("lbtnAdd");
            if ((!int.TryParse(box.Text.Trim(), out num) || (int.Parse(box.Text.Trim()) <= 0)) || box.Text.Trim().Contains("."))
            {
                ShowMsg("数量不能为空,必需为大于零的正整数", false);
            }
            else if ((e.CommandName == "add") && (button.Text == "添加"))
            {
                PurchaseShoppingCartItemInfo item = new PurchaseShoppingCartItemInfo();
                DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(skuId);
                if (num > ((int)skuContentBySku.Rows[0]["Stock"]))
                {
                    ShowMsg("商品库存不够", false);
                }
                else
                {
                    foreach (DataRow row in skuContentBySku.Rows)
                    {
                        if (!(string.IsNullOrEmpty(row["AttributeName"].ToString()) || string.IsNullOrEmpty(row["ValueStr"].ToString())))
                        {
                            object sKUContent = item.SKUContent;
                            item.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                        }
                    }
                    item.SkuId = skuId;
                    item.ProductId = (int)skuContentBySku.Rows[0]["ProductId"];
                    if (skuContentBySku.Rows[0]["SKU"] != DBNull.Value)
                    {
                        item.SKU = (string)skuContentBySku.Rows[0]["SKU"];
                    }
                    if (skuContentBySku.Rows[0]["Weight"] != DBNull.Value)
                    {
                        item.ItemWeight = (int)skuContentBySku.Rows[0]["Weight"];
                    }
                    item.ItemPurchasePrice = (decimal)skuContentBySku.Rows[0]["PurchasePrice"];
                    item.Quantity = num;
                    item.ItemListPrice = (decimal)skuContentBySku.Rows[0]["SalePrice"];
                    item.ItemDescription = (string)skuContentBySku.Rows[0]["ProductName"];
                    if (skuContentBySku.Rows[0]["CostPrice"] != DBNull.Value)
                    {
                        item.CostPrice = (decimal)skuContentBySku.Rows[0]["CostPrice"];
                    }
                    if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
                    {
                        item.ThumbnailsUrl = (string)skuContentBySku.Rows[0]["ThumbnailUrl40"];
                    }
                    if (SubsiteSalesHelper.AddPurchaseOrderItem(item, purchaseOrderId))
                    {
                        UpdatePurchaseOrder();
                        BindOrderItems();
                        ReBindData(true, false);
                    }
                    else
                    {
                        ShowMsg("添加商品失败", false);
                    }
                }
            }
        }

        private void LoadParameters()
        {
            purchaseOrderId = Page.Request.QueryString["PurchaseOrderId"];
            isAdd = Page.Request.QueryString["isAdd"];
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
                {
                    productCode = base.Server.UrlDecode(Page.Request.QueryString["productCode"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
                {
                    productName = base.Server.UrlDecode(Page.Request.QueryString["productName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productLineId"]))
                {
                    productLineId = new int?(int.Parse(Page.Request.QueryString["productLineId"], NumberStyles.None));
                }
                txtProductCode.Text = productCode;
                txtProductName.Text = productName;
            }
            else
            {
                productCode = txtProductCode.Text;
                productName = txtProductName.Text;
                productLineId = ddlProductLine.SelectedValue;
            }
        }



        private void ReBindData(bool isSearch, bool reBindByGet)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(txtProductCode.Text))
            {
                queryStrings.Add("productCode", txtProductCode.Text);
            }
            if (!string.IsNullOrEmpty(txtProductName.Text))
            {
                queryStrings.Add("productName", txtProductName.Text);
            }
            queryStrings.Add("productLineId", ddlProductLine.SelectedValue.ToString());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            queryStrings.Add("PurchaseOrderId", purchaseOrderId);
            queryStrings.Add("isAdd", "true");
            base.ReloadPage(queryStrings);
        }

        private void UpdatePurchaseOrder()
        {
            int num = 0;
            PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
            foreach (PurchaseOrderItemInfo info2 in purchaseOrder.PurchaseOrderItems)
            {
                new PurchaseOrderItemInfo();
                num += info2.ItemWeight * info2.Quantity;
            }
            purchaseOrder.Weight = num;
            SubsiteSalesHelper.UpdatePurchaseOrder(purchaseOrder);
        }
    }
}

