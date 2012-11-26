namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class OperationLogQuery
    {
        
       DateTime? _FromDate ;
        
       string _OperationUserName ;
        
       Pagination _Page ;
        
       DateTime? _ToDate ;

        public OperationLogQuery()
        {
            this.Page = new Pagination();
        }

        public DateTime? FromDate
        {
            
            get
            {
                return this._FromDate ;
            }
            
            set
            {
                this._FromDate  = value;
            }
        }

        public string OperationUserName
        {
            
            get
            {
                return this._OperationUserName ;
            }
            
            set
            {
                this._OperationUserName  = value;
            }
        }

        public Pagination Page
        {
            
            get
            {
                return this._Page ;
            }
            
            set
            {
                this._Page  = value;
            }
        }

        public DateTime? ToDate
        {
            
            get
            {
                return this._ToDate ;
            }
            
            set
            {
                this._ToDate  = value;
            }
        }
    }
}

