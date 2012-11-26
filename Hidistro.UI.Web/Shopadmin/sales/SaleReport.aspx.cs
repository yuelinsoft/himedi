using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class SaleReport : DistributorPage
    {
        int dayMonth = DateTime.Now.Month;
        SaleStatisticsType dayType = SaleStatisticsType.SaleCounts;
        int dayYear = DateTime.Now.Year;
        SaleStatisticsType monthType = SaleStatisticsType.SaleCounts;
        int monthYear = DateTime.Now.Year;

        private void BindDaySaleTotalStatistics()
        {
            DataTable table = SubsiteSalesHelper.GetDaySaleTotal(dayYear, dayMonth, dayType);
            if (radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                grdDaySaleTotalStatistics.Columns[1].Visible = true;
                grdDaySaleTotalStatistics.Columns[1].HeaderText = radioDayForSaleType.SelectedItem.Text;
                grdDaySaleTotalStatistics.Columns[2].Visible = false;
            }
            else
            {
                grdDaySaleTotalStatistics.Columns[1].Visible = false;
                grdDaySaleTotalStatistics.Columns[2].Visible = true;
                grdDaySaleTotalStatistics.Columns[2].HeaderText = radioDayForSaleType.SelectedItem.Text;
            }
            grdDaySaleTotalStatistics.DataSource = table;
            grdDaySaleTotalStatistics.DataBind();
            TableOfDay = table;
            lblDayAllTotal.Text = string.Format("总{0}：", radioDayForSaleType.SelectedItem.Text);
            decimal money = SubsiteSalesHelper.GetMonthSaleTotal(dayYear, dayMonth, dayType);
            if (radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                litDayAllTotal.Text = money.ToString();
            }
            else
            {
                litDayAllTotal.Text = Globals.FormatMoney(money);
            }
            lblDayMaxTotal.Text = string.Format("最高峰{0}：", radioDayForSaleType.SelectedItem.Text);
            decimal num2 = 0M;
            foreach (DataRow row in table.Rows)
            {
                if (((decimal)row["SaleTotal"]) > num2)
                {
                    num2 = (decimal)row["SaleTotal"];
                }
            }
            if (radioDayForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                litDayMaxTotal.Text = num2.ToString();
            }
            else
            {
                litDayMaxTotal.Text = Globals.FormatMoney(num2);
            }
        }

        private void BindMonthSaleTotalStatistics()
        {
            DataTable monthSaleTotal = SubsiteSalesHelper.GetMonthSaleTotal(monthYear, monthType);
            if (radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                grdMonthSaleTotalStatistics.Columns[1].Visible = true;
                grdMonthSaleTotalStatistics.Columns[1].HeaderText = radioMonthForSaleType.SelectedItem.Text;
                grdMonthSaleTotalStatistics.Columns[2].Visible = false;
            }
            else
            {
                grdMonthSaleTotalStatistics.Columns[1].Visible = false;
                grdMonthSaleTotalStatistics.Columns[2].Visible = true;
                grdMonthSaleTotalStatistics.Columns[2].HeaderText = radioMonthForSaleType.SelectedItem.Text;
            }
            grdMonthSaleTotalStatistics.DataSource = monthSaleTotal;
            grdMonthSaleTotalStatistics.DataBind();
            TableOfMonth = monthSaleTotal;
            lblMonthAllTotal.Text = string.Format("总{0}：", radioMonthForSaleType.SelectedItem.Text);
            decimal yearSaleTotal = SubsiteSalesHelper.GetYearSaleTotal(monthYear, monthType);
            if (radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                litMonthAllTotal.Text = yearSaleTotal.ToString();
            }
            else
            {
                litMonthAllTotal.Text = Globals.FormatMoney(yearSaleTotal);
            }
            lblMonthMaxTotal.Text = string.Format("最高峰{0}：", radioMonthForSaleType.SelectedItem.Text);
            decimal money = 0M;
            foreach (DataRow row in monthSaleTotal.Rows)
            {
                if (((decimal)row["SaleTotal"]) > money)
                {
                    money = (decimal)row["SaleTotal"];
                }
            }
            if (radioMonthForSaleType.SelectedValue == SaleStatisticsType.SaleCounts)
            {
                litMonthMaxTotal.Text = money.ToString();
            }
            else
            {
                litMonthMaxTotal.Text = Globals.FormatMoney(money);
            }
        }

        private void btnCreateReportOfDay_Click(object sender, EventArgs e)
        {
            string s = string.Empty + string.Format("总{0}：", radioDayForSaleType.SelectedItem.Text) + "," + litDayAllTotal.Text + ",\"\"," + string.Format("最高峰{0}：", radioDayForSaleType.SelectedItem.Text) + "," + litDayMaxTotal.Text + "\r\n\r\n日期," + radioDayForSaleType.SelectedItem.Text + ",比例\r\n";
            foreach (DataRow row in TableOfDay.Rows)
            {
                s = s + row["Date"].ToString();
                s = s + "," + row["SaleTotal"].ToString();
                s = s + "," + row["Percentage"].ToString() + "%\r\n";
            }
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "GB2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=SaleTotalStatistics.csv");
            Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Page.Response.ContentType = "application/octet-stream";
            Page.EnableViewState = false;
            Page.Response.Write(s);
            Page.Response.End();
        }

        private void btnCreateReportOfMonth_Click(object sender, EventArgs e)
        {
            string s = string.Empty + string.Format("总{0}：", radioMonthForSaleType.SelectedItem.Text) + "," + litMonthAllTotal.Text + ",\"\"," + string.Format("最高峰{0}：", radioMonthForSaleType.SelectedItem.Text) + "," + litMonthMaxTotal.Text + "\r\n\r\n月份," + radioMonthForSaleType.SelectedItem.Text + ",比例\r\n";
            foreach (DataRow row in TableOfMonth.Rows)
            {
                s = s + row["Date"].ToString();
                s = s + "," + row["SaleTotal"].ToString();
                s = s + "," + row["Percentage"].ToString() + "%\r\n";
            }
            Page.Response.Clear();
            Page.Response.Buffer = false;
            Page.Response.Charset = "GB2312";
            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=SaleTotalStatistics.csv");
            Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Page.Response.ContentType = "application/octet-stream";
            Page.EnableViewState = false;
            Page.Response.Write(s);
            Page.Response.End();
        }

        private void btnQueryDaySaleTotal_Click(object sender, EventArgs e)
        {
            ReBind();
        }

        private void btnQueryMonthSaleTotal_Click(object sender, EventArgs e)
        {
            ReBind();
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["monthYear"]))
                {
                    int.TryParse(Page.Request.QueryString["monthYear"], out monthYear);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["monthType"]))
                {
                    monthType = (SaleStatisticsType)Convert.ToInt32(Page.Request.QueryString["monthType"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dayYear"]))
                {
                    int.TryParse(Page.Request.QueryString["dayYear"], out dayYear);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dayMonth"]))
                {
                    int.TryParse(Page.Request.QueryString["dayMonth"], out dayMonth);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dayType"]))
                {
                    dayType = (SaleStatisticsType)Convert.ToInt32(Page.Request.QueryString["dayType"]);
                }
                dropMonthForYaer.SelectedValue = monthYear;
                radioMonthForSaleType.SelectedValue = monthType;
                dropDayForYear.SelectedValue = dayYear;
                dropMoth.SelectedValue = dayMonth;
                radioDayForSaleType.SelectedValue = dayType;
            }
            else
            {
                monthYear = dropMonthForYaer.SelectedValue;
                monthType = radioMonthForSaleType.SelectedValue;
                dayYear = dropDayForYear.SelectedValue;
                dayMonth = dropMoth.SelectedValue;
                dayType = radioDayForSaleType.SelectedValue;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQueryMonthSaleTotal.Click += new EventHandler(btnQueryMonthSaleTotal_Click);
            btnCreateReportOfMonth.Click += new EventHandler(btnCreateReportOfMonth_Click);
            btnQueryDaySaleTotal.Click += new EventHandler(btnQueryDaySaleTotal_Click);
            btnCreateReportOfDay.Click += new EventHandler(btnCreateReportOfDay_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindMonthSaleTotalStatistics();
                BindDaySaleTotalStatistics();
            }
        }

        private void ReBind()
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("monthYear", dropMonthForYaer.SelectedValue.ToString());
            queryStrings.Add("monthType", ((int)radioMonthForSaleType.SelectedValue).ToString());
            queryStrings.Add("dayYear", dropDayForYear.SelectedValue.ToString());
            queryStrings.Add("dayMonth", dropMoth.SelectedValue.ToString());
            queryStrings.Add("dayType", ((int)radioDayForSaleType.SelectedValue).ToString());
            base.ReloadPage(queryStrings);
        }

        public DataTable TableOfDay
        {
            get
            {
                if (ViewState["TableOfDay"] != null)
                {
                    return (DataTable)ViewState["TableOfDay"];
                }
                return null;
            }
            set
            {
                ViewState["TableOfDay"] = value;
            }
        }

        public DataTable TableOfMonth
        {
            get
            {
                if (ViewState["TableOfMonth"] != null)
                {
                    return (DataTable)ViewState["TableOfMonth"];
                }
                return null;
            }
            set
            {
                ViewState["TableOfMonth"] = value;
            }
        }
    }
}

