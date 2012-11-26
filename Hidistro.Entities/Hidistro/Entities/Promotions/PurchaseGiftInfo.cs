namespace Hidistro.Entities.Promotions
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PurchaseGiftInfo : PromotionInfo
    {
        
       int _BuyQuantity ;
        
       int _GiveQuantity ;
       IList<int> productList;

        public PurchaseGiftInfo()
        {
            base.PromoteType = PromoteType.PurchaseGift;
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 0x2710, RangeBoundaryType.Inclusive, Ruleset="ValPromotion", MessageTemplate="购买数量不能为空，数量大小1-10000之间")]
        public int BuyQuantity
        {
            
            get
            {
                return this._BuyQuantity ;
            }
            
            set
            {
                this._BuyQuantity  = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 0x2710, RangeBoundaryType.Inclusive, Ruleset="ValPromotion", MessageTemplate="赠送数量不能为空，数量大小1-10000之间")]
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

        public IList<int> ProductList
        {
            get
            {
                if (this.productList == null)
                {
                    this.productList = new List<int>();
                }
                return this.productList;
            }
            set
            {
                this.productList = value;
            }
        }
    }
}

