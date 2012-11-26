
using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Members)]
    public partial class ManageMembers : AdminPage
    {
        int? rankId;
        bool? approved;
        string realName;
        string searchKey;

        protected void BindData()
        {
            MemberQuery query = new MemberQuery();
            query.Username = searchKey;
            query.Realname = realName;
            query.GradeId = rankId;
            query.PageIndex = pager.PageIndex;
            query.IsApproved = approved;
            query.SortBy = grdMemberList.SortOrderBy;
            query.PageSize = pager.PageSize;
            if (grdMemberList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult members = MemberHelper.GetMembers(query);
            grdMemberList.DataSource = members.Data;
            grdMemberList.DataBind();
            pager1.TotalRecords = pager.TotalRecords = members.TotalRecords;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (exportFieldsCheckBoxList.SelectedItem == null)
            {
                ShowMsg("请选择需要导出的会员信息", false);
            }
            else
            {
                IList<string> fields = new List<string>();
                IList<string> list2 = new List<string>();
                foreach (ListItem item in exportFieldsCheckBoxList.Items)
                {
                    if (item.Selected)
                    {
                        fields.Add(item.Value);
                        list2.Add(item.Text);
                    }
                }
                MemberQuery query = new MemberQuery();
                query.Username = searchKey;
                query.Realname = realName;
                query.GradeId = rankId;
                DataTable membersNopage = MemberHelper.GetMembersNopage(query, fields);
                StringBuilder builder = new StringBuilder();
                foreach (string str in list2)
                {
                    builder.Append(str + ",");
                    if (str == list2[list2.Count - 1])
                    {
                        builder = builder.Remove(builder.Length - 1, 1);
                        builder.Append("\r\n");
                    }
                }
                foreach (DataRow row in membersNopage.Rows)
                {
                    foreach (string str2 in fields)
                    {
                        builder.Append(row[str2]).Append(",");
                        if (str2 == fields[list2.Count - 1])
                        {
                            builder = builder.Remove(builder.Length - 1, 1);
                            builder.Append("\r\n");
                        }
                    }
                }
                Page.Response.Clear();
                Page.Response.Buffer = false;
                Page.Response.Charset = "GB2312";
                if (exportFormatRadioButtonList.SelectedValue == "csv")
                {
                    Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.csv");
                    Page.Response.ContentType = "application/octet-stream";
                }
                else
                {
                    Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberInfo.txt");
                    Page.Response.ContentType = "application/vnd.ms-word";
                }
                Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
                Page.EnableViewState = false;
                Page.Response.Write(builder.ToString());
                Page.Response.End();
            }
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReBind(true);
        }

        private void ddlApproved_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReBind(false);
        }

        private void grdMemberList_ReBindData(object sender)
        {
            ReBind(false);
        }

        private void grdMemberList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            int userId = (int)grdMemberList.DataKeys[e.RowIndex].Value;
            if (!MemberHelper.Delete(userId))
            {
                ShowMsg("未知错误", false);
            }
            else
            {
                BindData();
                ShowMsg("成功删除了选择的会员", true);
            }
        }

        private void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
            int num = 0;
            foreach (GridViewRow row in grdMemberList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked && MemberHelper.Delete(Convert.ToInt32(grdMemberList.DataKeys[row.RowIndex].Value)))
                {
                    num++;
                }
            }
            if (num == 0)
            {
                ShowMsg("请先选择要删除的会员账号", false);
            }
            else
            {
                BindData();
                ShowMsg("成功删除了选择的会员", true);
            }
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["rankId"], out result))
                {
                    rankId = new int?(result);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["searchKey"]))
                {
                    searchKey = base.Server.UrlDecode(Page.Request.QueryString["searchKey"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["realName"]))
                {
                    realName = base.Server.UrlDecode(Page.Request.QueryString["realName"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["Approved"]))
                {
                    approved = new bool?(Convert.ToBoolean(Page.Request.QueryString["Approved"]));
                }
                rankList.SelectedValue = rankId;
                txtSearchText.Text = searchKey;
                txtRealName.Text = realName;
            }
            else
            {
                rankId = rankList.SelectedValue;
                searchKey = txtSearchText.Text;
                realName = txtRealName.Text.Trim();
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdMemberList.RowDeleting += new GridViewDeleteEventHandler(grdMemberList_RowDeleting);
            grdMemberList.ReBindData += new Grid.ReBindDataEventHandler(grdMemberList_ReBindData);
            lkbDelectCheck.Click += new EventHandler(lkbDelectCheck_Click);
            lkbDelectCheck1.Click += new EventHandler(lkbDelectCheck_Click);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            btnExport.Click += new EventHandler(btnExport_Click);
            ddlApproved.AutoPostBack = true;
            ddlApproved.SelectedIndexChanged += new EventHandler(ddlApproved_SelectedIndexChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                rankList.DataBind();
                rankList.SelectedValue = rankId;
                ddlApproved.DataBind();
                if (approved.HasValue)
                {
                    ddlApproved.SelectedValue = approved;
                }
                BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (rankList.SelectedValue.HasValue)
            {
                queryStrings.Add("rankId", rankList.SelectedValue.Value.ToString(CultureInfo.InvariantCulture));
            }
            queryStrings.Add("searchKey", txtSearchText.Text);
            queryStrings.Add("realName", txtRealName.Text);
            queryStrings.Add("pageSize", pager.PageSize.ToString(CultureInfo.InvariantCulture));
            queryStrings.Add("Approved", ddlApproved.SelectedValue.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

