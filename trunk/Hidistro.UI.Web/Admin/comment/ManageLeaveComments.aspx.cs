using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.ManageLeaveComments)]
    public partial class ManageLeaveComments : AdminPage
    {

        private void BindList()
        {
            LeaveCommentQuery query = new LeaveCommentQuery();
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
            {
                query.MessageStatus = (MessageStatus)int.Parse(base.Request.QueryString["MessageStatus"]);
                statusList.SelectedValue = query.MessageStatus;
            }
            DbQueryResult leaveComments = NoticeHelper.GetLeaveComments(query);
            leaveList.DataSource = leaveComments.Data;
            leaveList.DataBind();
            pager.TotalRecords = leaveComments.TotalRecords;
            pager1.TotalRecords = leaveComments.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> leaveIds = new List<long>();
            foreach (GridViewRow row in leaveList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    long item = (long)leaveList.DataKeys[row.RowIndex].Value;
                    leaveIds.Add(item);
                }
            }
            if (leaveIds.Count > 0)
            {
                NoticeHelper.DeleteLeaveComments(leaveIds);
                ShowMsg("成功删除了选择的消息.", true);
            }
            else
            {
                ShowMsg("请选择需要删除的消息.", false);
            }
            BindList();
        }

        private void leaveList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long leaveId = (long)leaveList.DataKeys[e.RowIndex].Value;
            if (NoticeHelper.DeleteLeaveComment(leaveId))
            {
                ShowMsg("删除成功", true);
                BindList();
            }
            else
            {
                ShowMsg("删除失败", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            leaveList.RowDeleting += new GridViewDeleteEventHandler(leaveList_RowDeleting);
            btnDeleteSelect.Click += new EventHandler(btnDeleteSelect_Click);
            btnDeleteSelect1.Click += new EventHandler(btnDeleteSelect_Click);
            statusList.SelectedIndexChanged += new EventHandler(statusList_SelectedIndexChanged);
            statusList.AutoPostBack = true;
            if (!Page.IsPostBack)
            {
                BindList();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void statusList_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("MessageStatus", ((int)statusList.SelectedValue).ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

