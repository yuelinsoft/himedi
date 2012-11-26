namespace Hidistro.Entities.Sales
{
    using Hidistro.Membership.Context;
    using System;
    using System.Runtime.CompilerServices;

    public class ShoppingCartItemInfo
    {
        
       int _CategoryId ;
        
       decimal? _DiscountRate ;
        
       int _GiveQuantity ;
        
       decimal _MemberPrice ;
        
       string _Name ;
        
       int _ProductId ;
        
       int _PurchaseGiftId ;
        
       string _PurchaseGiftName ;
        
       int _Quantity ;
        
       string _SKU ;
        
       string _SkuContent ;
        
       string _SkuId ;
        
       string _ThumbnailUrl100 ;
        
       string _ThumbnailUrl40 ;
        
       string _ThumbnailUrl60 ;
        
       int _UserId ;
        
       int _Weight ;
        
       int _WholesaleDiscountId ;
        
       string _WholesaleDiscountName ;

        public ShoppingCartItemInfo(string skuId, int productId, string sku, string name, decimal memberPrice, string skuContent, int quantity, int weight, int purchaseGiftId, string purchaseGiftName, int giveQuantity, int wholesaleDiscountId, string wholesaleDiscountName, decimal? discountRate, int categoryId, string thumbnailUrl40, string thumbnailUrl60, string thumbnailUrl100)
        {
            this.UserId = HiContext.Current.User.UserId;
            this.SkuId = skuId;
            this.ProductId = productId;
            this.SKU = sku;
            this.Name = name;
            this.MemberPrice = memberPrice;
            this.SkuContent = skuContent;
            this.Quantity = quantity;
            this.Weight = weight;
            this.PurchaseGiftId = purchaseGiftId;
            this.PurchaseGiftName = purchaseGiftName;
            this.GiveQuantity = giveQuantity;
            this.WholesaleDiscountId = wholesaleDiscountId;
            this.WholesaleDiscountName = wholesaleDiscountName;
            this.DiscountRate = discountRate;
            this.CategoryId = categoryId;
            this.ThumbnailUrl40 = thumbnailUrl40;
            this.ThumbnailUrl60 = thumbnailUrl60;
            this.ThumbnailUrl100 = thumbnailUrl100;
        }

        public int GetSubWeight()
        {
            return (this.Weight * this.ShippQuantity);
        }

        public int CategoryId
        {
            
            get
            {
                return this._CategoryId ;
            }
            
           set
            {
                this._CategoryId  = value;
            }
        }

        public decimal? DiscountRate
        {
            
            get
            {
                return this._DiscountRate ;
            }
            
           set
            {
                this._DiscountRate  = value;
            }
        }

        public int GiveQuantity
        {
            
            get
            {
                return this._GiveQuantity ;
            }
            
           set
            {
                this._GiveQuantity  = value;
            }
        }

        public decimal MemberPrice
        {
            
            get
            {
                return this._MemberPrice ;
            }
            
           set
            {
                this._MemberPrice  = value;
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

        public int PurchaseGiftId
        {
            
            get
            {
                return this._PurchaseGiftId ;
            }
            
           set
            {
                this._PurchaseGiftId  = value;
            }
        }

        public string PurchaseGiftName
        {
            
            get
            {
                return this._PurchaseGiftName ;
            }
            
           set
            {
                this._PurchaseGiftName  = value;
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

        public int ShippQuantity
        {
            get
            {
                return (this.Quantity + this.GiveQuantity);
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

        public string SkuContent
        {
            
            get
            {
                return this._SkuContent ;
            }
            
           set
            {
                this._SkuContent  = value;
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

        public decimal SubTotal
        {
            get
            {
                decimal num = this.MemberPrice * this.Quantity;
                if (this.DiscountRate.HasValue)
                {
                    num *= this.DiscountRate.Value / 100M;
                }
                return num;
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

        public int Weight
        {
            
            get
            {
                return this._Weight ;
            }
            
           set
            {
                this._Weight  = value;
            }
        }

        public int WholesaleDiscountId
        {
            
            get
            {
                return this._WholesaleDiscountId ;
            }
            
           set
            {
                this._WholesaleDiscountId  = value;
            }
        }

        public string WholesaleDiscountName
        {
            
            get
            {
                return this._WholesaleDiscountName ;
            }
            
           set
            {
                this._WholesaleDiscountName  = value;
            }
        }
    }
}

