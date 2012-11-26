namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderLookupItemInfo
    {
        
       decimal? _AppendMoney ;
        
       int? _CalculateMode ;
        
       bool _IsUserInputRequired ;
        
       int _LookupItemId ;
        
       int _LookupListId ;
        
       string _Name ;
        
       string _Remark ;
        
       string _UserInputContent ;
        
       string _UserInputTitle ;

        [RangeValidator(typeof(decimal), "0", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValOrderLookupItemInfo", MessageTemplate="附加金额不能为空,必须为非负数字,限制在1000万以内")]
        public decimal? AppendMoney
        {
            
            get
            {
                return this._AppendMoney ;
            }
            
            set
            {
                this._AppendMoney  = value;
            }
        }

        public int? CalculateMode
        {
            
            get
            {
                return this._CalculateMode ;
            }
            
            set
            {
                this._CalculateMode  = value;
            }
        }

        public bool IsUserInputRequired
        {
            
            get
            {
                return this._IsUserInputRequired ;
            }
            
            set
            {
                this._IsUserInputRequired  = value;
            }
        }

        public int LookupItemId
        {
            
            get
            {
                return this._LookupItemId ;
            }
            
            set
            {
                this._LookupItemId  = value;
            }
        }

        public int LookupListId
        {
            
            get
            {
                return this._LookupListId ;
            }
            
            set
            {
                this._LookupListId  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValOrderLookupItemInfo", MessageTemplate="订单可选内容名称不能为空，长度限制在60字符以内"), HtmlCoding]
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

        [StringLengthValidator(0, 300, Ruleset="ValOrderLookupItemInfo", MessageTemplate="备注长度限制在300字符以内"), HtmlCoding]
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

        [HtmlCoding]
        public string UserInputContent
        {
            
            get
            {
                return this._UserInputContent ;
            }
            
            set
            {
                this._UserInputContent  = value;
            }
        }

        [StringLengthValidator(0, 20, Ruleset="ValOrderLookupItemInfo", MessageTemplate="用户填写信息的标题长度限制在20字符以内"), HtmlCoding]
        public string UserInputTitle
        {
            
            get
            {
                return this._UserInputTitle ;
            }
            
            set
            {
                this._UserInputTitle  = value;
            }
        }
    }
}

