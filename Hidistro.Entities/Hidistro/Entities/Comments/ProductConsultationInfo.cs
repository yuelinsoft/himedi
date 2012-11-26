namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductConsultationInfo
    {
        
       DateTime _ConsultationDate ;
        
       int _ConsultationId ;
        
       string _ConsultationText ;
        
       int _ProductId ;
        
       DateTime? _ReplyDate ;
        
       string _ReplyText ;
        
       int? _ReplyUserId ;
        
       string _UserEmail ;
        
       int _UserId ;
        
       string _UserName ;

        public DateTime ConsultationDate
        {
            
            get
            {
                return this._ConsultationDate ;
            }
            
            set
            {
                this._ConsultationDate  = value;
            }
        }

        public int ConsultationId
        {
            
            get
            {
                return this._ConsultationId ;
            }
            
            set
            {
                this._ConsultationId  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="咨询内容为必填项，长度限制在300字符以内")]
        public string ConsultationText
        {
            
            get
            {
                return this._ConsultationText ;
            }
            
            set
            {
                this._ConsultationText  = value;
            }
        }

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

        public DateTime? ReplyDate
        {
            
            get
            {
                return this._ReplyDate ;
            }
            
            set
            {
                this._ReplyDate  = value;
            }
        }

        [NotNullValidator(Ruleset="Reply", MessageTemplate="回内容为必填项")]
        public string ReplyText
        {
            
            get
            {
                return this._ReplyText ;
            }
            
            set
            {
                this._ReplyText  = value;
            }
        }

        public int? ReplyUserId
        {
            
            get
            {
                return this._ReplyUserId ;
            }
            
            set
            {
                this._ReplyUserId  = value;
            }
        }

        [RegexValidator(@"^[a-zA-Z\.0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$", Ruleset="Refer", MessageTemplate="邮箱地址必须为有效格式"), StringLengthValidator(1, 0x100, Ruleset="Refer", MessageTemplate="邮箱不能为空，长度限制在256字符以内")]
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

