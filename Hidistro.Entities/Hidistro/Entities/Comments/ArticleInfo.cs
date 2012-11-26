namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ArticleInfo
    {
        
       DateTime _AddedDate ;
        
       int _ArticleId ;
        
       int _CategoryId ;
        
       string _CategoryName ;
        
       string _Content ;
        
       string _Description ;
        
       string _IconUrl ;
        
       bool _IsRelease ;
        
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

        public int ArticleId
        {
            
            get
            {
                return this._ArticleId ;
            }
            
            set
            {
                this._ArticleId  = value;
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

        public string CategoryName
        {
            
            get
            {
                return this._CategoryName ;
            }
            
            set
            {
                this._CategoryName  = value;
            }
        }

        [StringLengthValidator(1, 0x3b9ac9ff, Ruleset="ValArticleInfo", MessageTemplate="文章内容不能为空")]
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

        [HtmlCoding, StringLengthValidator(0, 300, Ruleset="ValArticleInfo", MessageTemplate="文章摘要的长度限制在300个字符以内")]
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

        public bool IsRelease
        {
            
            get
            {
                return this._IsRelease ;
            }
            
            set
            {
                this._IsRelease  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValArticleInfo", MessageTemplate="告诉搜索引擎此文章页面的主要内容，长度限制在100个字符以内")]
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

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValArticleInfo", MessageTemplate="让用户可以通过搜索引擎搜索到此文章的浏览页面，长度限制在100个字符以内")]
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

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValArticleInfo", MessageTemplate="文章标题不能为空，长度限制在60个字符以内")]
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

