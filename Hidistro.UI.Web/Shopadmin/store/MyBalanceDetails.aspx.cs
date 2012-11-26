using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyBalanceDetails : DistributorPage
    {

        DateTime? dataEnd;
        DateTime? dataStart;


        private void btnQueryBalanceDetails_Click(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("dataStart", calendarStart.SelectedDate.ToString());
            queryStrings.Add("dataEnd", calendarEnd.SelectedDate.ToString());
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }

        private void GetBalanceDetails()
        {
            BalanceDetailQuery query = new BalanceDetailQuery();
            query.FromDate = dataStart;
            query.ToDate = dataEnd;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            DbQueryResult myBalanceDetails = SubsiteStoreHelper.GetMyBalanceDetails(query);
            grdBalanceDetails.DataSource = myBalanceDetails.Data;
            grdBalanceDetails.DataBind();
            pager.TotalRecords = myBalanceDetails.TotalRecords;
            pager1.TotalRecords = myBalanceDetails.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataStart"]))
                {
                    dataStart = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataStart"])));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["dataEnd"]))
                {
                    dataEnd = new DateTime?(Convert.ToDateTime(base.Server.UrlDecode(Page.Request.QueryString["dataEnd"])));
                }
                calendarStart.SelectedDate = dataStart;
                calendarEnd.SelectedDate = dataEnd;
            }
            else
            {
                dataStart = calendarStart.SelectedDate;
                dataEnd = calendarEnd.SelectedDate;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQueryBalanceDetails.Click += new EventHandler(btnQueryBalanceDetails_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!base.IsPostBack)
            {
                GetBalanceDetails();
            }
        }
    }
}

