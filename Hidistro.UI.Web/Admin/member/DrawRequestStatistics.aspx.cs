
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.MemberDrawRequestStatistics)]
    public partial class DrawRequestStatistics : AdminPage
    {
        DateTime? dateEnd;
        DateTime? dateStart;
        string userName;

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.UserName = userName;
            query.FromDate = dateStart;
            query.ToDate = dateEnd;
            query.SortBy = "TradeDate";
            query.SortOrder = SortAction.Desc;
            query.TradeType = TradeTypes.DrawRequest;
            DbQueryResult balanceDetailsNoPage = MemberHelper.GetBalanceDetailsNoPage(query);
            string s = string.Empty + "用户名,交易时间,业务摘要,转出金额,当前余额\r\n";
            foreach (DataRow row in ((DataTable)balanceDetailsNoPage.Data).Rows)
            {
                s = s + row["UserName"];
                s = s + "," + row["TradeDate"];
                s = s + ",提现";
                s = s + "," + row["Expenses"];
                object obj2 = s;
                s = string.Concat(new object[] { obj2, ",", row["Balance"], "\r\n" });
            }
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "GB2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetailsStatistics.csv");
            Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Page.Response.ContentType = "application/octet-stream";
            Page.EnableViewState = false;
            Page.Response.Write(s);
            Page.Response.End();
        }

        private void btnQueryBalanceDrawRequest_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        public void GetBalanceDrawRequest()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.UserName = userName;
            query.FromDate = dateStart;
            query.ToDate = dateEnd;
            query.PageIndex = pager.PageIndex;
            query.TradeType = TradeTypes.DrawRequest;
            DbQueryResult balanceDetails = MemberHelper.GetBalanceDetails(query);
            grdBalanceDrawRequest.DataSource = balanceDetails.Data;
            grdBalanceDrawRequest.DataBind();
            pager1.TotalRecords = pager.TotalRecords = balanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = base.Server.UrlDecode(Page.Request.QueryString["userName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateStart"]))
                {
                    dateStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dateStart"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateEnd"]))
                {
                    dateEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dateEnd"])));
                }
                txtUserName.Text = userName;
                calendarStart.SelectedDate = dateStart;
                calendarEnd.SelectedDate = dateEnd;
            }
            else
            {
                userName = txtUserName.Text;
                dateStart = calendarStart.SelectedDate;
                dateEnd = calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQueryBalanceDrawRequest.Click += new EventHandler(btnQueryBalanceDrawRequest_Click);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                GetBalanceDrawRequest();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", txtUserName.Text);
            queryStrings.Add("dateStart", calendarStart.SelectedDate.ToString());
            queryStrings.Add("dateEnd", calendarEnd.SelectedDate.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

