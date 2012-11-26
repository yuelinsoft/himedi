namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class PromotionProductQuery
    {
        
       int? _CategoryId ;
        
       Pagination _Page ;
        
       string _ProductName ;

        public PromotionProductQuery()
        {
            this.Page = new Pagination();
        }

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

        public Pagination Page
        {
            
            get
            {
                return this._Page ;
            }
            
            set
            {
                this._Page  = value;
            }
        }

        public string ProductName
        {
            
            get
            {
                return this._ProductName ;
            }
            
            set
            {
                this._ProductName  = value;
            }
        }
    }
}

