namespace Hidistro.Entities.Promotions
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class FullFreeInfo : PromotionInfo
    {
        
       decimal _Amount ;
        
       bool _OptionFeeFree ;
        
       bool _ServiceChargeFree ;
        
       bool _ShipChargeFree ;

        public FullFreeInfo()
        {
            base.PromoteType = PromoteType.FullFree;
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

        public bool OptionFeeFree
        {
            
            get
            {
                return this._OptionFeeFree ;
            }
            
            set
            {
                this._OptionFeeFree  = value;
            }
        }

        public bool ServiceChargeFree
        {
            
            get
            {
                return this._ServiceChargeFree ;
            }
            
            set
            {
                this._ServiceChargeFree  = value;
            }
        }

        public bool ShipChargeFree
        {
            
            get
            {
                return this._ShipChargeFree ;
            }
            
            set
            {
                this._ShipChargeFree  = value;
            }
        }
    }
}

