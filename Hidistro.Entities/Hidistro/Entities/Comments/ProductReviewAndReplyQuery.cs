namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewAndReplyQuery : Pagination
    {
        
       int _ProductId ;
        
       long _ReviewId ;

        public virtual int ProductId
        {
            
            get
            {
                return this._ProductId ;
            }
            
            set
            {
                this._ProductId  = value;
            }
        }

        public long ReviewId
        {
            
            get
            {
                return this._ReviewId ;
            }
            
            set
            {
                this._ReviewId  = value;
            }
        }
    }
}

