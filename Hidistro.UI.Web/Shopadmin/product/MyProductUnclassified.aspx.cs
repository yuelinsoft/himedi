using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyProductUnclassified : DistributorPage
    {

        int? classId;
        string productcode;
        string searchkey;


        private void BindProducts()
        {
            ProductQuery query = new ProductQuery();
            query.Keywords = searchkey;
            query.PageSize = pager.PageSize;
            query.ProductCode = productcode;

            if (dropCategories.SelectedValue.HasValue)
            {
                query.CategoryId = dropCategories.SelectedValue;
                query.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(query.CategoryId.Value).Path;
            }
            query.PageIndex = pager.PageIndex;
            query.SortOrder = SortAction.Desc;
            query.SortBy = "DisplaySequence";
            DbQueryResult unclassifiedProducts = SubSiteProducthelper.GetUnclassifiedProducts(query);
            grdProducts.DataSource = unclassifiedProducts.Data;
            grdProducts.DataBind();
            pager.TotalRecords = unclassifiedProducts.TotalRecords;
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
                int num = SubSiteProducthelper.DeleteProducts(str);
                if (num > 0)
                {
                    ShowMsg(string.Format("成功删除了选择的{0}件商品", num), true);
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
                    SubSiteProducthelper.UpdateProductCategory(Convert.ToInt32(str2), newCategoryId);
                }
                dropCategories.SelectedValue = new int?(newCategoryId);
                BindProducts();
                ShowMsg("转移商品类型成功", true);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", txtSearchText.Text);
            queryStrings.Add("productCode", txtSKU.Text);
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("classId", dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }

        private void dropAddToCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistributorProductCategoriesDropDownList list = (DistributorProductCategoriesDropDownList)sender;
            GridViewRow namingContainer = (GridViewRow)list.NamingContainer;
            if (list.SelectedValue.HasValue)
            {
                SubsiteCatalogHelper.SetProductExtendCategory((int)grdProducts.DataKeys[namingContainer.RowIndex].Value, SubsiteCatalogHelper.GetCategory(list.SelectedValue.Value).Path + "|");
                ReBind();
            }
            else
            {
                SubsiteCatalogHelper.SetProductExtendCategory((int)grdProducts.DataKeys[namingContainer.RowIndex].Value, null);
                ReBind();
            }
        }

        private void grdProducts_ReBindData(object sender)
        {
            ReBind();
        }

        private void grdProducts_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DistributorProductCategoriesDropDownList list = (DistributorProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
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
                    literal.Text = SubsiteCatalogHelper.GetFullCategory((int)obj2);
                }
                DistributorProductCategoriesDropDownList list = (DistributorProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
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
                        literal2.Text = SubsiteCatalogHelper.GetFullCategory(int.Parse(s));
                        list.SelectedValue = new int?(int.Parse(s));
                    }
                }
            }
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropMoveToCategories.DataBind();
                if (!string.IsNullOrEmpty(Page.Request.QueryString["classId"]))
                {
                    int result = 0;
                    if (int.TryParse(Page.Request.QueryString["classId"], out result))
                    {
                        classId = new int?(result);
                    }
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
                {
                    searchkey = base.Server.UrlDecode(Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
                {
                    productcode = base.Server.UrlDecode(Page.Request.QueryString["productCode"]);
                }
                dropCategories.SelectedValue = classId;
                txtSearchText.Text = searchkey;
                txtSKU.Text = productcode;
            }
            else
            {
                searchkey = txtSearchText.Text;
                classId = dropCategories.SelectedValue;
                productcode = txtSKU.Text;
            }
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindProducts();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBind()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", txtSearchText.Text);
            queryStrings.Add("productCode", txtSKU.Text);
            if (dropCategories.SelectedValue.HasValue)
            {
                queryStrings.Add("classId", dropCategories.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            base.ReloadPage(queryStrings);
        }
    }
}

