namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductConsultationAndReplyQuery : Pagination
    {
        
       int? _CategoryId ;
        
       int _ConsultationId ;
        
       string _Keywords ;
        
       string _ProductCode ;
        
       int _ProductId ;
        
       ConsultationReplyType _Type ;
        
       int _UserId ;

        public int? CategoryId
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

        [HtmlCoding]
        public string Keywords
        {
            
            get
            {
                return this._Keywords ;
            }
            
            set
            {
                this._Keywords  = value;
            }
        }

        [HtmlCoding]
        public string ProductCode
        {
            
            get
            {
                return this._ProductCode ;
            }
            
            set
            {
                this._ProductCode  = value;
            }
        }

        public virtual int ProductId
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

        public ConsultationReplyType Type
        {
            
            get
            {
                return this._Type ;
            }
            
            set
            {
                this._Type  = value;
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
    }
}

