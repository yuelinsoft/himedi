namespace Hidistro.Entities.Members
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    [HasSelfValidation]
    public class BalanceDrawRequestInfo
    {
        
       string _AccountName ;
        
       decimal _Amount ;
        
       string _BankName ;
        
       string _MerchantCode ;
        
       string _Remark ;
        
       DateTime _RequestTime ;
        
       int _UserId ;
        
       string _UserName ;

        [HtmlCoding, StringLengthValidator(1, 30, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="开户人真实姓名不能为空,长度限制在30字符以内")]
        public string AccountName
        {
            
            get
            {
                return this._AccountName ;
            }
            
            set
            {
                this._AccountName  = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="提现金额不能为空，金额大小0.01-1000万之间")]
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

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="开银行名称不能为空,长度限制在60字符以内")]
        public string BankName
        {
            
            get
            {
                return this._BankName ;
            }
            
            set
            {
                this._BankName  = value;
            }
        }

        [RegexValidator("^[0-9]*$", Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="个人银行帐号只允许输入数字"), StringLengthValidator(1, 100, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="个人银行帐号不能为空,限制在100个字符以内")]
        public string MerchantCode
        {
            
            get
            {
                return this._MerchantCode ;
            }
            
            set
            {
                this._MerchantCode  = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValBalanceDrawRequestInfo", MessageTemplate="备注长度限制在300字符以内"), HtmlCoding]
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

        public DateTime RequestTime
        {
            
            get
            {
                return this._RequestTime ;
            }
            
            set
            {
                this._RequestTime  = value;
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

        [HtmlCoding]
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

