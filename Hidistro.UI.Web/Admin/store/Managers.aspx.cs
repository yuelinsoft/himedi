using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class Managers : AdminPage
    {
        private void BindData()
        {
            ManagerQuery managerQuery = GetManagerQuery();
            DbQueryResult managers = ManagerHelper.GetManagers(managerQuery);
            grdManager.DataSource = managers.Data;
            grdManager.DataBind();
            txtSearchText.Text = managerQuery.Username;
            dropRolesList.SelectedValue = managerQuery.RoleId;
            pager.TotalRecords = managers.TotalRecords;
            pager1.TotalRecords = managers.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReloadManagerLogs(true);
        }

        private ManagerQuery GetManagerQuery()
        {
            ManagerQuery query = new ManagerQuery();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["Username"]))
            {
                query.Username = base.Server.UrlDecode(Page.Request.QueryString["Username"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["RoleId"]))
            {
                query.RoleId = new Guid(Page.Request.QueryString["RoleId"]);
            }
            query.PageSize = pager.PageSize;
            query.PageIndex = pager.PageIndex;
            if (!string.IsNullOrEmpty(Page.Request.QueryString["SortBy"]))
            {
                query.SortBy = Page.Request.QueryString["SortBy"];
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["SortOrder"]))
            {
                query.SortOrder = SortAction.Desc;
            }
            return query;
        }

        private void grdManager_ReBindData(object sender)
        {
            ReloadManagerLogs(false);
        }

        private void grdManager_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = (int)grdManager.DataKeys[e.RowIndex].Value;
            if (HiContext.Current.User.UserId == userId)
            {
                ShowMsg("不能删除自己", false);
            }
            else if (!ManagerHelper.Delete(ManagerHelper.GetManager(userId).UserId))
            {
                ShowMsg("未知错误", false);
            }
            else
            {
                BindData();
                ShowMsg("成功删除了一个管理员", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            grdManager.ReBindData += new Grid.ReBindDataEventHandler(grdManager_ReBindData);

            grdManager.RowDeleting += new GridViewDeleteEventHandler(grdManager_RowDeleting);

            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);

            if (!Page.IsPostBack)
            {
                dropRolesList.DataBind();

                BindData();
            }

        }

        private void ReloadManagerLogs(bool isSearch)
        {

            NameValueCollection queryStrings = new NameValueCollection();

            queryStrings.Add("Username", txtSearchText.Text);

            queryStrings.Add("RoleId", Convert.ToString(dropRolesList.SelectedValue));

            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }

            queryStrings.Add("SortBy", grdManager.SortOrderBy);

            queryStrings.Add("SortOrder", SortAction.Desc.ToString());

            base.ReloadPage(queryStrings);

        }
    }
}

