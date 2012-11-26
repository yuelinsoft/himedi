using System;
using System.Runtime.CompilerServices;

namespace Hidistro.Entities.Sales
{
    public class PurchaseOrderItemInfo
    {

        decimal _ItemCostPrice;

        string _ItemDescription;

        string _ItemHomeSiteDescription;

        decimal _ItemListPrice;

        decimal _ItemPurchasePrice;

        int _ItemWeight;

        int _ProductId;

        string _PurchaseOrderId;

        int _Quantity;

        string _SKU;

        string _SKUContent;

        string _SkuId;

        string _ThumbnailsUrl;

        public decimal GetSubTotal()
        {
            return (ItemPurchasePrice * Quantity);
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

        public string ItemHomeSiteDescription
        {

            get
            {
                return _ItemHomeSiteDescription;
            }

            set
            {
                _ItemHomeSiteDescription = value;
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

        public decimal ItemPurchasePrice
        {

            get
            {
                return _ItemPurchasePrice;
            }

            set
            {
                _ItemPurchasePrice = value;
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
    }
}

