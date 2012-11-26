namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Catalog;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ProductSubjectName : Literal
    {
        
       int _SubjectId;

        protected override void Render(HtmlTextWriter writer)
        {
            XmlNode node = ProductBrowser.GetProductSubjectDocument().SelectSingleNode("root/Subject[SubjectId='" + this.SubjectId + "']");
            if (node != null)
            {
                if (node.SelectSingleNode("SubjectImg").InnerText != "")
                {
                    base.Text = "<img src=\"" + node.SelectSingleNode("SubjectImg").InnerText + "\"/>";
                }
                else
                {
                    base.Text = node.SelectSingleNode("SubjectName").InnerText;
                }
            }
            base.Render(writer);
        }

        public int SubjectId
        {
            
            get
            {
                return this._SubjectId;
            }
            
            set
            {
                this._SubjectId = value;
            }
        }
    }
}

