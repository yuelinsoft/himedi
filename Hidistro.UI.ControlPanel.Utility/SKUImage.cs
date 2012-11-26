namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SKUImage : WebControl
    {
        
       string _ImageUrl;
        
       string _ValueStr;

        protected override void Render(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.ImageUrl))
            {
                writer.Write(string.Format("<a href=\"#\">{0}</a>", this.ValueStr));
            }
            else
            {
                writer.Write(string.Format("<a  class=\"{0}\" href=\"#\"><img src=\"{1}\" width=\"23\" height=\"20\" alt=\"{2}\" /></a>", this.CssClass, Globals.ApplicationPath + this.ImageUrl, this.ValueStr));
            }
        }

        public string ImageUrl
        {
            
            get
            {
                return this._ImageUrl;
            }
            
            set
            {
                this._ImageUrl = value;
            }
        }

        public string ValueStr
        {
            
            get
            {
                return this._ValueStr;
            }
            
            set
            {
                this._ValueStr = value;
            }
        }
    }
}

