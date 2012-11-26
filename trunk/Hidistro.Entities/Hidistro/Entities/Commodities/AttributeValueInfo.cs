namespace Hidistro.Entities.Commodities
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class AttributeValueInfo
    {
        
       int _AttributeId ;
        
       int _DisplaySequence ;
        
       string _ImageUrl ;
        
       int _ValueId ;
        
       string _ValueStr ;

        public int AttributeId
        {
            
            get
            {
                return this._AttributeId ;
            }
            
            set
            {
                this._AttributeId  = value;
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

        public string ImageUrl
        {
            
            get
            {
                return this._ImageUrl ;
            }
            
            set
            {
                this._ImageUrl  = value;
            }
        }

        public int ValueId
        {
            
            get
            {
                return this._ValueId ;
            }
            
            set
            {
                this._ValueId  = value;
            }
        }

        public string ValueStr
        {
            
            get
            {
                return this._ValueStr ;
            }
            
            set
            {
                this._ValueStr  = value;
            }
        }
    }
}

