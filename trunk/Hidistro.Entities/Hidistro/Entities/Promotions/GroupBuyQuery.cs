namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class GroupBuyQuery : Pagination
    {
        
       string _ProductName ;

        public string ProductName
        {
            
            get
            {
                return this._ProductName ;
            }
            
            set
            {
                this._ProductName  = value;
            }
        }
    }
}

