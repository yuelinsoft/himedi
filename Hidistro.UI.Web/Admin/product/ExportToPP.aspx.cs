using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.TransferManager;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    /// <summary>
    /// 导出为拍拍数据包
    /// </summary>
    [PrivilegeCheck(Privilege.MakeProductsPack)]
    public partial class ExportToPP : AdminPage
    {
         int? _categoryId;
         DateTime? _endDate;
         bool _includeInStock;
         bool _includeOnSales;
         bool _includeUnSales;
         int? _lineId;
         string _productCode;
         string _productName;
         DateTime? _startDate;

        private void BindExporter()
        {
            dropExportVersions.Items.Clear();
            dropExportVersions.Items.Add(new ListItem("-请选择-", ""));
            Dictionary<string, string> exportAdapters = TransferHelper.GetExportAdapters(new YfxTarget("1.2"), "拍拍助理");
            foreach (string str in exportAdapters.Keys)
            {
                dropExportVersions.Items.Add(new ListItem(exportAdapters[str], str));
            }
        }

        private void BindProducts()
        {
            if (!((_includeUnSales || _includeOnSales) || _includeInStock))
            {
                ShowMsg("至少要选择包含一个商品状态", false);
            }
            else
            {
                DbQueryResult exportProducts = ProductHelper.GetExportProducts(GetQuery(), (string)ViewState["RemoveProductIds"]);
                grdProducts.DataSource = exportProducts.Data;
                grdProducts.DataBind();
                pager.TotalRecords = exportProducts.TotalRecords;
                lblTotals.Text = exportProducts.TotalRecords.ToString(CultureInfo.InvariantCulture);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string selectedValue = dropExportVersions.SelectedValue;
            if (string.IsNullOrEmpty(selectedValue) || (selectedValue.Length == 0))
            {
                ShowMsg("请选择一个导出版本", false);
            }
            else
            {
                bool includeCostPrice = false;
                bool includeStock = chkExportStock.Checked;
                bool flag3 = true;
                DataSet set = ProductHelper.GetExportProducts(GetQuery(), includeCostPrice, includeStock, (string)ViewState["RemoveProductIds"]);
                TransferHelper.GetExporter(selectedValue, new object[] { set, includeCostPrice, includeStock, flag3 }).DoExport();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReSearchProducts();
        }

        private AdvancedProductQuery GetQuery()
        {
            AdvancedProductQuery entity = new AdvancedProductQuery();
            entity.Keywords = _productName;
            entity.ProductCode = _productCode;
            entity.CategoryId = _categoryId;
            entity.ProductLineId = _lineId;
            entity.PageSize = pager.PageSize;
            entity.PageIndex = pager.PageIndex;
            entity.SaleStatus = ProductSaleStatus.OnSale;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "DisplaySequence";
            entity.StartDate = _startDate;
            entity.EndDate = _endDate;
            entity.IncludeInStock = _includeInStock;
            entity.IncludeOnSales = _includeOnSales;
            entity.IncludeUnSales = _includeUnSales;
            if (_categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(_categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            return entity;
        }

        protected void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                int num2 = (int)grdProducts.DataKeys[rowIndex].Value;
                string str = (string)ViewState["RemoveProductIds"];
                if (string.IsNullOrEmpty(str))
                {
                    str = num2.ToString();
                }
                else
                {
                    str = str + "," + num2.ToString();
                }
                ViewState["RemoveProductIds"] = str;
                BindProducts();
            }
        }

        private void LoadParameters()
        {
            int num;
            int num2;
            DateTime time;
            DateTime time2;
            _productName = txtSearchText.Text.Trim();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
            {
                _productName = Globals.UrlDecode(Page.Request.QueryString["productName"]);
                txtSearchText.Text = _productName;
            }
            _productCode = txtSKU.Text.Trim();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
            {
                _productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
                txtSKU.Text = _productCode;
            }
            _categoryId = dropCategories.SelectedValue;
            if (!(string.IsNullOrEmpty(Page.Request.QueryString["categoryId"]) || !int.TryParse(Page.Request.QueryString["categoryId"], out num)))
            {
                _categoryId = new int?(num);
                dropCategories.SelectedValue = _categoryId;
            }
            _lineId = dropLines.SelectedValue;
            if (!(string.IsNullOrEmpty(Page.Request.QueryString["lineId"]) || !int.TryParse(Page.Request.QueryString["lineId"], out num2)))
            {
                _lineId = new int?(num2);
                dropLines.SelectedValue = _lineId;
            }
            _startDate = calendarStartDate.SelectedDate;
            if (!(string.IsNullOrEmpty(Page.Request.QueryString["startDate"]) || !DateTime.TryParse(Page.Request.QueryString["startDate"], out time)))
            {
                _startDate = new DateTime?(time);
                calendarStartDate.SelectedDate = _startDate;
            }
            _endDate = calendarEndDate.SelectedDate;
            if (!(string.IsNullOrEmpty(Page.Request.QueryString["endDate"]) || !DateTime.TryParse(Page.Request.QueryString["endDate"], out time2)))
            {
                _endDate = new DateTime?(time2);
                calendarEndDate.SelectedDate = _endDate;
            }
            _includeOnSales = chkOnSales.Checked;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["includeOnSales"]))
            {
                bool.TryParse(Page.Request.QueryString["includeOnSales"], out _includeOnSales);
                chkOnSales.Checked = _includeOnSales;
            }
            _includeUnSales = chkUnSales.Checked;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["includeUnSales"]))
            {
                bool.TryParse(Page.Request.QueryString["includeUnSales"], out _includeUnSales);
                chkUnSales.Checked = _includeUnSales;
            }
            _includeInStock = chkInStock.Checked;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["includeInStock"]))
            {
                bool.TryParse(Page.Request.QueryString["includeInStock"], out _includeInStock);
                chkInStock.Checked = _includeInStock;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExport.Click += new EventHandler(btnExport_Click);
            grdProducts.RowCommand += new GridViewCommandEventHandler(grdProducts_RowCommand);
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropLines.DataBind();
                BindExporter();
            }
            LoadParameters();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindProducts();
            }
        }

        private void ReSearchProducts()
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("productName", Globals.UrlEncode(txtSearchText.Text.Trim()));
            values.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(txtSKU.Text.Trim())));
            values.Add("pageSize", pager.PageSize.ToString());
            values.Add("includeOnSales", chkOnSales.Checked.ToString());
            values.Add("includeUnSales", chkUnSales.Checked.ToString());
            values.Add("includeInStock", chkInStock.Checked.ToString());
            NameValueCollection queryStrings = values;
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", dropCategories.SelectedValue.ToString());
            }
            if (dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", dropLines.SelectedValue.ToString());
            }
            queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            if (calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", calendarStartDate.SelectedDate.Value.ToString());
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", calendarEndDate.SelectedDate.Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

