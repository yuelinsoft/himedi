using Hidistro.SaleSystem.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hidistro.Entities.Comments;
using System.Runtime.CompilerServices;
namespace Hidistro.UI.SaleSystem.Tags
{

    public class Common_ArticleCategoryList : ThemedTemplatedRepeater
    {
        
       int _MaxNum;
        public const string TagID = "list_Common_ArticleCategory";

        public Common_ArticleCategoryList()
        {
            base.ID = "list_Common_ArticleCategory";
        }

       IList<ArticleCategoryInfo> GetDataSource()
        {
            IList<ArticleCategoryInfo> articleMainCategories = CommentBrowser.GetArticleMainCategories();
            if ((this.MaxNum > 0) && (this.MaxNum < articleMainCategories.Count))
            {
                for (int i = articleMainCategories.Count - 1; i >= this.MaxNum; i--)
                {
                    articleMainCategories.RemoveAt(i);
                }
            }
            return articleMainCategories;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.DataSource = this.GetDataSource();
                base.DataBind();
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

        public int MaxNum
        {
            
            get
            {
                return this._MaxNum;
            }
            
            set
            {
                this._MaxNum = value;
            }
        }
    }
}

