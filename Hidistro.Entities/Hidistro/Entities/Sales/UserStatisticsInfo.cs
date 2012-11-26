namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UserStatisticsInfo
    {
        
       decimal _AllUserCounts ;
        
       long _RegionId ;
        
       string _RegionName ;
        
       int _Usercounts ;

        public decimal AllUserCounts
        {
            
            get
            {
                return this._AllUserCounts ;
            }
            
            set
            {
                this._AllUserCounts  = value;
            }
        }

        public decimal Lenth
        {
            get
            {
                return (this.Percentage * 4M);
            }
        }

        public decimal Percentage
        {
            get
            {
                if (this.AllUserCounts != 0M)
                {
                    return ((this.Usercounts / this.AllUserCounts) * 100M);
                }
                return 0M;
            }
        }

        public long RegionId
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

        public string RegionName
        {
            
            get
            {
                return this._RegionName ;
            }
            
            set
            {
                this._RegionName  = value;
            }
        }

        public int Usercounts
        {
            
            get
            {
                return this._Usercounts ;
            }
            
            set
            {
                this._Usercounts  = value;
            }
        }
    }
}

