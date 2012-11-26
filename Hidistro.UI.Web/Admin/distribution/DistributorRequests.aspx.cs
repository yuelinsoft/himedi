
using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.DistributorRequests)]
    public partial class DistributorRequests : AdminPage
    {
        string Keywords;
        string RealName;

        private void BindDistributorRequest()
        {
            DistributorQuery query = new DistributorQuery();
            query.IsApproved = false;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.Username = Keywords;
            query.RealName = RealName;
            query.SortBy = "CreateDate";
            query.SortOrder = SortAction.Desc;
            DbQueryResult distributors = DistributorHelper.GetDistributors(query);
            grdDistributorRequests.DataSource = distributors.Data;
            grdDistributorRequests.DataBind();
            pager.TotalRecords = distributors.TotalRecords;
            pager1.TotalRecords = distributors.TotalRecords;
        }

        private void btnRefuseDistrbutor_Click(object sender, EventArgs e)
        {
            if (DistributorHelper.RefuseDistributorRequest(int.Parse(txtUserId.Value)))
            {
                ShowMsg("成功的删除了申请信息", true);
                BindDistributorRequest();
            }
            else
            {
                ShowMsg("拒绝失败", false);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Keywords", txtUserName.Text.Trim());
            queryStrings.Add("RealName", txtRealName.Text.Trim());
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            queryStrings.Add("SortBy", grdDistributorRequests.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int result = 0;
                if (!(!string.IsNullOrEmpty(base.Request["id"]) && int.TryParse(base.Request["id"], out result)))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                Distributor distributor = DistributorHelper.GetDistributor(result);
                if (distributor == null)
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
                builder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
                builder.AppendFormat(",\"CompanyName\":\"{0}\"", distributor.CompanyName);
                builder.AppendFormat(",\"Email\":\"{0}\"", distributor.Email);
                builder.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor.RegionId, string.Empty));
                builder.AppendFormat(",\"Address\":\"{0}\"", distributor.Address);
                builder.AppendFormat(",\"QQ\":\"{0}\"", distributor.QQ);
                builder.AppendFormat(",\"MSN\":\"{0}\"", distributor.MSN);
                builder.AppendFormat(",\"PostCode\":\"{0}\"", distributor.Zipcode);
                builder.AppendFormat(",\"Wangwang\":\"{0}\"", distributor.Wangwang);
                builder.AppendFormat(",\"CellPhone\":\"{0}\"", distributor.CellPhone);
                builder.AppendFormat(",\"Telephone\":\"{0}\"", distributor.TelPhone);
                builder.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor.CreateDate);
                builder.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor.LastLoginDate);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnRefuseDistrbutor.Click += new EventHandler(btnRefuseDistrbutor_Click);
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["Keywords"]))
                {
                    Keywords = Page.Request.QueryString["Keywords"];
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["RealName"]))
                {
                    RealName = Page.Request.QueryString["RealName"];
                }
                txtUserName.Text = Keywords;
                txtRealName.Text = RealName;
            }
            else
            {
                Keywords = txtUserName.Text.Trim();
                RealName = txtRealName.Text.Trim();
            }
            if (!Page.IsPostBack)
            {
                BindDistributorRequest();
            }
        }
    }
}

