
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
    /// 导出为易分销数据包
    /// </summary>
    [PrivilegeCheck(Privilege.MakeProductsPack)]
    public partial class ExportToYfx : AdminPage
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
            Dictionary<string, string> exportAdapters = TransferHelper.GetExportAdapters(new YfxTarget("1.2"), "易分销");
            foreach (string str in exportAdapters.Keys)
            {
               dropExportVersions.Items.Add(new ListItem(exportAdapters[str], str));
            }
        }

        private void BindProducts()
        {
            if (!((this._includeUnSales ||_includeOnSales) ||_includeInStock))
            {
               ShowMsg("至少要选择包含一个商品状态", false);
            }
            else
            {
                DbQueryResult exportProducts = ProductHelper.GetExportProducts(this.GetQuery(), (string)this.ViewState["RemoveProductIds"]);
               grdProducts.DataSource = exportProducts.Data;
               grdProducts.DataBind();
               pager.TotalRecords = exportProducts.TotalRecords;
               lblTotals.Text = exportProducts.TotalRecords.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string selectedValue =dropExportVersions.SelectedValue;
            if (string.IsNullOrEmpty(selectedValue) || (selectedValue.Length == 0))
            {
               ShowMsg("请选择一个导出版本", false);
            }
            else
            {
                bool includeCostPrice =chkExportCostPrice.Checked;
                bool includeStock =chkExportStock.Checked;
                bool flag3 =chkExportImages.Checked;
                DataSet set = ProductHelper.GetExportProducts(this.GetQuery(), includeCostPrice, includeStock, (string)this.ViewState["RemoveProductIds"]);
                TransferHelper.GetExporter(selectedValue, new object[] { set, includeCostPrice, includeStock, flag3 }).DoExport();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
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
            if (this._categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(this._categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            return entity;
        }

        private void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                int proid = (int)this.grdProducts.DataKeys[rowIndex].Value;
                string proidstr = (string)this.ViewState["RemoveProductIds"];
                if (string.IsNullOrEmpty(proidstr))
                {
                    proidstr = proid.ToString();
                }
                else
                {
                    proidstr = proidstr + "," + proid.ToString();
                }
                ViewState["RemoveProductIds"] = proidstr;
               BindProducts();
            }
        }

        private void LoadParameters()
        {
            int num;
            int num2;
            DateTime time;
            DateTime time2;
           _productName =txtSearchText.Text.Trim();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
            {
               _productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
               txtSearchText.Text =_productName;
            }
           _productCode =txtSKU.Text.Trim();
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
            {
               _productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
               txtSKU.Text =_productCode;
            }
           _categoryId =dropCategories.SelectedValue;
            if (!(string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]) || !int.TryParse(this.Page.Request.QueryString["categoryId"], out num)))
            {
               _categoryId = new int?(num);
               dropCategories.SelectedValue =_categoryId;
            }
           _lineId =dropLines.SelectedValue;
            if (!(string.IsNullOrEmpty(this.Page.Request.QueryString["lineId"]) || !int.TryParse(this.Page.Request.QueryString["lineId"], out num2)))
            {
               _lineId = new int?(num2);
               dropLines.SelectedValue =_lineId;
            }
           _startDate =calendarStartDate.SelectedDate;
            if (!(string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]) || !DateTime.TryParse(this.Page.Request.QueryString["startDate"], out time)))
            {
               _startDate = new DateTime?(time);
               calendarStartDate.SelectedDate =_startDate;
            }
           _endDate =calendarEndDate.SelectedDate;
            if (!(string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]) || !DateTime.TryParse(this.Page.Request.QueryString["endDate"], out time2)))
            {
               _endDate = new DateTime?(time2);
               calendarEndDate.SelectedDate =_endDate;
            }
           _includeOnSales =chkOnSales.Checked;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeOnSales"]))
            {
                bool.TryParse(this.Page.Request.QueryString["includeOnSales"], out _includeOnSales);
               chkOnSales.Checked =_includeOnSales;
            }
           _includeUnSales =chkUnSales.Checked;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeUnSales"]))
            {
                bool.TryParse(this.Page.Request.QueryString["includeUnSales"], out _includeUnSales);
               chkUnSales.Checked =_includeUnSales;
            }
           _includeInStock =chkInStock.Checked;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["includeInStock"]))
            {
                bool.TryParse(this.Page.Request.QueryString["includeInStock"], out _includeInStock);
               chkInStock.Checked =_includeInStock;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
           btnSearch.Click += new EventHandler(this.btnSearch_Click);
           btnExport.Click += new EventHandler(this.btnExport_Click);
           grdProducts.RowCommand += new GridViewCommandEventHandler(this.grdProducts_RowCommand);
            if (!this.Page.IsPostBack)
            {
               dropCategories.DataBind();
               dropLines.DataBind();
               BindExporter();
            }
           LoadParameters();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
               BindProducts();
            }
        }

        private void ReSearchProducts()
        {
            NameValueCollection values = new NameValueCollection();
            values.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
            values.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
            values.Add("pageSize",pager.PageSize.ToString());
            values.Add("includeOnSales",chkOnSales.Checked.ToString());
            values.Add("includeUnSales",chkUnSales.Checked.ToString());
            values.Add("includeInStock",chkInStock.Checked.ToString());
            NameValueCollection queryStrings = values;
            if (this.dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId",dropCategories.SelectedValue.ToString());
            }
            if (this.dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId",dropLines.SelectedValue.ToString());
            }
            queryStrings.Add("pageIndex",pager.PageIndex.ToString());
            if (this.calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate",calendarStartDate.SelectedDate.Value.ToString());
            }
            if (this.calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate",calendarEndDate.SelectedDate.Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

