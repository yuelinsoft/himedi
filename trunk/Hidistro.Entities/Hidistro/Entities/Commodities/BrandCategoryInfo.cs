namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class BrandCategoryInfo
    {
        
       int _BrandId ;
        
       string _BrandName ;
        
       string _CompanyUrl ;
        
       string _Description ;
        
       int _DisplaySequence ;
        
       string _Logo ;
        
       string _MetaDescription ;
        
       string _MetaKeywords ;
        
       string _RewriteName ;
        
       string _Theme ;
       IList<int> productTypes;

        public int BrandId
        {
            
            get
            {
                return this._BrandId ;
            }
            
            set
            {
                this._BrandId  = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="ValBrandCategory", MessageTemplate="品牌名称不能为空，长度限制在30个字符以内")]
        public string BrandName
        {
            
            get
            {
                return this._BrandName ;
            }
            
            set
            {
                this._BrandName  = value;
            }
        }

        [ValidatorComposition(CompositionType.Or, Ruleset="ValBrandCategory", MessageTemplate="品牌官方网站的网址必须以http://开头，长度限制在100个字符以内"), RegexValidator("^(http)://.*", Ruleset="ValBrandCategory"), NotNullValidator(Negated=true, Ruleset="ValBrandCategory")]
        public string CompanyUrl
        {
            
            get
            {
                return this._CompanyUrl ;
            }
            
            set
            {
                this._CompanyUrl  = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValBrandCategory", MessageTemplate="品牌介绍的长度限制在300个字符以内")]
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

        public string Logo
        {
            
            get
            {
                return this._Logo ;
            }
            
            set
            {
                this._Logo  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内")]
        public string MetaDescription
        {
            
            get
            {
                return this._MetaDescription ;
            }
            
            set
            {
                this._MetaDescription  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内")]
        public string MetaKeywords
        {
            
            get
            {
                return this._MetaKeywords ;
            }
            
            set
            {
                this._MetaKeywords  = value;
            }
        }

        public IList<int> ProductTypes
        {
            get
            {
                if (this.productTypes == null)
                {
                    this.productTypes = new List<int>();
                }
                return this.productTypes;
            }
            set
            {
                this.productTypes = value;
            }
        }

        public string RewriteName
        {
            
            get
            {
                return this._RewriteName ;
            }
            
            set
            {
                this._RewriteName  = value;
            }
        }

        public string Theme
        {
            
            get
            {
                return this._Theme ;
            }
            
            set
            {
                this._Theme  = value;
            }
        }
    }
}

