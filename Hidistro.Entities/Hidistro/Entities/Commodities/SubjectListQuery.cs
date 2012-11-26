namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class SubjectListQuery : Pagination
    {
        
       string _CategoryIds ;
        
       string _Keywords ;
        
       int _MaxNum ;
        
       decimal? _MaxPrice ;
        
       decimal? _MinPrice ;
        
       SubjectType _ProductType ;

        public string CategoryIds
        {
            
            get
            {
                return this._CategoryIds ;
            }
            
            set
            {
                this._CategoryIds  = value;
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

        public SubjectType ProductType
        {
            
            get
            {
                return this._ProductType ;
            }
            
            set
            {
                this._ProductType  = value;
            }
        }
    }
}

