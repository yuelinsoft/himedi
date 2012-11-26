namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using System;
    using System.Runtime.CompilerServices;

    public class ReceiveMessageInfo
    {
        
       string _Addressee ;
        
       string _Addresser ;
        
       bool _IsRead ;
        
       DateTime _LastTime ;
        
       string _PublishContent ;
        
       DateTime _PublishDate ;
        
       long _ReceiveMessageId ;
        
       string _Title ;

        public string Addressee
        {
            
            get
            {
                return this._Addressee ;
            }
            
            set
            {
                this._Addressee  = value;
            }
        }

        public string Addresser
        {
            
            get
            {
                return this._Addresser ;
            }
            
            set
            {
                this._Addresser  = value;
            }
        }

        public bool IsRead
        {
            
            get
            {
                return this._IsRead ;
            }
            
            set
            {
                this._IsRead  = value;
            }
        }

        public DateTime LastTime
        {
            
            get
            {
                return this._LastTime ;
            }
            
            set
            {
                this._LastTime  = value;
            }
        }

        [HtmlCoding]
        public string PublishContent
        {
            
            get
            {
                return this._PublishContent ;
            }
            
            set
            {
                this._PublishContent  = value;
            }
        }

        public DateTime PublishDate
        {
            
            get
            {
                return this._PublishDate ;
            }
            
            set
            {
                this._PublishDate  = value;
            }
        }

        public long ReceiveMessageId
        {
            
            get
            {
                return this._ReceiveMessageId ;
            }
            
            set
            {
                this._ReceiveMessageId  = value;
            }
        }

        [HtmlCoding]
        public string Title
        {
            
            get
            {
                return this._Title ;
            }
            
            set
            {
                this._Title  = value;
            }
        }
    }
}

