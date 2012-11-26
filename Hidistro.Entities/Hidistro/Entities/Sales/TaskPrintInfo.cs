namespace Hidistro.Entities.Sales
{
    using System;
    using System.Data;
    using System.Runtime.CompilerServices;

    public class TaskPrintInfo
    {
        
       DateTime _CreateTime ;
        
       string _Creator ;
        
       int _HasPrinted ;
        
       bool _IsPO ;
        
       object _Orders ;
        
       int _TaskId ;

        public DateTime CreateTime
        {
            
            get
            {
                return this._CreateTime ;
            }
            
            set
            {
                this._CreateTime  = value;
            }
        }

        public string Creator
        {
            
            get
            {
                return this._Creator ;
            }
            
            set
            {
                this._Creator  = value;
            }
        }

        public int HasPrinted
        {
            
            get
            {
                return this._HasPrinted ;
            }
            
            set
            {
                this._HasPrinted  = value;
            }
        }

        public bool IsPO
        {
            
            get
            {
                return this._IsPO ;
            }
            
            set
            {
                this._IsPO  = value;
            }
        }

        public object Orders
        {
            
            get
            {
                return this._Orders ;
            }
            
            set
            {
                this._Orders  = value;
            }
        }

        public int OrdersCount
        {
            get
            {
                if (this.Orders != null)
                {
                    return ((DataTable) this.Orders).Rows.Count;
                }
                return 0;
            }
        }

        public int TaskId
        {
            
            get
            {
                return this._TaskId ;
            }
            
            set
            {
                this._TaskId  = value;
            }
        }
    }
}

