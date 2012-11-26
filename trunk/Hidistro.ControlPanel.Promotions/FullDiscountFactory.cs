using Hidistro.Entities.Promotions;
using System;

namespace Hidistro.ControlPanel.Promotions
{
    internal class FullDiscountFactory : PromotionFactory
    {
        static readonly FullDiscountFactory _defaultInstance = new FullDiscountFactory();
        PromotionsProvider provider;

        static FullDiscountFactory()
        {
            _defaultInstance.provider = PromotionsProvider.Instance();
        }

        FullDiscountFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return provider.CreateFullDiscount(promote as FullDiscountInfo);
        }

        public static FullDiscountFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

