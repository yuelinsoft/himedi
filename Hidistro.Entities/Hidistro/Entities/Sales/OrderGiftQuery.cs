namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderGiftQuery : Pagination
    {
        
       string _OrderId ;

        public string OrderId
        {
            
            get
            {
                return this._OrderId ;
            }
            
            set
            {
                this._OrderId  = value;
            }
        }
    }
}

