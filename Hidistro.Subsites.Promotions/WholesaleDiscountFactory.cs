namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Entities.Promotions;
    using System;

    internal class WholesaleDiscountFactory : PromotionFactory
    {
       static readonly WholesaleDiscountFactory _defaultInstance = new WholesaleDiscountFactory();
       SubsitePromotionsProvider provider;

        static WholesaleDiscountFactory()
        {
            _defaultInstance.provider = SubsitePromotionsProvider.Instance();
        }

       WholesaleDiscountFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return this.provider.CreateWholesaleDiscount(promote as WholesaleDiscountInfo);
        }

        public static WholesaleDiscountFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

