using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ProductReviewsManage)]
    public partial class ProductReviews : AdminPage
    {
        int? categoryId;
        string keywords = string.Empty;
        string productCode;


        private void BindPtReview()
        {
            ProductReviewQuery entity = new ProductReviewQuery();
            entity.Keywords = keywords;
            entity.CategoryId = categoryId;
            entity.ProductCode = productCode;
            entity.PageIndex = pager.PageIndex;
            entity.PageSize = pager.PageSize;
            entity.SortOrder = SortAction.Desc;
            entity.SortBy = "ReviewDate";

            Globals.EntityCoding(entity, true);
            int total = 0;
            DataSet productReviews = ProductCommentHelper.GetProductReviews(out total, entity);
            dlstPtReviews.DataSource = productReviews.Tables[0].DefaultView;
            dlstPtReviews.DataBind();
            pager.TotalRecords = total;
            pager1.TotalRecords = total;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadProductReviews(true);
        }

        private void dlstPtReviews_DeleteCommand(object source, DataListCommandEventArgs e)
        {
            if (ProductCommentHelper.DeleteProductReview((long)Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture)) > 0)
            {
                ShowMsg("成功删除了选择的商品评论回复", true);
                BindPtReview();
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            dlstPtReviews.DeleteCommand += new DataListCommandEventHandler(dlstPtReviews_DeleteCommand);
            SetSearchControl();
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                BindPtReview();
            }
        }

        private void ReloadProductReviews(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", txtSearchText.Text.Trim());
            queryStrings.Add("CategoryId", dropCategories.SelectedValue.ToString());
            queryStrings.Add("productCode", txtSKU.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["Keywords"]))
                {
                    keywords = base.Server.UrlDecode(Page.Request.QueryString["Keywords"]);
                }
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["CategoryId"], out result))
                {
                    categoryId = new int?(result);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productCode"]))
                {
                    productCode = base.Server.UrlDecode(Page.Request.QueryString["productCode"]);
                }
                txtSearchText.Text = keywords;
                txtSKU.Text = productCode;
                dropCategories.DataBind();
                dropCategories.SelectedValue = categoryId;
            }
            else
            {
                keywords = txtSearchText.Text;
                productCode = txtSKU.Text;
                categoryId = dropCategories.SelectedValue;
            }
        }
    }
}

