namespace Hidistro.Entities.Store
{
    using Hidistro.Entities.Commodities;
    using System;
    using System.Runtime.CompilerServices;

    public class ProductSubjectInfo
    {
        
       string _Categories ;
        
       string _CategoryName ;
        
       string _Keywords ;
        
       int _MaxNum ;
        
       decimal? _MaxPrice ;
        
       decimal? _MinPrice ;
        
       int _SubjectId ;
        
       string _SubjectName ;
        
       SubjectType _Type ;

        public string Categories
        {
            
            get
            {
                return this._Categories ;
            }
            
            set
            {
                this._Categories  = value;
            }
        }

        public string CategoryName
        {
            
            get
            {
                return this._CategoryName ;
            }
            
            set
            {
                this._CategoryName  = value;
            }
        }

        public string Keywords
        {
            
            get
            {
                return this._Keywords ;
            }
            
            set
            {
                this._Keywords  = value;
            }
        }

        public int MaxNum
        {
            
            get
            {
                return this._MaxNum ;
            }
            
            set
            {
                this._MaxNum  = value;
            }
        }

        public decimal? MaxPrice
        {
            
            get
            {
                return this._MaxPrice ;
            }
            
            set
            {
                this._MaxPrice  = value;
            }
        }

        public decimal? MinPrice
        {
            
            get
            {
                return this._MinPrice ;
            }
            
            set
            {
                this._MinPrice  = value;
            }
        }

        public int SubjectId
        {
            
            get
            {
                return this._SubjectId ;
            }
            
            set
            {
                this._SubjectId  = value;
            }
        }

        public string SubjectName
        {
            
            get
            {
                return this._SubjectName ;
            }
            
            set
            {
                this._SubjectName  = value;
            }
        }

        public SubjectType Type
        {
            
            get
            {
                return this._Type ;
            }
            
            set
            {
                this._Type  = value;
            }
        }
    }
}

