namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ProductTypeInfo
    {
        
       string _Remark ;
        
       int _TypeId ;
        
       string _TypeName ;
       IList<int> brands;

        public IList<int> Brands
        {
            get
            {
                if (this.brands == null)
                {
                    this.brands = new List<int>();
                }
                return this.brands;
            }
            set
            {
                this.brands = value;
            }
        }

        [StringLengthValidator(0, 100, Ruleset="ValProductType", MessageTemplate="备注的长度限制在0-100个字符之间"), HtmlCoding]
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

        public int TypeId
        {
            
            get
            {
                return this._TypeId ;
            }
            
            set
            {
                this._TypeId  = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="ValProductType", MessageTemplate="商品类型名称不能为空，长度限制在1-30个字符之间")]
        public string TypeName
        {
            
            get
            {
                return this._TypeName ;
            }
            
            set
            {
                this._TypeName  = value;
            }
        }
    }
}

