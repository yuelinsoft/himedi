namespace Hidistro.UI.SaleSystem.Tags
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_Search_SortPrice : LinkButton
    {
        
       string _Alt;
        
       string _AscImageUrl;
        
       string _DefaultImageUrl;
        
       string _DescImageUrl;
       string imageFormat = "<img border=\"0\" src=\"{0}\" alt=\"{1}\" />";
       Hidistro.UI.SaleSystem.Tags.ImagePosition position;
       bool showText = true;
        public const string TagID = "btn_Common_Search_SortPrice";

        public event Common_Search_SortTime.SortingHandler Sorting;

        public Common_Search_SortPrice()
        {
            base.ID = "btn_Common_Search_SortPrice";
        }

       void Common_Search_SortPrice_Click(object sender, EventArgs e)
        {
            string sortOrder = string.Empty;
            if (this.Page.Request.QueryString["sortOrder"] == "Desc")
            {
                sortOrder = "Asc";
            }
            else
            {
                sortOrder = "Desc";
            }
            this.OnSorting(sortOrder, "SalePrice");
        }

       string GetImageTag()
        {
            if (string.IsNullOrEmpty(this.ImageUrl))
            {
                return string.Empty;
            }
            return string.Format(CultureInfo.InvariantCulture, this.imageFormat, new object[] { this.ImageUrl, this.Alt });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.Click += new EventHandler(this.Common_Search_SortPrice_Click);
        }

       void OnSorting(string sortOrder, string sortOrderBy)
        {
            if (this.Sorting != null)
            {
                this.Sorting(sortOrder, sortOrderBy);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]) && (this.Page.Request.QueryString["sortOrderBy"] == "SalePrice"))
            {
                if (this.Page.Request.QueryString["sortOrder"] == "Desc")
                {
                    this.ImageUrl = this.DescImageUrl;
                    this.Alt = "按价格升序排序";
                }
                else
                {
                    this.ImageUrl = this.AscImageUrl;
                    this.Alt = "按价格降序排序";
                }
            }
            else
            {
                this.ImageUrl = this.DefaultImageUrl;
            }
            base.Attributes.Add("name", this.NamingContainer.UniqueID + "$" + this.ID);
            string imageTag = this.GetImageTag();
            if (!this.ShowText)
            {
                base.Text = "";
            }
            if (this.ImagePosition == Hidistro.UI.SaleSystem.Tags.ImagePosition.Right)
            {
                base.Text = base.Text + imageTag;
            }
            else
            {
                base.Text = imageTag + base.Text;
            }
            base.Render(writer);
        }

        public string Alt
        {
            
            get
            {
                return this._Alt;
            }
            
            set
            {
                this._Alt = value;
            }
        }

        public string AscImageUrl
        {
            
            get
            {
                return this._AscImageUrl;
            }
            
            set
            {
                this._AscImageUrl = value;
            }
        }

        public string DefaultImageUrl
        {
            
            get
            {
                return this._DefaultImageUrl;
            }
            
            set
            {
                this._DefaultImageUrl = value;
            }
        }

        public string DescImageUrl
        {
            
            get
            {
                return this._DescImageUrl;
            }
            
            set
            {
                this._DescImageUrl = value;
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

        public Hidistro.UI.SaleSystem.Tags.ImagePosition ImagePosition
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public string ImageUrl
        {
            get
            {
                if (this.ViewState["Src"] != null)
                {
                    return (string) this.ViewState["Src"];
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.ViewState["Src"] = value;
                }
                else
                {
                    this.ViewState["Src"] = null;
                }
            }
        }

        public bool ShowText
        {
            get
            {
                return this.showText;
            }
            set
            {
                this.showText = value;
            }
        }
    }
}

