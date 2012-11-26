namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ProductBrowseQuery : Pagination
    {
        
       int? _BrandId ;
        
       int? _CategoryId ;
        
       bool _IsPrecise ;
        
       string _Keywords ;
        
       decimal? _MaxSalePrice ;
        
       decimal? _MinSalePrice ;
        
       string _ProductCode ;
        
       string _SubjectType ;
       IList<AttributeValueInfo> attributeValues;

        public IList<AttributeValueInfo> AttributeValues
        {
            get
            {
                if (this.attributeValues == null)
                {
                    this.attributeValues = new List<AttributeValueInfo>();
                }
                return this.attributeValues;
            }
            set
            {
                this.attributeValues = value;
            }
        }

        public int? BrandId
        {
            
            get
            {
                return this._BrandId ;
            }
            
            set
            {
                this._BrandId  = value;
            }
        }

        public int? CategoryId
        {
            
            get
            {
                return this._CategoryId ;
            }
            
            set
            {
                this._CategoryId  = value;
            }
        }

        public bool IsPrecise
        {
            
            get
            {
                return this._IsPrecise ;
            }
            
            set
            {
                this._IsPrecise  = value;
            }
        }

        public string Keywords
        {
            
            get
            {
                return this._Keywords ;
            }
            
            set
            {
                this._Keywords  = value;
            }
        }

        public decimal? MaxSalePrice
        {
            
            get
            {
                return this._MaxSalePrice ;
            }
            
            set
            {
                this._MaxSalePrice  = value;
            }
        }

        public decimal? MinSalePrice
        {
            
            get
            {
                return this._MinSalePrice ;
            }
            
            set
            {
                this._MinSalePrice  = value;
            }
        }

        public string ProductCode
        {
            
            get
            {
                return this._ProductCode ;
            }
            
            set
            {
                this._ProductCode  = value;
            }
        }

        public string SubjectType
        {
            
            get
            {
                return this._SubjectType ;
            }
            
            set
            {
                this._SubjectType  = value;
            }
        }
    }
}

