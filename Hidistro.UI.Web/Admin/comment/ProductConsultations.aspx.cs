using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductConsultationsManage)]
    public partial class ProductConsultations : AdminPage
    {
        int? categoryId;
        string keywords = string.Empty;
        string productCode;

        private void BindConsultation()
        {
            ProductConsultationAndReplyQuery entity = new ProductConsultationAndReplyQuery();
            entity.Keywords = keywords;
            entity.CategoryId = categoryId;
            entity.ProductCode = productCode;
            entity.PageIndex = pager.PageIndex;
            entity.PageSize = pager.PageSize;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "ReplyDate";
            entity.Type = ConsultationReplyType.NoReply;
             Globals.EntityCoding(entity, true);
            DbQueryResult consultationProducts = ProductCommentHelper.GetConsultationProducts(entity);
            grdConsultation.DataSource = consultationProducts.Data;
            grdConsultation.DataBind();
            pager.TotalRecords = consultationProducts.TotalRecords;
            pager1.TotalRecords = consultationProducts.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadProductConsultations(true);
        }

        private void grdConsultation_ReBindData(object sender)
        {
            BindConsultation();
        }

        private void grdConsultation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int consultationId = (int)grdConsultation.DataKeys[e.RowIndex].Value;
            if (ProductCommentHelper.DeleteProductConsultation(consultationId) > 0)
            {
                ShowMsg("成功删除了选择的商品咨询", true);
                BindConsultation();
            }
            else
            {
                ShowMsg("删除商品咨询失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetSearchControl();
            grdConsultation.RowDeleting += new GridViewDeleteEventHandler(grdConsultation_RowDeleting);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdConsultation.ReBindData += new Grid.ReBindDataEventHandler(grdConsultation_ReBindData);
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                BindConsultation();
            }
        }

        private void ReloadProductConsultations(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", dropCategories.SelectedValue.ToString());
            queryStrings.Add("productCode", txtSKU.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", hrefPageSize.SelectedSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["Keywords"]))
                {
                    keywords = Page.Request.QueryString["Keywords"];
                }
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["CategoryId"], out result))
                {
                    categoryId = new int?(result);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
                {
                    productCode = Page.Request.QueryString["productCode"];
                }
                txtSearchText.Text = keywords;
                txtSKU.Text = productCode;
                dropCategories.DataBind();
                dropCategories.SelectedValue = categoryId;
            }
            else
            {
                keywords = txtSearchText.Text.Trim();
                productCode = txtSKU.Text.Trim();
                categoryId = dropCategories.SelectedValue;
            }
        }
    }
}

