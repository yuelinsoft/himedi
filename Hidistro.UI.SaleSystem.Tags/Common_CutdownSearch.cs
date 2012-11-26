namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_CutdownSearch : AscxTemplatedWebControl
    {
       IButton btnSearch;
        public const string TagID = "search_Common_CutdownSearch";
       TextBox txtEndPrice;
       TextBox txtKeywords;
       TextBox txtStartPrice;

        public event ReSearchEventHandler ReSearch;

        public Common_CutdownSearch()
        {
            base.ID = "search_Common_CutdownSearch";
        }

        protected override void AttachChildControls()
        {
            this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
            this.txtKeywords = (TextBox) this.FindControl("txtKeywords");
            this.txtStartPrice = (TextBox) this.FindControl("txtStartPrice");
            this.txtEndPrice = (TextBox) this.FindControl("txtEndPrice");
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
                {
                    this.txtKeywords.Text = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
                {
                    this.txtStartPrice.Text = this.Page.Request.QueryString["minSalePrice"].ToString();
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
                {
                    this.txtEndPrice.Text = this.Page.Request.QueryString["maxSalePrice"].ToString();
                }
            }
        }

       void btnSearch_Click(object sender, EventArgs e)
        {
            this.OnReSearch(sender, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_Search/Skin-Common_CutdownSearch.ascx";
            }
            base.OnInit(e);
        }

        public void OnReSearch(object sender, EventArgs e)
        {
            if (this.ReSearch != null)
            {
                this.ReSearch(sender, e);
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public ProductBrowseQuery Item
        {
            get
            {
                ProductBrowseQuery entity = new ProductBrowseQuery();
                if (this.txtKeywords != null)
                {
                    entity.Keywords = this.txtKeywords.Text.Trim().Replace("'", "").Replace("\"", "");
                }
                if (this.txtStartPrice != null)
                {
                    decimal result = 0M;
                    if (!string.IsNullOrEmpty(this.txtStartPrice.Text.Trim()) && decimal.TryParse(this.txtStartPrice.Text.Trim(), out result))
                    {
                        entity.MinSalePrice = new decimal?(result);
                    }
                    else
                    {
                        entity.MinSalePrice = null;
                    }
                }
                if (this.txtEndPrice != null)
                {
                    decimal num2 = 0M;
                    if (!string.IsNullOrEmpty(this.txtEndPrice.Text.Trim()) && decimal.TryParse(this.txtEndPrice.Text.Trim(), out num2))
                    {
                        entity.MaxSalePrice = new decimal?(num2);
                    }
                    else
                    {
                        entity.MaxSalePrice = null;
                    }
                }
                entity.ProductCode = "";
                Globals.EntityCoding(entity, true);
                return entity;
            }
        }

        public delegate void ReSearchEventHandler(object sender, EventArgs e);
    }
}

