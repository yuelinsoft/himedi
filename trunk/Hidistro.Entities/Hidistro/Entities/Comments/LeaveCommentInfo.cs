namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class LeaveCommentInfo
    {
        
       DateTime _LastDate ;
        
       long _LeaveId ;
        
       string _PublishContent ;
        
       DateTime _PublishDate ;
        
       string _Title ;
        
       int? _UserId ;
        
       string _UserName ;

        public DateTime LastDate
        {
            
            get
            {
                return this._LastDate ;
            }
            
            set
            {
                this._LastDate  = value;
            }
        }

        public long LeaveId
        {
            
            get
            {
                return this._LeaveId ;
            }
            
            set
            {
                this._LeaveId  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="Refer", MessageTemplate="留言内容为必填项，长度限制在300字符以内")]
        public string PublishContent
        {
            
            get
            {
                return this._PublishContent ;
            }
            
            set
            {
                this._PublishContent  = value;
            }
        }

        public DateTime PublishDate
        {
            
            get
            {
                return this._PublishDate ;
            }
            
            set
            {
                this._PublishDate  = value;
            }
        }

        [StringLengthValidator(1, 100, Ruleset="Refer", MessageTemplate="标题为必填项，长度限制在100字符以内"), HtmlCoding]
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

        public int? UserId
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

        [StringLengthValidator(1, 60, Ruleset="Refer", MessageTemplate="用户名为必填项，长度限制在60字符以内"), HtmlCoding]
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

