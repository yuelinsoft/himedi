namespace Hidistro.Entities.Promotions
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class WholesaleDiscountInfo : PromotionInfo
    {
       IList<int> _productList;
        
       int _DiscountValue ;
        
       int _Quantity ;

        public WholesaleDiscountInfo()
        {
            base.PromoteType = PromoteType.WholesaleDiscount;
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 100, RangeBoundaryType.Inclusive, Ruleset="ValPromotion", MessageTemplate="折扣率不能为空，大小1-100之间")]
        public int DiscountValue
        {
            
            get
            {
                return this._DiscountValue ;
            }
            
            set
            {
                this._DiscountValue  = value;
            }
        }

        public IList<int> ProductList
        {
            get
            {
                if (this._productList == null)
                {
                    this._productList = new List<int>();
                }
                return this._productList;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 0x2710, RangeBoundaryType.Inclusive, Ruleset="ValPromotion", MessageTemplate="购买数量不能为空，数量大小1-10000之间")]
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
    }
}

