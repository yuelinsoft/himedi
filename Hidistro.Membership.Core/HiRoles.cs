using System;
using System.Security.Principal;
using System.Web.Security;

namespace Hidistro.Membership.Core
{
    public static class HiRoles
    {
        public static string[] GetRolesFromPrinciple(IPrincipal user)
        {

            RolePrincipal principal = user as RolePrincipal;

            string[] roles = null;

            if (principal != null)
            {
                roles =  principal.GetRoles();
            }

            return roles;

        }
    }
}

