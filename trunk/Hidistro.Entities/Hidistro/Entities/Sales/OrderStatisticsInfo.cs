namespace Hidistro.Entities.Sales
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class OrderStatisticsInfo
    {
        
       DataTable _OrderTbl ;
        
       decimal _ProfitsOfPage ;
        
       decimal _ProfitsOfSearch ;
        
       int _TotalCount ;
        
       decimal _TotalOfPage ;
        
       decimal _TotalOfSearch ;

        public DataTable OrderTbl
        {
            
            get
            {
                return this._OrderTbl ;
            }
            
            set
            {
                this._OrderTbl  = value;
            }
        }

        public decimal ProfitsOfPage
        {
            
            get
            {
                return this._ProfitsOfPage ;
            }
            
            set
            {
                this._ProfitsOfPage  = value;
            }
        }

        public decimal ProfitsOfSearch
        {
            
            get
            {
                return this._ProfitsOfSearch ;
            }
            
            set
            {
                this._ProfitsOfSearch  = value;
            }
        }

        public int TotalCount
        {
            
            get
            {
                return this._TotalCount ;
            }
            
            set
            {
                this._TotalCount  = value;
            }
        }

        public decimal TotalOfPage
        {
            
            get
            {
                return this._TotalOfPage ;
            }
            
            set
            {
                this._TotalOfPage  = value;
            }
        }

        public decimal TotalOfSearch
        {
            
            get
            {
                return this._TotalOfSearch ;
            }
            
            set
            {
                this._TotalOfSearch  = value;
            }
        }
    }
}

