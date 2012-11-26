namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class GiftQuery
    {
        
       string _Name ;
        
       Pagination _Page ;

        public GiftQuery()
        {
            this.Page = new Pagination();
        }

        public string Name
        {
            
            get
            {
                return this._Name ;
            }
            
            set
            {
                this._Name  = value;
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
    }
}

