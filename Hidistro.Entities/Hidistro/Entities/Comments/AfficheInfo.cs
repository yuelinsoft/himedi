namespace Hidistro.Entities.Comments
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class AfficheInfo
    {
        
       DateTime _AddedDate ;
        
       int _AfficheId ;
        
       string _Content ;
        
       string _Title ;

        public DateTime AddedDate
        {
            
            get
            {
                return this._AddedDate ;
            }
            
            set
            {
                this._AddedDate  = value;
            }
        }

        public int AfficheId
        {
            
            get
            {
                return this._AfficheId ;
            }
            
            set
            {
                this._AfficheId  = value;
            }
        }

        [StringLengthValidator(1, 0x3b9ac9ff, Ruleset="ValAfficheInfo", MessageTemplate="公告内容不能为空")]
        public string Content
        {
            
            get
            {
                return this._Content ;
            }
            
            set
            {
                this._Content  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValAfficheInfo", MessageTemplate="公告标题不能为空，长度限制在60个字符以内"), HtmlCoding]
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
    }
}

