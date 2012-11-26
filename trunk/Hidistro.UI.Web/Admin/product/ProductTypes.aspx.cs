using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductTypes)]
    public partial class ProductTypes : AdminPage
    {
        string searchkey;


        private void BindTypes()
        {
            ProductTypeQuery query = new ProductTypeQuery();
            query.TypeName = searchkey;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            DbQueryResult productTypes = ProductTypeHelper.GetProductTypes(query);
            grdProductTypes.DataSource = productTypes.Data;
            grdProductTypes.DataBind();
            pager.TotalRecords = productTypes.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void grdProductTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int typeId = (int)grdProductTypes.DataKeys[e.RowIndex].Value;
            IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId, AttributeUseageMode.Choose);
            if (ProductTypeHelper.DeleteProductType(typeId))
            {
                foreach (AttributeInfo info in attributes)
                {
                    foreach (AttributeValueInfo info2 in info.AttributeValues)
                    {
                        StoreHelper.DeleteImage(info2.ImageUrl);
                    }
                }
                BindTypes();
                ShowMsg("成功删除了一个商品类型", true);
            }
            else
            {
                ShowMsg("删除商品类型失败, 可能有商品正在使用该类型", false);
            }
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
                {
                    searchkey = Globals.UrlDecode(Page.Request.QueryString["searchKey"]);
                }
                txtSearchText.Text = searchkey;
            }
            else
            {
                searchkey = txtSearchText.Text.Trim();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            grdProductTypes.RowDeleting += new GridViewDeleteEventHandler(grdProductTypes_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindTypes();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", txtSearchText.Text);
            queryStrings.Add("pageSize", "10");
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

