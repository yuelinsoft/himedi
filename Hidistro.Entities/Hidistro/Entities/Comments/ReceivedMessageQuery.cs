namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ReceivedMessageQuery : Pagination
    {
        
       bool? _IsRead ;
        
       Hidistro.Entities.Comments.MessageStatus _MessageStatus ;
        
       string _UserName ;

        public bool? IsRead
        {
            
            get
            {
                return this._IsRead ;
            }
            
            set
            {
                this._IsRead  = value;
            }
        }

        public Hidistro.Entities.Comments.MessageStatus MessageStatus
        {
            
            get
            {
                return this._MessageStatus ;
            }
            
            set
            {
                this._MessageStatus  = value;
            }
        }

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

