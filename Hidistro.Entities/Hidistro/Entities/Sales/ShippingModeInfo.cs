namespace Hidistro.Entities.Sales
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ShippingModeInfo
    {
        
       decimal? _AddPrice ;
        
       int? _AddWeight ;
        
       string _Description ;
        
       int _DisplaySequence ;
        
       string _ExpressCompanyAbb ;
        
       string _ExpressCompanyName ;
        
       int _ModeId ;
        
       string _Name ;
        
       decimal _Price ;
        
       int _TemplateId ;
        
       string _TemplateName ;
        
       int _Weight ;
       IList<ExpressCompanyInfo> expressCompany;
       IList<ShippingModeGroupInfo> modeGroup;

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo"), NotNullValidator(Negated=true, Ruleset="ValShippingModeInfo"), ValidatorComposition(CompositionType.Or, Ruleset="ValShippingModeInfo", MessageTemplate="默认加价必须限制在1000万以内")]
        public decimal? AddPrice
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

        [ValidatorComposition(CompositionType.Or, Ruleset="ValShippingModeInfo", MessageTemplate="加价重量必须限制在100千克以内"), RangeValidator(0, RangeBoundaryType.Inclusive, 0x186a0, RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo"), NotNullValidator(Negated=true, Ruleset="ValShippingModeInfo")]
        public int? AddWeight
        {
            
            get
            {
                return this._AddWeight ;
            }
            
            set
            {
                this._AddWeight  = value;
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

        public IList<ExpressCompanyInfo> ExpressCompany
        {
            get
            {
                if (this.expressCompany == null)
                {
                    this.expressCompany = new List<ExpressCompanyInfo>();
                }
                return this.expressCompany;
            }
            set
            {
                this.expressCompany = value;
            }
        }

        public string ExpressCompanyAbb
        {
            
            get
            {
                return this._ExpressCompanyAbb ;
            }
            
            set
            {
                this._ExpressCompanyAbb  = value;
            }
        }

        public string ExpressCompanyName
        {
            
            get
            {
                return this._ExpressCompanyName ;
            }
            
            set
            {
                this._ExpressCompanyName  = value;
            }
        }

        public IList<ShippingModeGroupInfo> ModeGroup
        {
            get
            {
                if (this.modeGroup == null)
                {
                    this.modeGroup = new List<ShippingModeGroupInfo>();
                }
                return this.modeGroup;
            }
            set
            {
                this.modeGroup = value;
            }
        }

        public int ModeId
        {
            
            get
            {
                return this._ModeId ;
            }
            
            set
            {
                this._ModeId  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValShippingModeInfo", MessageTemplate="配送方式名称不能为空，长度限制在60字符以内")]
        public string Name
        {
            
            get
            {
                return this._Name ;
            }
            
            set
            {
                this._Name  = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo", MessageTemplate="默认起步价不能为空,限制在1000万以内")]
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

        public string TemplateName
        {
            
            get
            {
                return this._TemplateName ;
            }
            
            set
            {
                this._TemplateName  = value;
            }
        }

        [RangeValidator(0, RangeBoundaryType.Inclusive, 0x186a0, RangeBoundaryType.Inclusive, Ruleset="ValShippingModeInfo", MessageTemplate="起步重量不能为空,限制在100千克以内")]
        public int Weight
        {
            
            get
            {
                return this._Weight ;
            }
            
            set
            {
                this._Weight  = value;
            }
        }
    }
}

