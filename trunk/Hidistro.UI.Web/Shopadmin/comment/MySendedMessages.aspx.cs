namespace Hidistro.UI.Web.Shopadmin
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public partial class MySendedMessages : DistributorPage
    {

        private void BindData()
        {
            SendedMessageQuery query = new SendedMessageQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.UserName = HiContext.Current.User.Username;
            DbQueryResult sendedMessages = SubsiteCommentsHelper.GetSendedMessages(query);
            this.messagesList.DataSource = sendedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = sendedMessages.TotalRecords;
            this.pager1.TotalRecords = sendedMessages.TotalRecords;
        }

        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> sendedMessageList = new List<long>();
            foreach (GridViewRow row in this.messagesList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    long item = (long) this.messagesList.DataKeys[row.RowIndex].Value;
                    sendedMessageList.Add(item);
                }
            }
            if (sendedMessageList.Count > 0)
            {
                SubsiteCommentsHelper.DeleteSendedMessages(sendedMessageList);
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
            long sendMessageId = (long) this.messagesList.DataKeys[e.RowIndex].Value;
            if (SubsiteCommentsHelper.DeleteSendedMessage(sendMessageId))
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
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }
    }
}

