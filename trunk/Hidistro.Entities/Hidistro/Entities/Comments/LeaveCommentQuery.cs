namespace Hidistro.Entities.Comments
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class LeaveCommentQuery : Pagination
    {
        
       int? _AgentId ;
        
       Hidistro.Entities.Comments.MessageStatus _MessageStatus ;

        public int? AgentId
        {
            
            get
            {
                return this._AgentId ;
            }
            
            set
            {
                this._AgentId  = value;
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
    }
}

