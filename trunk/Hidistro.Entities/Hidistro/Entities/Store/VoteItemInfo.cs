namespace Hidistro.Entities.Store
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class VoteItemInfo
    {
        
       int _ItemCount ;
        
       decimal _Percentage ;
        
       long _VoteId ;
        
       long _VoteItemId ;
        
       string _VoteItemName ;

        public int ItemCount
        {
            
            get
            {
                return this._ItemCount ;
            }
            
            set
            {
                this._ItemCount  = value;
            }
        }

        public decimal Lenth
        {
            get
            {
                return this.Percentage * Convert.ToDecimal((double)4.2);
            }
        }

        public decimal Percentage
        {
            
            get
            {
                return this._Percentage ;
            }
            
            set
            {
                this._Percentage  = value;
            }
        }

        public long VoteId
        {
            
            get
            {
                return this._VoteId ;
            }
            
            set
            {
                this._VoteId  = value;
            }
        }

        public long VoteItemId
        {
            
            get
            {
                return this._VoteItemId ;
            }
            
            set
            {
                this._VoteItemId  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="VoteItem", MessageTemplate="提供给用户选择的内容，长度限制在60个字符以内")]
        public string VoteItemName
        {
            
            get
            {
                return this._VoteItemName ;
            }
            
            set
            {
                this._VoteItemName  = value;
            }
        }
    }
}

