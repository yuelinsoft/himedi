
using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.purchaseOrder
{
    public partial class ChangePurchaseOrderItems : AdminPage
    {
        int distorUserId;
        string IsAdd = string.Empty;
        string productCode;
        int? productLineId;
        string productName;
        string purchaseOrderId = string.Empty;


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
            DbQueryResult submitPuchaseProductsByDistorUserId = ProductHelper.GetSubmitPuchaseProductsByDistorUserId(entity, distorUserId);
            grdAuthorizeProducts.DataSource = submitPuchaseProductsByDistorUserId.Data;
            grdAuthorizeProducts.DataBind();
            pager.TotalRecords = submitPuchaseProductsByDistorUserId.TotalRecords;
            pager1.TotalRecords = submitPuchaseProductsByDistorUserId.TotalRecords;
        }

        private void BindOrderItems()
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
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
                DataTable skusByProductIdByDistorId = ProductHelper.GetSkusByProductIdByDistorId(Convert.ToInt32(grdAuthorizeProducts.DataKeys[e.Row.RowIndex].Value), distorUserId);
                Grid grid = (Grid)e.Row.FindControl("grdSkus");
                if (grid != null)
                {
                    grid.DataSource = skusByProductIdByDistorId;
                    grid.DataBind();
                }
            }
        }

        private void grdOrderItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string skuId = e.CommandArgument.ToString();
            if (e.CommandName == "UPDATE_ITEMS")
            {
                if (SalesHelper.GetPurchaseOrder(purchaseOrderId).PurchaseOrderItems.Count <= 1)
                {
                    ShowMsg("采购单的最后一件商品不允许删除", false);
                }
                else if (SalesHelper.DeletePurchaseOrderItem(purchaseOrderId, skuId))
                {
                    UpdatePurchaseOrder();
                    BindOrderItems();
                    ReBindData(true, false);
                    ShowMsg("删除成功！", true);
                }
                else
                {
                    ShowMsg("删除失败！", false);
                }
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
                }
                else
                {
                    int skuStock = OrderHelper.GetSkuStock(skuId);
                    if (num > skuStock)
                    {
                        ShowMsg("此商品库存不够", false);
                    }
                    else if (num <= 0)
                    {
                        ShowMsg("商品购买数量不能为0", false);
                    }
                    else if (SalesHelper.UpdatePurchaseOrderItemQuantity(purchaseOrderId, skuId, num))
                    {
                        UpdatePurchaseOrder();
                        BindOrderItems();
                        pager1.TotalRecords = pager.TotalRecords = 0;
                        grdAuthorizeProducts.DataSource = null;
                        grdAuthorizeProducts.DataBind();
                        ShowMsg("修改成功！", true);
                    }
                    else
                    {
                        ShowMsg("修改失败！", false);
                    }
                }
            }
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
                DataTable skuContentBySkuBuDistorUserId = ProductHelper.GetSkuContentBySkuBuDistorUserId(skuId, distorUserId);
                if (num > ((int)skuContentBySkuBuDistorUserId.Rows[0]["Stock"]))
                {
                    ShowMsg("商品库存不够", false);
                }
                else
                {
                    foreach (DataRow row in skuContentBySkuBuDistorUserId.Rows)
                    {
                        if (!(string.IsNullOrEmpty(row["AttributeName"].ToString()) || string.IsNullOrEmpty(row["ValueStr"].ToString())))
                        {
                            object sKUContent = item.SKUContent;
                            item.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                        }
                    }
                    item.SkuId = skuId;
                    item.ProductId = (int)skuContentBySkuBuDistorUserId.Rows[0]["ProductId"];
                    if (skuContentBySkuBuDistorUserId.Rows[0]["SKU"] != DBNull.Value)
                    {
                        item.SKU = (string)skuContentBySkuBuDistorUserId.Rows[0]["SKU"];
                    }
                    if (skuContentBySkuBuDistorUserId.Rows[0]["Weight"] != DBNull.Value)
                    {
                        item.ItemWeight = (int)skuContentBySkuBuDistorUserId.Rows[0]["Weight"];
                    }
                    item.ItemPurchasePrice = (decimal)skuContentBySkuBuDistorUserId.Rows[0]["PurchasePrice"];
                    item.Quantity = num;
                    item.ItemListPrice = (decimal)skuContentBySkuBuDistorUserId.Rows[0]["SalePrice"];
                    item.ItemDescription = (string)skuContentBySkuBuDistorUserId.Rows[0]["ProductName"];
                    if (skuContentBySkuBuDistorUserId.Rows[0]["CostPrice"] != DBNull.Value)
                    {
                        item.CostPrice = (decimal)skuContentBySkuBuDistorUserId.Rows[0]["CostPrice"];
                    }
                    if (skuContentBySkuBuDistorUserId.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
                    {
                        item.ThumbnailsUrl = (string)skuContentBySkuBuDistorUserId.Rows[0]["ThumbnailUrl40"];
                    }
                    if (SalesHelper.AddPurchaseOrderItem(item, purchaseOrderId))
                    {
                        UpdatePurchaseOrder();
                        BindOrderItems();
                        ReBindData(true, false);
                        ShowMsg("添加规格商品成功", true);
                    }
                    else
                    {
                        ShowMsg("添加规格商品失败", false);
                    }
                }
            }
        }

        private void LoadParameters()
        {
            purchaseOrderId = Page.Request.QueryString["PurchaseOrderId"];
            IsAdd = Page.Request.QueryString["IsAdd"];
            int.TryParse(Page.Request.QueryString["DistorUserId"], out distorUserId);
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
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdOrderItems.RowCommand += new GridViewCommandEventHandler(grdOrderItems_RowCommand);
            grdAuthorizeProducts.RowDataBound += new GridViewRowEventHandler(grdAuthorizeProducts_RowDataBound);
            if (!Page.IsPostBack)
            {
                BindOrderItems();
                if (!string.IsNullOrEmpty(IsAdd))
                {
                    BindData();
                }
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
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            queryStrings.Add("PurchaseOrderId", purchaseOrderId);
            queryStrings.Add("DistorUserId", distorUserId.ToString());
            queryStrings.Add("IsAdd", "true");
            base.ReloadPage(queryStrings);
        }

        private void UpdatePurchaseOrder()
        {
            PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
            int num = 0;
            foreach (PurchaseOrderItemInfo info2 in purchaseOrder.PurchaseOrderItems)
            {
                new PurchaseOrderItemInfo();
                num += info2.ItemWeight * info2.Quantity;
            }
            purchaseOrder.Weight = num;
            SalesHelper.UpdatePurchaseOrder(purchaseOrder);
        }
    }
}

