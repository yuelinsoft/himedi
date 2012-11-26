using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public partial class EditStocks : AdminPage
    {

        string productIds = string.Empty;

        private void BindProduct()
        {
            string str = Page.Request.QueryString["ProductIds"];
            if (!string.IsNullOrEmpty(str))
            {
                grdSelectedProducts.DataSource = ProductHelper.GetSkuStocks(str);
                grdSelectedProducts.DataBind();
            }
        }

        private void btnOperationOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(productIds))
            {
                ShowMsg("没有要修改的商品", false);
            }
            else
            {
                int result = 0;
                if (!int.TryParse(txtAddStock.Text, out result))
                {
                    ShowMsg("请输入正确的库存格式", false);
                }
                else if (ProductHelper.AddSkuStock(productIds, result))
                {
                    BindProduct();
                    ShowMsg("修改商品的库存成功", true);
                }
                else
                {
                    ShowMsg("修改商品的库存失败", true);
                }
            }
        }

        private void btnSaveStock_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> skuStocks = null;
            if (grdSelectedProducts.Rows.Count > 0)
            {
                skuStocks = new Dictionary<string, int>();
                foreach (GridViewRow row in grdSelectedProducts.Rows)
                {
                    int result = 0;
                    TextBox box = row.FindControl("txtStock") as TextBox;
                    if (int.TryParse(box.Text, out result))
                    {
                        string key = (string)grdSelectedProducts.DataKeys[row.RowIndex].Value;
                        skuStocks.Add(key, result);
                    }
                }
                if (skuStocks.Count > 0)
                {
                    if (ProductHelper.UpdateSkuStock(skuStocks))
                    {
                        ShowMsg("批量修改库存成功", true);
                    }
                    else
                    {
                        ShowMsg("批量修改库存失败", false);
                    }
                }
                BindProduct();
            }
        }

        private void btnTargetOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(productIds))
            {
                ShowMsg("没有要修改的商品", false);
            }
            else
            {
                int result = 0;
                if (!int.TryParse(txtTagetStock.Text, out result))
                {
                    ShowMsg("请输入正确的库存格式", false);
                }
                else if (result < 0)
                {
                    ShowMsg("商品库存不能小于0", false);
                }
                else if (ProductHelper.UpdateSkuStock(productIds, result))
                {
                    BindProduct();
                    ShowMsg("修改商品的库存成功", true);
                }
                else
                {
                    ShowMsg("修改商品的库存失败", true);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            productIds = Page.Request.QueryString["productIds"];
            btnSaveStock.Click += new EventHandler(btnSaveStock_Click);
            btnTargetOK.Click += new EventHandler(btnTargetOK_Click);
            btnOperationOK.Click += new EventHandler(btnOperationOK_Click);
            if (!Page.IsPostBack)
            {
                BindProduct();
            }
        }
    }
}

