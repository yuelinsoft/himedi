namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductLineInfo
    {
        
       int _LineId ;
        
       string _Name ;
        
       string _SupplierName ;

        public int LineId
        {
            
            get
            {
                return this._LineId ;
            }
            
            set
            {
                this._LineId  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValProductLine", MessageTemplate="产品线名称不能为空，长度限制在1-60个字符之间")]
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

        public string SupplierName
        {
            
            get
            {
                return this._SupplierName ;
            }
            
            set
            {
                this._SupplierName  = value;
            }
        }
    }
}

