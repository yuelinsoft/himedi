namespace Hidistro.Entities.Sales
{
    using Hidistro.Entities.Promotions;
    using Hidistro.Membership.Context;
    using Hidistro.Membership.Core.Enums;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class OrderInfo
    {

        int _ActivityId;

        string _ActivityName;

        string _Address;

        decimal _AdjustedDiscount;

        string _CellPhone;

        string _CloseReason;

        decimal _CouponAmount;

        string _CouponCode;

        string _CouponName;

        decimal _CouponValue;

        decimal _DiscountAmount;

        int _DiscountId;

        string _DiscountName;

        decimal _DiscountValue;

        Hidistro.Entities.Promotions.DiscountValueType _DiscountValueType;

        bool _EightFree;

        string _EmailAddress;

        string _ExpressCompanyAbb;

        string _ExpressCompanyName;

        DateTime _FinishDate;

        decimal _Freight;

        string _GatewayOrderId;

        int _GroupBuyId;

        Hidistro.Entities.Promotions.GroupBuyStatus _GroupBuyStatus;

        OrderMark? _ManagerMark;

        string _ManagerRemark;

        string _ModeName;

        string _MSN;

        decimal _NeedPrice;

        DateTime _OrderDate;

        string _OrderId;

        bool _OrderOptionFree;

        Hidistro.Entities.Sales.OrderStatus _OrderStatus;

        decimal _PayCharge;

        DateTime _PayDate;

        string _PaymentType;

        int _PaymentTypeId;

        int _Points;

        bool _ProcedureFeeFree;

        string _QQ;

        string _RealModeName;

        string _RealName;

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

        string _TelPhone;

        int _UserId;

        string _Username;

        string _Wangwang;

        string _ZipCode;
        decimal adjustedFreigh;
        decimal adjustedPayCharge;
        IList<OrderGiftInfo> gifts;
        Dictionary<string, LineItemInfo> lineItems;
        IList<OrderOptionInfo> orderOptions;

        public static event EventHandler<EventArgs> Closed;

        public static event EventHandler<EventArgs> Created;

        public static event EventHandler<EventArgs> Deliver;

        public static event EventHandler<EventArgs> Payment;

        public static event EventHandler<EventArgs> Refund;

        public OrderInfo()
        {
            this.OrderStatus = Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay;
            this.RefundStatus = Hidistro.Entities.Sales.RefundStatus.None;
        }

        public bool CheckAction(OrderActions action)
        {
            if ((this.OrderStatus != Hidistro.Entities.Sales.OrderStatus.Finished) && (this.OrderStatus != Hidistro.Entities.Sales.OrderStatus.Closed))
            {
                switch (action)
                {
                    case OrderActions.BUYER_PAY:
                    case OrderActions.SUBSITE_SELLER_MODIFY_DELIVER_ADDRESS:
                    case OrderActions.SUBSITE_SELLER_MODIFY_PAYMENT_MODE:
                    case OrderActions.SUBSITE_SELLER_MODIFY_SHIPPING_MODE:
                    case OrderActions.SELLER_CONFIRM_PAY:
                    case OrderActions.SELLER_MODIFY_TRADE:
                    case OrderActions.SELLER_CLOSE:
                    case OrderActions.SUBSITE_SELLER_MODIFY_GIFTS:
                        return (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay);

                    case OrderActions.BUYER_CONFIRM_GOODS:
                    case OrderActions.SELLER_FINISH_TRADE:
                        return (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.SellerAlreadySent);

                    case OrderActions.SELLER_SEND_GOODS:
                        return (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid);

                    case OrderActions.SELLER_REJECT_REFUND:
                        return ((this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid) || (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.SellerAlreadySent));

                    case OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS:
                    case OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE:
                    case OrderActions.MASTER_SELLER_MODIFY_GIFTS:
                        return ((this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.WaitBuyerPay) || (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid));

                    case OrderActions.SUBSITE_CREATE_PURCHASEORDER:
                        return (((this.GroupBuyId > 0) && (this.GroupBuyStatus == Hidistro.Entities.Promotions.GroupBuyStatus.Success)) && (this.OrderStatus == Hidistro.Entities.Sales.OrderStatus.BuyerAlreadyPaid));
                }
            }
            return false;
        }

        public decimal GetAmount()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                num += info.GetSubTotal();
            }
            return num;
        }

        public virtual decimal GetCostPrice()
        {
            decimal num = 0M;
            foreach (LineItemInfo info in this.LineItems.Values)
            {
                num += info.ItemCostPrice * info.ShipmentQuantity;
            }
            foreach (OrderGiftInfo info2 in this.Gifts)
            {
                num += info2.CostPrice * info2.Quantity;
            }
            if (HiContext.Current.SiteSettings.IsDistributorSettings || (HiContext.Current.User.UserRole == UserRole.Distributor))
            {
                num += this.GetOptionPrice();
                num += this.Freight;
            }
            return num;
        }

        public decimal GetDiscountAmount()
        {

            if (this.DiscountAmount > 0M)
            {
                return DiscountAmount;
            }

            if ((!string.IsNullOrEmpty(this.DiscountName) && (this.DiscountValue > 0M)) && Enum.IsDefined(typeof(Hidistro.Entities.Promotions.DiscountValueType), this.DiscountValueType))
            {

                if (this.DiscountValueType == Hidistro.Entities.Promotions.DiscountValueType.Amount)
                {
                    return this.DiscountValue;
                }

                decimal amount = GetAmount();

                //return (amount - (amount * (this.DiscountValue / 100M)));
                return amount - amount * this.DiscountValue / 100M;

            }
            return 0M;
        }

        public decimal GetDiscountedAmount()
        {
            return this.GetAmount() - GetDiscountAmount();
        }

        public int GetGroupBuyOerderNumber()
        {
            if (this.GroupBuyId > 0)
            {
                foreach (LineItemInfo info in this.LineItems.Values)
                {
                    return info.Quantity;
                }
            }
            return 0;
        }

        public decimal GetOptionPrice()
        {
            if (this.OrderOptionFree)
            {
                return 0M;
            }
            decimal num = 0M;
            foreach (OrderOptionInfo info in this.OrderOptions)
            {
                num += info.AdjustedPrice;
            }
            return num;
        }

        public virtual decimal GetProfit()
        {
            return ((this.GetTotal() - this.RefundAmount) - this.GetCostPrice());
        }

        public decimal GetTotal()
        {
            decimal num = this.GetDiscountedAmount() + this.AdjustedFreight;
            num += this.AdjustedPayCharge;
            num += this.GetOptionPrice();
            if (!string.IsNullOrEmpty(this.CouponCode))
            {
                num -= this.CouponValue;
            }
            return (num + this.AdjustedDiscount);
        }

        public int GetTotalPoints()
        {
            decimal total = this.GetTotal();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if ((total / masterSettings.PointsRate) > 2147483647M)
            {
                return 0x7fffffff;
            }
            return Convert.ToInt32((decimal)(total / masterSettings.PointsRate));
        }

        public void OnClosed()
        {
            if (Closed != null)
            {
                Closed(this, new EventArgs());
            }
        }

        public static void OnClosed(OrderInfo order)
        {
            if (Closed != null)
            {
                Closed(order, new EventArgs());
            }
        }

        public void OnCreated()
        {
            if (Created != null)
            {
                Created(this, new EventArgs());
            }
        }

        public static void OnCreated(OrderInfo order)
        {
            if (Created != null)
            {
                Created(order, new EventArgs());
            }
        }

        public void OnDeliver()
        {
            if (Deliver != null)
            {
                Deliver(this, new EventArgs());
            }
        }

        public static void OnDeliver(OrderInfo order)
        {
            if (Deliver != null)
            {
                Deliver(order, new EventArgs());
            }
        }

        public void OnPayment()
        {
            if (Payment != null)
            {
                Payment(this, new EventArgs());
            }
        }

        public static void OnPayment(OrderInfo order)
        {
            if (Payment != null)
            {
                Payment(order, new EventArgs());
            }
        }

        public void OnRefund()
        {
            if (Refund != null)
            {
                Refund(this, new EventArgs());
            }
        }

        public static void OnRefund(OrderInfo order)
        {
            if (Refund != null)
            {
                Refund(order, new EventArgs());
            }
        }

        public int ActivityId
        {

            get
            {
                return this._ActivityId;
            }

            set
            {
                this._ActivityId = value;
            }
        }

        public string ActivityName
        {

            get
            {
                return this._ActivityName;
            }

            set
            {
                this._ActivityName = value;
            }
        }

        public string Address
        {

            get
            {
                return this._Address;
            }

            set
            {
                this._Address = value;
            }
        }

        [RangeValidator(typeof(decimal), "-10000000", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "订单折扣不能为空，金额大小负1000万-1000万之间")]
        public decimal AdjustedDiscount
        {

            get
            {
                return this._AdjustedDiscount;
            }

            set
            {
                this._AdjustedDiscount = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "运费不能为空，金额大小0-1000万之间")]
        public decimal AdjustedFreight
        {
            get
            {
                return (this.EightFree ? 0M : this.adjustedFreigh);
            }
            set
            {
                this.adjustedFreigh = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValOrder", MessageTemplate = "支付手续费不能为空，金额大小0-1000万之间")]
        public decimal AdjustedPayCharge
        {
            get
            {
                return (this.ProcedureFeeFree ? 0M : this.adjustedPayCharge);
            }
            set
            {
                this.adjustedPayCharge = value;
            }
        }

        public string CellPhone
        {

            get
            {
                return this._CellPhone;
            }

            set
            {
                this._CellPhone = value;
            }
        }

        public string CloseReason
        {

            get
            {
                return this._CloseReason;
            }

            set
            {
                this._CloseReason = value;
            }
        }

        public decimal CouponAmount
        {

            get
            {
                return this._CouponAmount;
            }

            set
            {
                this._CouponAmount = value;
            }
        }

        public string CouponCode
        {

            get
            {
                return this._CouponCode;
            }

            set
            {
                this._CouponCode = value;
            }
        }

        public string CouponName
        {

            get
            {
                return this._CouponName;
            }

            set
            {
                this._CouponName = value;
            }
        }

        public decimal CouponValue
        {

            get
            {
                return this._CouponValue;
            }

            set
            {
                this._CouponValue = value;
            }
        }

        public decimal DiscountAmount
        {

            get
            {
                return this._DiscountAmount;
            }

            set
            {
                this._DiscountAmount = value;
            }
        }

        public int DiscountId
        {

            get
            {
                return this._DiscountId;
            }

            set
            {
                this._DiscountId = value;
            }
        }

        public string DiscountName
        {

            get
            {
                return this._DiscountName;
            }

            set
            {
                this._DiscountName = value;
            }
        }

        public decimal DiscountValue
        {

            get
            {
                return this._DiscountValue;
            }

            set
            {
                this._DiscountValue = value;
            }
        }

        public Hidistro.Entities.Promotions.DiscountValueType DiscountValueType
        {

            get
            {
                return this._DiscountValueType;
            }

            set
            {
                this._DiscountValueType = value;
            }
        }

        public bool EightFree
        {

            get
            {
                return this._EightFree;
            }

            set
            {
                this._EightFree = value;
            }
        }

        public string EmailAddress
        {

            get
            {
                return this._EmailAddress;
            }

            set
            {
                this._EmailAddress = value;
            }
        }

        public string ExpressCompanyAbb
        {

            get
            {
                return this._ExpressCompanyAbb;
            }

            set
            {
                this._ExpressCompanyAbb = value;
            }
        }

        public string ExpressCompanyName
        {

            get
            {
                return this._ExpressCompanyName;
            }

            set
            {
                this._ExpressCompanyName = value;
            }
        }

        public DateTime FinishDate
        {

            get
            {
                return this._FinishDate;
            }

            set
            {
                this._FinishDate = value;
            }
        }

        public decimal Freight
        {

            get
            {
                return this._Freight;
            }

            set
            {
                this._Freight = value;
            }
        }

        public string GatewayOrderId
        {

            get
            {
                return this._GatewayOrderId;
            }

            set
            {
                this._GatewayOrderId = value;
            }
        }

        public IList<OrderGiftInfo> Gifts
        {
            get
            {
                if (this.gifts == null)
                {
                    this.gifts = new List<OrderGiftInfo>();
                }
                return this.gifts;
            }
        }

        public int GroupBuyId
        {

            get
            {
                return this._GroupBuyId;
            }

            set
            {
                this._GroupBuyId = value;
            }
        }

        public Hidistro.Entities.Promotions.GroupBuyStatus GroupBuyStatus
        {

            get
            {
                return this._GroupBuyStatus;
            }

            set
            {
                this._GroupBuyStatus = value;
            }
        }

        public Dictionary<string, LineItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new Dictionary<string, LineItemInfo>();
                }
                return this.lineItems;
            }
        }

        public OrderMark? ManagerMark
        {

            get
            {
                return this._ManagerMark;
            }

            set
            {
                this._ManagerMark = value;
            }
        }

        public string ManagerRemark
        {

            get
            {
                return this._ManagerRemark;
            }

            set
            {
                this._ManagerRemark = value;
            }
        }

        public string ModeName
        {

            get
            {
                return this._ModeName;
            }

            set
            {
                this._ModeName = value;
            }
        }

        public string MSN
        {

            get
            {
                return this._MSN;
            }

            set
            {
                this._MSN = value;
            }
        }

        public decimal NeedPrice
        {

            get
            {
                return this._NeedPrice;
            }

            set
            {
                this._NeedPrice = value;
            }
        }

        public DateTime OrderDate
        {

            get
            {
                return this._OrderDate;
            }

            set
            {
                this._OrderDate = value;
            }
        }

        public string OrderId
        {

            get
            {
                return this._OrderId;
            }

            set
            {
                this._OrderId = value;
            }
        }

        public bool OrderOptionFree
        {

            get
            {
                return this._OrderOptionFree;
            }

            set
            {
                this._OrderOptionFree = value;
            }
        }

        public IList<OrderOptionInfo> OrderOptions
        {
            get
            {
                if (this.orderOptions == null)
                {
                    this.orderOptions = new List<OrderOptionInfo>();
                }
                return this.orderOptions;
            }
        }

        public Hidistro.Entities.Sales.OrderStatus OrderStatus
        {

            get
            {
                return this._OrderStatus;
            }

            set
            {
                this._OrderStatus = value;
            }
        }

        public decimal PayCharge
        {

            get
            {
                return this._PayCharge;
            }

            set
            {
                this._PayCharge = value;
            }
        }

        public DateTime PayDate
        {

            get
            {
                return this._PayDate;
            }

            set
            {
                this._PayDate = value;
            }
        }

        public string PaymentType
        {

            get
            {
                return this._PaymentType;
            }

            set
            {
                this._PaymentType = value;
            }
        }

        public int PaymentTypeId
        {

            get
            {
                return this._PaymentTypeId;
            }

            set
            {
                this._PaymentTypeId = value;
            }
        }

        public int Points
        {

            get
            {
                return this._Points;
            }

            set
            {
                this._Points = value;
            }
        }

        public bool ProcedureFeeFree
        {

            get
            {
                return this._ProcedureFeeFree;
            }

            set
            {
                this._ProcedureFeeFree = value;
            }
        }

        public string QQ
        {

            get
            {
                return this._QQ;
            }

            set
            {
                this._QQ = value;
            }
        }

        public string RealModeName
        {

            get
            {
                return this._RealModeName;
            }

            set
            {
                this._RealModeName = value;
            }
        }

        public string RealName
        {

            get
            {
                return this._RealName;
            }

            set
            {
                this._RealName = value;
            }
        }

        public int RealShippingModeId
        {

            get
            {
                return this._RealShippingModeId;
            }

            set
            {
                this._RealShippingModeId = value;
            }
        }

        public decimal RefundAmount
        {

            get
            {
                return this._RefundAmount;
            }

            set
            {
                this._RefundAmount = value;
            }
        }

        public string RefundRemark
        {

            get
            {
                return this._RefundRemark;
            }

            set
            {
                this._RefundRemark = value;
            }
        }

        public Hidistro.Entities.Sales.RefundStatus RefundStatus
        {

            get
            {
                return this._RefundStatus;
            }

            set
            {
                this._RefundStatus = value;
            }
        }

        public int RegionId
        {

            get
            {
                return this._RegionId;
            }

            set
            {
                this._RegionId = value;
            }
        }

        public string Remark
        {

            get
            {
                return this._Remark;
            }

            set
            {
                this._Remark = value;
            }
        }

        public string ShipOrderNumber
        {

            get
            {
                return this._ShipOrderNumber;
            }

            set
            {
                this._ShipOrderNumber = value;
            }
        }

        public DateTime ShippingDate
        {

            get
            {
                return this._ShippingDate;
            }

            set
            {
                this._ShippingDate = value;
            }
        }

        public int ShippingModeId
        {

            get
            {
                return this._ShippingModeId;
            }

            set
            {
                this._ShippingModeId = value;
            }
        }

        public string ShippingRegion
        {

            get
            {
                return this._ShippingRegion;
            }

            set
            {
                this._ShippingRegion = value;
            }
        }

        public string ShipTo
        {

            get
            {
                return this._ShipTo;
            }

            set
            {
                this._ShipTo = value;
            }
        }

        public string TelPhone
        {

            get
            {
                return this._TelPhone;
            }

            set
            {
                this._TelPhone = value;
            }
        }

        public int UserId
        {

            get
            {
                return this._UserId;
            }

            set
            {
                this._UserId = value;
            }
        }

        public string Username
        {

            get
            {
                return this._Username;
            }

            set
            {
                this._Username = value;
            }
        }

        public string Wangwang
        {

            get
            {
                return this._Wangwang;
            }

            set
            {
                this._Wangwang = value;
            }
        }

        public int Weight
        {
            get
            {
                int num = 0;
                foreach (LineItemInfo info in this.LineItems.Values)
                {
                    num += info.ItemWeight * info.ShipmentQuantity;
                }
                return num;
            }
        }

        public string ZipCode
        {

            get
            {
                return this._ZipCode;
            }

            set
            {
                this._ZipCode = value;
            }
        }
    }
}

