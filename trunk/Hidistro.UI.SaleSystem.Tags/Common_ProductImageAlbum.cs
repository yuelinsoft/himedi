﻿namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_ProductImageAlbum : HyperLink
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("ProductImages", new object[] { this.Page.Request.QueryString["productId"] });
            base.Render(writer);
        }
    }
}

