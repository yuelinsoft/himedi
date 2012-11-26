
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
    [PrivilegeCheck(Privilege.MemberBalanceStatistics)]
    public partial class BalanceStatistics : AdminPage
    {
        DateTime? dateEnd;
        DateTime? dateStart;
        string userName;

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = dateStart;
            query.ToDate = dateEnd;
            query.SortBy = "TradeDate";
            query.SortOrder = SortAction.Desc;
            query.UserName = userName;
            DbQueryResult balanceDetailsNoPage = MemberHelper.GetBalanceDetailsNoPage(query);
            string s = string.Empty + "用户名,交易时间,业务摘要,转入金额,转出金额,当前余额\r\n";
            foreach (DataRow row in ((DataTable)balanceDetailsNoPage.Data).Rows)
            {
                string str2 = string.Empty;
                switch (Convert.ToInt32(row["TradeType"]))
                {
                    case 1:
                        str2 = "自助充值";
                        break;

                    case 2:
                        str2 = "后台加款";
                        break;

                    case 3:
                        str2 = "消费";
                        break;

                    case 4:
                        str2 = "提现";
                        break;

                    case 5:
                        str2 = "订单退款";
                        break;

                    default:
                        str2 = "其他";
                        break;
                }
                s = s + row["UserName"];
                s = s + "," + row["TradeDate"];
                s = s + "," + str2;
                s = s + "," + row["Income"];
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

        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void GetBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = dateStart;
            query.ToDate = dateEnd;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.SortBy = "TradeDate";
            query.SortOrder = SortAction.Desc;
            query.UserName = userName;
            DbQueryResult balanceDetails = MemberHelper.GetBalanceDetails(query);
            grdBalanceDetails.DataSource = balanceDetails.Data;
            grdBalanceDetails.DataBind();
            pager1.TotalRecords = pager.TotalRecords = balanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = Page.Request.QueryString["userName"].ToString();
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateStart"]))
                {
                    dateStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dateStart"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dateEnd"]))
                {
                    dateEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dateEnd"])));
                }
                calendarStart.SelectedDate = dateStart;
                calendarEnd.SelectedDate = dateEnd;
                txtUserName.Text = userName;
            }
            else
            {
                dateStart = calendarStart.SelectedDate;
                dateEnd = calendarEnd.SelectedDate;
                userName = txtUserName.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQueryBalanceDetails.Click += new EventHandler(btnQueryBalanceDetails_Click);
            btnCreateReport.Click += new EventHandler(btnCreateReport_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                GetBalanceDetails();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", userName);
            queryStrings.Add("dateStart", calendarStart.SelectedDate.ToString());
            queryStrings.Add("dateEnd", calendarEnd.SelectedDate.ToString());
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

