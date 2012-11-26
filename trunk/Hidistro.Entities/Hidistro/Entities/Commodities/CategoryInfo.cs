namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    [HasSelfValidation]
    public class CategoryInfo
    {
        
       int? _AssociatedProductType ;
        
       int _CategoryId ;
        
       int _Depth ;
        
       string _Description ;
        
       int _DisplaySequence ;
        
       bool _HasChildren ;
        
       string _MetaDescription ;
        
       string _MetaKeywords ;
        
       string _MetaTitle ;
        
       string _Name ;
        
       string _Notes1 ;
        
       string _Notes2 ;
        
       string _Notes3 ;
        
       string _Notes4 ;
        
       string _Notes5 ;
        
       int? _ParentCategoryId ;
        
       string _Path ;
        
       string _RewriteName ;
        
       string _SKUPrefix ;
        
       string _Theme ;

        [SelfValidation(Ruleset="ValCategory")]
        public void CheckCategory(ValidationResults results)
        {
            if (!(string.IsNullOrEmpty(this.SKUPrefix) || ((this.SKUPrefix.Length <= 5) && Regex.IsMatch(this.SKUPrefix, "(?!_)(?!-)[a-zA-Z0-9_-]+"))))
            {
                results.AddResult(new ValidationResult("商家编码前缀长度限制在5个字符以内,只能以字母或数字开头", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.RewriteName) || ((this.RewriteName.Length <=60) && Regex.IsMatch(this.RewriteName, "(^[-_a-zA-Z0-9]+$)"))))
            {
                results.AddResult(new ValidationResult("使用URL重写长度限制在60个字符以内，必须为字母数字-和_", this, "", "", null));
            }
        }

        public int? AssociatedProductType
        {
            
            get
            {
                return this._AssociatedProductType ;
            }
            
            set
            {
                this._AssociatedProductType  = value;
            }
        }

        public int CategoryId
        {
            
            get
            {
                return this._CategoryId ;
            }
            
            set
            {
                this._CategoryId  = value;
            }
        }

        public int Depth
        {
            
            get
            {
                return this._Depth ;
            }
            
            set
            {
                this._Depth  = value;
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

        public bool HasChildren
        {
            
            get
            {
                return this._HasChildren ;
            }
            
            set
            {
                this._HasChildren  = value;
            }
        }

        [StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="告诉搜索引擎此分类浏览页面的主要内容，长度限制在100个字符以内"), HtmlCoding]
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

        [StringLengthValidator(0, 100, Ruleset="ValCategory", MessageTemplate="让用户可以通过搜索引擎搜索到此分类的浏览页面，长度限制在100个字符以内"), HtmlCoding]
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

        [StringLengthValidator(0, 50, Ruleset="ValCategory", MessageTemplate="告诉搜索引擎此分类浏览页面的标题，长度限制在50个字符以内"), HtmlCoding]
        public string MetaTitle
        {
            
            get
            {
                return this._MetaTitle ;
            }
            
            set
            {
                this._MetaTitle  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValCategory", MessageTemplate="分类名称不能为空，长度限制在60个字符以内"), HtmlCoding]
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

        public string Notes1
        {
            
            get
            {
                return this._Notes1 ;
            }
            
            set
            {
                this._Notes1  = value;
            }
        }

        public string Notes2
        {
            
            get
            {
                return this._Notes2 ;
            }
            
            set
            {
                this._Notes2  = value;
            }
        }

        public string Notes3
        {
            
            get
            {
                return this._Notes3 ;
            }
            
            set
            {
                this._Notes3  = value;
            }
        }

        public string Notes4
        {
            
            get
            {
                return this._Notes4 ;
            }
            
            set
            {
                this._Notes4  = value;
            }
        }

        public string Notes5
        {
            
            get
            {
                return this._Notes5 ;
            }
            
            set
            {
                this._Notes5  = value;
            }
        }

        public int? ParentCategoryId
        {
            
            get
            {
                return this._ParentCategoryId ;
            }
            
            set
            {
                this._ParentCategoryId  = value;
            }
        }

        public string Path
        {
            
            get
            {
                return this._Path ;
            }
            
            set
            {
                this._Path  = value;
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

        [HtmlCoding]
        public string SKUPrefix
        {
            
            get
            {
                return this._SKUPrefix ;
            }
            
            set
            {
                this._SKUPrefix  = value;
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

        public int TopCategoryId
        {
            get
            {
                if (this.Depth == 1)
                {
                    return this.CategoryId;
                }
                return int.Parse(this.Path.Substring(0, this.Path.IndexOf("|")));
            }
        }
    }
}

