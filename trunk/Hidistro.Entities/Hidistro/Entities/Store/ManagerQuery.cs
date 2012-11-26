namespace Hidistro.Entities.Store
{
    using Hidistro.Core.Entities;
    using System;
    using System.Runtime.CompilerServices;

    public class ManagerQuery : Pagination
    {
        
       Guid _RoleId ;
        
       string _Username ;

        public Guid RoleId
        {
            
            get
            {
                return this._RoleId ;
            }
            
            set
            {
                this._RoleId  = value;
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

