namespace Hidistro.Entities.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingModeGroupInfo
    {
        
       decimal _AddPrice ;
        
       int _GroupId ;
        
       decimal _Price ;
        
       int _TemplateId ;
       IList<ShippingRegionInfo> modeRegions = new List<ShippingRegionInfo>();

        public decimal AddPrice
        {
            
            get
            {
                return this._AddPrice ;
            }
            
            set
            {
                this._AddPrice  = value;
            }
        }

        public int GroupId
        {
            
            get
            {
                return this._GroupId ;
            }
            
            set
            {
                this._GroupId  = value;
            }
        }

        public IList<ShippingRegionInfo> ModeRegions
        {
            get
            {
                return this.modeRegions;
            }
            set
            {
                this.modeRegions = value;
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

        public int TemplateId
        {
            
            get
            {
                return this._TemplateId ;
            }
            
            set
            {
                this._TemplateId  = value;
            }
        }
    }
}

