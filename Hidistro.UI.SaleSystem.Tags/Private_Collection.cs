namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Private_Collection : HyperLink
    {
       object productId;

        protected override void OnDataBinding(EventArgs e)
        {
            this.productId = DataBinder.Eval(this.Page.GetDataItem(), "ProductId");
            base.OnDataBinding(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.ImageUrl))
            {
                if (base.ImageUrl.StartsWith("~"))
                {
                    base.ImageUrl = base.ResolveUrl(base.ImageUrl);
                }
                else if (base.ImageUrl.StartsWith("/"))
                {
                    base.ImageUrl = HiContext.Current.GetSkinPath() + base.ImageUrl;
                }
                else
                {
                    base.ImageUrl = HiContext.Current.GetSkinPath() + "/" + base.ImageUrl;
                }
            }
            else if (string.IsNullOrEmpty(this.Text))
            {
                base.Text = "收藏";
            }
            base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("User_Favorites", new object[] { this.productId });
            base.Render(writer);
        }
    }
}

