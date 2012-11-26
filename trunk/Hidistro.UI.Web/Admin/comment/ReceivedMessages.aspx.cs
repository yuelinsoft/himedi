using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
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
    [PrivilegeCheck(Privilege.ReceivedMessages)]
    public partial class ReceivedMessages : AdminPage
    {
        private void BindData()
        {
            int num;
            ReceivedMessageQuery query = new ReceivedMessageQuery();
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.SortBy = messagesList.SortOrderBy;
            if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
            {
                query.MessageStatus = (MessageStatus)int.Parse(base.Request.QueryString["MessageStatus"]);
                statusList.SelectedValue = query.MessageStatus;
            }
            if (int.TryParse(base.Request.QueryString["IsRead"], out num))
            {
                query.IsRead = new bool?(Convert.ToBoolean(num));
            }
            query.UserName = "admin";
            if (messagesList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult receivedMessages = NoticeHelper.GetReceivedMessages(query);
            messagesList.DataSource = receivedMessages.Data;
            messagesList.DataBind();
            pager.TotalRecords = receivedMessages.TotalRecords;
            pager1.TotalRecords = receivedMessages.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> receivedMessageList = new List<long>();
            foreach (GridViewRow row in messagesList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    long item = (long)messagesList.DataKeys[row.RowIndex].Value;
                    receivedMessageList.Add(item);
                }
            }
            if (receivedMessageList.Count > 0)
            {
                NoticeHelper.DeleteReceiedMessages(receivedMessageList);
                ShowMsg("成功删除了选择的消息.", true);
            }
            else
            {
                ShowMsg("请选择需要删除的消息.", false);
            }
            BindData();
        }

        private void messagesList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long receiveMessageId = (long)messagesList.DataKeys[e.RowIndex].Value;
            if (NoticeHelper.DeleteReceivedMessage(receiveMessageId))
            {
                ShowMsg("删除成功", true);
                BindData();
            }
            else
            {
                ShowMsg("删除失败", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            messagesList.RowDeleting += new GridViewDeleteEventHandler(messagesList_RowDeleting);
            btnDeleteSelect.Click += new EventHandler(btnDeleteSelect_Click);
            btnDeleteSelect1.Click += new EventHandler(btnDeleteSelect_Click);
            statusList.SelectedIndexChanged += new EventHandler(statusList_SelectedIndexChanged);
            if (!Page.IsPostBack)
            {
                statusList.DataBind();
                statusList.AutoPostBack = true;
                BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void statusList_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("MessageStatus", ((int)statusList.SelectedValue).ToString());
            if (!string.IsNullOrEmpty(base.Request.QueryString["IsRead"]))
            {
                queryStrings.Add("IsRead", base.Request.QueryString["IsRead"]);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

