namespace Hidistro.Core.Entities
{
    using Hidistro.Core.Enums;
    using System;
    using System.Runtime.CompilerServices;

    public class Pagination
    {
        
       bool _IsCount;
        
       int _PageIndex;
        
       int _PageSize;
        
       string _SortBy;
        
       SortAction _SortOrder;

        public Pagination()
        {
            this.IsCount = true;
            this.PageSize = 10;
        }

        public bool IsCount
        {
            
            get
            {
                return this._IsCount;
            }
            
            set
            {
                this._IsCount = value;
            }
        }

        public int PageIndex
        {
            
            get
            {
                return this._PageIndex;
            }
            
            set
            {
                this._PageIndex = value;
            }
        }

        public int PageSize
        {
            
            get
            {
                return this._PageSize;
            }
            
            set
            {
                this._PageSize = value;
            }
        }

        public string SortBy
        {
            
            get
            {
                return this._SortBy;
            }
            
            set
            {
                this._SortBy = value;
            }
        }

        public SortAction SortOrder
        {
            
            get
            {
                return this._SortOrder;
            }
            
            set
            {
                this._SortOrder = value;
            }
        }
    }
}

