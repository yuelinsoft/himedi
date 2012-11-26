namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class ProductBrowseInfo
    {
        
       string _BrandName ;
        
       string _CategoryName ;
        
       DataTable _DbAttribute ;
        
       DataTable _DBConsultations ;
        
       DataTable _DbCorrelatives ;
        
       DataTable _DBReviews ;
        
       DataTable _DbSKUs ;
        
       ProductInfo _Product ;

        public string BrandName
        {
            
            get
            {
                return this._BrandName ;
            }
            
            set
            {
                this._BrandName  = value;
            }
        }

        public string CategoryName
        {
            
            get
            {
                return this._CategoryName ;
            }
            
            set
            {
                this._CategoryName  = value;
            }
        }

        public DataTable DbAttribute
        {
            
            get
            {
                return this._DbAttribute ;
            }
            
            set
            {
                this._DbAttribute  = value;
            }
        }

        public DataTable DBConsultations
        {
            
            get
            {
                return this._DBConsultations ;
            }
            
            set
            {
                this._DBConsultations  = value;
            }
        }

        public DataTable DbCorrelatives
        {
            
            get
            {
                return this._DbCorrelatives ;
            }
            
            set
            {
                this._DbCorrelatives  = value;
            }
        }

        public DataTable DBReviews
        {
            
            get
            {
                return this._DBReviews ;
            }
            
            set
            {
                this._DBReviews  = value;
            }
        }

        public DataTable DbSKUs
        {
            
            get
            {
                return this._DbSKUs ;
            }
            
            set
            {
                this._DbSKUs  = value;
            }
        }

        public ProductInfo Product
        {
            
            get
            {
                return this._Product ;
            }
            
            set
            {
                this._Product  = value;
            }
        }
    }
}

