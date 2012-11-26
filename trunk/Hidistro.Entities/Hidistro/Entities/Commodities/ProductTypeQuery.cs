namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductTypeQuery : Pagination
    {
        
       string _TypeName ;

        public string TypeName
        {
            
            get
            {
                return this._TypeName ;
            }
            
            set
            {
                this._TypeName  = value;
            }
        }
    }
}

