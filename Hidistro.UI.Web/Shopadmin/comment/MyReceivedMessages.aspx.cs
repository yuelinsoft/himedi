namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.UI.WebControls;

    public partial class MyReceivedMessages : DistributorPage
    {

        private void BindData()
        {
            int num;
            ReceivedMessageQuery query2 = new ReceivedMessageQuery();
            query2.PageIndex = this.pager.PageIndex;
            query2.PageSize = this.pager.PageSize;
            query2.SortBy = this.messagesList.SortOrderBy;
            query2.UserName = HiContext.Current.User.Username;
            ReceivedMessageQuery query = query2;
            if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
            {
                query.MessageStatus = (MessageStatus) int.Parse(base.Request.QueryString["MessageStatus"]);
                this.statusList.SelectedValue = query.MessageStatus;
            }
            if (int.TryParse(base.Request.QueryString["IsRead"], out num))
            {
                query.IsRead = new bool?(Convert.ToBoolean(num));
            }
            if (this.messagesList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult receivedMessages = SubsiteCommentsHelper.GetReceivedMessages(query);
            this.messagesList.DataSource = receivedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = receivedMessages.TotalRecords;
            this.pager1.TotalRecords = receivedMessages.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> receivedMessageList = new List<long>();
            foreach (GridViewRow row in this.messagesList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    long item = (long) this.messagesList.DataKeys[row.RowIndex].Value;
                    receivedMessageList.Add(item);
                }
            }
            if (receivedMessageList.Count > 0)
            {
                SubsiteCommentsHelper.DeleteReceiedMessages(receivedMessageList);
                this.ShowMsg("成功删除了选择的消息.", true);
            }
            else
            {
                this.ShowMsg("请选择需要删除的消息.", false);
            }
            this.BindData();
        }

        private void messagesList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            long receiveMessageId = (long) this.messagesList.DataKeys[e.RowIndex].Value;
            if (SubsiteCommentsHelper.DeleteReceivedMessage(receiveMessageId))
            {
                this.ShowMsg("删除成功", true);
                this.BindData();
            }
            else
            {
                this.ShowMsg("删除失败", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.messagesList.RowDeleting += new GridViewDeleteEventHandler(this.messagesList_RowDeleting);
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            this.btnDeleteSelect1.Click += new EventHandler(this.btnDeleteSelect_Click);
            this.statusList.SelectedIndexChanged += new EventHandler(this.statusList_SelectedIndexChanged);
            this.statusList.AutoPostBack = true;
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

        private void statusList_SelectedIndexChanged(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("MessageStatus", ((int) this.statusList.SelectedValue).ToString());
            if (!string.IsNullOrEmpty(base.Request.QueryString["IsRead"]))
            {
                queryStrings.Add("IsRead", base.Request.QueryString["IsRead"]);
            }
            base.ReloadPage(queryStrings);
        }
    }
}

