namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.SaleSystem.Comments;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class Common_ArticleList : ThemedTemplatedRepeater
    {
        
       int _SubjectId;

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                XmlNode node = CommentBrowser.GetArticleSubjectDocument().SelectSingleNode("root/Subject[SubjectId='" + this.SubjectId + "']");
                int? maxNum = null;
                string innerText = string.Empty;
                string keyWords = string.Empty;
                if (node != null)
                {
                    innerText = node.SelectSingleNode("Categories").InnerText;
                    if (!string.IsNullOrEmpty(innerText))
                    {
                        innerText = "(" + innerText + ")";
                    }
                    maxNum = new int?(int.Parse(node.SelectSingleNode("MaxNum").InnerText));
                    keyWords = node.SelectSingleNode("Keywords").InnerText;
                }
                base.DataSource = CommentBrowser.GetArticleList(maxNum, innerText, keyWords);
                base.DataBind();
            }
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

