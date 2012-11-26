namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    public class UserPointInfo
    {
        
       int? _Increased ;
        
       long _JournalNumber ;
        
       string _OrderId ;
        
       int _Points ;
        
       int? _Reduced ;
        
       string _Remark ;
        
       DateTime _TradeDate ;
        
       UserPointTradeType _TradeType ;
        
       int _UserId ;

        public int? Increased
        {
            
            get
            {
                return this._Increased ;
            }
            
            set
            {
                this._Increased  = value;
            }
        }

        public long JournalNumber
        {
            
            get
            {
                return this._JournalNumber ;
            }
            
            set
            {
                this._JournalNumber  = value;
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

        public int Points
        {
            
            get
            {
                return this._Points ;
            }
            
            set
            {
                this._Points  = value;
            }
        }

        public int? Reduced
        {
            
            get
            {
                return this._Reduced ;
            }
            
            set
            {
                this._Reduced  = value;
            }
        }

        public string Remark
        {
            
            get
            {
                return this._Remark ;
            }
            
            set
            {
                this._Remark  = value;
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

        public UserPointTradeType TradeType
        {
            
            get
            {
                return this._TradeType ;
            }
            
            set
            {
                this._TradeType  = value;
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

