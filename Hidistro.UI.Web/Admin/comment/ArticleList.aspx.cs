using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Articles)]
    public partial class ArticleList : AdminPage
    {
        int? categoryId;
        DateTime? endArticleTime;
        string keywords = string.Empty;
        DateTime? startArticleTime;

        private void BindSearch()
        {
            ArticleQuery articleQuery = new ArticleQuery();
            articleQuery.StartArticleTime = startArticleTime;
            articleQuery.EndArticleTime = endArticleTime;
            articleQuery.Keywords = Globals.HtmlEncode(keywords);
            articleQuery.CategoryId = categoryId;
            articleQuery.PageIndex = pager.PageIndex;
            articleQuery.PageSize = pager.PageSize;
            articleQuery.SortBy = grdArticleList.SortOrderBy;
            articleQuery.SortOrder = SortAction.Desc;
            DbQueryResult articleList = ArticleHelper.GetArticleList(articleQuery);
            grdArticleList.DataSource = articleList.Data;
            grdArticleList.DataBind();
            pager.TotalRecords = articleList.TotalRecords;
            pager1.TotalRecords = articleList.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadActicleList(true);
        }

        private void grdArticleList_ReBindData(object sender)
        {
            BindSearch();
        }

        private void grdArticleList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Release")
            {
                int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
                int articId = (int)grdArticleList.DataKeys[rowIndex].Value;
                bool release = false;
                string str = "取消";
                if (e.CommandArgument.ToString().ToLower() == "false")
                {
                    release = true;
                    str = "发布";
                }
                if (ArticleHelper.UpdateRelease(articId, release))
                {
                    ShowMsg(str + "当前文章成功！", true);
                }
                else
                {
                    ShowMsg(str + "当前文章失败！", false);
                }
                ReloadActicleList(false);
            }
        }

        private void grdArticleList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int articleId = (int)grdArticleList.DataKeys[e.RowIndex].Value;
            if (ArticleHelper.DeleteArticle(articleId))
            {
                BindSearch();
                ShowMsg("成功删除了一篇文章", true);
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            IList<int> articles = new List<int>();
            int num = 0;
            foreach (GridViewRow row in grdArticleList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num++;
                    int item = Convert.ToInt32(grdArticleList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    articles.Add(item);
                }
            }
            if (num != 0)
            {
                int num3 = ArticleHelper.DeleteArticles(articles);
                BindSearch();
                ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除{0}篇文章", new object[] { num3 }), true);
            }
            else
            {
                ShowMsg("请先选择需要删除的文章", false);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
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
                DateTime now = DateTime.Now;
                if (DateTime.TryParse(Page.Request.QueryString["StartArticleTime"], out now))
                {
                    startArticleTime = new DateTime?(now);
                }
                DateTime time2 = DateTime.Now;
                if (DateTime.TryParse(Page.Request.QueryString["EndArticleTime"], out time2))
                {
                    endArticleTime = new DateTime?(time2);
                }
                txtKeywords.Text = keywords;
                calendarStartDataTime.SelectedDate = startArticleTime;
                calendarEndDataTime.SelectedDate = endArticleTime;
            }
            else
            {
                keywords = txtKeywords.Text;
                startArticleTime = calendarStartDataTime.SelectedDate;
                endArticleTime = calendarEndDataTime.SelectedDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdArticleList.RowDeleting += new GridViewDeleteEventHandler(grdArticleList_RowDeleting);
            lkbtnDeleteCheck.Click += new EventHandler(lkbtnDeleteCheck_Click);
            grdArticleList.ReBindData += new Grid.ReBindDataEventHandler(grdArticleList_ReBindData);
            grdArticleList.RowCommand += new GridViewCommandEventHandler(grdArticleList_RowCommand);
            LoadParameters();
            if (!Page.IsPostBack)
            {
                dropArticleCategory.DataBind();
                dropArticleCategory.SelectedValue = categoryId;
                BindSearch();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReloadActicleList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", Globals.UrlEncode(txtKeywords.Text.Trim()));
            queryStrings.Add("CategoryId", dropArticleCategory.SelectedValue.ToString());
            if (calendarStartDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("StartArticleTime", calendarStartDataTime.SelectedDate.ToString());
            }
            if (calendarEndDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("EndArticleTime", calendarEndDataTime.SelectedDate.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("PageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("SortBy", grdArticleList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

