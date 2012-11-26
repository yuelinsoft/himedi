using System;
using Hidistro.Core;

namespace Hidistro.AccountCenter.Profile
{
    public abstract class PersonalMasterProvider : PersonalProvider
    {
        static readonly PersonalMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.ProfileData,Hidistro.AccountCenter.Data") as PersonalMasterProvider);

        protected PersonalMasterProvider()
        {
        }

        public static PersonalMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

