namespace Hidistro.Entities.Promotions
{
    using System;

    public enum PromotionActionStatus
    {
        DuplicateName = 1,
        SameCondition = 2,
        Success = 0,
        UnknowError = 0x63
    }
}

