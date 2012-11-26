namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_BrandsSearchList : WebControl
    {
        
       string _AllCss;
        
       string _SelectCss;
       string urlFormat;
       string urlFormatForALL;

        public Common_BrandsSearchList()
        {
            this.AllCss = "cate_search_selectcss";
            this.SelectCss = "cate_search_selectcss";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.urlFormat = this.Context.Request.RawUrl;
            if (this.urlFormat.IndexOf("?") >= 0)
            {
                string oldValue = this.urlFormat.Substring(this.urlFormat.IndexOf("?") + 1);
                string[] strArray = oldValue.Split(new char[] { Convert.ToChar("&") });
                this.urlFormat = this.urlFormat.Replace(oldValue, "");
                foreach (string str2 in strArray)
                {
                    if (!str2.ToLower().StartsWith("brand=") && !str2.ToLower().StartsWith("pageindex="))
                    {
                        this.urlFormat = this.urlFormat + str2 + "&";
                    }
                }
                this.urlFormatForALL = this.urlFormat.Substring(0, this.urlFormat.Length - 1);
                this.urlFormat = this.urlFormat + "brand=";
            }
            else
            {
                this.urlFormatForALL = this.urlFormat;
                this.urlFormat = this.urlFormat + "?brand=";
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderButton(writer);
        }

       void RenderButton(HtmlTextWriter writer)
        {
            DataTable brandsByCategoryId = ProductBrowser.GetBrandsByCategoryId(this.CategoryId);
            if ((brandsByCategoryId != null) && (brandsByCategoryId.Rows.Count > 0))
            {
                WebControl control = new WebControl(HtmlTextWriterTag.Label);
                control.Controls.Add(new LiteralControl("<span>品牌：</span>"));
                control.RenderControl(writer);
                WebControl control2 = new WebControl(HtmlTextWriterTag.A);
                control2.Controls.Add(new LiteralControl("全部"));
                control2.Attributes.Add("href", this.urlFormatForALL);
                if (this.Context.Request.RawUrl.IndexOf("brand") < 0)
                {
                    control2.Attributes.Add("class", this.AllCss);
                }
                control2.RenderControl(writer);
                foreach (DataRow row in brandsByCategoryId.Rows)
                {
                    WebControl control3 = new WebControl(HtmlTextWriterTag.A);
                    control3.Controls.Add(new LiteralControl(row["BrandName"].ToString()));
                    if (this.SelectedBrand == int.Parse(row["BrandId"].ToString()))
                    {
                        control3.Attributes.Add("class", this.SelectCss);
                    }
                    else
                    {
                        control3.Attributes.Add("href", this.urlFormat + row["BrandId"].ToString());
                    }
                    control3.RenderControl(writer);
                }
            }
        }

        public string AllCss
        {
            
            get
            {
                return this._AllCss;
            }
            
            set
            {
                this._AllCss = value;
            }
        }

        public int CategoryId
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
                {
                    return int.Parse(this.Page.Request.QueryString["categoryId"], NumberStyles.None);
                }
                return 0;
            }
            set
            {
            }
        }

        public string SelectCss
        {
            
            get
            {
                return this._SelectCss;
            }
            
            set
            {
                this._SelectCss = value;
            }
        }

        [Browsable(false)]
        public int SelectedBrand
        {
            get
            {
                int result = 0;
                if (!string.IsNullOrEmpty(this.Context.Request.QueryString["brand"]))
                {
                    int.TryParse(this.Context.Request.QueryString["brand"], out result);
                }
                return result;
            }
        }
    }
}

