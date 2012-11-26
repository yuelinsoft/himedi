namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PromotionInfo
    {
        
       int _ActivityId ;
        
       string _Description ;
        
       string _Name ;
        
       Hidistro.Entities.Promotions.PromoteType _PromoteType ;
       IList<int> memberGradeIds = new List<int>();

        public int ActivityId
        {
            
            get
            {
                return this._ActivityId ;
            }
            
            set
            {
                this._ActivityId  = value;
            }
        }

        public string Description
        {
            
            get
            {
                return this._Description ;
            }
            
            set
            {
                this._Description  = value;
            }
        }

        public IList<int> MemberGradeIds
        {
            get
            {
                return this.memberGradeIds;
            }
            set
            {
                this.memberGradeIds = value;
            }
        }

        [HtmlCoding, StringLengthValidator(1, 60, Ruleset="ValPromotion", MessageTemplate="促销活动名称不能为空，长度限制在1-60个字符之间")]
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

        public Hidistro.Entities.Promotions.PromoteType PromoteType
        {
            
            get
            {
                return this._PromoteType ;
            }
            
            protected set
            {
                this._PromoteType  = value;
            }
        }
    }
}

