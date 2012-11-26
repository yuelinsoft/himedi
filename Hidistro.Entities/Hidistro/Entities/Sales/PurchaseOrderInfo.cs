namespace Hidistro.Entities.Sales
{
    using Hidistro.Core;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PurchaseOrderInfo
    {

        string _Address;

        decimal _AdjustedDiscount;

        decimal _AdjustedFreight;

        string _CellPhone;

        string _CloseReason;

        string _DistributorEmail;

        int _DistributorId;

        string _DistributorMSN;

        string _Distributorname;

        string _DistributorQQ;

        string _DistributorRealName;

        string _DistributorWangwang;

        string _ExpressCompanyAbb;

        string _ExpressCompanyName;

        DateTime _FinishDate;

        decimal _Freight;

        OrderMark? _ManagerMark;

        string _ManagerRemark;

        string _ModeName;

        string _OrderId;

        decimal _OrderTotal;

        DateTime _PayDate;

        DateTime _PurchaseDate;

        string _PurchaseOrderId;

        OrderStatus _PurchaseStatus;

        string _RealModeName;

        int _RealShippingModeId;

        decimal _RefundAmount;

        string _RefundRemark;

        Hidistro.Entities.Sales.RefundStatus _RefundStatus;

        int _RegionId;

        string _Remark;

        string _ShipOrderNumber;

        DateTime _ShippingDate;

        int _ShippingModeId;

        string _ShippingRegion;

        string _ShipTo;

        string _TaobaoOrderId;

        string _TelPhone;

        int _Weight;

        string _ZipCode;
        IList<PurchaseOrderGiftInfo> purchaseOrderGifts;
        IList<PurchaseOrderItemInfo> purchaseOrderItems;
        IList<PurchaseOrderOptionInfo> purchaseOrderOptions;

        public bool CheckAction(PurchaseOrderActions action)
        {
            if ((PurchaseStatus != OrderStatus.Finished) && (PurchaseStatus != OrderStatus.Closed))
            {
                switch (action)
                {
                    case PurchaseOrderActions.DISTRIBUTOR_CLOSE:
                    case PurchaseOrderActions.DISTRIBUTOR_MODIFY_GIFTS:
                    case PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY:
                    case PurchaseOrderActions.MASTER__CLOSE:
                    case PurchaseOrderActions.MASTER__MODIFY_AMOUNT:
                    case PurchaseOrderActions.MASTER_CONFIRM_PAY:
                        return (PurchaseStatus == OrderStatus.WaitBuyerPay);

                    case PurchaseOrderActions.DISTRIBUTOR_CONFIRM_GOODS:
                    case PurchaseOrderActions.MASTER_FINISH_TRADE:
                        return (PurchaseStatus == OrderStatus.SellerAlreadySent);

                    case PurchaseOrderActions.MASTER__MODIFY_SHIPPING_MODE:
                        return ((PurchaseStatus == OrderStatus.WaitBuyerPay) || (PurchaseStatus == OrderStatus.BuyerAlreadyPaid));

                    case PurchaseOrderActions.MASTER_MODIFY_DELIVER_ADDRESS:
                        return ((PurchaseStatus == OrderStatus.WaitBuyerPay) || (PurchaseStatus == OrderStatus.BuyerAlreadyPaid));

                    case PurchaseOrderActions.MASTER_SEND_GOODS:
                        return (PurchaseStatus == OrderStatus.BuyerAlreadyPaid);

                    case PurchaseOrderActions.MASTER_REJECT_REFUND:
                        return ((PurchaseStatus == OrderStatus.BuyerAlreadyPaid) || (PurchaseStatus == OrderStatus.SellerAlreadySent));
                }
            }
            return false;
        }

        public decimal GetGiftAmount()
        {
            decimal num = 0M;
            foreach (PurchaseOrderGiftInfo info in PurchaseOrderGifts)
            {
                num += info.GetSubTotal();
            }
            return num;
        }

        public decimal GetOptionPrice()
        {
            decimal totalOptionPrice = 0M;

            foreach (PurchaseOrderOptionInfo orderOptions in PurchaseOrderOptions)
            {
                totalOptionPrice += orderOptions.AdjustedPrice;
            }
            return totalOptionPrice;
        }

        public decimal GetProductAmount()
        {
            decimal orderItemsPrice = 0M;
            foreach (PurchaseOrderItemInfo item in PurchaseOrderItems)
            {
                orderItemsPrice += item.GetSubTotal();
            }
            return orderItemsPrice;
        }

        public decimal GetPurchaseCostPrice()
        {
            decimal num = 0M;
            foreach (PurchaseOrderItemInfo info in PurchaseOrderItems)
            {
                num += info.ItemCostPrice * info.Quantity;
            }
            foreach (PurchaseOrderGiftInfo info2 in PurchaseOrderGifts)
            {
                num += info2.CostPrice * info2.Quantity;
            }
            return num;
        }

        public decimal GetPurchaseProfit()
        {
            return GetPurchaseTotal() - RefundAmount - GetPurchaseCostPrice();
        }

        public decimal GetPurchaseTotal()
        {
            return GetOptionPrice() + GetProductAmount() + GetGiftAmount() + AdjustedFreight + AdjustedDiscount;
        }

        public string Address
        {

            get
            {
                return _Address;
            }

            set
            {
                _Address = value;
            }
        }

        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValPurchaseOrder", MessageTemplate = "采购单折扣不能为空，金额大小负1000万-1000万之间")]
        public decimal AdjustedDiscount
        {

            get
            {
                return _AdjustedDiscount;
            }

            set
            {
                _AdjustedDiscount = value;
            }
        }

        public decimal AdjustedFreight
        {

            get
            {
                return _AdjustedFreight;
            }

            set
            {
                _AdjustedFreight = value;
            }
        }

        public string CellPhone
        {

            get
            {
                return _CellPhone;
            }

            set
            {
                _CellPhone = value;
            }
        }

        public string CloseReason
        {

            get
            {
                return _CloseReason;
            }

            set
            {
                _CloseReason = value;
            }
        }

        public string DistributorEmail
        {

            get
            {
                return _DistributorEmail;
            }

            set
            {
                _DistributorEmail = value;
            }
        }

        public int DistributorId
        {

            get
            {
                return _DistributorId;
            }

            set
            {
                _DistributorId = value;
            }
        }

        public string DistributorMSN
        {

            get
            {
                return _DistributorMSN;
            }

            set
            {
                _DistributorMSN = value;
            }
        }

        public string Distributorname
        {

            get
            {
                return _Distributorname;
            }

            set
            {
                _Distributorname = value;
            }
        }

        public string DistributorQQ
        {

            get
            {
                return _DistributorQQ;
            }

            set
            {
                _DistributorQQ = value;
            }
        }

        public string DistributorRealName
        {

            get
            {
                return _DistributorRealName;
            }

            set
            {
                _DistributorRealName = value;
            }
        }

        public string DistributorWangwang
        {

            get
            {
                return _DistributorWangwang;
            }

            set
            {
                _DistributorWangwang = value;
            }
        }

        public string ExpressCompanyAbb
        {

            get
            {
                return _ExpressCompanyAbb;
            }

            set
            {
                _ExpressCompanyAbb = value;
            }
        }

        public string ExpressCompanyName
        {

            get
            {
                return _ExpressCompanyName;
            }

            set
            {
                _ExpressCompanyName = value;
            }
        }

        public DateTime FinishDate
        {

            get
            {
                return _FinishDate;
            }

            set
            {
                _FinishDate = value;
            }
        }

        public decimal Freight
        {

            get
            {
                return _Freight;
            }

            set
            {
                _Freight = value;
            }
        }

        public bool IsManualPurchaseOrder
        {
            get
            {
                return string.IsNullOrEmpty(OrderId);
            }
        }

        public OrderMark? ManagerMark
        {

            get
            {
                return _ManagerMark;
            }

            set
            {
                _ManagerMark = value;
            }
        }

        public string ManagerRemark
        {

            get
            {
                return _ManagerRemark;
            }

            set
            {
                _ManagerRemark = value;
            }
        }

        public string ModeName
        {

            get
            {
                return _ModeName;
            }

            set
            {
                _ModeName = value;
            }
        }

        public string OrderId
        {

            get
            {
                return _OrderId;
            }

            set
            {
                _OrderId = value;
            }
        }

        public decimal OrderTotal
        {

            get
            {
                return _OrderTotal;
            }

            set
            {
                _OrderTotal = value;
            }
        }

        public DateTime PayDate
        {

            get
            {
                return _PayDate;
            }

            set
            {
                _PayDate = value;
            }
        }

        public DateTime PurchaseDate
        {

            get
            {
                return _PurchaseDate;
            }

            set
            {
                _PurchaseDate = value;
            }
        }

        public IList<PurchaseOrderGiftInfo> PurchaseOrderGifts
        {
            get
            {
                if (purchaseOrderGifts == null)
                {
                    purchaseOrderGifts = new List<PurchaseOrderGiftInfo>();
                }
                return purchaseOrderGifts;
            }
        }

        public string PurchaseOrderId
        {

            get
            {
                return _PurchaseOrderId;
            }

            set
            {
                _PurchaseOrderId = value;
            }
        }

        public IList<PurchaseOrderItemInfo> PurchaseOrderItems
        {
            get
            {
                if (purchaseOrderItems == null)
                {
                    purchaseOrderItems = new List<PurchaseOrderItemInfo>();
                }
                return purchaseOrderItems;
            }
        }

        public IList<PurchaseOrderOptionInfo> PurchaseOrderOptions
        {
            get
            {
                if (purchaseOrderOptions == null)
                {
                    purchaseOrderOptions = new List<PurchaseOrderOptionInfo>();
                }
                return purchaseOrderOptions;
            }
        }

        public OrderStatus PurchaseStatus
        {

            get
            {
                return _PurchaseStatus;
            }

            set
            {
                _PurchaseStatus = value;
            }
        }

        public string RealModeName
        {

            get
            {
                return _RealModeName;
            }

            set
            {
                _RealModeName = value;
            }
        }

        public int RealShippingModeId
        {

            get
            {
                return _RealShippingModeId;
            }

            set
            {
                _RealShippingModeId = value;
            }
        }

        public decimal RefundAmount
        {

            get
            {
                return _RefundAmount;
            }

            set
            {
                _RefundAmount = value;
            }
        }

        public string RefundRemark
        {

            get
            {
                return _RefundRemark;
            }

            set
            {
                _RefundRemark = value;
            }
        }

        public Hidistro.Entities.Sales.RefundStatus RefundStatus
        {

            get
            {
                return _RefundStatus;
            }

            set
            {
                _RefundStatus = value;
            }
        }

        public int RegionId
        {

            get
            {
                return _RegionId;
            }

            set
            {
                _RegionId = value;
            }
        }

        public string Remark
        {

            get
            {
                return _Remark;
            }

            set
            {
                _Remark = value;
            }
        }

        [HtmlCoding]
        public string ShipOrderNumber
        {

            get
            {
                return _ShipOrderNumber;
            }

            set
            {
                _ShipOrderNumber = value;
            }
        }

        public DateTime ShippingDate
        {

            get
            {
                return _ShippingDate;
            }

            set
            {
                _ShippingDate = value;
            }
        }

        public int ShippingModeId
        {

            get
            {
                return _ShippingModeId;
            }

            set
            {
                _ShippingModeId = value;
            }
        }

        public string ShippingRegion
        {

            get
            {
                return _ShippingRegion;
            }

            set
            {
                _ShippingRegion = value;
            }
        }

        public string ShipTo
        {

            get
            {
                return _ShipTo;
            }

            set
            {
                _ShipTo = value;
            }
        }

        public string TaobaoOrderId
        {

            get
            {
                return _TaobaoOrderId;
            }

            set
            {
                _TaobaoOrderId = value;
            }
        }

        public string TelPhone
        {

            get
            {
                return _TelPhone;
            }

            set
            {
                _TelPhone = value;
            }
        }

        public int Weight
        {

            get
            {
                return _Weight;
            }

            set
            {
                _Weight = value;
            }
        }

        public string ZipCode
        {

            get
            {
                return _ZipCode;
            }

            set
            {
                _ZipCode = value;
            }
        }
    }
}

