namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Xml;

    public class SubjectGoodsList : HtmlTemplatedWebControl
    {
       Pager pager;
       Common_GoodsList_Subject rptSubGoodsList;
       int subjectId;

        protected override void AttachChildControls()
        {
            this.rptSubGoodsList = (Common_GoodsList_Subject) this.FindControl("list_Common_GoodsList_Subject");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                if (!int.TryParse(this.Page.Request.QueryString["SubjectId"], out this.subjectId))
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.BindList();
                }
            }
        }

       void BindList()
        {
            SubjectListQuery productBrowseQuery = this.GetProductBrowseQuery();
            DbQueryResult subjectProduct = ProductBrowser.GetSubjectProduct(productBrowseQuery);
            this.rptSubGoodsList.DataSource = subjectProduct.Data;
            this.rptSubGoodsList.DataBind();
            this.pager.TotalRecords = (int) (subjectProduct.TotalRecords * (Convert.ToDouble(this.pager.PageSize) / ((double) productBrowseQuery.PageSize)));
        }

       SubjectListQuery GetProductBrowseQuery()
        {
            SubjectListQuery query = new SubjectListQuery();
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = 20;
            XmlNode node = ProductBrowser.GetProductSubjectDocument().SelectSingleNode("root/Subject[SubjectId='" + this.subjectId + "']");
            if (node != null)
            {
                query.ProductType = (SubjectType) int.Parse(node.SelectSingleNode("Type").InnerText);
                query.CategoryIds = node.SelectSingleNode("Categories").InnerText;
                string innerText = node.SelectSingleNode("MaxPrice").InnerText;
                if (!string.IsNullOrEmpty(innerText))
                {
                    int result = 0;
                    if (int.TryParse(innerText, out result))
                    {
                        query.MaxPrice = new decimal?(result);
                    }
                }
                string str2 = node.SelectSingleNode("MinPrice").InnerText;
                if (!string.IsNullOrEmpty(str2))
                {
                    int num2 = 0;
                    if (int.TryParse(str2, out num2))
                    {
                        query.MinPrice = new decimal?(num2);
                    }
                }
                query.Keywords = node.SelectSingleNode("Keywords").InnerText;
            }
            return query;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-SubjectGoodsList.html";
            }
            base.OnInit(e);
        }
    }
}

