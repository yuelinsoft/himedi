namespace Hidistro.Subsites.Promotions
{
    using Hidistro.Entities.Promotions;
    using System;

    internal class FullFreeFactory : PromotionFactory
    {
       static readonly FullFreeFactory _defaultInstance = new FullFreeFactory();
       SubsitePromotionsProvider provider;

        static FullFreeFactory()
        {
            _defaultInstance.provider = SubsitePromotionsProvider.Instance();
        }

       FullFreeFactory()
        {
        }

        public override PromotionActionStatus Create(PromotionInfo promote)
        {
            return this.provider.CreateFullFree(promote as FullFreeInfo);
        }

        public static FullFreeFactory Instance()
        {
            return _defaultInstance;
        }
    }
}

