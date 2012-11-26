namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;

    public class ShippingAddressInfo
    {
        
       string _Address ;
        
       string _CellPhone ;
        
       int _RegionId ;
        
       int _ShippingId ;
        
       string _ShipTo ;
        
       string _TelPhone ;
        
       int _UserId ;
        
       string _Zipcode ;

        [HtmlCoding]
        public string Address
        {
            
            get
            {
                return this._Address ;
            }
            
            set
            {
                this._Address  = value;
            }
        }

        [HtmlCoding]
        public string CellPhone
        {
            
            get
            {
                return this._CellPhone ;
            }
            
            set
            {
                this._CellPhone  = value;
            }
        }

        public int RegionId
        {
            
            get
            {
                return this._RegionId ;
            }
            
            set
            {
                this._RegionId  = value;
            }
        }

        public int ShippingId
        {
            
            get
            {
                return this._ShippingId ;
            }
            
            set
            {
                this._ShippingId  = value;
            }
        }

        [HtmlCoding]
        public string ShipTo
        {
            
            get
            {
                return this._ShipTo ;
            }
            
            set
            {
                this._ShipTo  = value;
            }
        }

        [HtmlCoding]
        public string TelPhone
        {
            
            get
            {
                return this._TelPhone ;
            }
            
            set
            {
                this._TelPhone  = value;
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

        public string Zipcode
        {
            
            get
            {
                return this._Zipcode ;
            }
            
            set
            {
                this._Zipcode  = value;
            }
        }
    }
}

