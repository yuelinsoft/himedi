namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseShoppingCartItemInfo
    {
        
       decimal _CostPrice ;
        
       string _ItemDescription ;
        
       decimal _ItemListPrice ;
        
       decimal _ItemPurchasePrice ;
        
       int _ItemWeight ;
        
       int _ProductId ;
        
       int _Quantity ;
        
       string _SKU ;
        
       string _SKUContent ;
        
       string _SkuId ;
        
       string _ThumbnailsUrl ;

        public decimal GetSubTotal()
        {
            return (this.ItemPurchasePrice * this.Quantity);
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

        public decimal ItemListPrice
        {
            
            get
            {
                return this._ItemListPrice ;
            }
            
            set
            {
                this._ItemListPrice  = value;
            }
        }

        public decimal ItemPurchasePrice
        {
            
            get
            {
                return this._ItemPurchasePrice ;
            }
            
            set
            {
                this._ItemPurchasePrice  = value;
            }
        }

        public int ItemWeight
        {
            
            get
            {
                return this._ItemWeight ;
            }
            
            set
            {
                this._ItemWeight  = value;
            }
        }

        public int ProductId
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

        public string SKU
        {
            
            get
            {
                return this._SKU ;
            }
            
            set
            {
                this._SKU  = value;
            }
        }

        public string SKUContent
        {
            
            get
            {
                return this._SKUContent ;
            }
            
            set
            {
                this._SKUContent  = value;
            }
        }

        public string SkuId
        {
            
            get
            {
                return this._SkuId ;
            }
            
            set
            {
                this._SkuId  = value;
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

