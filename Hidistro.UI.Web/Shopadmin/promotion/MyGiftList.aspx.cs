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
    public partial class MyGiftList : DistributorPage
    {

        string giftName;


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindData();
                DeleteGriftById();
            }
            CheckBoxColumn.RegisterClientCheckEvents(Page, Page.Form.ClientID);
        }


        void BindData()
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
            DbQueryResult abstroGiftsById = SubsiteGiftHelper.GetAbstroGiftsById(query);
            grdGift.DataSource = abstroGiftsById.Data;
            grdGift.DataBind();
            pager.TotalRecords = abstroGiftsById.TotalRecords;
            pager1.TotalRecords = abstroGiftsById.TotalRecords;
        }

        protected void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReloadGiftsList(true);
        }

        void DeleteGriftById()
        {
            if ((!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && (base.Request.QueryString["oper"].Trim() == "delete")) && (!string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && (int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0)))
            {
                int giftId = int.Parse(base.Request.QueryString["GiftId"].Trim());
                GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
                if (SubsiteGiftHelper.DeleteGiftById(giftId))
                {
                    ReloadGiftsList(true);
                    ShowMsg("删除礼品" + giftDetails.Name + "成功！", true);
                }
                else
                {
                    ShowMsg("删除礼品" + giftDetails.Name + "失败！", false);
                }
            }
        }

        void grdGift_ReBindData(object sender)
        {
            ReloadGiftsList(false);
        }

        protected void lkbtnDelete_Click(object sender, EventArgs e)
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
                ShowMsg("请先选择要删除的礼品", false);
            }
            else
            {
                bool success = true;
                string msg = "删除礼品成功！";
                foreach (GridViewRow row2 in grdGift.Rows)
                {
                    CheckBox box2 = (CheckBox)row2.FindControl("checkboxCol");
                    if (box2.Checked)
                    {
                        int giftId = (int)grdGift.DataKeys[row2.RowIndex].Value;
                        if (!SubsiteGiftHelper.DeleteGiftById(giftId))
                        {
                            success = false;
                            msg = "删除礼品失败！";
                            break;
                        }
                    }
                }
                ShowMsg(msg, success);
                ReloadGiftsList(true);
            }
        }

        void LoadParameters()
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
            lkbtnDelete.Click += new EventHandler(lkbtnDelete_Click);
            grdGift.ReBindData += new Grid.ReBindDataEventHandler(grdGift_ReBindData);
        }


        void ReloadGiftsList(bool isSearch)
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

