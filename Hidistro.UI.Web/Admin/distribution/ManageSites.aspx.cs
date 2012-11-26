using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    /// <summary>
    /// 站点管理
    /// </summary>
    [PrivilegeCheck(Privilege.ManageDistributorSites)]
    public partial class ManageSites : AdminPage
    {
        string trueName;
        string userName;

        private void BindSites()
        {
            Pagination pagination = new Pagination();
            pagination.PageIndex = pager.PageIndex;
            pagination.PageSize = pager.PageSize;
            int total = 0;
            DataTable table = DistributorHelper.GetDistributorSites(pagination, userName, trueName, out total);
            grdDistributorSites.DataSource = table;
            grdDistributorSites.DataBind();
            pager.TotalRecords = total;
            pager1.TotalRecords = total;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(int.Parse(hidUserId.Value));
            siteSettings.SiteUrl = txtFirstSiteUrl.Text.Trim();
            siteSettings.SiteUrl2 = txtSencondSiteUrl.Text.Trim();
            siteSettings.RecordCode = txtFirstRecordCode.Text.Trim();
            siteSettings.RecordCode2 = txtSecondRecordCode.Text.Trim();
            SettingsManager.Save(siteSettings);
            BindSites();
            ShowMsg("修改域名成功", true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void grdDistributorSites_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "open")
            {
                int num = Convert.ToInt32(e.CommandArgument);
                int userId = Convert.ToInt32(grdDistributorSites.DataKeys[num].Value);
                Literal literal = (Literal)grdDistributorSites.Rows[num].Cells[3].FindControl("litState");
                if (literal.Text == "开启")
                {
                    if (!DistributorHelper.CloseSite(userId))
                    {
                        ShowMsg("暂停失败", false);
                    }
                }
                else if (!DistributorHelper.OpenSite(userId))
                {
                    ShowMsg("开启失败", false);
                }
            }
            BindSites();
        }

        protected void grdDistributorSites_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = (Literal)e.Row.FindControl("litState");
                ImageLinkButton button = (ImageLinkButton)e.Row.FindControl("btnIsOpen");
                button.CommandArgument = e.Row.RowIndex.ToString();
                if (literal.Text == "False")
                {
                    literal.Text = "开启";
                    button.Text = "暂停";
                    button.DeleteMsg = "暂停后，该分销子站将不能访问，但分销商仍可登录后台，确认要暂停吗？";
                }
                else
                {
                    literal.Text = "暂停";
                    button.Text = "开启";
                    button.DeleteMsg = "开启后，该分销子站将可以重新访问，确认要开启吗？";
                }
            }
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = Page.Request.QueryString["userName"];
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["trueName"]))
                {
                    trueName = Page.Request.QueryString["trueName"];
                }
                txtDistributorName.Text = userName;
                txtTrueName.Text = trueName;
            }
            else
            {
                userName = txtDistributorName.Text.Trim();
                trueName = txtTrueName.Text.Trim();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request["showMessage"]) && (Request["showMessage"] == "true"))
            {
                int result = 0;

                if (!(!string.IsNullOrEmpty(Request["userId"]) && int.TryParse(Request["userId"], out result)))
                {
                    Response.Write("{\"Status\":\"0\"}");
                    Response.End();
                    return;
                }

                SiteSettings siteSettings = SettingsManager.GetSiteSettings(result);

                if (siteSettings == null)
                {
                    Response.Write("{\"Status\":\"0\"}");
                    Response.End();
                    return;
                }

                Distributor distributor = DistributorHelper.GetDistributor(siteSettings.UserId.Value);
                if (distributor == null)
                {
                    Response.Write("{\"Status\":\"0\"}");
                    Response.End();
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
                builder.AppendFormat(",\"Domain1\":\"{0}\"", siteSettings.SiteUrl);
                builder.AppendFormat(",\"Domain2\":\"{0}\"", siteSettings.SiteUrl2);
                builder.AppendFormat(",\"Code1\":\"{0}\"", siteSettings.RecordCode);
                builder.AppendFormat(",\"Code2\":\"{0}\"", siteSettings.RecordCode2);
                Response.Clear();
                Response.ContentType = "application/json";
                Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                Response.End();
            }
            grdDistributorSites.RowDataBound += new GridViewRowEventHandler(grdDistributorSites_RowDataBound);
            grdDistributorSites.RowCommand += new GridViewCommandEventHandler(grdDistributorSites_RowCommand);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            LoadParameters();
            if (!IsPostBack)
            {
                litServerIp.Text = Request.ServerVariables.Get("Local_Addr");
                BindSites();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", txtDistributorName.Text.Trim());
            queryStrings.Add("trueName", txtTrueName.Text.Trim());
            queryStrings.Add("pageSize", hrefPageSize.SelectedSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            ReloadPage(queryStrings);
        }
    }
}

