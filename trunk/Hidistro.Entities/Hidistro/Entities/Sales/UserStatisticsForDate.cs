namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserStatisticsForDate
    {
        
       int _TimePoint ;
        
       int _UserCounts ;

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
    }
}

