namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class SendMessageInfo
    {
        
       string _Addressee ;
        
       string _Addresser ;
        
       string _PublishContent ;
        
       DateTime _PublishDate ;
        
       long? _ReceiveMessageId ;
        
       long _SendMessageId ;
        
       string _Title ;

        public string Addressee
        {
            
            get
            {
                return this._Addressee ;
            }
            
            set
            {
                this._Addressee  = value;
            }
        }

        public string Addresser
        {
            
            get
            {
                return this._Addresser ;
            }
            
            set
            {
                this._Addresser  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 300, Ruleset="ValSendMessage", MessageTemplate="回复内容必填项，长度限制在300字符以内")]
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

        public long? ReceiveMessageId
        {
            
            get
            {
                return this._ReceiveMessageId ;
            }
            
            set
            {
                this._ReceiveMessageId  = value;
            }
        }

        public long SendMessageId
        {
            
            get
            {
                return this._SendMessageId ;
            }
            
            set
            {
                this._SendMessageId  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValSendMessage", MessageTemplate="回复标题必填项，长度限制在60字符以内"), HtmlCoding]
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

