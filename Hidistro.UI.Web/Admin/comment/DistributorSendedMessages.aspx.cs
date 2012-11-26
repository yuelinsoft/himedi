using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    public partial class DistributorSendedMessages : AdminPage
    {
        private void BindData()
        {
            SendedMessageQuery query = new SendedMessageQuery();
            query.UserName = "admin";
            query.SortBy = messagesList.SortOrderBy;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            if (messagesList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult distributorSendedMessages = NoticeHelper.GetDistributorSendedMessages(query);
            messagesList.DataSource = distributorSendedMessages.Data;
            messagesList.DataBind();
            pager.TotalRecords = distributorSendedMessages.TotalRecords;
            pager1.TotalRecords = distributorSendedMessages.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> sendedMessageList = new List<long>();
            foreach (GridViewRow row in messagesList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    long item = (long)messagesList.DataKeys[row.RowIndex].Value;
                    sendedMessageList.Add(item);
                }
            }
            if (sendedMessageList.Count > 0)
            {
                NoticeHelper.DeleteSendedMessages(sendedMessageList);
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
            long sendMessageId = (long)messagesList.DataKeys[e.RowIndex].Value;
            if (NoticeHelper.DeleteSendedMessage(sendMessageId))
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
            if (!Page.IsPostBack)
            {
                BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }
    }
}

