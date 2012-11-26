using Hidistro.Entities.Store;
using System;

namespace Hidistro.ControlPanel.Store
{
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class PrivilegeCheckAttribute : Attribute
    {
        Hidistro.Entities.Store.Privilege privilege;

        public PrivilegeCheckAttribute(Hidistro.Entities.Store.Privilege privilege)
        {
            this.privilege = privilege;
        }

        public Hidistro.Entities.Store.Privilege Privilege
        {
            get
            {
                return privilege;
            }
        }
    }
}

