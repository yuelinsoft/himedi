namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class OrderGiftInfo
    {
        
       decimal _CostPrice ;
        
       int _GiftId ;
        
       string _GiftName ;
        
       string _OrderId ;
        
       int _Quantity ;
        
       string _ThumbnailsUrl ;

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

