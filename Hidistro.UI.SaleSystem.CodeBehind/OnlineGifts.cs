namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.SaleSystem.Catalog;
    using Hidistro.UI.Common.Controls;
    using System;

    public class OnlineGifts : HtmlTemplatedWebControl
    {
       Pager pager;
       ThemedTemplatedRepeater rptGifts;

        protected override void AttachChildControls()
        {
            this.rptGifts = (ThemedTemplatedRepeater) this.FindControl("rptGifts");
            this.pager = (Pager) this.FindControl("pager");
            if (!this.Page.IsPostBack)
            {
                this.BindGift();
            }
        }

       void BindGift()
        {
            Pagination page = new Pagination();
            page.PageIndex = this.pager.PageIndex;
            int num = 10;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["pageSize"]) && int.TryParse(this.Page.Request.QueryString["pageSize"], out num))
            {
                page.PageSize = num;
            }
            DbQueryResult onlineGifts = ProductBrowser.GetOnlineGifts(page);
            this.rptGifts.DataSource = onlineGifts.Data;
            this.rptGifts.DataBind();
            this.pager.TotalRecords = onlineGifts.TotalRecords;
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-OnlineGifts.html";
            }
            base.OnInit(e);
        }
    }
}

