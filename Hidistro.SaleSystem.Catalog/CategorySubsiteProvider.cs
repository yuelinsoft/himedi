using System;
using Hidistro.Core;

namespace Hidistro.SaleSystem.Catalog
{
    public abstract class CategorySubsiteProvider : CategoryProvider
    {
       static readonly CategorySubsiteProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.CategoryData,Hidistro.SaleSystem.DistributionData") as CategorySubsiteProvider);

        protected CategorySubsiteProvider()
        {
        }

        public static CategorySubsiteProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

