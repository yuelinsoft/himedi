namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UserOrderQuery : Pagination
    {
        
       DateTime? _EndDate ;
        
       string _OrderId ;
        
       string _ShipTo ;
        
       DateTime? _StartDate ;
        
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

