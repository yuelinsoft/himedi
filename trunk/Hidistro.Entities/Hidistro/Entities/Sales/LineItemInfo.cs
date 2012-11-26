using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Sales
{
    public class LineItemInfo
    {

        decimal _ItemAdjustedPrice;

        decimal _ItemCostPrice;

        string _ItemDescription;

        decimal _ItemListPrice;

        int _ItemWeight;

        int _ProductId;

        int _PurchaseGiftId;

        string _PurchaseGiftName;

        int _Quantity;

        int _ShipmentQuantity;

        string _SKU;

        string _SKUContent;

        string _SkuId;

        string _ThumbnailsUrl;

        int _WholesaleDiscountId;

        string _WholesaleDiscountName;

        public LineItemInfo(string skuId, int productId, string sku, int quantity, int shipmentQty, decimal costPrice, decimal itemListPrice, decimal itemAdjustedPrice, string itemDescription, string thumbnailsUrl, int weight, int purchaseGiftId, string purchaseGiftName, int wholesaleDiscountId, string wholesaleDiscountName, string skuContent)
        {
            SkuId = skuId;
            ProductId = productId;
            SKU = sku;
            Quantity = quantity;
            ShipmentQuantity = shipmentQty;
            ItemCostPrice = costPrice;
            ItemListPrice = itemListPrice;
            ItemAdjustedPrice = itemAdjustedPrice;
            ItemDescription = itemDescription;
            ThumbnailsUrl = thumbnailsUrl;
            ItemWeight = weight;
            PurchaseGiftId = purchaseGiftId;
            PurchaseGiftName = purchaseGiftName;
            WholesaleDiscountId = wholesaleDiscountId;
            WholesaleDiscountName = wholesaleDiscountName;
            SKUContent = skuContent;
        }

        public decimal GetSubTotal()
        {
            return (ItemAdjustedPrice * Quantity);
        }

        public decimal ItemAdjustedPrice
        {

            get
            {
                return _ItemAdjustedPrice;
            }

            set
            {
                _ItemAdjustedPrice = value;
            }
        }

        public decimal ItemCostPrice
        {

            get
            {
                return _ItemCostPrice;
            }

            set
            {
                _ItemCostPrice = value;
            }
        }

        public string ItemDescription
        {

            get
            {
                return _ItemDescription;
            }

            set
            {
                _ItemDescription = value;
            }
        }

        public decimal ItemListPrice
        {

            get
            {
                return _ItemListPrice;
            }

            set
            {
                _ItemListPrice = value;
            }
        }

        public int ItemWeight
        {

            get
            {
                return _ItemWeight;
            }

            set
            {
                _ItemWeight = value;
            }
        }

        public int ProductId
        {

            get
            {
                return _ProductId;
            }

            set
            {
                _ProductId = value;
            }
        }

        public int PurchaseGiftId
        {

            get
            {
                return _PurchaseGiftId;
            }

            set
            {
                _PurchaseGiftId = value;
            }
        }

        public string PurchaseGiftName
        {

            get
            {
                return _PurchaseGiftName;
            }

            set
            {
                _PurchaseGiftName = value;
            }
        }

        public int Quantity
        {

            get
            {
                return _Quantity;
            }

            set
            {
                _Quantity = value;
            }
        }

        public int ShipmentQuantity
        {

            get
            {
                return _ShipmentQuantity;
            }

            set
            {
                _ShipmentQuantity = value;
            }
        }

        public string SKU
        {

            get
            {
                return _SKU;
            }

            set
            {
                _SKU = value;
            }
        }

        public string SKUContent
        {

            get
            {
                return _SKUContent;
            }

            set
            {
                _SKUContent = value;
            }
        }

        public string SkuId
        {

            get
            {
                return _SkuId;
            }

            set
            {
                _SkuId = value;
            }
        }

        public string ThumbnailsUrl
        {

            get
            {
                return _ThumbnailsUrl;
            }

            set
            {
                _ThumbnailsUrl = value;
            }
        }

        public int WholesaleDiscountId
        {

            get
            {
                return _WholesaleDiscountId;
            }

            set
            {
                _WholesaleDiscountId = value;
            }
        }

        public string WholesaleDiscountName
        {

            get
            {
                return _WholesaleDiscountName;
            }

            set
            {
                _WholesaleDiscountName = value;
            }
        }
    }
}

