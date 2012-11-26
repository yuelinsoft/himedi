namespace Hidistro.Entities.Sales
{
    using System;

    public enum OrderStatus
    {
        All = 0,
        BuyerAlreadyPaid = 2,
        Closed = 4,
        Finished = 5,
        History = 0x63,
        SellerAlreadySent = 3,
        WaitBuyerPay = 1
    }
}

