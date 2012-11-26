namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class SendedMessageQuery : Pagination
    {
        
       int _IsReply ;
        
       string _UserName ;

        public int IsReply
        {
            
            get
            {
                return this._IsReply ;
            }
            
            set
            {
                this._IsReply  = value;
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

