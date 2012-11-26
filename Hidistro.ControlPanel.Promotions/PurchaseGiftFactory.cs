using Hidistro.Entities.Promotions;
using System;

namespace Hidistro.ControlPanel.Promotions
{
    internal class PurchaseGiftFactory : PromotionFactory
    {
        static readonly PurchaseGiftFactory _defaultInstance = new PurchaseGiftFactory();
        PromotionsProvider provider;

        static PurchaseGiftFactory()
        {
            _defaultInstance.provider = PromotionsProvider.Instance();
        }

        PurchaseGiftFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return provider.CreatePurchaseGift(promote as PurchaseGiftInfo);
        }

        public static PurchaseGiftFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

