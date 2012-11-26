namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class HelpInfo
    {
        
       DateTime _AddedDate ;
        
       int _CategoryId ;
        
       string _Content ;
        
       string _Description ;
        
       int _HelpId ;
        
       bool _IsShowFooter ;
        
       string _MetaDescription ;
        
       string _MetaKeywords ;
        
       string _Title ;

        public DateTime AddedDate
        {
            
            get
            {
                return this._AddedDate ;
            }
            
            set
            {
                this._AddedDate  = value;
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

        [StringLengthValidator(1, 0x3b9ac9ff, Ruleset="ValHelpInfo", MessageTemplate="帮助内容不能为空")]
        public string Content
        {
            
            get
            {
                return this._Content ;
            }
            
            set
            {
                this._Content  = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValHelpInfo", MessageTemplate="摘要的长度限制在300个字符以内"), HtmlCoding]
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

        public int HelpId
        {
            
            get
            {
                return this._HelpId ;
            }
            
            set
            {
                this._HelpId  = value;
            }
        }

        public bool IsShowFooter
        {
            
            get
            {
                return this._IsShowFooter ;
            }
            
            set
            {
                this._IsShowFooter  = value;
            }
        }

        [StringLengthValidator(0, 100, Ruleset="ValHelpInfo", MessageTemplate="告诉搜索引擎此帮助页面的主要内容，长度限制在100个字符以内"), HtmlCoding]
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

        [StringLengthValidator(0, 100, Ruleset="ValHelpInfo", MessageTemplate="让用户可以通过搜索引擎搜索到此帮助的浏览页面，长度限制在100个字符以内"), HtmlCoding]
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

        [StringLengthValidator(1, 60, Ruleset="ValHelpInfo", MessageTemplate="帮助主题不能为空，长度限制在60个字符以内"), HtmlCoding]
        public string Title
        {
            
            get
            {
                return this._Title ;
            }
            
            set
            {
                this._Title  = value;
            }
        }
    }
}

