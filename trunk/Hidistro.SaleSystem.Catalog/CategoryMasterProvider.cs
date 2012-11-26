
using System;
using Hidistro.Core;
namespace Hidistro.SaleSystem.Catalog
{
    public abstract class CategoryMasterProvider : CategoryProvider
    {
       static readonly CategoryMasterProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.CategoryData,Hidistro.SaleSystem.Data") as CategoryMasterProvider);

        protected CategoryMasterProvider()
        {
        }

        public static CategoryMasterProvider CreateInstance()
        {
            return _defaultInstance;
        }
    }
}

