namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web.UI;

    [ParseChildren(true)]
    public class SearchResult : SearchTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            if (!this.Page.IsPostBack)
            {
                string title = string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]) ? "商品搜索" : this.Page.Request.QueryString["keywords"];
                PageTitle.AddTitle(title, HiContext.Current.Context);
            }
            base.AttachChildControls();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-SearchResult.html";
            }
            base.OnInit(e);
        }
    }
}

