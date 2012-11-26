namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductReviewInfo
    {
        
       int _ProductId ;
        
       DateTime _ReviewDate ;
        
       long _ReviewId ;
        
       string _ReviewText ;
        
       string _UserEmail ;
        
       int _UserId ;
        
       string _UserName ;

        public int ProductId
        {
            
            get
            {
                return this._ProductId ;
            }
            
            set
            {
                this._ProductId  = value;
            }
        }

        public DateTime ReviewDate
        {
            
            get
            {
                return this._ReviewDate ;
            }
            
            set
            {
                this._ReviewDate  = value;
            }
        }

        public long ReviewId
        {
            
            get
            {
                return this._ReviewId ;
            }
            
            set
            {
                this._ReviewId  = value;
            }
        }

        [StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="评论内容为必填项，长度限制在300字符以内"), HtmlCoding]
        public string ReviewText
        {
            
            get
            {
                return this._ReviewText ;
            }
            
            set
            {
                this._ReviewText  = value;
            }
        }

        [StringLengthValidator(1, 0x100, Ruleset="Refer", MessageTemplate="邮箱不能为空，长度限制在256字符以内"), RegexValidator(@"^[a-zA-Z\.0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", Ruleset="Refer", MessageTemplate="邮箱地址必须为有效格式")]
        public string UserEmail
        {
            
            get
            {
                return this._UserEmail ;
            }
            
            set
            {
                this._UserEmail  = value;
            }
        }

        public int UserId
        {
            
            get
            {
                return this._UserId ;
            }
            
            set
            {
                this._UserId  = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="Refer", MessageTemplate="用户昵称为必填项，长度限制在30字符以内"), HtmlCoding]
        public string UserName
        {
            
            get
            {
                return this._UserName ;
            }
            
            set
            {
                this._UserName  = value;
            }
        }
    }
}

