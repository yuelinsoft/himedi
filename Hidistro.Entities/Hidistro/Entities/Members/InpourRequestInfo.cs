namespace Hidistro.Entities.Members
{
    using System;
    using System.Runtime.CompilerServices;

    public class InpourRequestInfo
    {
        
       decimal _InpourBlance ;
        
       string _InpourId ;
        
       int _PaymentId ;
        
       DateTime _TradeDate ;
        
       int _UserId ;

        public decimal InpourBlance
        {
            
            get
            {
                return this._InpourBlance ;
            }
            
            set
            {
                this._InpourBlance  = value;
            }
        }

        public string InpourId
        {
            
            get
            {
                return this._InpourId ;
            }
            
            set
            {
                this._InpourId  = value;
            }
        }

        public int PaymentId
        {
            
            get
            {
                return this._PaymentId ;
            }
            
            set
            {
                this._PaymentId  = value;
            }
        }

        public DateTime TradeDate
        {
            
            get
            {
                return this._TradeDate ;
            }
            
            set
            {
                this._TradeDate  = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return this._UserId ;
            }
            
            set
            {
                this._UserId  = value;
            }
        }
    }
}

