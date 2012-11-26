namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public partial class MyProductReviews : DistributorPage
    {
         int? categoryId;
         string keywords = string.Empty;
         string productCode;

        private void BindPtReview()
        {
            ProductReviewQuery query2 = new ProductReviewQuery();
            query2.Keywords = this.keywords;
            query2.CategoryId = this.categoryId;
            query2.ProductCode = this.productCode;
            query2.PageIndex = this.pager.PageIndex;
            query2.PageSize = this.pager.PageSize;
            query2.SortOrder = SortAction.Desc;
            query2.SortBy = "ReviewDate";
            ProductReviewQuery entity = query2;
            int total = 0;
            Globals.EntityCoding(entity, true);
            DataSet productReviews = SubsiteCommentsHelper.GetProductReviews(out total, entity);
            this.dlstPtReviews.DataSource = productReviews.Tables[0].DefaultView;
            this.dlstPtReviews.DataBind();
            this.pager.TotalRecords = total;
            this.pager1.TotalRecords = total;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductReviews(true);
        }

        private void dlstPtReviews_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            if (SubsiteCommentsHelper.DeleteProductReview((long) Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture)) > 0)
            {
                this.ShowMsg("成功删除了选择的商品评论回复", true);
                this.BindPtReview();
            }
            else
            {
                this.ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetSearchControl();
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.dlstPtReviews.DeleteCommand += new DataListCommandEventHandler(this.dlstPtReviews_DeleteCommand);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.BindPtReview();
            }
        }

        private void ReloadProductReviews(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", this.txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
            queryStrings.Add("productCode", this.txtSKU.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", this.pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", this.pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
                {
                    this.keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
                }
                int result = 0;
                if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out result))
                {
                    this.categoryId = new int?(result);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
                {
                    this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
                }
                this.txtSearchText.Text = this.keywords;
                this.txtSKU.Text = this.productCode;
                this.dropCategories.DataBind();
                this.dropCategories.SelectedValue = this.categoryId;
            }
            else
            {
                this.keywords = this.txtSearchText.Text;
                this.productCode = this.txtSKU.Text;
                this.categoryId = this.dropCategories.SelectedValue;
            }
        }
    }
}

