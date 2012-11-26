using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorAccount)]
    public partial class DistributorAccountQuery : AdminPage
    {
        string realName;
        string searchKey;

        private void btnQuery_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        public void GetBalance()
        {
            DistributorQuery query = new DistributorQuery();
            query.Username = searchKey;
            query.RealName = realName;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            DbQueryResult distributorBalance = DistributorHelper.GetDistributorBalance(query);
            grdDistributorAccountList.DataSource = distributorBalance.Data;
            grdDistributorAccountList.DataBind();
            pager.TotalRecords = distributorBalance.TotalRecords;
            pager1.TotalRecords = distributorBalance.TotalRecords;
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
                {
                    searchKey = base.Server.UrlDecode(Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["realName"]))
                {
                    realName = base.Server.UrlDecode(Page.Request.QueryString["realName"]);
                }
                txtUserName.Text = searchKey;
                txtRealName.Text = realName;
            }
            else
            {
                searchKey = txtUserName.Text;
                realName = txtRealName.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnQuery.Click += new EventHandler(btnQuery_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!base.IsPostBack)
            {
                GetBalance();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("searchKey", txtUserName.Text);
            queryStrings.Add("realName", txtRealName.Text);
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("pageSize", pager.PageSize.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

