namespace Hidistro.Entities.Distribution
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    [HasSelfValidation]
    public class SiteRequestInfo
    {
        
       string _FirstRecordCode ;
        
       string _FirstSiteUrl ;
        
       string _RefuseReason ;
        
       int _RequestId ;
        
       SiteRequestStatus _RequestStatus ;
        
       DateTime _RequestTime ;
        
       string _SecondRecordCode ;
        
       string _SecondSiteUrl ;
        
       int _UserId ;

        [SelfValidation(Ruleset="ValSiteRequest")]
        public void CheckMemberEmail(ValidationResults results)
        {
            if (!Regex.IsMatch(this.FirstSiteUrl, @"[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?$"))
            {
                results.AddResult(new ValidationResult("第一个域名的格式错误", this, "", "", null));
            }
            if (!(string.IsNullOrEmpty(this.SecondSiteUrl) || Regex.IsMatch(this.SecondSiteUrl, @"[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?$")))
            {
                results.AddResult(new ValidationResult("第二个域名的格式错误", this, "", "", null));
            }
        }

        [StringLengthValidator(1, 20, Ruleset="ValSiteRequest", MessageTemplate="第一个域名的备案号不能为空,长度限制在20个字符以内")]
        public string FirstRecordCode
        {
            
            get
            {
                return this._FirstRecordCode ;
            }
            
            set
            {
                this._FirstRecordCode  = value;
            }
        }

        [StringLengthValidator(1, 30, Ruleset="ValSiteRequest", MessageTemplate="第一个域名不能为空,长度限制在30个字符以内,必须为有效格式")]
        public string FirstSiteUrl
        {
            
            get
            {
                return this._FirstSiteUrl ;
            }
            
            set
            {
                this._FirstSiteUrl  = value;
            }
        }

        public string RefuseReason
        {
            
            get
            {
                return this._RefuseReason ;
            }
            
            set
            {
                this._RefuseReason  = value;
            }
        }

        public int RequestId
        {
            
            get
            {
                return this._RequestId ;
            }
            
            set
            {
                this._RequestId  = value;
            }
        }

        public SiteRequestStatus RequestStatus
        {
            
            get
            {
                return this._RequestStatus ;
            }
            
            set
            {
                this._RequestStatus  = value;
            }
        }

        public DateTime RequestTime
        {
            
            get
            {
                return this._RequestTime ;
            }
            
            set
            {
                this._RequestTime  = value;
            }
        }

        [ValidatorComposition(CompositionType.Or, Ruleset="ValSiteRequest", MessageTemplate="第二个域名的备案号长度限制在20个字符以内"), IgnoreNulls, StringLengthValidator(1, 20, Ruleset="ValSiteRequest"), StringLengthValidator(0, Ruleset="ValSiteRequest")]
        public string SecondRecordCode
        {
            
            get
            {
                return this._SecondRecordCode ;
            }
            
            set
            {
                this._SecondRecordCode  = value;
            }
        }

        [StringLengthValidator(0, Ruleset="ValSiteRequest"), StringLengthValidator(1, 30, Ruleset="ValSiteRequest"), IgnoreNulls, ValidatorComposition(CompositionType.Or, Ruleset="ValSiteRequest", MessageTemplate="第二个域名长度限制在30个字符以内,必须为有效格式")]
        public string SecondSiteUrl
        {
            
            get
            {
                return this._SecondSiteUrl ;
            }
            
            set
            {
                this._SecondSiteUrl  = value;
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

