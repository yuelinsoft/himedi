namespace Hidistro.Entities.Store
{
    using System;
    using System.Runtime.CompilerServices;

    public class OperationLogEntry
    {
        
       DateTime _AddedTime ;
        
       string _Description ;
        
       string _IpAddress ;
        
       long _LogId ;
        
       string _PageUrl ;
        
       Hidistro.Entities.Store.Privilege _Privilege ;
        
       string _UserName ;

        public DateTime AddedTime
        {
            
            get
            {
                return this._AddedTime ;
            }
            
            set
            {
                this._AddedTime  = value;
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

        public string IpAddress
        {
            
            get
            {
                return this._IpAddress ;
            }
            
            set
            {
                this._IpAddress  = value;
            }
        }

        public long LogId
        {
            
            get
            {
                return this._LogId ;
            }
            
            set
            {
                this._LogId  = value;
            }
        }

        public string PageUrl
        {
            
            get
            {
                return this._PageUrl ;
            }
            
            set
            {
                this._PageUrl  = value;
            }
        }

        public Hidistro.Entities.Store.Privilege Privilege
        {
            
            get
            {
                return this._Privilege ;
            }
            
            set
            {
                this._Privilege  = value;
            }
        }

        public string UserName
        {
            
            get
            {
                return this._UserName ;
            }
            
            set
            {
                this._UserName  = value;
            }
        }
    }
}

