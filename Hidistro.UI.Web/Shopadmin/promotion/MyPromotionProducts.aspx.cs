using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyPromotionProducts : DistributorPage
    {
        int activeId;
        int? categoryId;
        string keywords;
        int pageindex = 1;
        int pageindex1 = 1;
        int pageSize = 10;


        private void BindProducts()
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = txtSearchText.Text;
            query.CategoryId = dropCategories.SelectedValue;
            query.PageSize = 10;
            query.PageIndex = pageindex;
            query.SaleStatus = ProductSaleStatus.OnSale;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            DbQueryResult products = SubSiteProducthelper.GetProducts(query);
            dlstProducts.DataSource = products.Data;
            dlstProducts.DataBind();
            pager.TotalRecords = products.TotalRecords;
        }

        private void BindPromoteProducts()
        {
            Pagination page = new Pagination();
            page.PageSize = 10;
            page.PageIndex = pagerBuyToSend.PageIndex;
            page.SortOrder = SortAction.Desc;
            page.SortBy = "DisplaySequence";
            DbQueryResult activeProducts = SubsitePromoteHelper.GetActiveProducts(page, activeId);
            dlstSearchProducts.DataSource = activeProducts.Data;
            dlstSearchProducts.DataBind();
            pagerBuyToSend.TotalRecords = activeProducts.TotalRecords;
        }

        private void btnAddSearch_Click(object sender, EventArgs e)
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = txtSearchText.Text;
            query.CategoryId = dropCategories.SelectedValue;
            query.SaleStatus = ProductSaleStatus.OnSale;
            IList<int> productIds = SubSiteProducthelper.GetProductIds(query);
            foreach (int num in productIds)
            {
                if (!ProductIds.Contains(num))
                {
                    ProductIds.Add(num);
                    SubsitePromoteHelper.InsertPromotionProduct(activeId, num);
                }
            }
            ProductIds = productIds;
            BindPromoteProducts();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ProductIds = null;
            SubsitePromoteHelper.DeletePromotionProducts(activeId);
            ReBind(false);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/promotion/MyPromoteSales.aspx");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void dlstProducts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "check")
            {
                IList<int> productIds = ProductIds;
                int itemIndex = e.Item.ItemIndex;
                int item = int.Parse(dlstProducts.DataKeys[itemIndex].ToString());
                if (!productIds.Contains(item))
                {
                    productIds.Add(item);
                    ProductIds = productIds;
                    if (SubsitePromoteHelper.InsertPromotionProduct(activeId, item))
                    {
                        BindPromoteProducts();
                    }
                }
            }
        }

        private void dlstSearchProducts_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            IList<int> productIds = ProductIds;
            int itemIndex = e.Item.ItemIndex;
            int item = int.Parse(dlstSearchProducts.DataKeys[itemIndex].ToString());
            productIds.Remove(item);
            ProductIds = productIds;
            if (SubsitePromoteHelper.DeletePromotionProducts(activeId, item))
            {
                BindPromoteProducts();
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(base.Request.QueryString["pageindex1"]))
                {
                    int.TryParse(base.Request.QueryString["pageindex1"], out pageindex1);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["pageindex"]))
                {
                    int.TryParse(base.Request.QueryString["pageindex"], out pageindex);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["pageSize"]))
                {
                    int.TryParse(base.Request.QueryString["pageSize"], out pageSize);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["Keywords"]))
                {
                    keywords = base.Request.QueryString["Keywords"];
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["categoryId"]))
                {
                    int result = 0;
                    if (int.TryParse(base.Request.QueryString["categoryId"], out result))
                    {
                        categoryId = new int?(result);
                    }
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["ActiveId"]))
                {
                    int.TryParse(base.Request.QueryString["ActiveId"], out activeId);
                }
                txtSearchText.Text = keywords;
            }
            else
            {
                keywords = txtSearchText.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnClear.Click += new EventHandler(btnClear_Click);
            dlstProducts.ItemCommand += new DataListCommandEventHandler(dlstProducts_ItemCommand);
            dlstSearchProducts.DeleteCommand += new DataListCommandEventHandler(dlstSearchProducts_DeleteCommand);
            btnAddSearch.Click += new EventHandler(btnAddSearch_Click);
            btnReturn.Click += new EventHandler(btnReturn_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["ActiveId"]))
            {
                int.TryParse(base.Request.QueryString["ActiveId"], out activeId);
            }
            LoadParameters();
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropCategories.SelectedValue = categoryId;
                BindProducts();
                PromotionInfo promotionInfoById = new PromotionInfo();
                promotionInfoById = SubsitePromoteHelper.GetPromotionInfoById(activeId);
                lblPromtion.Text = promotionInfoById.Name;
                ProductIds = SubsitePromoteHelper.GetPromotionProducts(activeId);
                BindPromoteProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", txtSearchText.Text.Trim());
            queryStrings.Add("pageSize", pageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageindex", pager.PageIndex.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageindex1", pagerBuyToSend.PageIndex.ToString());
            }
            queryStrings.Add("categoryId", dropCategories.SelectedValue.ToString());
            queryStrings.Add("ActiveId", activeId.ToString());
            base.ReloadPage(queryStrings);
        }

        private IList<int> ProductIds
        {
            get
            {
                if (ViewState["ProductIds"] == null)
                {
                    return new List<int>();
                }
                return (IList<int>)ViewState["ProductIds"];
            }
            set
            {
                ViewState["ProductIds"] = value;
            }
        }
    }
}

