namespace Hidistro.Entities.Members
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class MemberQuery : Pagination
    {
        
       int? _GradeId ;
        
       bool? _IsApproved ;
        
       string _Realname ;
        
       string _Username ;

        public int? GradeId
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

        public bool? IsApproved
        {
            
            get
            {
                return this._IsApproved ;
            }
            
            set
            {
                this._IsApproved  = value;
            }
        }

        public string Realname
        {
            
            get
            {
                return this._Realname ;
            }
            
            set
            {
                this._Realname  = value;
            }
        }

        public string Username
        {
            
            get
            {
                return this._Username ;
            }
            
            set
            {
                this._Username  = value;
            }
        }
    }
}

