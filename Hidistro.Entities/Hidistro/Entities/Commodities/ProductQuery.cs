namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductQuery : Pagination
    {
        
       int? _BrandId ;
        
       int? _CategoryId ;
        
       DateTime? _EndDate ;
        
       int? _IsMakeTaobao ;
        
       string _Keywords ;
        
       string _MaiCategoryPath ;
        
       decimal? _MaxSalePrice ;
        
       decimal? _MinSalePrice ;
        
       Hidistro.Entities.Commodities.PenetrationStatus _PenetrationStatus ;
        
       string _ProductCode ;
        
       int? _ProductLineId ;
        
       Hidistro.Entities.Commodities.PublishStatus _PublishStatus ;
        
       ProductSaleStatus _SaleStatus ;
        
       DateTime? _StartDate ;
        
       int? _UserId ;

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

        public DateTime? EndDate
        {
            
            get
            {
                return this._EndDate ;
            }
            
            set
            {
                this._EndDate  = value;
            }
        }

        public int? IsMakeTaobao
        {
            
            get
            {
                return this._IsMakeTaobao ;
            }
            
            set
            {
                this._IsMakeTaobao  = value;
            }
        }

        [HtmlCoding]
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

        public string MaiCategoryPath
        {
            
            get
            {
                return this._MaiCategoryPath ;
            }
            
            set
            {
                this._MaiCategoryPath  = value;
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

        public Hidistro.Entities.Commodities.PenetrationStatus PenetrationStatus
        {
            
            get
            {
                return this._PenetrationStatus ;
            }
            
            set
            {
                this._PenetrationStatus  = value;
            }
        }

        [HtmlCoding]
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

        public int? ProductLineId
        {
            
            get
            {
                return this._ProductLineId ;
            }
            
            set
            {
                this._ProductLineId  = value;
            }
        }

        public Hidistro.Entities.Commodities.PublishStatus PublishStatus
        {
            
            get
            {
                return this._PublishStatus ;
            }
            
            set
            {
                this._PublishStatus  = value;
            }
        }

        public ProductSaleStatus SaleStatus
        {
            
            get
            {
                return this._SaleStatus ;
            }
            
            set
            {
                this._SaleStatus  = value;
            }
        }

        public DateTime? StartDate
        {
            
            get
            {
                return this._StartDate ;
            }
            
            set
            {
                this._StartDate  = value;
            }
        }

        public int? UserId
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
    }
}

