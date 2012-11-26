using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class UnderlingIncreaseStatistics : DistributorPage
    {


        private void BindMonthUserIncrease()
        {
            IList<UserStatisticsForDate> list = UnderlingHelper.GetUserIncrease(new int?(drpYearOfMonth.SelectedValue), new int?(drpMonthOfMonth.SelectedValue), null);
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
            litlOfMonth.Text = drpYearOfMonth.SelectedValue.ToString() + "年" + drpMonthOfMonth.SelectedValue.ToString() + "月";
        }

        private void BindWeekUserIncrease()
        {
            int? year = null;
            IList<UserStatisticsForDate> list = UnderlingHelper.GetUserIncrease(year, null, 7);
            string str = string.Empty;
            string str2 = string.Empty;
            foreach (UserStatisticsForDate date in list)
            {
                int num;
                if (string.IsNullOrEmpty(str))
                {
                    if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                    {
                        str = str + ((DateTime.Now.Month > 9) ? (num = DateTime.Now.Month - 1).ToString() : ("0" + (num = DateTime.Now.Month - 1).ToString() + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()))));
                    }
                    else
                    {
                        str = str + ((DateTime.Now.Month > 9) ? DateTime.Now.Month.ToString() : ("0" + DateTime.Now.Month.ToString() + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()))));
                    }
                }
                else if ((DateTime.Now.Date.Day < 7) && (date.TimePoint > 7))
                {
                    string str3 = str;
                    str = str3 + "|" + ((DateTime.Now.Month > 10) ? (num = DateTime.Now.Month - 1).ToString() : ("0" + (num = DateTime.Now.Month - 1).ToString())) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()));
                }
                else
                {
                    string str4 = str;
                    str = str4 + "|" + ((DateTime.Now.Month > 10) ? DateTime.Now.Month.ToString() : ("0" + DateTime.Now.Month.ToString())) + "-" + ((date.TimePoint > 9) ? date.TimePoint.ToString() : ("0" + date.TimePoint.ToString()));
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

        private void BindYearUserIncrease()
        {
            int? month = null;
            IList<UserStatisticsForDate> list = UnderlingHelper.GetUserIncrease(new int?(drpYearOfYear.SelectedValue), month, null);
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
            litlOfYear.Text = drpYearOfYear.SelectedValue.ToString() + "年";
        }

        protected void btnOfMonth_Click(object sender, EventArgs e)
        {
            BindMonthUserIncrease();
        }

        protected void btnOfYear_Click(object sender, EventArgs e)
        {
            BindYearUserIncrease();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnOfMonth.Click += new EventHandler(btnOfMonth_Click);
            btnOfYear.Click += new EventHandler(btnOfYear_Click);
            if (!Page.IsPostBack)
            {
                BindWeekUserIncrease();
                BindMonthUserIncrease();
                BindYearUserIncrease();
            }
        }
    }
}

