namespace Hidistro.Entities.Commodities
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class AttributeInfo
    {
        
       int _AttributeId ;
        
       string _AttributeName ;
        
       int _DisplaySequence ;
        
       int _TypeId ;
        
       string _TypeName ;
        
       AttributeUseageMode _UsageMode ;
        
       bool _UseAttributeImage ;
       IList<AttributeValueInfo> attributeValues;

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

        [StringLengthValidator(1, 30, Ruleset="ValAttribute", MessageTemplate="扩展属性的名称，长度在1至30个字符之间")]
        public string AttributeName
        {
            
            get
            {
                return this._AttributeName ;
            }
            
            set
            {
                this._AttributeName  = value;
            }
        }

        public IList<AttributeValueInfo> AttributeValues
        {
            get
            {
                if (this.attributeValues == null)
                {
                    this.attributeValues = new List<AttributeValueInfo>();
                }
                return this.attributeValues;
            }
            set
            {
                this.attributeValues = value;
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

        public bool IsMultiView
        {
            get
            {
                return (this.UsageMode == AttributeUseageMode.MultiView);
            }
        }

        public int TypeId
        {
            
            get
            {
                return this._TypeId ;
            }
            
            set
            {
                this._TypeId  = value;
            }
        }

        public string TypeName
        {
            
            get
            {
                return this._TypeName ;
            }
            
            set
            {
                this._TypeName  = value;
            }
        }

        public AttributeUseageMode UsageMode
        {
            
            get
            {
                return this._UsageMode ;
            }
            
            set
            {
                this._UsageMode  = value;
            }
        }

        public bool UseAttributeImage
        {
            
            get
            {
                return this._UseAttributeImage ;
            }
            
            set
            {
                this._UseAttributeImage  = value;
            }
        }

        public string ValuesString
        {
            get
            {
                string str = string.Empty;
                foreach (AttributeValueInfo info in this.AttributeValues)
                {
                    str = str + info.ValueStr + ",";
                }
                if (str.Length > 0)
                {
                    str = str.Substring(0, str.Length - 1);
                }
                return str;
            }
        }
    }
}

