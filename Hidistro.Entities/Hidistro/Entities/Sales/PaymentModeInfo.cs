namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PaymentModeInfo
    {
        
       decimal _Charge ;
        
       string _Description ;
        
       int _DisplaySequence ;
        
       string _Gateway ;
        
       bool _IsPercent ;
        
       bool _IsUseInpour ;
        
       int _ModeId ;
        
       string _Name ;
        
       string _Settings ;

        public decimal CalcPayCharge(decimal cartMoney)
        {
            if (!this.IsPercent)
            {
                return this.Charge;
            }
            return (cartMoney * (this.Charge / 100M));
        }

        public decimal Charge
        {
            
            get
            {
                return this._Charge ;
            }
            
            set
            {
                this._Charge  = value;
            }
        }

        public string Description
        {
            
            get
            {
                return this._Description ;
            }
            
            set
            {
                this._Description  = value;
            }
        }

        public int DisplaySequence
        {
            
            get
            {
                return this._DisplaySequence ;
            }
            
            set
            {
                this._DisplaySequence  = value;
            }
        }

        public string Gateway
        {
            
            get
            {
                return this._Gateway ;
            }
            
            set
            {
                this._Gateway  = value;
            }
        }

        public bool IsPercent
        {
            
            get
            {
                return this._IsPercent ;
            }
            
            set
            {
                this._IsPercent  = value;
            }
        }

        public bool IsUseInpour
        {
            
            get
            {
                return this._IsUseInpour ;
            }
            
            set
            {
                this._IsUseInpour  = value;
            }
        }

        public int ModeId
        {
            
            get
            {
                return this._ModeId ;
            }
            
            set
            {
                this._ModeId  = value;
            }
        }

        [HtmlCoding]
        public string Name
        {
            
            get
            {
                return this._Name ;
            }
            
            set
            {
                this._Name  = value;
            }
        }

        public string Settings
        {
            
            get
            {
                return this._Settings ;
            }
            
            set
            {
                this._Settings  = value;
            }
        }
    }
}

