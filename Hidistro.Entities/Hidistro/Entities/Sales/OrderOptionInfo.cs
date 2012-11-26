namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml.Serialization;

    [Serializable, XmlInclude(typeof(OrderOptionInfo)), XmlInclude(typeof(OrderOptionInfo)), SoapInclude(typeof(OrderOptionInfo))]
    public class OrderOptionInfo
    {
        
       decimal _AdjustedPrice ;
        
       string _CustomerDescription ;
        
       string _CustomerTitle ;
        
       string _ItemDescription ;
        
       string _ListDescription ;
        
       int _LookupItemId ;
        
       int _LookupListId ;
        
       string _OrderId ;

        public decimal AdjustedPrice
        {
            
            get
            {
                return this._AdjustedPrice ;
            }
            
            set
            {
                this._AdjustedPrice  = value;
            }
        }

        public string CustomerDescription
        {
            
            get
            {
                return this._CustomerDescription ;
            }
            
            set
            {
                this._CustomerDescription  = value;
            }
        }

        public string CustomerTitle
        {
            
            get
            {
                return this._CustomerTitle ;
            }
            
            set
            {
                this._CustomerTitle  = value;
            }
        }

        public string ItemDescription
        {
            
            get
            {
                return this._ItemDescription ;
            }
            
            set
            {
                this._ItemDescription  = value;
            }
        }

        public string ListDescription
        {
            
            get
            {
                return this._ListDescription ;
            }
            
            set
            {
                this._ListDescription  = value;
            }
        }

        public int LookupItemId
        {
            
            get
            {
                return this._LookupItemId ;
            }
            
            set
            {
                this._LookupItemId  = value;
            }
        }

        public int LookupListId
        {
            
            get
            {
                return this._LookupListId ;
            }
            
            set
            {
                this._LookupListId  = value;
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
    }
}

