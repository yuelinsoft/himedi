namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class AccountSummaryInfo
    {
        
       decimal _AccountAmount ;
        
       decimal _DrawRequestBalance ;
        
       decimal _FreezeBalance ;
        
       decimal _UseableBalance ;

        public decimal AccountAmount
        {
            
            get
            {
                return this._AccountAmount ;
            }
            
            set
            {
                this._AccountAmount  = value;
            }
        }

        public decimal DrawRequestBalance
        {
            
            get
            {
                return this._DrawRequestBalance ;
            }
            
            set
            {
                this._DrawRequestBalance  = value;
            }
        }

        public decimal FreezeBalance
        {
            
            get
            {
                return this._FreezeBalance ;
            }
            
            set
            {
                this._FreezeBalance  = value;
            }
        }

        public decimal UseableBalance
        {
            
            get
            {
                return this._UseableBalance ;
            }
            
            set
            {
                this._UseableBalance  = value;
            }
        }
    }
}

