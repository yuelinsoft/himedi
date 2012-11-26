namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewQuery : Pagination
    {
        
       int? _CategoryId ;
        
       string _Keywords ;
        
       string _ProductCode ;

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

        [HtmlCoding]
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

        [HtmlCoding]
        public string ProductCode
        {
            
            get
            {
                return this._ProductCode ;
            }
            
            set
            {
                this._ProductCode  = value;
            }
        }
    }
}

