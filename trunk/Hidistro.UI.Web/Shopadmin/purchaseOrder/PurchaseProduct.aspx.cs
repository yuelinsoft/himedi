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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class PurchaseProduct : DistributorPage
    {
        string productCode;
        int? productLineId;
        string productName;


        private PurchaseShoppingCartItemInfo AddPurchaseShoppingCartItemInfo(string skuId, int quantity)
        {
            PurchaseShoppingCartItemInfo info = new PurchaseShoppingCartItemInfo();
            DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(skuId);
            if (quantity > ((int)skuContentBySku.Rows[0]["Stock"]))
            {
                return null;
            }
            foreach (DataRow row in skuContentBySku.Rows)
            {
                if (!(string.IsNullOrEmpty(row["AttributeName"].ToString()) || string.IsNullOrEmpty(row["ValueStr"].ToString())))
                {
                    object sKUContent = info.SKUContent;
                    info.SKUContent = string.Concat(new object[] { sKUContent, row["AttributeName"], ":", row["ValueStr"], "; " });
                }
            }
            info.SkuId = skuId;
            info.ProductId = (int)skuContentBySku.Rows[0]["ProductId"];
            if (skuContentBySku.Rows[0]["SKU"] != DBNull.Value)
            {
                info.SKU = (string)skuContentBySku.Rows[0]["SKU"];
            }
            if (skuContentBySku.Rows[0]["Weight"] != DBNull.Value)
            {
                info.ItemWeight = (int)skuContentBySku.Rows[0]["Weight"];
            }
            info.ItemPurchasePrice = (decimal)skuContentBySku.Rows[0]["PurchasePrice"];
            info.Quantity = quantity;
            info.ItemListPrice = (decimal)skuContentBySku.Rows[0]["SalePrice"];
            info.ItemDescription = (string)skuContentBySku.Rows[0]["ProductName"];
            if (skuContentBySku.Rows[0]["CostPrice"] != DBNull.Value)
            {
                info.CostPrice = (decimal)skuContentBySku.Rows[0]["CostPrice"];
            }
            if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != DBNull.Value)
            {
                info.ThumbnailsUrl = (string)skuContentBySku.Rows[0]["ThumbnailUrl40"];
            }
            return info;
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
            else if (e.CommandName == "add")
            {
                if (button.Text == "添加")
                {
                    PurchaseShoppingCartItemInfo item = new PurchaseShoppingCartItemInfo();
                    item = AddPurchaseShoppingCartItemInfo(skuId, num);
                    if (item == null)
                    {
                        ShowMsg("商品库存不够", false);
                    }
                    else if (SubsiteSalesHelper.AddPurchaseItem(item))
                    {
                        BindData();
                    }
                    else
                    {
                        ShowMsg("添加商品失败", false);
                    }
                }
                else if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem(skuId))
                {
                    BindData();
                }
                else
                {
                    ShowMsg("删除失败", false);
                }
            }
        }

        public void grdSkus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Grid grid = (Grid)sender;
                string str = grid.DataKeys[e.Row.RowIndex].Value.ToString();
                LinkButton button = (LinkButton)e.Row.FindControl("lbtnAdd");
                foreach (PurchaseShoppingCartItemInfo info in SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos())
                {
                    if (info.SkuId == str)
                    {
                        button.Text = "取消";
                        button.Attributes.Add("style", "color:Red");
                    }
                }
            }
        }

        private void lkbtnAdddCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = true;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (GridViewRow row in grdAuthorizeProducts.Rows)
            {
                GridView view = row.FindControl("grdSkus") as GridView;
                foreach (GridViewRow row2 in view.Rows)
                {
                    CheckBox box = (CheckBox)row2.FindControl("checkboxCol");
                    TextBox box2 = row2.FindControl("txtNum") as TextBox;
                    if ((box != null) && box.Checked)
                    {
                        int num2;
                        num++;
                        if ((!int.TryParse(box2.Text.Trim(), out num2) || (int.Parse(box2.Text.Trim()) <= 0)) || box2.Text.Trim().Contains("."))
                        {
                            flag = false;
                            break;
                        }
                        dictionary.Add(view.DataKeys[row2.RowIndex].Value.ToString(), num2);
                    }
                }
                if (!flag)
                {
                    break;
                }
            }
            if (num == 0)
            {
                ShowMsg("请先选择要添加的商品", false);
            }
            else if (!flag)
            {
                ShowMsg("数量不能为空,必需为大于零的正整数", false);
            }
            else
            {
                int num3 = 0;
                foreach (KeyValuePair<string, int> pair in dictionary)
                {
                    PurchaseShoppingCartItemInfo item = new PurchaseShoppingCartItemInfo();
                    item = AddPurchaseShoppingCartItemInfo(pair.Key, Convert.ToInt32(pair.Value));
                    if (item == null)
                    {
                        ShowMsg("商品库存不够", false);
                        break;
                    }
                    if (!SubsiteSalesHelper.AddPurchaseItem(item))
                    {
                        break;
                    }
                    num3++;
                }
                if (num3 > 0)
                {
                    ShowMsg(string.Format("成功添加了{0}件商品", num3), true);
                    BindData();
                }
                else
                {
                    ShowMsg("添加商品失败", false);
                }
            }
        }

        private void lkbtncancleCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = true;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            foreach (GridViewRow row in grdAuthorizeProducts.Rows)
            {
                GridView view = row.FindControl("grdSkus") as GridView;
                foreach (GridViewRow row2 in view.Rows)
                {
                    CheckBox box = (CheckBox)row2.FindControl("checkboxCol");
                    TextBox box2 = row2.FindControl("txtNum") as TextBox;
                    if ((box != null) && box.Checked)
                    {
                        int num2;
                        num++;
                        if ((!int.TryParse(box2.Text.Trim(), out num2) || (int.Parse(box2.Text.Trim()) <= 0)) || box2.Text.Trim().Contains("."))
                        {
                            flag = false;
                            break;
                        }
                        dictionary.Add((string)view.DataKeys[row2.RowIndex].Value, num2);
                    }
                }
                if (!flag)
                {
                    break;
                }
            }
            if (num == 0)
            {
                ShowMsg("请先选择要添加的商品", false);
            }
            else if (!flag)
            {
                ShowMsg("数量不能为空,必需为大于零的正整数", false);
            }
            else
            {
                int num3 = 0;
                foreach (KeyValuePair<string, int> pair in dictionary)
                {
                    if (!SubsiteSalesHelper.DeletePurchaseShoppingCartItem(pair.Key))
                    {
                        break;
                    }
                    num3++;
                }
                if (num3 > 0)
                {
                    ShowMsg(string.Format("成功取消了{0}件商品", num3), true);
                    BindData();
                }
                else
                {
                    ShowMsg("取消商品失败", false);
                }
            }
        }

        private void LoadParameters()
        {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            lkbtnAdddCheck.Click += new EventHandler(lkbtnAdddCheck_Click);
            lkbtncancleCheck.Click += new EventHandler(lkbtncancleCheck_Click);
            grdAuthorizeProducts.RowDataBound += new GridViewRowEventHandler(grdAuthorizeProducts_RowDataBound);
            if (!base.IsPostBack)
            {
                ddlProductLine.DataBind();
                ddlProductLine.SelectedValue = productLineId;
                BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
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
            base.ReloadPage(queryStrings);
        }
    }
}

