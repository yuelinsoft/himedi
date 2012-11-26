using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin.product
{
    /// <summary>
    /// 分销授权产品线
    /// </summary>
    public partial class AuthorizeProducts : DistributorPage
    {
        int? lineId;
        string name;
        string productCode;

        private void BindData()
        {
            ProductQuery entity = new ProductQuery();

            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.ProductCode = productCode;
            entity.Keywords = name;
            entity.ProductLineId = lineId;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";//grdAuthorizeProducts.SortOrderBy;// 

            Globals.EntityCoding(entity, true);

            DbQueryResult authorizeProducts = SubSiteProducthelper.GetAuthorizeProducts(entity, true);

            grdAuthorizeProducts.DataSource = authorizeProducts.Data;

            grdAuthorizeProducts.DataBind();

            pager.TotalRecords = authorizeProducts.TotalRecords;

            pager1.TotalRecords = authorizeProducts.TotalRecords;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReBindData(true);
        }

        protected void grdAuthorizeProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow)((Control)e.CommandSource).NamingContainer;

            int productId = (int)grdAuthorizeProducts.DataKeys[namingContainer.RowIndex].Value;

            if (e.CommandName == "download")
            {

                SubSiteProducthelper.DownloadProduct(productId);

                ReBindData(false);

            }

        }

        protected void lkbtnDownloadCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdAuthorizeProducts.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    num++;
                }
            }
            if (num == 0)
            {
                ShowMsg("请先选择要下载的商品", false);
            }
            else
            {
                foreach (GridViewRow row2 in grdAuthorizeProducts.Rows)
                {
                    CheckBox box2 = (CheckBox)row2.FindControl("checkboxCol");
                    if (box2.Checked)
                    {
                        int productId = (int)grdAuthorizeProducts.DataKeys[row2.RowIndex].Value;
                        SubSiteProducthelper.DownloadProduct(productId);
                    }
                }
                ReBindData(false);
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
                if (!string.IsNullOrEmpty(Page.Request.QueryString["name"]))
                {
                    name = base.Server.UrlDecode(Page.Request.QueryString["name"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["lineId"]))
                {
                    int result = 0;
                    if (int.TryParse(Page.Request.QueryString["lineId"], out result))
                    {
                        lineId = new int?(result);
                    }
                    //litPageTitle.Text = "＂" + base.Server.UrlDecode(Page.Request.QueryString["lineName"]) + "＂产品线下商品列表";
                }
                txtSKU.Text = productCode;
                txtName.Text = name;
            }
            else
            {
                productCode = txtSKU.Text;
                name = txtName.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBindData(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (!string.IsNullOrEmpty(txtSKU.Text))
            {
                queryStrings.Add("ProductCode", txtSKU.Text);
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                queryStrings.Add("name", txtName.Text);
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

