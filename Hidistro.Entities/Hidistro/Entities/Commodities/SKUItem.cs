namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SKUItem : IComparable
    {
        
       int _AlertStock ;
        
       decimal _CostPrice ;
        
       int _ProductId ;
        
       decimal _PurchasePrice ;
        
       decimal _SalePrice ;
        
       string _SKU ;
        
       string _SkuId ;
        
       int _Stock ;
        
       int _Weight ;
       Dictionary<int, decimal> distributorPrices;
       Dictionary<int, decimal> memberPrices;
       Dictionary<int, int> skuItems;

        public int CompareTo(object obj)
        {
            SKUItem item = obj as SKUItem;
            if (item == null)
            {
                return -1;
            }
            if (item.SkuItems.Count != this.SkuItems.Count)
            {
                return -1;
            }
            foreach (int num in item.SkuItems.Keys)
            {
                if (item.SkuItems[num] != this.SkuItems[num])
                {
                    return -1;
                }
            }
            return 0;
        }

        public int AlertStock
        {
            
            get
            {
                return this._AlertStock ;
            }
            
            set
            {
                this._AlertStock  = value;
            }
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

        public Dictionary<int, decimal> DistributorPrices
        {
            get
            {
                return (this.distributorPrices ?? (this.distributorPrices = new Dictionary<int, decimal>()));
            }
        }

        public Dictionary<int, decimal> MemberPrices
        {
            get
            {
                return (this.memberPrices ?? (this.memberPrices = new Dictionary<int, decimal>()));
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

        public decimal SalePrice
        {
            
            get
            {
                return this._SalePrice ;
            }
            
            set
            {
                this._SalePrice  = value;
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

        public Dictionary<int, int> SkuItems
        {
            get
            {
                return (this.skuItems ?? (this.skuItems = new Dictionary<int, int>()));
            }
        }

        public int Stock
        {
            
            get
            {
                return this._Stock ;
            }
            
            set
            {
                this._Stock  = value;
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
    }
}

