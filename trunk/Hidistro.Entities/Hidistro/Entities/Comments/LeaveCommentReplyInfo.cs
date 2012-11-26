namespace Hidistro.Entities.Comments
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class LeaveCommentReplyInfo
    {
        
       long _LeaveId ;
        
       string _ReplyContent ;
        
       DateTime _ReplyDate ;
        
       long _ReplyId ;
        
       int _UserId ;

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

        [NotNullValidator(Ruleset="ValLeaveCommentReply", MessageTemplate="回复内容不能为空")]
        public string ReplyContent
        {
            
            get
            {
                return this._ReplyContent ;
            }
            
            set
            {
                this._ReplyContent  = value;
            }
        }

        public DateTime ReplyDate
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

        public long ReplyId
        {
            
            get
            {
                return this._ReplyId ;
            }
            
            set
            {
                this._ReplyId  = value;
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

