namespace Hidistro.Entities.Store
{
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class FriendlyLinksInfo
    {
        
       int _DisplaySequence ;
        
       string _ImageUrl ;
        
       int? _LinkId ;
        
       string _LinkUrl ;
        
       string _Title ;
        
       bool _Visible ;

        public int DisplaySequence
        {
            
            get
            {
                return this._DisplaySequence ;
            }
            
            set
            {
                this._DisplaySequence  = value;
            }
        }

        public string ImageUrl
        {
            
            get
            {
                return this._ImageUrl ;
            }
            
            set
            {
                this._ImageUrl  = value;
            }
        }

        public int? LinkId
        {
            
            get
            {
                return this._LinkId ;
            }
            
            set
            {
                this._LinkId  = value;
            }
        }

        [IgnoreNulls, RegexValidator(@"^(http://).*[\.]+.*", Ruleset="ValFriendlyLinksInfo"), ValidatorComposition(CompositionType.Or, Ruleset="ValFriendlyLinksInfo", MessageTemplate="网站地址必须为有效格式"), StringLengthValidator(0, Ruleset="ValFriendlyLinksInfo")]
        public string LinkUrl
        {
            
            get
            {
                return this._LinkUrl ;
            }
            
            set
            {
                this._LinkUrl  = value;
            }
        }

        [StringLengthValidator(0, 60, Ruleset="ValFriendlyLinksInfo", MessageTemplate="网站名称长度限制在60个字符以内")]
        public string Title
        {
            
            get
            {
                return this._Title ;
            }
            
            set
            {
                this._Title  = value;
            }
        }

        public bool Visible
        {
            
            get
            {
                return this._Visible ;
            }
            
            set
            {
                this._Visible  = value;
            }
        }
    }
}

