using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Distributor)]
    public partial class ManageDistributor : AdminPage
    {
        int? gradeId;
        int? lineId;
        string realName;
        string userName;

        private void BindDistributors()
        {
            DistributorQuery query = new DistributorQuery();
            query.IsApproved = true;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.GradeId = gradeId;
            query.LineId = lineId;
            query.Username = userName;
            query.RealName = realName;
            query.SortBy = grdDistributorList.SortOrderBy;
            if (grdDistributorList.SortOrder.ToLower() == "desc")
            {
                query.SortOrder = SortAction.Desc;
            }
            DbQueryResult distributors = DistributorHelper.GetDistributors(query);
            grdDistributorList.DataSource = distributors.Data;
            grdDistributorList.DataBind();
            pager.TotalRecords = distributors.TotalRecords;
            pager1.TotalRecords = distributors.TotalRecords;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (exportFieldsCheckBoxList.SelectedItem == null)
            {
                ShowMsg("请选择需要导出的分销商信息", false);
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
                DataTable distributorsNopage = DistributorHelper.GetDistributorsNopage(fields);
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
                foreach (DataRow row in distributorsNopage.Rows)
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
                    Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorInfo.csv");
                    Page.Response.ContentType = "application/octet-stream";
                }
                else
                {
                    Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorInfo.txt");
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

        private void CallBack()
        {
            bool flag = !string.IsNullOrEmpty(base.Request["showMessage"]) && (base.Request["showMessage"] == "true");
            if (!string.IsNullOrEmpty(base.Request["showDistributorAccountSummary"]) && (base.Request["showDistributorAccountSummary"] == "true"))
            {
                int result = 0;
                if (!(!string.IsNullOrEmpty(base.Request["id"]) && int.TryParse(base.Request["id"], out result)))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                    return;
                }
                Distributor distributor = DistributorHelper.GetDistributor(result);
                if (distributor == null)
                {
                    base.GotoResourceNotFound();
                    return;
                }
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(",\"AccountAmount\":\"{0}\"", distributor.Balance);
                builder.AppendFormat(",\"UseableBalance\":\"{0}\"", distributor.Balance - distributor.RequestBalance);
                builder.AppendFormat(",\"FreezeBalance\":\"{0}\"", distributor.RequestBalance);
                builder.AppendFormat(",\"DrawRequestBalance\":\"{0}\"", distributor.RequestBalance);
                builder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
                builder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
                base.Response.Clear();
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"Status\":\"1\"" + builder.ToString() + "}");
                base.Response.End();
            }
            if (flag)
            {
                int num2 = 0;
                if (!(!string.IsNullOrEmpty(base.Request["id"]) && int.TryParse(base.Request["id"], out num2)))
                {
                    base.Response.Write("{\"Status\":\"0\"}");
                    base.Response.End();
                }
                else
                {
                    Distributor distributor2 = DistributorHelper.GetDistributor(num2);
                    if (distributor2 == null)
                    {
                        base.Response.Write("{\"Status\":\"0\"}");
                        base.Response.End();
                    }
                    else
                    {
                        StringBuilder builder2 = new StringBuilder();
                        builder2.AppendFormat(",\"UserName\":\"{0}\"", distributor2.Username);
                        builder2.AppendFormat(",\"RealName\":\"{0}\"", distributor2.RealName);
                        builder2.AppendFormat(",\"CompanyName\":\"{0}\"", distributor2.CompanyName);
                        builder2.AppendFormat(",\"Email\":\"{0}\"", distributor2.Email);
                        builder2.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor2.RegionId, string.Empty));
                        builder2.AppendFormat(",\"Address\":\"{0}\"", distributor2.Address);
                        builder2.AppendFormat(",\"QQ\":\"{0}\"", distributor2.QQ);
                        builder2.AppendFormat(",\"MSN\":\"{0}\"", distributor2.MSN);
                        builder2.AppendFormat(",\"PostCode\":\"{0}\"", distributor2.Zipcode);
                        builder2.AppendFormat(",\"Wangwang\":\"{0}\"", distributor2.Wangwang);
                        builder2.AppendFormat(",\"CellPhone\":\"{0}\"", distributor2.CellPhone);
                        builder2.AppendFormat(",\"Telephone\":\"{0}\"", distributor2.TelPhone);
                        builder2.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor2.CreateDate);
                        builder2.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor2.LastLoginDate);
                        base.Response.Clear();
                        base.Response.ContentType = "application/json";
                        base.Response.Write("{\"Status\":\"1\"" + builder2.ToString() + "}");
                        base.Response.End();
                    }
                }
            }
        }

        private bool DeleteDistributorFile(int distributorUserId)
        {
            string path = Page.Request.MapPath(Globals.ApplicationPath + "/Storage/sites/") + distributorUserId;
            string str2 = Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + distributorUserId;
            if (Directory.Exists(path) && Directory.Exists(str2))
            {
                try
                {
                    DeleteFolder(path);
                    DeleteFolder(str2);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static void DeleteFolder(string dir)
        {
            foreach (string str in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(str))
                {
                    FileInfo info = new FileInfo(str);
                    if (info.Attributes.ToString().IndexOf("Readonly") != 1)
                    {
                        info.Attributes = FileAttributes.Normal;
                    }
                    File.Delete(str);
                }
                else
                {
                    DeleteFolder(str);
                }
            }
            Directory.Delete(dir);
        }

        private void grdDistributorList_RowCommand(object serder, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int distributorUserId = (int)grdDistributorList.DataKeys[rowIndex].Value;
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(distributorUserId);
            if (e.CommandName == "StopCooperation")
            {
                if (!((siteSettings == null) || siteSettings.Disabled))
                {
                    ShowMsg("请先暂停该分销商的站点", false);
                }
                else if (DistributorHelper.Delete(distributorUserId))
                {
                    DeleteDistributorFile(distributorUserId);
                    BindDistributors();
                    ShowMsg("成功的清除了该分销商及该分销商下的所有数据", true);
                }
                else
                {
                    ShowMsg("清除去失败", false);
                }
            }
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["userName"]))
                {
                    userName = Page.Request.QueryString["userName"];
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["realName"]))
                {
                    realName = Page.Request.QueryString["realName"];
                }
                int result = 0;
                if (int.TryParse(Page.Request.QueryString["gradeId"], out result))
                {
                    gradeId = new int?(result);
                }
                int num2 = 0;
                if (int.TryParse(Page.Request.QueryString["LineId"], out num2))
                {
                    lineId = new int?(num2);
                }
                txtUserName.Text = userName;
                txtTrueName.Text = realName;
            }
            else
            {
                userName = txtUserName.Text;
                realName = txtTrueName.Text;
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            grdDistributorList.RowCommand += new GridViewCommandEventHandler(grdDistributorList_RowCommand);
            btnExport.Click += new EventHandler(btnExport_Click);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CallBack();
            LoadParameters();
            if (!base.IsPostBack)
            {
                dropGrade.DataBind();
                dropGrade.SelectedValue = gradeId;
                BindDistributors();
                exportFieldsCheckBoxList.Items.Remove(new ListItem("积分", "Points"));
                exportFieldsCheckBoxList.Items.Remove(new ListItem("生日", "BirthDate"));
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("userName", txtUserName.Text.Trim());
            queryStrings.Add("realName", txtTrueName.Text.Trim());
            queryStrings.Add("gradeId", dropGrade.SelectedValue.ToString());
            queryStrings.Add("PageSize", pager.PageSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }
    }
}

