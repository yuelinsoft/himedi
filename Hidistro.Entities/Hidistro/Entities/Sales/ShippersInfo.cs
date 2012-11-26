namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ShippersInfo
    {
        
       string _Address ;
        
       string _CellPhone ;
        
       int _DistributorUserId ;
        
       bool _IsDefault ;
        
       int _RegionId ;
        
       string _Remark ;
        
       int _ShipperId ;
        
       string _ShipperName ;
        
       string _ShipperTag ;
        
       string _TelPhone ;
        
       string _Zipcode ;

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="Valshipper", MessageTemplate="详细地址不能为空，长度限制在300个字符以内")]
        public string Address
        {
            
            get
            {
                return this._Address ;
            }
            
            set
            {
                this._Address  = value;
            }
        }

        [StringLengthValidator(0, 20, Ruleset="Valshipper", MessageTemplate="手机号码的长度限制在20个字符以内"), HtmlCoding]
        public string CellPhone
        {
            
            get
            {
                return this._CellPhone ;
            }
            
            set
            {
                this._CellPhone  = value;
            }
        }

        public int DistributorUserId
        {
            
            get
            {
                return this._DistributorUserId ;
            }
            
            set
            {
                this._DistributorUserId  = value;
            }
        }

        public bool IsDefault
        {
            
            get
            {
                return this._IsDefault ;
            }
            
            set
            {
                this._IsDefault  = value;
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

        [HtmlCoding, StringLengthValidator(0, 300, Ruleset="Valshipper", MessageTemplate="备注的长度限制在300个字符以内")]
        public string Remark
        {
            
            get
            {
                return this._Remark ;
            }
            
            set
            {
                this._Remark  = value;
            }
        }

        public int ShipperId
        {
            
            get
            {
                return this._ShipperId ;
            }
            
            set
            {
                this._ShipperId  = value;
            }
        }

        [RegexValidator(@"[\u4e00-\u9fa5a-zA-Z]+[\u4e00-\u9fa5_a-zA-Z0-9]*", Ruleset="Valshipper", MessageTemplate="发货人姓名只能是汉字或字母开头"), HtmlCoding, StringLengthValidator(2, 20, Ruleset="Valshipper", MessageTemplate="发货人姓名不能为空，长度在2-20个字符之间")]
        public string ShipperName
        {
            
            get
            {
                return this._ShipperName ;
            }
            
            set
            {
                this._ShipperName  = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="Valshipper", MessageTemplate="发货点不能为空，长度限制在30个字符以内"), HtmlCoding]
        public string ShipperTag
        {
            
            get
            {
                return this._ShipperTag ;
            }
            
            set
            {
                this._ShipperTag  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 20, Ruleset="Valshipper", MessageTemplate="电话号码的长度限制在20个字符以内")]
        public string TelPhone
        {
            
            get
            {
                return this._TelPhone ;
            }
            
            set
            {
                this._TelPhone  = value;
            }
        }

        [StringLengthValidator(0, 20, Ruleset="Valshipper", MessageTemplate="邮编的长度限制在20个字符以内"), HtmlCoding]
        public string Zipcode
        {
            
            get
            {
                return this._Zipcode ;
            }
            
            set
            {
                this._Zipcode  = value;
            }
        }
    }
}

