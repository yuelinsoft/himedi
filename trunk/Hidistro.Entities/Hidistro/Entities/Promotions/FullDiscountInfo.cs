namespace Hidistro.Entities.Promotions
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    [HasSelfValidation]
    public class FullDiscountInfo : PromotionInfo
    {
        
       decimal _Amount ;
        
       decimal _DiscountValue ;
        
       DiscountValueType _ValueType ;

        public FullDiscountInfo()
        {
            base.PromoteType = PromoteType.FullDiscount;
        }

        [SelfValidation(Ruleset="ValPromotion")]
        public void ValValue(ValidationResults result)
        {
            if (this.ValueType == DiscountValueType.Amount)
            {
                if ((this.DiscountValue < Convert.ToDecimal((double) 0.01)) || (this.DiscountValue > 10000000M))
                {
                    result.AddResult(new ValidationResult("折扣值在0.01-1000万之间", this, "", "", null));
                }
                if (this.DiscountValue > this.Amount)
                {
                    result.AddResult(new ValidationResult("折扣值不能大于满足金额", this, "", "", null));
                }
            }
            else if ((this.ValueType == DiscountValueType.Percent) && ((this.DiscountValue < 1M) || (this.DiscountValue > 100M)))
            {
                result.AddResult(new ValidationResult("折扣率在1-100之间", this, "", "", null));
            }
        }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValPromotion", MessageTemplate="满足金额不能为空，金额大小0.01-1000万之间")]
        public decimal Amount
        {
            
            get
            {
                return this._Amount ;
            }
            
            set
            {
                this._Amount  = value;
            }
        }

        public decimal DiscountValue
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

        public DiscountValueType ValueType
        {
            
            get
            {
                return this._ValueType ;
            }
            
            set
            {
                this._ValueType  = value;
            }
        }
    }
}

