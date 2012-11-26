using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Core;

namespace Hidistro.UI.Common.Controls
{
    public abstract class ControlProvider
    {
       static readonly ControlProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.UI.Common.Data.SqlCommonDataProvider, Hidistro.UI.Common.Data") as ControlProvider);

        protected ControlProvider()
        {
        }

        public abstract DataTable GetBrandCategories();
        public abstract DataTable GetBrandCategoriesByTypeId(int typeId);
        public abstract void GetMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum);
        public abstract IList<ProductLineInfo> GetProductLineList();
        public abstract IList<ProductTypeInfo> GetProductTypes();
        public abstract IList<ShippingModeInfo> GetShippingModes();
        public abstract DataTable GetSkuContentBySku(string skuId);
        public static ControlProvider Instance()
        {
            return _defaultInstance;
        }
    }
}

