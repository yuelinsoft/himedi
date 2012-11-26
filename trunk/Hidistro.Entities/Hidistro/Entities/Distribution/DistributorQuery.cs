namespace Hidistro.Entities.Distribution
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class DistributorQuery : Pagination
    {
        
       int? _GradeId ;
        
       bool _IsApproved ;
        
       int? _LineId ;
        
       string _RealName ;
        
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

        public bool IsApproved
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

        public int? LineId
        {
            
            get
            {
                return this._LineId ;
            }
            
            set
            {
                this._LineId  = value;
            }
        }

        public string RealName
        {
            
            get
            {
                return this._RealName ;
            }
            
            set
            {
                this._RealName  = value;
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

