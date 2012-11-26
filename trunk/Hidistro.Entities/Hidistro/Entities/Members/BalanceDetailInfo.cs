namespace Hidistro.Entities.Members
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class BalanceDetailInfo
    {
        
       decimal _Balance ;
        
       decimal? _Expenses ;
        
       decimal? _Income ;
        
       long _JournalNumber ;
        
       string _Remark ;
        
       DateTime _TradeDate ;
        
       TradeTypes _TradeType ;
        
       int _UserId ;
        
       string _UserName ;

        public decimal Balance
        {
            
            get
            {
                return this._Balance ;
            }
            
            set
            {
                this._Balance  = value;
            }
        }

        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValBalanceDetail"), NotNullValidator(Negated=true, Ruleset="ValBalanceDetail"), ValidatorComposition(CompositionType.Or, Ruleset="ValBalanceDetail", MessageTemplate="本次支出的金额，金额大小正负1000万之间")]
        public decimal? Expenses
        {
            
            get
            {
                return this._Expenses ;
            }
            
            set
            {
                this._Expenses  = value;
            }
        }

        [NotNullValidator(Negated=true, Ruleset="ValBalanceDetail"), ValidatorComposition(CompositionType.Or, Ruleset="ValBalanceDetail", MessageTemplate="本次收入的金额，金额大小正负1000万之间"), RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Exclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValBalanceDetail")]
        public decimal? Income
        {
            
            get
            {
                return this._Income ;
            }
            
            set
            {
                this._Income  = value;
            }
        }

        public long JournalNumber
        {
            
            get
            {
                return this._JournalNumber ;
            }
            
            set
            {
                this._JournalNumber  = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValBalanceDetail", MessageTemplate="备注的长度限制在300个字符以内")]
        public string Remark
        {
            
            get
            {
                return this._Remark ;
            }
            
            set
            {
                this._Remark  = value;
            }
        }

        public DateTime TradeDate
        {
            
            get
            {
                return this._TradeDate ;
            }
            
            set
            {
                this._TradeDate  = value;
            }
        }

        public TradeTypes TradeType
        {
            
            get
            {
                return this._TradeType ;
            }
            
            set
            {
                this._TradeType  = value;
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

        public string UserName
        {
            
            get
            {
                return this._UserName ;
            }
            
            set
            {
                this._UserName  = value;
            }
        }
    }
}

