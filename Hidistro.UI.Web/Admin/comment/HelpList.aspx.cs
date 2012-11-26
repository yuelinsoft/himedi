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
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Helps)]
    public partial class HelpList : AdminPage
    {
        int? categoryId;
        DateTime? endTime;
        string keywords = string.Empty;
        DateTime? startTime;


        private void BindSearch()
        {
            HelpQuery helpQuery = new HelpQuery();
            helpQuery.StartArticleTime = startTime;
            helpQuery.EndArticleTime = endTime;
            helpQuery.Keywords = Globals.HtmlEncode(keywords);
            helpQuery.CategoryId = categoryId;
            helpQuery.PageIndex = pager.PageIndex;
            helpQuery.PageSize = pager.PageSize;
            helpQuery.SortBy = grdHelpList.SortOrderBy;
            helpQuery.SortOrder = SortAction.Desc;
            DbQueryResult helpList = ArticleHelper.GetHelpList(helpQuery);
            grdHelpList.DataSource = helpList.Data;
            grdHelpList.DataBind();
            pager.TotalRecords = helpList.TotalRecords;
            pager1.TotalRecords = helpList.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadHelpList(true);
        }

        private void grdHelpList_ReBindData(object sender)
        {
            BindSearch();
        }

        private void grdHelpList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ArticleHelper.DeleteHelp((int)grdHelpList.DataKeys[e.RowIndex].Value))
            {
                BindSearch();
                ShowMsg("成功删除了选择的帮助", true);
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            IList<int> helps = new List<int>();
            int num = 0;
            foreach (GridViewRow row in grdHelpList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num++;
                    int item = Convert.ToInt32(grdHelpList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture);
                    helps.Add(item);
                }
            }
            if (num != 0)
            {
                int num3 = ArticleHelper.DeleteHelps(helps);
                BindSearch();
                ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除\"{0}\"篇帮助", new object[] { num3 }), true);
            }
            else
            {
                ShowMsg("请先选择需要删除的帮助", false);
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
                if (DateTime.TryParse(Page.Request.QueryString["StartTime"], out now))
                {
                    startTime = new DateTime?(now);
                }
                DateTime time2 = DateTime.Now;
                if (DateTime.TryParse(Page.Request.QueryString["EndTime"], out time2))
                {
                    endTime = new DateTime?(time2);
                }
                txtkeyWords.Text = keywords;
                calendarStartDataTime.SelectedDate = startTime;
                calendarEndDataTime.SelectedDate = endTime;
            }
            else
            {
                keywords = txtkeyWords.Text;
                startTime = calendarStartDataTime.SelectedDate;
                endTime = calendarEndDataTime.SelectedDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdHelpList.RowDeleting += new GridViewDeleteEventHandler(grdHelpList_RowDeleting);
            grdHelpList.ReBindData += new Grid.ReBindDataEventHandler(grdHelpList_ReBindData);
            lkbtnDeleteCheck.Click += new EventHandler(lkbtnDeleteCheck_Click);
            LoadParameters();
            if (!Page.IsPostBack)
            {
                dropHelpCategory.DataBind();
                dropHelpCategory.SelectedValue = categoryId;
                BindSearch();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReloadHelpList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", txtkeyWords.Text.Trim());
            queryStrings.Add("CategoryId", dropHelpCategory.SelectedValue.ToString());
            if (calendarStartDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("StartTime", calendarStartDataTime.SelectedDate.ToString());
            }
            if (calendarEndDataTime.SelectedDate.HasValue)
            {
                queryStrings.Add("EndTime", calendarEndDataTime.SelectedDate.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("SortBy", grdHelpList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

