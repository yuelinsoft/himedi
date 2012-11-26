using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Gifts)]
    public partial class Gifts : AdminPage
    {

        string giftName;
        private void BindData()
        {
            GiftQuery query = new GiftQuery();
            query.Name = giftName;
            query.Page.PageSize = pager.PageSize;
            query.Page.PageIndex = pager.PageIndex;
            query.Page.SortBy = grdGift.SortOrderBy;
            if (grdGift.SortOrder.ToLower() == "desc")
            {
                query.Page.SortOrder = SortAction.Desc;
            }
            DbQueryResult gifts = GiftHelper.GetGifts(query);
            grdGift.DataSource = gifts.Data;
            grdGift.DataBind();
            pager.TotalRecords = gifts.TotalRecords;
            pager1.TotalRecords = gifts.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            ReloadGiftsList(true);
        }

        private void grdGift_ReBindData(object sender)
        {
            BindData();
        }

        private void grdGift_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int giftId = (int)grdGift.DataKeys[e.RowIndex].Value;
            if (GiftHelper.DeleteGift(giftId))
            {
                ShowMsg("成功的删除了一件礼品信息", true);
                BindData();
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        private void grdGift_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int num = (int)grdGift.DataKeys[e.NewEditIndex].Value;
            HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/promotion/EditGift.aspx?giftId={0}", num)));
        }

        private void lkbDelectCheck_Click(object sender, EventArgs e)
        {
            int num = 0;
            foreach (GridViewRow row in grdGift.Rows)
            {
                CheckBox box = (CheckBox)row.FindControl("checkboxCol");
                if (box.Checked && GiftHelper.DeleteGift(Convert.ToInt32(grdGift.DataKeys[row.RowIndex].Value, CultureInfo.InvariantCulture)))
                {
                    num++;
                }
            }
            if (num > 0)
            {
                ShowMsg(string.Format("成功的删除了{0}件礼品", num), true);
                BindData();
            }
            else
            {
                ShowMsg("请选择您要删除的礼品", false);
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
            lkbDelectCheck.Click += new EventHandler(lkbDelectCheck_Click);
            grdGift.RowEditing += new GridViewEditEventHandler(grdGift_RowEditing);
            grdGift.RowDeleting += new GridViewDeleteEventHandler(grdGift_RowDeleting);
            grdGift.ReBindData += new Grid.ReBindDataEventHandler(grdGift_ReBindData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            if (!Page.IsPostBack)
            {
                BindData();
                UpdateIsDownLoad();
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

        private void UpdateIsDownLoad()
        {
            if (((!string.IsNullOrEmpty(base.Request.QueryString["oper"]) && (base.Request.QueryString["oper"].Trim() == "update")) && (!string.IsNullOrEmpty(base.Request.QueryString["GiftId"]) && !string.IsNullOrEmpty(base.Request.QueryString["Status"]))) && ((int.Parse(base.Request.QueryString["GiftId"].Trim()) > 0) && (int.Parse(base.Request.QueryString["Status"].Trim()) >= 0)))
            {
                int giftId = int.Parse(base.Request.QueryString["GiftId"]);
                bool isdownload = false;
                string str = "取消";
                if (int.Parse(base.Request.QueryString["Status"].Trim()) == 1)
                {
                    isdownload = true;
                    str = "允许";
                }
                if (GiftHelper.UpdateIsDownLoad(giftId, isdownload))
                {
                    BindData();
                    ShowMsg(str + "当前礼品下载成功！", true);
                }
                else
                {
                    ShowMsg(str + "当前礼品下载失败！", false);
                }
            }
        }
    }
}

