using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.UserIncreaseStatistics)]
    public partial class UserIncreaseStatistics : AdminPage
    {
        private void BindDaysAddUser()
        {
            int? year = null;
            IList<UserStatisticsForDate> list = SalesHelper.GetUserAdd(year, null, 7);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                int num;
                if (string.IsNullOrEmpty(str))
                {
                    if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                    {
                        str = str + ((DateTime.Now.Month > 9) ? (num = DateTime.Now.Month - 1).ToString(CultureInfo.InvariantCulture) : ("0" + (num = DateTime.Now.Month - 1).ToString(CultureInfo.InvariantCulture) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)))));
                    }
                    else
                    {
                        str = str + ((DateTime.Now.Month > 9) ? DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) : ("0" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)))));
                    }
                }
                else if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                {
                    string str3 = str;
                    str = str3 + "|" + ((DateTime.Now.Month > 10) ? (num = DateTime.Now.Month - 1).ToString(CultureInfo.InvariantCulture) : ("0" + (num = DateTime.Now.Month - 1).ToString(CultureInfo.InvariantCulture))) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    string str4 = str;
                    str = str4 + "|" + ((DateTime.Now.Month > 10) ? DateTime.Now.Month.ToString(CultureInfo.InvariantCulture) : ("0" + DateTime.Now.Month.ToString(CultureInfo.InvariantCulture))) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString(CultureInfo.InvariantCulture) : ("0" + date.TimePoint.ToString(CultureInfo.InvariantCulture)));
                }
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + date.UserCounts;
                }
                else
                {
                    str2 = str2 + "|" + date.UserCounts;
                }
            }
            imgChartOfSevenDay.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", str, str2);
        }

        private void BindMonthAddUser()
        {
            IList<UserStatisticsForDate> list = SalesHelper.GetUserAdd(new int?(drpYearOfMonth.SelectedValue), new int?(drpMonthOfMonth.SelectedValue), null);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = str + date.TimePoint;
                }
                else
                {
                    str = str + "|" + date.TimePoint;
                }
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + date.UserCounts;
                }
                else
                {
                    str2 = str2 + "|" + date.UserCounts;
                }
            }
            imgChartOfMonth.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", str, str2);
            litlOfMonth.Text = drpYearOfMonth.SelectedValue.ToString(CultureInfo.InvariantCulture) + "年" + drpMonthOfMonth.SelectedValue.ToString(CultureInfo.InvariantCulture) + "月";
        }

        private void BindYearAddUser()
        {
            int? month = null;
            IList<UserStatisticsForDate> list = SalesHelper.GetUserAdd(new int?(drpYearOfYear.SelectedValue), month, null);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                if (string.IsNullOrEmpty(str))
                {
                    str = str + date.TimePoint;
                }
                else
                {
                    str = str + "|" + date.TimePoint;
                }
                if (string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + date.UserCounts;
                }
                else
                {
                    str2 = str2 + "|" + date.UserCounts;
                }
            }
            imgChartOfYear.Src = Globals.ApplicationPath + string.Format("/UserStatisticeChart.aspx?ChartType={0}&XValues={1}&YValues={2}", "bar", str, str2);
            litlOfYear.Text = drpYearOfYear.SelectedValue.ToString(CultureInfo.InvariantCulture) + "年";
        }

        private void btnOfMonth_Click(object sender, EventArgs e)
        {
            BindMonthAddUser();
        }

        private void btnOfYear_Click(object sender, EventArgs e)
        {
            BindYearAddUser();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnOfMonth.Click += new EventHandler(btnOfMonth_Click);
            btnOfYear.Click += new EventHandler(btnOfYear_Click);
            if (!Page.IsPostBack)
            {
                BindDaysAddUser();
                BindMonthAddUser();
                BindYearAddUser();
            }
        }
    }
}

