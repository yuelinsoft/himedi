namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OrderQuery : Pagination
    {
        
       DateTime? _EndDate ;
        
       int? _GroupBuyId ;
        
       int? _IsPrinted ;
        
       string _OrderId ;
        
       int? _PaymentType ;
        
       string _ProductName ;
        
       int? _RegionId ;
        
       int? _ShippingModeId ;
        
       string _ShipTo ;
        
       DateTime? _StartDate ;
        
       OrderStatus _Status ;
        
       string _UserName ;

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

        public int? GroupBuyId
        {
            
            get
            {
                return this._GroupBuyId ;
            }
            
            set
            {
                this._GroupBuyId  = value;
            }
        }

        public int? IsPrinted
        {
            
            get
            {
                return this._IsPrinted ;
            }
            
            set
            {
                this._IsPrinted  = value;
            }
        }

        public string OrderId
        {
            
            get
            {
                return this._OrderId ;
            }
            
            set
            {
                this._OrderId  = value;
            }
        }

        public int? PaymentType
        {
            
            get
            {
                return this._PaymentType ;
            }
            
            set
            {
                this._PaymentType  = value;
            }
        }

        public string ProductName
        {
            
            get
            {
                return this._ProductName ;
            }
            
            set
            {
                this._ProductName  = value;
            }
        }

        public int? RegionId
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

        public int? ShippingModeId
        {
            
            get
            {
                return this._ShippingModeId ;
            }
            
            set
            {
                this._ShippingModeId  = value;
            }
        }

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

        public OrderStatus Status
        {
            
            get
            {
                return this._Status ;
            }
            
            set
            {
                this._Status  = value;
            }
        }

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

