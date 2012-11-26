namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleQuery : Pagination
    {
        
       int? _CategoryId ;
        
       DateTime? _EndArticleTime ;
        
       string _Keywords ;
        
       DateTime? _StartArticleTime ;

        public int? CategoryId
        {
            
            get
            {
                return this._CategoryId ;
            }
            
            set
            {
                this._CategoryId  = value;
            }
        }

        public DateTime? EndArticleTime
        {
            
            get
            {
                return this._EndArticleTime ;
            }
            
            set
            {
                this._EndArticleTime  = value;
            }
        }

        public string Keywords
        {
            
            get
            {
                return this._Keywords ;
            }
            
            set
            {
                this._Keywords  = value;
            }
        }

        public DateTime? StartArticleTime
        {
            
            get
            {
                return this._StartArticleTime ;
            }
            
            set
            {
                this._StartArticleTime  = value;
            }
        }
    }
}

