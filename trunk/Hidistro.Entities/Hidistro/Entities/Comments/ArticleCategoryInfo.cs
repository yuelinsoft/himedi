namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleCategoryInfo
    {
        
       int _CategoryId ;
        
       string _Description ;
        
       int _DisplaySequence ;
        
       string _IconUrl ;
        
       string _Name ;

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

        [StringLengthValidator(0, 300, Ruleset="ValArticleCategoryInfo", MessageTemplate="分类介绍最多只能输入300个字符"), HtmlCoding]
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

        public string IconUrl
        {
            
            get
            {
                return this._IconUrl ;
            }
            
            set
            {
                this._IconUrl  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValArticleCategoryInfo", MessageTemplate="分类名称不能为空，长度限制在60个字符以内"), HtmlCoding]
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
    }
}

