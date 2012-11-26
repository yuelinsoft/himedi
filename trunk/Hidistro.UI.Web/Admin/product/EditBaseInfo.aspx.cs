using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.EditProducts)]
    public partial class EditBaseInfo : AdminPage
    {
        string productIds = string.Empty;

        private void BindProduct()
        {
            string str = Page.Request.QueryString["ProductIds"];
            if (!string.IsNullOrEmpty(str))
            {
                grdSelectedProducts.DataSource = ProductHelper.GetProductBaseInfo(str);
                grdSelectedProducts.DataBind();
            }
        }

        private void btnAddOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPrefix.Text.Trim()) && string.IsNullOrEmpty(txtSuffix.Text.Trim()))
            {
                ShowMsg("前后缀不能同时为空", false);
            }
            else
            {
                if (ProductHelper.UpdateProductNames(productIds, txtPrefix.Text.Trim(), txtSuffix.Text.Trim()))
                {
                    ShowMsg("为商品名称添加前后缀成功", true);
                }
                else
                {
                    ShowMsg("为商品名称添加前后缀失败", false);
                }
                BindProduct();
            }
        }

        private void btnReplaceOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOleWord.Text.Trim()))
            {
                ShowMsg("查找字符串不能为空", false);
            }
            else
            {
                if (ProductHelper.ReplaceProductNames(productIds, txtOleWord.Text.Trim(), txtNewWord.Text.Trim()))
                {
                    ShowMsg("为商品名称替换字符串缀成功", true);
                }
                else
                {
                    ShowMsg("为商品名称替换字符串缀失败", false);
                }
                BindProduct();
            }
        }

        private void btnSaveInfo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductId");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("ProductCode");
            dt.Columns.Add("MarketPrice");
            dt.Columns.Add("LowestSalePrice");
            if (grdSelectedProducts.Rows.Count > 0)
            {
                decimal result = 0M;
                decimal num2 = 0M;
                foreach (GridViewRow row in grdSelectedProducts.Rows)
                {
                    int num3 = (int)grdSelectedProducts.DataKeys[row.RowIndex].Value;
                    TextBox box = row.FindControl("txtProductName") as TextBox;
                    TextBox box2 = row.FindControl("txtProductCode") as TextBox;
                    TextBox box3 = row.FindControl("txtMarketPrice") as TextBox;
                    TextBox box4 = row.FindControl("txtLowestSalePrice") as TextBox;
                    if ((string.IsNullOrEmpty(box.Text.Trim()) || string.IsNullOrEmpty(box4.Text.Trim())) || (!string.IsNullOrEmpty(box3.Text.Trim()) && !decimal.TryParse(box3.Text.Trim(), out result)))
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(box3.Text.Trim()))
                    {
                        result = 0M;
                    }
                    if (decimal.TryParse(box4.Text.Trim(), out num2))
                    {
                        DataRow row2 = dt.NewRow();
                        row2["ProductId"] = num3;
                        row2["ProductName"] = Globals.HtmlEncode(box.Text.Trim());
                        row2["ProductCode"] = Globals.HtmlEncode(box2.Text.Trim());
                        if (result >= 0M)
                        {
                            row2["MarketPrice"] = result;
                        }
                        row2["LowestSalePrice"] = num2;
                        dt.Rows.Add(row2);
                    }
                }
                if (ProductHelper.UpdateProductBaseInfo(dt))
                {
                    ShowMsg("批量修改商品信息成功", true);
                }
                else
                {
                    ShowMsg("批量修改商品信息失败", false);
                }
                BindProduct();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            productIds = Page.Request.QueryString["productIds"];
            btnSaveInfo.Click += new EventHandler(btnSaveInfo_Click);
            btnAddOK.Click += new EventHandler(btnAddOK_Click);
            btnReplaceOK.Click += new EventHandler(btnReplaceOK_Click);
            if (!Page.IsPostBack)
            {
                BindProduct();
            }
        }
    }
}

