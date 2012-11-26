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
    public partial class MyGroupBuys : DistributorPage
    {

        string productName = string.Empty;

        private void BindGroupBuy()
        {
            GroupBuyQuery query = new GroupBuyQuery();
            query.ProductName = productName;
            query.PageIndex = pager.PageIndex;
            query.PageSize = pager.PageSize;
            query.SortBy = "DisplaySequence";
            query.SortOrder = SortAction.Desc;
            DbQueryResult groupBuyList = SubsitePromoteHelper.GetGroupBuyList(query);
            grdGroupBuyList.DataSource = groupBuyList.Data;
            grdGroupBuyList.DataBind();
            pager.TotalRecords = groupBuyList.TotalRecords;
            pager1.TotalRecords = groupBuyList.TotalRecords;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadHelpList(true);
        }

        private void grdGroupBuyList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int groupBuyId = (int)grdGroupBuyList.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((grdGroupBuyList.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            int replaceGroupBuyId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (grdGroupBuyList.Rows.Count - 1))
                {
                    replaceGroupBuyId = (int)grdGroupBuyList.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((grdGroupBuyList.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceGroupBuyId = (int)grdGroupBuyList.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((grdGroupBuyList.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            }
            if (replaceGroupBuyId > 0)
            {
                SubsitePromoteHelper.SwapGroupBuySequence(groupBuyId, replaceGroupBuyId, displaySequence, replaceDisplaySequence);
                BindGroupBuy();
            }
        }

        private void grdGroupBuyList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                FormatedMoneyLabel label = (FormatedMoneyLabel)e.Row.FindControl("lblCurrentPrice");
                int groupBuyId = Convert.ToInt32(grdGroupBuyList.DataKeys[e.Row.RowIndex].Value.ToString());
                int prodcutQuantity = int.Parse(DataBinder.Eval(e.Row.DataItem, "ProdcutQuantity").ToString());
                label.Money = SubsitePromoteHelper.GetCurrentPrice(groupBuyId, prodcutQuantity);
            }
        }

        private void grdGroupBuyList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GroupBuyInfo groupBuy = SubsitePromoteHelper.GetGroupBuy((int)grdGroupBuyList.DataKeys[e.RowIndex].Value);
            if ((groupBuy.Status == GroupBuyStatus.UnderWay) || (groupBuy.Status == GroupBuyStatus.EndUntreated))
            {
                ShowMsg("团购活动正在进行中或结束未处理，不允许删除", false);
            }
            else if (SubsitePromoteHelper.DeleteGroupBuy((int)grdGroupBuyList.DataKeys[e.RowIndex].Value))
            {
                BindGroupBuy();
                ShowMsg("成功删除了选择的团购活动", true);
            }
            else
            {
                ShowMsg("删除失败", false);
            }
        }

        private void lkbtnDeleteCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdGroupBuyList.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked)
                {
                    num++;
                    SubsitePromoteHelper.DeleteGroupBuy(Convert.ToInt32(grdGroupBuyList.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture));
                }
            }
            if (num != 0)
            {
                BindGroupBuy();
                ShowMsg(string.Format(CultureInfo.InvariantCulture, "成功删除\"{0}\"条团购活动", new object[] { num }), true);
            }
            else
            {
                ShowMsg("请先选择需要删除的团购活动", false);
            }
        }

        private void LoadParameters()
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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grdGroupBuyList.RowDeleting += new GridViewDeleteEventHandler(grdGroupBuyList_RowDeleting);
            grdGroupBuyList.RowDataBound += new GridViewRowEventHandler(grdGroupBuyList_RowDataBound);
            grdGroupBuyList.RowCommand += new GridViewCommandEventHandler(grdGroupBuyList_RowCommand);
            lkbtnDeleteCheck.Click += new EventHandler(lkbtnDeleteCheck_Click);
            LoadParameters();
            if (!base.IsPostBack)
            {
                BindGroupBuy();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
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
            queryStrings.Add("SortBy", grdGroupBuyList.SortOrderBy);
            queryStrings.Add("SortOrder", SortAction.Desc.ToString());
            base.ReloadPage(queryStrings);
        }
    }
}

