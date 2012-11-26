using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MyGifts : DistributorPage
    {

        string giftName;

        private void BindData()
        {
            GiftQuery query = new GiftQuery();
            query.Name = Globals.HtmlEncode(giftName);
            query.Page.PageSize = pager.PageSize;
            query.Page.PageIndex = pager.PageIndex;
            query.Page.SortBy = grdGift.SortOrderBy;
            if (grdGift.SortOrder.ToLower() == "desc")
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            DbQueryResult gifts = SubsiteGiftHelper.GetGifts(query);
            grdGift.DataSource = gifts.Data;
            grdGift.DataBind();
            pager.TotalRecords = gifts.TotalRecords;
            pager1.TotalRecords = gifts.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReloadGiftsList(true);
        }

        private void DownLoad()
        {
            if ((!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && (base.Request.QueryString["oper"].Trim() == "download")) && (!string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && (int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0)))
            {
                GiftInfo giftDetails = GiftHelper.GetGiftDetails(int.Parse(base.Request.QueryString["GiftId"].Trim()));
                if (giftDetails.IsDownLoad && (giftDetails.PurchasePrice > 0M))
                {
                    if (SubsiteGiftHelper.DownLoadGift(giftDetails))
                    {
                        ReloadGiftsList(true);
                        ShowMsg("下载礼品" + giftDetails.Name + "成功！", true);
                    }
                    else
                    {
                        ShowMsg("下载礼品" + giftDetails.Name + "失败！", false);
                    }
                }
            }
        }

        private void grdGift_ReBindData(object sender)
        {
            ReloadGiftsList(false);
        }

        private void lkbtnDownloadCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdGift.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    num++;
                }
            }
            if (num == 0)
            {
                ShowMsg("请先选择要下载的礼品", false);
            }
            else
            {
                foreach (GridViewRow row2 in grdGift.Rows)
                {
                    CheckBox box2 = (CheckBox)row2.FindControl("checkboxCol");
                    if (box2.Checked)
                    {
                        int giftId = (int)grdGift.DataKeys[row2.RowIndex].Value;
                        SubsiteGiftHelper.DownLoadGift(GiftHelper.GetGiftDetails(giftId));
                    }
                }
                ShowMsg("下载的礼品成功", true);
                ReloadGiftsList(true);
            }
        }

        private void LoadParameters()
        {
            if (!base.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["GiftName"]))
                {
                    giftName = Page.Request.QueryString["GiftName"];
                }
                txtSearchText.Text = Globals.HtmlDecode(giftName);
            }
            else
            {
                giftName = Globals.HtmlEncode(txtSearchText.Text.Trim());
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            btnSearchButton.Click += new EventHandler(btnSearchButton_Click);
            lkbtnDownloadCheck1.Click += new EventHandler(lkbtnDownloadCheck_Click);
            grdGift.ReBindData += new Grid.ReBindDataEventHandler(grdGift_ReBindData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindData();
                DownLoad();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }

        private void ReloadGiftsList(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("GiftName", Globals.HtmlEncode(txtSearchText.Text.Trim()));
            queryStrings.Add("pageSize", hrefPageSize.SelectedSize.ToString());
            if (!isSearch)
            {
                queryStrings.Add("PageIndex", pager.PageIndex.ToString());
            }
            base.ReloadPage(queryStrings);
        }
    }
}

