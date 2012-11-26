namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderGiftQuery : Pagination
    {
        
       string _PurchaseOrderId ;

        public string PurchaseOrderId
        {
            
            get
            {
                return this._PurchaseOrderId ;
            }
            
            set
            {
                this._PurchaseOrderId  = value;
            }
        }
    }
}

