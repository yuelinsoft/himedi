namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class SaleStatisticsAnalyzeInfo
    {
        
       int _OrderCounts ;
        
       decimal _OrderTotals ;
        
       int _OrderUserCounts ;
        
       int _UserCounts ;
        
       int _VisitCounts ;

        public int OrderCounts
        {
            
            get
            {
                return this._OrderCounts ;
            }
            
            set
            {
                this._OrderCounts  = value;
            }
        }

        public decimal OrderTotals
        {
            
            get
            {
                return this._OrderTotals ;
            }
            
            set
            {
                this._OrderTotals  = value;
            }
        }

        public int OrderUserCounts
        {
            
            get
            {
                return this._OrderUserCounts ;
            }
            
            set
            {
                this._OrderUserCounts  = value;
            }
        }

        public int UserCounts
        {
            
            get
            {
                return this._UserCounts ;
            }
            
            set
            {
                this._UserCounts  = value;
            }
        }

        public int VisitCounts
        {
            
            get
            {
                return this._VisitCounts ;
            }
            
            set
            {
                this._VisitCounts  = value;
            }
        }
    }
}

