using System;

namespace Hidistro.ControlPanel.Store
{
    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AdministerCheckAttribute : Attribute
    {
       bool administratorOnly;

        public AdministerCheckAttribute()
        {
        }

        public AdministerCheckAttribute(bool administratorOnly)
        {
            this.administratorOnly = administratorOnly;
        }

        public bool AdministratorOnly
        {
            get
            {
                return administratorOnly;
            }
        }
    }
}

