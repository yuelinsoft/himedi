namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;

    [ParseChildren(true)]
    public class GroupBuyProducts : HtmlTemplatedWebControl
    {
       Pager pager;
       ThemedTemplatedRepeater rptProduct;

        protected override void AttachChildControls()
        {
            this.rptProduct = (ThemedTemplatedRepeater) this.FindControl("rptProduct");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                this.BindProduct();
            }
        }

       void BindProduct()
        {
            int num;
            DataSet groupByProductList = ProductBrowser.GetGroupByProductList(this.GetProductBrowseQuery(), out num);
            this.rptProduct.DataSource = groupByProductList;
            this.rptProduct.DataBind();
            this.pager.TotalRecords = num;
        }

       ProductBrowseQuery GetProductBrowseQuery()
        {
            ProductBrowseQuery query = new ProductBrowseQuery();
            query.PageIndex = this.pager.PageIndex;
            int result = 10;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageSize"]) && int.TryParse(this.Page.Request.QueryString["pageSize"], out result))
            {
                query.PageSize = result;
                return query;
            }
            query.PageSize = 10;
            return query;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-GroupBuyProducts.html";
            }
            base.OnInit(e);
        }
    }
}

