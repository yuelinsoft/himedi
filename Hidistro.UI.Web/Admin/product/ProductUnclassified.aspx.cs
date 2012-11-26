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
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductUnclassified)]
    public partial class ProductUnclassified : AdminPage
    {
        int? categoryId;
        int? lineId;
        string productCode;
        string searchkey;
        DateTime? startDate;
        DateTime? endDate;

        private void BindProducts()
        {
            LoadParameters();
            ProductQuery query = new ProductQuery();
            query.Keywords = searchkey;
            query.ProductCode = productCode;
            query.PageSize = pager.PageSize;
            query.CategoryId = categoryId;
            query.StartDate = startDate;
            query.EndDate = endDate;
            query.ProductLineId = lineId;
            int? selectedValue = dropBrandList.SelectedValue;
            query.BrandId = selectedValue.HasValue ? dropBrandList.SelectedValue : null;
            query.PageIndex = pager.PageIndex;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            if (categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(query.CategoryId.Value).Path;
            }
            DbQueryResult unclassifiedProducts = ProductHelper.GetUnclassifiedProducts(query);
            grdProducts.DataSource = unclassifiedProducts.Data;
            grdProducts.DataBind();
            pager1.TotalRecords = pager.TotalRecords = unclassifiedProducts.TotalRecords;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请先选择要删除的商品", false);
            }
            else
            {
                int num = ProductHelper.RemoveProduct(str);
                if (num > 0)
                {
                    ShowMsg(string.Format("成功删除了选择的商品", num), true);
                    BindProducts();
                }
                else
                {
                    ShowMsg("删除商品失败，未知错误", false);
                }
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            int newCategoryId = dropMoveToCategories.SelectedValue.HasValue ? dropMoveToCategories.SelectedValue.Value : 0;
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请选择要转移的商品", false);
            }
            else
            {
                string[] strArray;
                if (!str.Contains(","))
                {
                    strArray = new string[] { str };
                }
                else
                {
                    strArray = str.Split(new char[] { ',' });
                }
                foreach (string str2 in strArray)
                {
                    ProductHelper.UpdateProductCategory(Convert.ToInt32(str2), newCategoryId);
                }
                dropCategories.SelectedValue = new int?(newCategoryId);
                ReBind(false);
                ShowMsg("转移商品类型成功", true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void btnSetCategories_Click(object sender, EventArgs e)
        {
            if (dropMoveToCategories.SelectedValue.HasValue)
            {
                int num = dropMoveToCategories.SelectedValue.Value;
            }
            string str = base.Request.Form["CheckBoxGroup"];
            if (string.IsNullOrEmpty(str))
            {
                ShowMsg("请选择要设置的商品", false);
            }
            else
            {
                string[] strArray;
                if (!str.Contains(","))
                {
                    strArray = new string[] { str };
                }
                else
                {
                    strArray = str.Split(new char[] { ',' });
                }
                foreach (string str2 in strArray)
                {
                    if (dropAddToAllCategories.SelectedValue.HasValue)
                    {
                        CatalogHelper.SetProductExtendCategory(Convert.ToInt32(str2), CatalogHelper.GetCategory(dropAddToAllCategories.SelectedValue.Value).Path + "|");
                    }
                    else
                    {
                        CatalogHelper.SetProductExtendCategory(Convert.ToInt32(str2), null);
                    }
                }
                ReBind(false);
                ShowMsg("批量设置扩展分类成功", true);
            }
        }

        private void dropAddToCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductCategoriesDropDownList list = (ProductCategoriesDropDownList)sender;
            GridViewRow namingContainer = (GridViewRow)list.NamingContainer;
            if (list.SelectedValue.HasValue)
            {
                CatalogHelper.SetProductExtendCategory((int)grdProducts.DataKeys[namingContainer.RowIndex].Value, CatalogHelper.GetCategory(list.SelectedValue.Value).Path + "|");
                ReBind(false);
            }
            else
            {
                CatalogHelper.SetProductExtendCategory((int)grdProducts.DataKeys[namingContainer.RowIndex].Value, null);
                ReBind(false);
            }
        }

        private void grdProducts_ReBindData(object sender)
        {
            ReBind(false);
        }

        private void grdProducts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ProductCategoriesDropDownList list = (ProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
                list.SelectedIndexChanged += new EventHandler(dropAddToCategories_SelectedIndexChanged);
            }
        }

        private void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = (Literal)e.Row.FindControl("litMainCategory");
                literal.Text = "-";
                object obj2 = DataBinder.Eval(e.Row.DataItem, "CategoryId");
                if ((obj2 != null) && (obj2 != DBNull.Value))
                {
                    literal.Text = CatalogHelper.GetFullCategory((int)obj2);
                }
                ProductCategoriesDropDownList list = (ProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
                list.DataBind();
                Literal literal2 = (Literal)e.Row.FindControl("litExtendCategory");
                literal2.Text = "-";
                object obj3 = DataBinder.Eval(e.Row.DataItem, "ExtendCategoryPath");
                if ((obj3 != null) && (obj3 != DBNull.Value))
                {
                    string s = (string)obj3;
                    if (s.Length > 0)
                    {
                        s = s.Substring(0, s.Length - 1);
                        if (s.Contains("|"))
                        {
                            s = s.Substring(s.LastIndexOf('|') + 1);
                        }
                        literal2.Text = CatalogHelper.GetFullCategory(int.Parse(s));
                        list.SelectedValue = new int?(int.Parse(s));
                    }
                }
            }
        }

        private void LoadParameters()
        {
            int result = 0;
            if (int.TryParse(Page.Request.QueryString["categoryId"], out result))
            {
                categoryId = new int?(result);
            }
            int num2 = 0;
            if (int.TryParse(Page.Request.QueryString["brandId"], out num2))
            {
                dropBrandList.SelectedValue = new int?(num2);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
            {
                productCode = Globals.UrlDecode(Page.Request.QueryString["productCode"]);
            }
            int num3 = 0;
            if (int.TryParse(Page.Request.QueryString["lineId"], out num3))
            {
                lineId = new int?(num3);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
            {
                searchkey = Globals.UrlDecode(Page.Request.QueryString["searchKey"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["startDate"]))
            {
                startDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["startDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["endDate"]))
            {
                endDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["endDate"]));
            }
            dropCategories.DataBind();
            dropCategories.SelectedValue = categoryId;
            dropLines.DataBind();
            dropLines.SelectedValue = lineId;
            txtSearchText.Text = searchkey;
            txtSKU.Text = productCode;
            calendarStartDate.SelectedDate = startDate;
            calendarEndDate.SelectedDate = endDate;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdProducts.ReBindData += new Grid.ReBindDataEventHandler(grdProducts_ReBindData);
            grdProducts.RowCreated += new GridViewRowEventHandler(grdProducts_RowCreated);
            grdProducts.RowDataBound += new GridViewRowEventHandler(grdProducts_RowDataBound);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnMove.Click += new EventHandler(btnMove_Click);
            btnSetCategories.Click += new EventHandler(btnSetCategories_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dropMoveToCategories.DataBind();
                dropBrandList.DataBind();
                BindProducts();
                dropAddToAllCategories.DataBind();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", Globals.UrlEncode(txtSearchText.Text.Trim()));
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("categoryId", dropCategories.SelectedValue.ToString());
            }
            if (dropBrandList.SelectedValue.HasValue)
            {
                queryStrings.Add("brandId", dropBrandList.SelectedValue.ToString());
            }
            if (dropLines.SelectedValue.HasValue)
            {
                queryStrings.Add("lineId", dropLines.SelectedValue.ToString());
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!string.IsNullOrEmpty(txtSKU.Text.Trim()))
            {
                queryStrings.Add("productCode", txtSKU.Text.Trim());
            }
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
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

