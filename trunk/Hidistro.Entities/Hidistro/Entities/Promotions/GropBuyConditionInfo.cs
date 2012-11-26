namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class GropBuyConditionInfo
    {
        
       int _Count ;
        
       int _GroupBuyId ;
        
       decimal _Price ;

        public int Count
        {
            
            get
            {
                return this._Count ;
            }
            
            set
            {
                this._Count  = value;
            }
        }

        public int GroupBuyId
        {
            
            get
            {
                return this._GroupBuyId ;
            }
            
            set
            {
                this._GroupBuyId  = value;
            }
        }

        public decimal Price
        {
            
            get
            {
                return this._Price ;
            }
            
            set
            {
                this._Price  = value;
            }
        }
    }
}

