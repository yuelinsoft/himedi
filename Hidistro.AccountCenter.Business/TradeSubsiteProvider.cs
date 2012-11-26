using System;
using Hidistro.Core;

namespace Hidistro.AccountCenter.Business
{
    public abstract class TradeSubsiteProvider : TradeProvider
    {
        static readonly TradeSubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.BusinessData,Hidistro.AccountCenter.DistributionData") as TradeSubsiteProvider);

        protected TradeSubsiteProvider()
        {
        }

        public static TradeSubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

