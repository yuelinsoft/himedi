using Hishop.Components.Validation.Validators;
using System;
using System.Globalization;

namespace Hidistro.Membership.Core
{
    public class RoleInfo : IComparable
    {
        string description;
        string name;
        Guid roleID;

        public RoleInfo()
        {
            roleID = Guid.Empty;
        }

        public RoleInfo(Guid roleID, string name)
        {
            this.roleID = Guid.Empty;
            this.roleID = roleID;
            this.name = name;
        }

        public int CompareTo(object obj)
        {
            RoleInfo info = obj as RoleInfo;
            if (info != null)
            {
                if (RoleID == info.RoleID)
                {
                    return 0;
                }
                return string.Compare(Name, info.Name, true, CultureInfo.InvariantCulture);
            }
            return -1;
        }

        public override bool Equals(object obj)
        {
            RoleInfo info = obj as RoleInfo;
            return (((info != null) && (info.RoleID == RoleID)) && (info.Name == Name));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        [StringLengthValidator(0, 100, Ruleset = "ValRoleInfo", MessageTemplate = "职能说明的长度限制在100个字符以内")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public bool IsRoleIDAssigned
        {
            get
            {
                return (RoleID != Guid.Empty);
            }
        }

        [StringLengthValidator(1, 60, Ruleset = "ValRoleInfo", MessageTemplate = "部门名称不能为空，长度限制在60个字符以内")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public Guid RoleID
        {
            get
            {
                return roleID;
            }
            set
            {
                roleID = value;
            }
        }
    }
}

