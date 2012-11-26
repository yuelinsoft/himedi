namespace Hidistro.Entities.Sales
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingRegionInfo
    {
        
       int _GroupId ;
        
       int _RegionId ;
        
       int _TemplateId ;

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

        public int RegionId
        {
            
            get
            {
                return this._RegionId ;
            }
            
            set
            {
                this._RegionId  = value;
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

