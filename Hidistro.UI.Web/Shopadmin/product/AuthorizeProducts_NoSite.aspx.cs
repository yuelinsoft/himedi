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
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 分销授权产品线
    /// </summary>
    public partial class AuthorizeProducts_NoSite : DistributorPage
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

            if (grdAuthorizeProducts.SortOrder.ToLower() == "desc")
            {
                entity.SortOrder = SortAction.Desc;
            }

            entity.SortBy = grdAuthorizeProducts.SortOrderBy;
            Globals.EntityCoding(entity, true);
            DbQueryResult authorizeProducts = SubSiteProducthelper.GetAuthorizeProducts(entity, false);
            grdAuthorizeProducts.DataSource = authorizeProducts.Data;
            grdAuthorizeProducts.DataBind();
            pager.TotalRecords = authorizeProducts.TotalRecords;
            pager1.TotalRecords = authorizeProducts.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBindData(true);
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
                    litPageTitle.Text = "＂" + base.Server.UrlDecode(Page.Request.QueryString["lineName"]) + "＂产品线下商品列表";
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
            btnSearch.Click += new EventHandler(btnSearch_Click);
            if (!Page.IsPostBack)
            {
                BindData();
            }
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
            if (!string.IsNullOrEmpty(Page.Request.QueryString["lineId"]))
            {
                queryStrings.Add("lineId", Page.Request.QueryString["lineId"]);
                queryStrings.Add("lineName", base.Server.UrlDecode(Page.Request.QueryString["lineName"]));
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

