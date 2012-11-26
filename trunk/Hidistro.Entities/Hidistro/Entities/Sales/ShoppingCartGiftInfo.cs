namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class ShoppingCartGiftInfo
    {
        
       decimal _CostPrice ;
        
       int _GiftId ;
        
       string _Name ;
        
       int _NeedPoint ;
        
       decimal _PurchasePrice ;
        
       int _Quantity ;
        
       string _ThumbnailUrl100 ;
        
       string _ThumbnailUrl40 ;
        
       string _ThumbnailUrl60 ;
        
       int _UserId ;

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

        public int NeedPoint
        {
            
            get
            {
                return this._NeedPoint ;
            }
            
            set
            {
                this._NeedPoint  = value;
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

        public int SubPointTotal
        {
            get
            {
                return (this.NeedPoint * this.Quantity);
            }
        }

        public string ThumbnailUrl100
        {
            
            get
            {
                return this._ThumbnailUrl100 ;
            }
            
            set
            {
                this._ThumbnailUrl100  = value;
            }
        }

        public string ThumbnailUrl40
        {
            
            get
            {
                return this._ThumbnailUrl40 ;
            }
            
            set
            {
                this._ThumbnailUrl40  = value;
            }
        }

        public string ThumbnailUrl60
        {
            
            get
            {
                return this._ThumbnailUrl60 ;
            }
            
            set
            {
                this._ThumbnailUrl60  = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return this._UserId ;
            }
            
            set
            {
                this._UserId  = value;
            }
        }
    }
}

