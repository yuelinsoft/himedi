namespace Hidistro.Entities.Commodities
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ProductInfo
    {
        
       DateTime _AddedDate ;
        
       int? _BrandId ;
        
       int _CategoryId ;
        
       string _Description ;
        
       int _DisplaySequence ;
        
       string _ExtendCategoryPath ;
        
       bool _HasSKU ;
        
       string _ImageUrl1 ;
        
       string _ImageUrl2 ;
        
       string _ImageUrl3 ;
        
       string _ImageUrl4 ;
        
       string _ImageUrl5 ;
        
       int _LineId ;
        
       decimal _LowestSalePrice ;
        
       string _MainCategoryPath ;
        
       decimal? _MarketPrice ;
        
       string _MetaDescription ;
        
       string _MetaKeywords ;
        
       Hidistro.Entities.Commodities.PenetrationStatus _PenetrationStatus ;
        
       string _ProductCode ;
        
       int _ProductId ;
        
       string _ProductName ;
        
       int _SaleCounts ;
        
       ProductSaleStatus _SaleStatus ;
        
       string _ShortDescription ;
        
       string _ThumbnailUrl100 ;
        
       string _ThumbnailUrl160 ;
        
       string _ThumbnailUrl180 ;
        
       string _ThumbnailUrl220 ;
        
       string _ThumbnailUrl310 ;
        
       string _ThumbnailUrl40 ;
        
       string _ThumbnailUrl410 ;
        
       string _ThumbnailUrl60 ;
        
       string _Title ;
        
       int? _TypeId ;
        
       string _Unit ;
        
       int _VistiCounts ;
       SKUItem defaultSku;
       Dictionary<string, SKUItem> skus;

        public DateTime AddedDate
        {
            
            get
            {
                return this._AddedDate ;
            }
            
            set
            {
                this._AddedDate  = value;
            }
        }

        public int AlertStock
        {
            get
            {
                return this.DefaultSku.AlertStock;
            }
        }

        public int? BrandId
        {
            
            get
            {
                return this._BrandId ;
            }
            
            set
            {
                this._BrandId  = value;
            }
        }

        public int CategoryId
        {
            
            get
            {
                return this._CategoryId ;
            }
            
            set
            {
                this._CategoryId  = value;
            }
        }

        public decimal CostPrice
        {
            get
            {
                return this.DefaultSku.CostPrice;
            }
        }

        public SKUItem DefaultSku
        {
            get
            {
                return (this.defaultSku ?? (this.defaultSku = this.Skus.Values.First<SKUItem>()));
            }
        }

        public string Description
        {
            
            get
            {
                return this._Description ;
            }
            
            set
            {
                this._Description  = value;
            }
        }

        public int DisplaySequence
        {
            
            get
            {
                return this._DisplaySequence ;
            }
            
            set
            {
                this._DisplaySequence  = value;
            }
        }

        public string ExtendCategoryPath
        {
            
            get
            {
                return this._ExtendCategoryPath ;
            }
            
            set
            {
                this._ExtendCategoryPath  = value;
            }
        }

        public bool HasSKU
        {
            
            get
            {
                return this._HasSKU ;
            }
            
            set
            {
                this._HasSKU  = value;
            }
        }

        public string ImageUrl1
        {
            
            get
            {
                return this._ImageUrl1 ;
            }
            
            set
            {
                this._ImageUrl1  = value;
            }
        }

        public string ImageUrl2
        {
            
            get
            {
                return this._ImageUrl2 ;
            }
            
            set
            {
                this._ImageUrl2  = value;
            }
        }

        public string ImageUrl3
        {
            
            get
            {
                return this._ImageUrl3 ;
            }
            
            set
            {
                this._ImageUrl3  = value;
            }
        }

        public string ImageUrl4
        {
            
            get
            {
                return this._ImageUrl4 ;
            }
            
            set
            {
                this._ImageUrl4  = value;
            }
        }

        public string ImageUrl5
        {
            
            get
            {
                return this._ImageUrl5 ;
            }
            
            set
            {
                this._ImageUrl5  = value;
            }
        }

        public int LineId
        {
            
            get
            {
                return this._LineId ;
            }
            
            set
            {
                this._LineId  = value;
            }
        }

        public decimal LowestSalePrice
        {
            
            get
            {
                return this._LowestSalePrice ;
            }
            
            set
            {
                this._LowestSalePrice  = value;
            }
        }

        public string MainCategoryPath
        {
            
            get
            {
                return this._MainCategoryPath ;
            }
            
            set
            {
                this._MainCategoryPath  = value;
            }
        }

        public decimal? MarketPrice
        {
            
            get
            {
                return this._MarketPrice ;
            }
            
            set
            {
                this._MarketPrice  = value;
            }
        }

        public decimal MaxSalePrice
        {
            get
            {
                decimal[] maxSalePrice = new decimal[1];
                foreach (SKUItem item in this.Skus.Values.Where<SKUItem>(delegate (SKUItem sku) {
                    return sku.SalePrice > maxSalePrice[0];
                }))
                {
                    maxSalePrice[0] = item.SalePrice;
                }
                return maxSalePrice[0];
            }
        }

        [HtmlCoding]
        public string MetaDescription
        {
            
            get
            {
                return this._MetaDescription ;
            }
            
            set
            {
                this._MetaDescription  = value;
            }
        }

        [HtmlCoding]
        public string MetaKeywords
        {
            
            get
            {
                return this._MetaKeywords ;
            }
            
            set
            {
                this._MetaKeywords  = value;
            }
        }

        public decimal MinSalePrice
        {
            get
            {
                decimal[] minSalePrice = new decimal[] { 79228162514264337593543950335M };
                foreach (SKUItem item in this.Skus.Values.Where<SKUItem>(delegate (SKUItem sku) {
                    return sku.SalePrice < minSalePrice[0];
                }))
                {
                    minSalePrice[0] = item.SalePrice;
                }
                return minSalePrice[0];
            }
        }

        public Hidistro.Entities.Commodities.PenetrationStatus PenetrationStatus
        {
            
            get
            {
                return this._PenetrationStatus ;
            }
            
            set
            {
                this._PenetrationStatus  = value;
            }
        }

        public string ProductCode
        {
            
            get
            {
                return this._ProductCode ;
            }
            
            set
            {
                this._ProductCode  = value;
            }
        }

        public int ProductId
        {
            
            get
            {
                return this._ProductId ;
            }
            
            set
            {
                this._ProductId  = value;
            }
        }

        [HtmlCoding]
        public string ProductName
        {
            
            get
            {
                return this._ProductName ;
            }
            
            set
            {
                this._ProductName  = value;
            }
        }

        public decimal PurchasePrice
        {
            get
            {
                return this.DefaultSku.PurchasePrice;
            }
        }

        public int SaleCounts
        {
            
            get
            {
                return this._SaleCounts ;
            }
            
            set
            {
                this._SaleCounts  = value;
            }
        }

        public ProductSaleStatus SaleStatus
        {
            
            get
            {
                return this._SaleStatus ;
            }
            
            set
            {
                this._SaleStatus  = value;
            }
        }

        [HtmlCoding]
        public string ShortDescription
        {
            
            get
            {
                return this._ShortDescription ;
            }
            
            set
            {
                this._ShortDescription  = value;
            }
        }

        public string SKU
        {
            get
            {
                return this.DefaultSku.SKU;
            }
        }

        public string SkuId
        {
            get
            {
                return this.DefaultSku.SkuId;
            }
        }

        public Dictionary<string, SKUItem> Skus
        {
            get
            {
                return (this.skus ?? (this.skus = new Dictionary<string, SKUItem>()));
            }
        }

        public int Stock
        {
            get
            {
                return this.Skus.Values.Sum<SKUItem>(delegate (SKUItem sku) {
                    return sku.Stock;
                });
            }
        }

        public string ThumbnailUrl100
        {
            
            get
            {
                return this._ThumbnailUrl100 ;
            }
            
            set
            {
                this._ThumbnailUrl100  = value;
            }
        }

        public string ThumbnailUrl160
        {
            
            get
            {
                return this._ThumbnailUrl160 ;
            }
            
            set
            {
                this._ThumbnailUrl160  = value;
            }
        }

        public string ThumbnailUrl180
        {
            
            get
            {
                return this._ThumbnailUrl180 ;
            }
            
            set
            {
                this._ThumbnailUrl180  = value;
            }
        }

        public string ThumbnailUrl220
        {
            
            get
            {
                return this._ThumbnailUrl220 ;
            }
            
            set
            {
                this._ThumbnailUrl220  = value;
            }
        }

        public string ThumbnailUrl310
        {
            
            get
            {
                return this._ThumbnailUrl310 ;
            }
            
            set
            {
                this._ThumbnailUrl310  = value;
            }
        }

        public string ThumbnailUrl40
        {
            
            get
            {
                return this._ThumbnailUrl40 ;
            }
            
            set
            {
                this._ThumbnailUrl40  = value;
            }
        }

        public string ThumbnailUrl410
        {
            
            get
            {
                return this._ThumbnailUrl410 ;
            }
            
            set
            {
                this._ThumbnailUrl410  = value;
            }
        }

        public string ThumbnailUrl60
        {
            
            get
            {
                return this._ThumbnailUrl60 ;
            }
            
            set
            {
                this._ThumbnailUrl60  = value;
            }
        }

        [HtmlCoding]
        public string Title
        {
            
            get
            {
                return this._Title ;
            }
            
            set
            {
                this._Title  = value;
            }
        }

        public int? TypeId
        {
            
            get
            {
                return this._TypeId ;
            }
            
            set
            {
                this._TypeId  = value;
            }
        }

        public string Unit
        {
            
            get
            {
                return this._Unit ;
            }
            
            set
            {
                this._Unit  = value;
            }
        }

        public int VistiCounts
        {
            
            get
            {
                return this._VistiCounts ;
            }
            
            set
            {
                this._VistiCounts  = value;
            }
        }

        public int Weight
        {
            get
            {
                return this.DefaultSku.Weight;
            }
        }
    }
}

