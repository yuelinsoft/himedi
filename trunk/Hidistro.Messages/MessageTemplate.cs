namespace Hidistro.Messages
{
    using System;
    using System.Runtime.CompilerServices;

    public class MessageTemplate
    {
        
       string _EmailBody ;
        
       string _EmailSubject ;
        
       string _InnerMessageBody ;
        
       string _InnerMessageSubject ;
        
       string _MessageType ;
        
       string _Name ;
        
       bool _SendEmail ;
        
       bool _SendInnerMessage ;
        
       bool _SendSMS ;
        
       string _SMSBody ;
        
       string _TagDescription ;

        public MessageTemplate()
        {
        }

        public MessageTemplate(string tagDescription, string name)
        {
            this.TagDescription = tagDescription;
            this.Name = name;
        }

        public string EmailBody
        {
            
            get
            {
                return this._EmailBody ;
            }
            
            set
            {
                this._EmailBody  = value;
            }
        }

        public string EmailSubject
        {
            
            get
            {
                return this._EmailSubject ;
            }
            
            set
            {
                this._EmailSubject  = value;
            }
        }

        public string InnerMessageBody
        {
            
            get
            {
                return this._InnerMessageBody ;
            }
            
            set
            {
                this._InnerMessageBody  = value;
            }
        }

        public string InnerMessageSubject
        {
            
            get
            {
                return this._InnerMessageSubject ;
            }
            
            set
            {
                this._InnerMessageSubject  = value;
            }
        }

        public string MessageType
        {
            
            get
            {
                return this._MessageType ;
            }
            
            set
            {
                this._MessageType  = value;
            }
        }

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

        public bool SendEmail
        {
            
            get
            {
                return this._SendEmail ;
            }
            
            set
            {
                this._SendEmail  = value;
            }
        }

        public bool SendInnerMessage
        {
            
            get
            {
                return this._SendInnerMessage ;
            }
            
            set
            {
                this._SendInnerMessage  = value;
            }
        }

        public bool SendSMS
        {
            
            get
            {
                return this._SendSMS ;
            }
            
            set
            {
                this._SendSMS  = value;
            }
        }

        public string SMSBody
        {
            
            get
            {
                return this._SMSBody ;
            }
            
            set
            {
                this._SMSBody  = value;
            }
        }

        public string TagDescription
        {
            
            get
            {
                return this._TagDescription ;
            }
            
           set
            {
                this._TagDescription  = value;
            }
        }
    }
}

