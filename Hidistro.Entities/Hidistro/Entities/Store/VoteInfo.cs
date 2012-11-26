namespace Hidistro.Entities.Store
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class VoteInfo
    {
        
       bool _IsBackup ;
        
       int _MaxCheck ;
        
       int _VoteCounts ;
        
       long _VoteId ;
        
       IList<VoteItemInfo> _VoteItems ;
        
       string _VoteName ;

        public bool IsBackup
        {
            
            get
            {
                return this._IsBackup ;
            }
            
            set
            {
                this._IsBackup  = value;
            }
        }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 100, RangeBoundaryType.Inclusive, Ruleset="ValVote", MessageTemplate="最多可选项数不允许为空，范围为1-100之间的整数")]
        public int MaxCheck
        {
            
            get
            {
                return this._MaxCheck ;
            }
            
            set
            {
                this._MaxCheck  = value;
            }
        }

        public int VoteCounts
        {
            
            get
            {
                return this._VoteCounts ;
            }
            
            set
            {
                this._VoteCounts  = value;
            }
        }

        public long VoteId
        {
            
            get
            {
                return this._VoteId ;
            }
            
            set
            {
                this._VoteId  = value;
            }
        }

        public IList<VoteItemInfo> VoteItems
        {
            
            get
            {
                return this._VoteItems ;
            }
            
            set
            {
                this._VoteItems  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValVote", MessageTemplate="投票调查的标题不能为空，长度限制在60个字符以内")]
        public string VoteName
        {
            
            get
            {
                return this._VoteName ;
            }
            
            set
            {
                this._VoteName  = value;
            }
        }
    }
}

