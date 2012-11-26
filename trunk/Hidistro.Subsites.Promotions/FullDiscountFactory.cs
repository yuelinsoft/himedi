namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Entities.Promotions;
    using System;

    internal class FullDiscountFactory : PromotionFactory
    {
       static readonly FullDiscountFactory _defaultInstance = new FullDiscountFactory();
       SubsitePromotionsProvider provider;

        static FullDiscountFactory()
        {
            _defaultInstance.provider = SubsitePromotionsProvider.Instance();
        }

       FullDiscountFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return this.provider.CreateFullDiscount(promote as FullDiscountInfo);
        }

        public static FullDiscountFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

