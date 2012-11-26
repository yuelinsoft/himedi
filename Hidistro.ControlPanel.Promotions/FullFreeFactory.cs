using Hidistro.Entities.Promotions;
using System;

namespace Hidistro.ControlPanel.Promotions
{
    internal class FullFreeFactory : PromotionFactory
    {
        static readonly FullFreeFactory _defaultInstance = new FullFreeFactory();
        PromotionsProvider provider;

        static FullFreeFactory()
        {
            _defaultInstance.provider = PromotionsProvider.Instance();
        }

        FullFreeFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return provider.CreateFullFree(promote as FullFreeInfo);
        }

        public static FullFreeFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

