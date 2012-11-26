namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CountDownInfo
    {
        
       string _Content ;
        
       int _CountDownId ;
        
       decimal _CountDownPrice ;
        
       int _DisplaySequence ;
        
       DateTime _EndDate ;
        
       int _ProductId ;

        public string Content
        {
            
            get
            {
                return this._Content ;
            }
            
            set
            {
                this._Content  = value;
            }
        }

        public int CountDownId
        {
            
            get
            {
                return this._CountDownId ;
            }
            
            set
            {
                this._CountDownId  = value;
            }
        }

        public decimal CountDownPrice
        {
            
            get
            {
                return this._CountDownPrice ;
            }
            
            set
            {
                this._CountDownPrice  = value;
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

        public DateTime EndDate
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

        public int ProductId
        {
            
            get
            {
                return this._ProductId ;
            }
            
            set
            {
                this._ProductId  = value;
            }
        }
    }
}

