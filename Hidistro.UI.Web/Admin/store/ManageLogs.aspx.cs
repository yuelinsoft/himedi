using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class ManageLogs : AdminPage
    {
        public void BindLogs()
        {
            DbQueryResult logs = EventLogs.GetLogs(GetOperationLogQuery());
            dlstLog.DataSource = logs.Data;
            dlstLog.DataBind();
            SetSearchControl();
            pager.TotalRecords = logs.TotalRecords;
            pager1.TotalRecords = logs.TotalRecords;
        }

        private void btnQueryLogs_Click(object sender, EventArgs e)
        {
            ReloadManagerLogs(true);
        }

        private void DeleteCheck()
        {
            string strIds = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                strIds = base.Request["CheckBoxGroup"];
            }
            if (strIds.Length <= 0)
            {
                ShowMsg("请先选择要删除的操作日志项", false);
            }
            else
            {
                int num = EventLogs.DeleteLogs(strIds);
                BindLogs();
                ShowMsg(string.Format("成功删除了{0}个操作日志", num), true);
            }
        }

        private void dlstLog_DeleteCommand(object sender, DataListCommandEventArgs e)
        {
            long logId = (long)dlstLog.DataKeys[e.Item.ItemIndex];
            if (EventLogs.DeleteLog(logId))
            {
                BindLogs();
                ShowMsg("成功删除了单个操作日志", true);
            }
            else
            {
                ShowMsg("在删除过程中出现未知错误", false);
            }
        }

        private OperationLogQuery GetOperationLogQuery()
        {
            OperationLogQuery query = new OperationLogQuery();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["OperationUserName"]))
            {
                query.OperationUserName = base.Server.UrlDecode(Page.Request.QueryString["OperationUserName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["FromDate"]))
            {
                query.FromDate = new DateTime?(Convert.ToDateTime(Page.Request.QueryString["FromDate"]));
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["ToDate"]))
            {
                query.ToDate = new DateTime?(Convert.ToDateTime(Page.Request.QueryString["ToDate"]));
            }
            query.Page.PageIndex = pager.PageIndex;
            query.Page.PageSize = pager.PageSize;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["SortBy"]))
            {
                query.Page.SortBy = Page.Request.QueryString["SortBy"];
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["SortOrder"]))
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            return query;
        }

        private void lkbDeleteAll_Click(object sender, EventArgs e)
        {
            if (EventLogs.DeleteAllLogs())
            {
                BindLogs();
                ShowMsg("成功删除了所有操作日志", true);
            }
            else
            {
                ShowMsg("在删除过程中出现未知错误", false);
            }
        }

        private void lkbDeleteCheck_Click(object sender, EventArgs e)
        {
            DeleteCheck();
        }

        private void lkbDeleteCheck1_Click(object sender, EventArgs e)
        {
            DeleteCheck();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dlstLog.DeleteCommand += new DataListCommandEventHandler(dlstLog_DeleteCommand);
            btnQueryLogs.Click += new EventHandler(btnQueryLogs_Click);
            lkbDeleteCheck.Click += new EventHandler(lkbDeleteCheck_Click);
            lkbDeleteCheck1.Click += new EventHandler(lkbDeleteCheck1_Click);
            lkbDeleteAll.Click += new EventHandler(lkbDeleteAll_Click);
            if (!Page.IsPostBack)
            {
                dropOperationUserNames.DataBind();
                BindLogs();
            }
        }

        private void ReloadManagerLogs(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("OperationUserName", dropOperationUserNames.SelectedValue);
            if (calenderFromDate.SelectedDate.HasValue)
            {
                queryStrings.Add("FromDate", calenderFromDate.SelectedDate.ToString());
            }
            if (calenderToDate.SelectedDate.HasValue)
            {
                queryStrings.Add("ToDate", calenderToDate.SelectedDate.ToString());
            }
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void SetSearchControl()
        {
            if (!Page.IsCallback)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["OperationUserName"]))
                {
                    dropOperationUserNames.SelectedValue = base.Server.UrlDecode(Page.Request.QueryString["OperationUserName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["FromDate"]))
                {
                    calenderFromDate.SelectedDate = new DateTime?(Convert.ToDateTime(Page.Request.QueryString["FromDate"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["ToDate"]))
                {
                    calenderToDate.SelectedDate = new DateTime?(Convert.ToDateTime(Page.Request.QueryString["ToDate"]));
                }
            }
        }
    }
}

