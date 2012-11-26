using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyCountDowns : DistributorPage
    {

        string productName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadParameters();

            if (!base.IsPostBack)
            {
                BindCountDown();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }


        void BindCountDown()
        {
            GroupBuyQuery query = new GroupBuyQuery();
            query.ProductName = productName;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            DbQueryResult countDownList = SubsitePromoteHelper.GetCountDownList(query);
            grdCountDownsList.DataSource = countDownList.Data;
            grdCountDownsList.DataBind();
            pager.TotalRecords = countDownList.TotalRecords;
            pager1.TotalRecords = countDownList.TotalRecords;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadHelpList(true);
        }

        protected void grdGroupBuyList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int countDownId = (int)grdCountDownsList.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((grdCountDownsList.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            int replaceCountDownId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (grdCountDownsList.Rows.Count - 1))
                {
                    replaceCountDownId = (int)grdCountDownsList.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((grdCountDownsList.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceCountDownId = (int)grdCountDownsList.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((grdCountDownsList.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            }
            if (replaceCountDownId > 0)
            {
                SubsitePromoteHelper.SwapCountDownSequence(countDownId, replaceCountDownId, displaySequence, replaceDisplaySequence);
                BindCountDown();
            }
        }

        protected void grdGroupBuyList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (SubsitePromoteHelper.DeleteCountDown((int)grdCountDownsList.DataKeys[e.RowIndex].Value))
            {
                BindCountDown();
                ShowMsg("成功删除了选择的限时抢购活动", true);
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        protected void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdCountDownsList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num++;
                    SubsitePromoteHelper.DeleteCountDown(Convert.ToInt32(grdCountDownsList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture));
                }
            }
            if (num != 0)
            {
                BindCountDown();
                ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除\"{0}\"条限时抢购活动", new object[] { num }), true);
            }
            else
            {
                ShowMsg("请先选择需要删除的限时抢购活动", false);
            }
        }

        void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["productName"]))
                {
                    productName = base.Server.UrlDecode(Page.Request.QueryString["productName"]);
                }
                txtProductName.Text = productName;
            }
            else
            {
                productName = txtProductName.Text;
            }
        }


        private void ReloadHelpList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("productName", txtProductName.Text.Trim());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            queryStrings.Add("PageSize", hrefPageSize.SelectedSize.ToString());
            queryStrings.Add("SortBy", grdCountDownsList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

