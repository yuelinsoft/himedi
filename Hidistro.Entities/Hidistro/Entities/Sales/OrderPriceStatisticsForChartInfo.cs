namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class OrderPriceStatisticsForChartInfo
    {
        
       decimal _Price ;
        
       int _TimePoint ;

        public decimal Price
        {
            
            get
            {
                return this._Price ;
            }
            
            set
            {
                this._Price  = value;
            }
        }

        public int TimePoint
        {
            
            get
            {
                return this._TimePoint ;
            }
            
            set
            {
                this._TimePoint  = value;
            }
        }
    }
}

