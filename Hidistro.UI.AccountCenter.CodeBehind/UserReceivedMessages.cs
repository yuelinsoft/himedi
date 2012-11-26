namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.AccountCenter.Comments;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI.WebControls;

    public class UserReceivedMessages : MemberTemplatedWebControl
    {
       IButton btnDeleteSelect;
       Common_Messages_UserReceivedMessageList CmessagesList;
       Grid messagesList;
       Pager pager;

        protected override void AttachChildControls()
        {
            this.CmessagesList = (Common_Messages_UserReceivedMessageList) this.FindControl("Grid_Common_Messages_UserReceivedMessageList");
            this.messagesList = (Grid) this.CmessagesList.FindControl("gridMessageList");
            this.pager = (Pager) this.FindControl("pager");
            this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            this.messagesList.RowDeleting += new GridViewDeleteEventHandler(this.messagesList_RowDeleted);
            if (!this.Page.IsPostBack)
            {
                PageTitle.AddSiteNameTitle("收件箱", HiContext.Current.Context);
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

       void BindData()
        {
            ReceivedMessageQuery query = new ReceivedMessageQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.UserName = HiContext.Current.User.Username;
            DbQueryResult receivedMessages = CommentsHelper.GetReceivedMessages(query);
            if (((DataTable) receivedMessages.Data).Rows.Count <= 0)
            {
                query.PageIndex = this.messagesList.PageIndex - 1;
                receivedMessages = CommentsHelper.GetReceivedMessages(query);
                this.messagesList.DataSource = receivedMessages.Data;
            }
            this.messagesList.DataSource = receivedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = receivedMessages.TotalRecords;
        }

       void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> receivedMessageList = new List<long>();
            foreach (GridViewRow row in this.messagesList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    Label label = (Label) row.FindControl("lblMessage");
                    if (label != null)
                    {
                        receivedMessageList.Add(Convert.ToInt64(label.Text));
                    }
                }
            }
            if (receivedMessageList.Count > 0)
            {
                CommentsHelper.DeleteReceiedMessages(receivedMessageList);
            }
            else
            {
                this.ShowMessage("请选中要删除的收件", false);
            }
            this.BindData();
        }

       void messagesList_RowDeleted(object sender, GridViewDeleteEventArgs e)
        {
            Label label = (Label) this.messagesList.Rows[e.RowIndex].FindControl("lblMessage");
            if (label != null)
            {
                CommentsHelper.DeleteReceivedMessage(Convert.ToInt64(label.Text));
            }
            this.BindData();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserReceivedMessages.html";
            }
            base.OnInit(e);
        }
    }
}

