namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderOptionInfo
    {
        
       decimal _AdjustedPrice ;
        
       string _CustomerDescription ;
        
       string _CustomerTitle ;
        
       string _ItemDescription ;
        
       string _ListDescription ;
        
       int _LookupItemId ;
        
       int _LookupListId ;
        
       string _PurchaseOrderId ;

        public decimal AdjustedPrice
        {
            
            get
            {
                return this._AdjustedPrice ;
            }
            
            set
            {
                this._AdjustedPrice  = value;
            }
        }

        public string CustomerDescription
        {
            
            get
            {
                return this._CustomerDescription ;
            }
            
            set
            {
                this._CustomerDescription  = value;
            }
        }

        public string CustomerTitle
        {
            
            get
            {
                return this._CustomerTitle ;
            }
            
            set
            {
                this._CustomerTitle  = value;
            }
        }

        public string ItemDescription
        {
            
            get
            {
                return this._ItemDescription ;
            }
            
            set
            {
                this._ItemDescription  = value;
            }
        }

        public string ListDescription
        {
            
            get
            {
                return this._ListDescription ;
            }
            
            set
            {
                this._ListDescription  = value;
            }
        }

        public int LookupItemId
        {
            
            get
            {
                return this._LookupItemId ;
            }
            
            set
            {
                this._LookupItemId  = value;
            }
        }

        public int LookupListId
        {
            
            get
            {
                return this._LookupListId ;
            }
            
            set
            {
                this._LookupListId  = value;
            }
        }

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

