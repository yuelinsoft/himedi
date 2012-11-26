namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderQuery : Pagination
    {
        
       string _DistributorName ;
        
       DateTime? _EndDate ;
        
       bool _IsManualPurchaseOrder ;
        
       int? _IsPrinted ;
        
       string _OrderId ;
        
       string _ProductName ;
        
       string _PurchaseOrderId ;
        
       OrderStatus _PurchaseStatus ;
        
       int? _ShippingModeId ;
        
       string _ShipTo ;
        
       DateTime? _StartDate ;

        public string DistributorName
        {
            
            get
            {
                return this._DistributorName ;
            }
            
            set
            {
                this._DistributorName  = value;
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

        public bool IsManualPurchaseOrder
        {
            
            get
            {
                return this._IsManualPurchaseOrder ;
            }
            
            set
            {
                this._IsManualPurchaseOrder  = value;
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

        public string PurchaseOrderId
        {
            
            get
            {
                return this._PurchaseOrderId ;
            }
            
            set
            {
                this._PurchaseOrderId  = value;
            }
        }

        public OrderStatus PurchaseStatus
        {
            
            get
            {
                return this._PurchaseStatus ;
            }
            
            set
            {
                this._PurchaseStatus  = value;
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
    }
}

