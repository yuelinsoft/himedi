namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.SaleSystem.Tags;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class ProductSearch : HtmlTemplatedWebControl
    {
       IButton btnFuzzySearch;
       IButton btnSearch;
       Common_AdvSearch_Class dropCategories;
       TextBox txtKeywords;
       TextBox txtMaxPrice;
       TextBox txtMinPrice;
       TextBox txtSKU;

        protected override void AttachChildControls()
        {
            this.txtKeywords = (TextBox) this.FindControl("txtKeywords");
            this.dropCategories = (Common_AdvSearch_Class) this.FindControl("drop_Common_AdvSearch_Class");
            this.txtMinPrice = (TextBox) this.FindControl("txtMinPrice");
            this.txtMaxPrice = (TextBox) this.FindControl("txtMaxPrice");
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.btnFuzzySearch = ButtonManager.Create(this.FindControl("btnFuzzySearch"));
            this.txtSKU = (TextBox) this.FindControl("txtSKU");
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            this.btnFuzzySearch.Click += new EventHandler(this.btnFuzzySearch_Click);
            PageTitle.AddSiteNameTitle("商品搜索", HiContext.Current.Context);
        }

       void btnFuzzySearch_Click(object sender, EventArgs e)
        {
            this.SearchProducts(false);
        }

       void btnSearch_Click(object sender, EventArgs e)
        {
            this.SearchProducts(true);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-ProductSearch.html";
            }
            base.OnInit(e);
        }

       void SearchProducts(bool isPrecise)
        {
            decimal num;
            decimal num2;
            string str = Globals.GetSiteUrls().UrlData.FormatUrl("SearchResult") + "?";
            string str2 = string.Empty;
            if (!string.IsNullOrEmpty(this.txtKeywords.Text.Trim()))
            {
                str2 = str2 + "keywords=" + Globals.UrlEncode(this.txtKeywords.Text.Trim().Replace("'", "").Replace("\"", "")) + "&";
            }
            if (this.dropCategories.SelectedValue.HasValue)
            {
                object obj2 = str2;
                str2 = string.Concat(new object[] { obj2, "categoryId=", this.dropCategories.SelectedValue.Value, "&" });
            }
            if (!string.IsNullOrEmpty(this.txtMinPrice.Text.Trim()) && decimal.TryParse(this.txtMinPrice.Text.Trim(), out num))
            {
                object obj3 = str2;
                str2 = string.Concat(new object[] { obj3, "minSalePrice=", num, "&" });
            }
            if (!string.IsNullOrEmpty(this.txtMaxPrice.Text.Trim()) && decimal.TryParse(this.txtMaxPrice.Text.Trim(), out num2))
            {
                object obj4 = str2;
                str2 = string.Concat(new object[] { obj4, "&maxSalePrice=", num2, "&" });
            }
            if (!string.IsNullOrEmpty(this.txtSKU.Text))
            {
                str2 = str2 + "&productCode=" + Globals.UrlEncode(this.txtSKU.Text) + "&";
            }
            if (string.IsNullOrEmpty(str2))
            {
                this.ShowMessage("请填写搜索条件", false);
            }
            else
            {
                str2 = str2 + "isPrecise=" + isPrecise;
                this.Page.Response.Redirect(str + str2);
            }
        }
    }
}

