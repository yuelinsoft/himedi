namespace Hidistro.Entities.Members
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberGradeInfo
    {
        
       string _Description ;
        
       int _Discount ;
        
       int _GradeId ;
        
       bool _IsDefault ;
        
       string _Name ;
        
       int _Points ;

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValMemberGrade", MessageTemplate="备注的长度限制在100个字符以内")]
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

        [RangeValidator(typeof(int), "1", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset="ValMemberGrade", MessageTemplate="等级折扣只能是1-100之间的整数")]
        public int Discount
        {
            
            get
            {
                return this._Discount ;
            }
            
            set
            {
                this._Discount  = value;
            }
        }

        public int GradeId
        {
            
            get
            {
                return this._GradeId ;
            }
            
            set
            {
                this._GradeId  = value;
            }
        }

        public bool IsDefault
        {
            
            get
            {
                return this._IsDefault ;
            }
            
            set
            {
                this._IsDefault  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValMemberGrade", MessageTemplate="会员等级名称不能为空，长度限制在60个字符以内"), HtmlCoding]
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

        [RangeValidator(0, RangeBoundaryType.Inclusive, 0x7fffffff, RangeBoundaryType.Inclusive, Ruleset="ValMemberGrade", MessageTemplate="满足积分为大于等于0的整数")]
        public int Points
        {
            
            get
            {
                return this._Points ;
            }
            
            set
            {
                this._Points  = value;
            }
        }
    }
}

