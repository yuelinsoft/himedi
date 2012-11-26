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
    using System.Web.UI.WebControls;

    public class UserSendedMessages : MemberTemplatedWebControl
    {
       IButton btnDeleteSelect;
       Common_Messages_UserSendedMessageList CmessagesList;
       Grid messagesList;
       Pager pager;

        protected override void AttachChildControls()
        {
            this.CmessagesList = (Common_Messages_UserSendedMessageList) this.FindControl("Grid_Common_Messages_UserSendedMessageList");
            this.messagesList = (Grid) this.CmessagesList.FindControl("messagesList");
            this.pager = (Pager) this.FindControl("pager");
            this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
            this.messagesList.RowDeleting += new GridViewDeleteEventHandler(this.messagesList_RowDeleted);
            this.btnDeleteSelect.Click += new EventHandler(this.btnDeleteSelect_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
        }

       void BindData()
        {
            SendedMessageQuery query = new SendedMessageQuery();
            query.PageIndex = this.pager.PageIndex;
            query.PageSize = this.pager.PageSize;
            query.UserName = HiContext.Current.User.Username;
            DbQueryResult sendedMessages = CommentsHelper.GetSendedMessages(query);
            this.messagesList.DataSource = sendedMessages.Data;
            this.messagesList.DataBind();
            this.pager.TotalRecords = sendedMessages.TotalRecords;
        }

       void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            IList<long> sendedMessageList = new List<long>();
            foreach (GridViewRow row in this.messagesList.Rows)
            {
                CheckBox box = (CheckBox) row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    Label label = (Label) row.FindControl("lblMessage");
                    if (label != null)
                    {
                        sendedMessageList.Add(Convert.ToInt64(label.Text));
                    }
                }
            }
            if (sendedMessageList.Count > 0)
            {
                CommentsHelper.DeleteSendedMessages(sendedMessageList);
                this.BindData();
            }
            else
            {
                this.ShowMessage("请选中要删除的信息", false);
            }
        }

       void messagesList_RowDeleted(object sender, GridViewDeleteEventArgs e)
        {
            Label label = (Label) this.messagesList.Rows[e.RowIndex].FindControl("lblMessage");
            if (label != null)
            {
                CommentsHelper.DeleteSendedMessage(Convert.ToInt64(label.Text));
            }
            this.BindData();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "User/Skin-UserSendedMessages.html";
            }
            base.OnInit(e);
        }
    }
}

