using System;
using Hidistro.Core;

namespace Hidistro.AccountCenter.Business
{
    public abstract class TradeMasterProvider : TradeProvider
    {
        static readonly TradeMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.BusinessData,Hidistro.AccountCenter.Data") as TradeMasterProvider);

        protected TradeMasterProvider()
        {
        }

        public static TradeMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

