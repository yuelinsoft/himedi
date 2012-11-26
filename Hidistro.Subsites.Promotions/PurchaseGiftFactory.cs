namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Entities.Promotions;
    using System;

    internal class PurchaseGiftFactory : PromotionFactory
    {
       static readonly PurchaseGiftFactory _defaultInstance = new PurchaseGiftFactory();
       SubsitePromotionsProvider provider;

        static PurchaseGiftFactory()
        {
            _defaultInstance.provider = SubsitePromotionsProvider.Instance();
        }

       PurchaseGiftFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return this.provider.CreatePurchaseGift(promote as PurchaseGiftInfo);
        }

        public static PurchaseGiftFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

