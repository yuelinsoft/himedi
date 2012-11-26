namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderGiftInfo
    {
        
       decimal _CostPrice ;
        
       int _GiftId ;
        
       string _GiftName ;
        
       string _PurchaseOrderId ;
        
       decimal _PurchasePrice ;
        
       int _Quantity ;
        
       string _ThumbnailsUrl ;

        public decimal GetSubTotal()
        {
            return (this.PurchasePrice * this.Quantity);
        }

        public decimal CostPrice
        {
            
            get
            {
                return this._CostPrice ;
            }
            
            set
            {
                this._CostPrice  = value;
            }
        }

        public int GiftId
        {
            
            get
            {
                return this._GiftId ;
            }
            
            set
            {
                this._GiftId  = value;
            }
        }

        public string GiftName
        {
            
            get
            {
                return this._GiftName ;
            }
            
            set
            {
                this._GiftName  = value;
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

        public decimal PurchasePrice
        {
            
            get
            {
                return this._PurchasePrice ;
            }
            
            set
            {
                this._PurchasePrice  = value;
            }
        }

        public int Quantity
        {
            
            get
            {
                return this._Quantity ;
            }
            
            set
            {
                this._Quantity  = value;
            }
        }

        public string ThumbnailsUrl
        {
            
            get
            {
                return this._ThumbnailsUrl ;
            }
            
            set
            {
                this._ThumbnailsUrl  = value;
            }
        }
    }
}

