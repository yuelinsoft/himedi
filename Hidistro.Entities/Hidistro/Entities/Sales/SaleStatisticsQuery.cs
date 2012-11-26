namespace Hidistro.Entities.Sales
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SaleStatisticsQuery : Pagination
    {
        
       DateTime? _EndDate ;
        
       string _QueryKey ;
        
       DateTime? _StartDate ;

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

        public string QueryKey
        {
            
            get
            {
                return this._QueryKey ;
            }
            
            set
            {
                this._QueryKey  = value;
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

