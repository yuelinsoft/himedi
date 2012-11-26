using System;
using Hidistro.Core;

namespace Hidistro.AccountCenter.Profile
{
    public abstract class PersonalSubsiteProvider : PersonalProvider
    {
        static readonly PersonalSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.ProfileData,Hidistro.AccountCenter.DistributionData") as PersonalSubsiteProvider);

        protected PersonalSubsiteProvider()
        {
        }

        public static PersonalSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

