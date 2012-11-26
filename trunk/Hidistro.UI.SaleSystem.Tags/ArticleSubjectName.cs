using Hidistro.SaleSystem.Comments;
using System;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
    public class ArticleSubjectName : Literal
    {

        int _SubjectId;

        protected override void Render(HtmlTextWriter writer)
        {
            XmlNode node = CommentBrowser.GetArticleSubjectDocument().SelectSingleNode("root/Subject[SubjectId='" + this.SubjectId + "']");
            if (node != null)
            {
                base.Text = node.SelectSingleNode("SubjectName").InnerText;
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

