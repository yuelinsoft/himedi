namespace Hidistro.Entities.Store
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class AdvPositionInfo
    {
        
       string _AdvHtml ;
        
       string _AdvPositionName ;

        public string AdvHtml
        {
            
            get
            {
                return this._AdvHtml ;
            }
            
            set
            {
                this._AdvHtml  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValAdvPositionInfo", MessageTemplate="请输入广告位名称，长度限制在60字符以内"), HtmlCoding]
        public string AdvPositionName
        {
            
            get
            {
                return this._AdvPositionName ;
            }
            
            set
            {
                this._AdvPositionName  = value;
            }
        }
    }
}

