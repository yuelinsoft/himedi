using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class ManageUnderlings : DistributorPage
    {


        private void BindData()
        {
            MemberQuery memberQuery = GetMemberQuery();
            DbQueryResult members = UnderlingHelper.GetMembers(memberQuery);
            grdUnderlings.DataSource = members.Data;
            grdUnderlings.DataBind();
            pager.TotalRecords = members.TotalRecords;
            pager1.TotalRecords = members.TotalRecords;
            txtUsername.Text = memberQuery.Username;
            txtRealName.Text = memberQuery.Realname;
            dropMemberGrade.SelectedValue = memberQuery.GradeId;
            ddlApproved.SelectedValue = memberQuery.IsApproved;
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
                query.Username = txtUsername.Text.Trim();
                query.Realname = txtRealName.Text.Trim();
                query.GradeId = dropMemberGrade.SelectedValue;
                DataTable membersNopage = UnderlingHelper.GetMembersNopage(query, fields);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadManageUnderlings(true);
        }

        private void ddlApproved_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadManageUnderlings(false);
        }

        private MemberQuery GetMemberQuery()
        {
            MemberQuery query = new MemberQuery();
            if (!string.IsNullOrEmpty(Page.Request.QueryString["GradeId"]))
            {
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["GradeId"], out result))
                {
                    query.GradeId = new int?(result);
                }
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["Username"]))
            {
                query.Username = base.Server.UrlDecode(Page.Request.QueryString["Username"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["Realname"]))
            {
                query.Realname = base.Server.UrlDecode(Page.Request.QueryString["Realname"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["Approved"]))
            {
                query.IsApproved = new bool?(Convert.ToBoolean(Page.Request.QueryString["Approved"]));
            }
            query.PageSize = pager.PageSize;
            query.PageIndex = pager.PageIndex;
            return query;
        }

        protected void grdUnderlings_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = (int)grdUnderlings.DataKeys[e.RowIndex].Value;
            if (UnderlingHelper.DeleteMember(userId))
            {
                BindData();
                ShowMsg("成功删除了选择的会员", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        protected void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdUnderlings.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked && UnderlingHelper.DeleteMember(Convert.ToInt32(grdUnderlings.DataKeys[row.RowIndex].Value)))
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

        protected void Page_Load(object sender, EventArgs e)
        {
            grdUnderlings.RowDeleting += new GridViewDeleteEventHandler(grdUnderlings_RowDeleting);
            lkbDelectCheck.Click += new EventHandler(lkbDelectCheck_Click);
            lkbDelectCheck1.Click += new EventHandler(lkbDelectCheck_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExport.Click += new EventHandler(btnExport_Click);
            ddlApproved.AutoPostBack = true;
            ddlApproved.SelectedIndexChanged += new EventHandler(ddlApproved_SelectedIndexChanged);
            if (!Page.IsPostBack)
            {
                dropMemberGrade.DataBind();
                ddlApproved.DataBind();
                BindData();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReloadManageUnderlings(bool isSeach)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            if (dropMemberGrade.SelectedValue.HasValue)
            {
                queryStrings.Add("GradeId", dropMemberGrade.SelectedValue.Value.ToString());
            }
            queryStrings.Add("Username", txtUsername.Text);
            queryStrings.Add("Realname", txtRealName.Text);
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            queryStrings.Add("Approved", ddlApproved.SelectedValue.ToString());
            if (!isSeach)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

