namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderLookupListInfo
    {
        
       string _Description ;
        
       int _LookupListId ;
        
       string _Name ;
        
       SelectModeTypes _SelectMode ;

        [HtmlCoding, StringLengthValidator(0, 300, Ruleset="ValOrderLookupListInfo", MessageTemplate="备注长度限制在300字符以内")]
        public string Description
        {
            
            get
            {
                return this._Description ;
            }
            
            set
            {
                this._Description  = value;
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

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValOrderLookupListInfo", MessageTemplate="选项名称不能为空，长度限制在60字符以内")]
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

        public SelectModeTypes SelectMode
        {
            
            get
            {
                return this._SelectMode ;
            }
            
            set
            {
                this._SelectMode  = value;
            }
        }
    }
}

