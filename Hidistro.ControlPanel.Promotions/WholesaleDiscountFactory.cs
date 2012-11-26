using Hidistro.Entities.Promotions;
using System;

namespace Hidistro.ControlPanel.Promotions
{
    internal class WholesaleDiscountFactory : PromotionFactory
    {
        static readonly WholesaleDiscountFactory _defaultInstance = new WholesaleDiscountFactory();
        PromotionsProvider provider;

        static WholesaleDiscountFactory()
        {
            _defaultInstance.provider = PromotionsProvider.Instance();
        }

        WholesaleDiscountFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return provider.CreateWholesaleDiscount(promote as WholesaleDiscountInfo);
        }

        public static WholesaleDiscountFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

