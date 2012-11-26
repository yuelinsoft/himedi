using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Sales
{
    public class AdminStatisticsInfo : StatisticsInfo
    {

        int _DistributorApplyRequestWaitDispose;

        decimal _DistrosBalance;

        decimal _MembersBalance;

        int _ProductAlert;

        int _PurchaseOrderNumbToday;

        decimal _PurchaseOrderPriceToday;

        decimal _PurchaseOrderProfitToday;

        public decimal BalanceTotal
        {
            get
            {
                return MembersBalance + DistrosBalance;
            }
        }

        public int DistributorApplyRequestWaitDispose
        {

            get
            {
                return _DistributorApplyRequestWaitDispose;
            }

            set
            {
                _DistributorApplyRequestWaitDispose = value;
            }
        }

        public decimal DistrosBalance
        {

            get
            {
                return _DistrosBalance;
            }

            set
            {
                _DistrosBalance = value;
            }
        }

        public decimal MembersBalance
        {

            get
            {
                return _MembersBalance;
            }

            set
            {
                _MembersBalance = value;
            }
        }

        public int ProductAlert
        {

            get
            {
                return _ProductAlert;
            }

            set
            {
                _ProductAlert = value;
            }
        }

        public decimal ProfitTotal
        {
            get
            {
                return (PurchaseOrderProfitToday + base.OrderProfitToday);
            }
        }

        public int PurchaseOrderNumbToday
        {

            get
            {
                return _PurchaseOrderNumbToday;
            }

            set
            {
                _PurchaseOrderNumbToday = value;
            }
        }

        public decimal PurchaseOrderPriceToday
        {

            get
            {
                return _PurchaseOrderPriceToday;
            }

            set
            {
                _PurchaseOrderPriceToday = value;
            }
        }

        public decimal PurchaseOrderProfitToday
        {

            get
            {
                return _PurchaseOrderProfitToday;
            }

            set
            {
                _PurchaseOrderProfitToday = value;
            }
        }
    }
}

