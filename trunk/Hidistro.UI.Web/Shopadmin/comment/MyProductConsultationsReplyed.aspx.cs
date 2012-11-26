namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public partial class MyProductConsultationsReplyed : DistributorPage
    {
         int? categoryId;
         string keywords = string.Empty;
         string productCode;

        private void BinddlstProductConsultation()
        {
            ProductConsultationAndReplyQuery query2 = new ProductConsultationAndReplyQuery();
            query2.Keywords = this.keywords;
            query2.CategoryId = this.categoryId;
            query2.ProductCode = this.productCode;
            query2.PageIndex = this.pager.PageIndex;
            query2.PageSize = this.pager.PageSize;
            query2.SortOrder = SortAction.Desc;
            query2.SortBy = "ReplyDate";
            query2.Type = ConsultationReplyType.Replyed;
            ProductConsultationAndReplyQuery entity = query2;
            Globals.EntityCoding(entity, true);
            DbQueryResult consultationProducts = SubsiteCommentsHelper.GetConsultationProducts(entity);
            this.grdConsultation.DataSource = consultationProducts.Data;
            this.grdConsultation.DataBind();
            this.pager.TotalRecords = consultationProducts.TotalRecords;
            this.pager1.TotalRecords = consultationProducts.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.ReloadProductConsultationsReplyed(true);
        }

        private void grdConsultation_ReBindData(object sender)
        {
            this.BinddlstProductConsultation();
        }

        private void grdConsultation_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int consultationId = (int) this.grdConsultation.DataKeys[e.RowIndex].Value;
            if (SubsiteCommentsHelper.DeleteProductConsultation(consultationId) > 0)
            {
                this.ShowMsg("成功删除了选择的商品咨询", true);
                this.BinddlstProductConsultation();
            }
            else
            {
                this.ShowMsg("删除商品咨询失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetSearchControl();
            this.grdConsultation.RowDeleting += new GridViewDeleteEventHandler(this.grdConsultation_RowDeleting);
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.grdConsultation.ReBindData += new Grid.ReBindDataEventHandler(this.grdConsultation_ReBindData);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.BinddlstProductConsultation();
            }
        }

        private void ReloadProductConsultationsReplyed(bool isSearch)
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

