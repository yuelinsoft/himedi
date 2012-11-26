namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Runtime.CompilerServices;

    public class AdvancedProductQuery : ProductQuery
    {
        
       bool _IncludeInStock ;
        
       bool _IncludeOnSales ;
        
       bool _IncludeUnSales ;

        public bool IncludeInStock
        {
            
            get
            {
                return this._IncludeInStock ;
            }
            
            set
            {
                this._IncludeInStock  = value;
            }
        }

        public bool IncludeOnSales
        {
            
            get
            {
                return this._IncludeOnSales ;
            }
            
            set
            {
                this._IncludeOnSales  = value;
            }
        }

        public bool IncludeUnSales
        {
            
            get
            {
                return this._IncludeUnSales ;
            }
            
            set
            {
                this._IncludeUnSales  = value;
            }
        }
    }
}

