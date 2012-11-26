namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Entities.Promotions;
    using System;

    internal abstract class PromotionFactory
    {
        protected PromotionFactory()
        {
        }

        public static PromotionFactory Create(PromoteType promoteType)
        {
            if (promoteType == PromoteType.FullDiscount)
            {
                return FullDiscountFactory.Instance();
            }
            if (promoteType == PromoteType.FullFree)
            {
                return FullFreeFactory.Instance();
            }
            if (promoteType == PromoteType.PurchaseGift)
            {
                return PurchaseGiftFactory.Instance();
            }
            if (promoteType == PromoteType.WholesaleDiscount)
            {
                return WholesaleDiscountFactory.Instance();
            }
            return null;
        }

        public abstract PromotionActionStatus Create(PromotionInfo promote);
    }
}

