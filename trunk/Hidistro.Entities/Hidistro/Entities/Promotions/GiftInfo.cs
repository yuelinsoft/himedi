namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using Hishop.Components.Validation;
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Runtime.CompilerServices;

    public class GiftInfo
    {
        
       decimal? _CostPrice ;
        
       int _GiftId ;
        
       string _ImageUrl ;
        
       bool _IsDownLoad ;
        
       string _LongDescription ;
        
       decimal? _MarketPrice ;
        
       string _Meta_Description ;
        
       string _Meta_Keywords ;
        
       string _Name ;
        
       int _NeedPoint ;
        
       decimal _PurchasePrice ;
        
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
        
       string _Unit ;

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValGift"), NotNullValidator(Negated=true, Ruleset="ValGift"), ValidatorComposition(CompositionType.Or, Ruleset="ValGift", MessageTemplate="成本价格，金额大小0.01-1000万之间")]
        public decimal? CostPrice
        {
            
            get
            {
                return this._CostPrice ;
            }
            
            set
            {
                this._CostPrice  = value;
            }
        }

        public int GiftId
        {
            
            get
            {
                return this._GiftId ;
            }
            
            set
            {
                this._GiftId  = value;
            }
        }

        public string ImageUrl
        {
            
            get
            {
                return this._ImageUrl ;
            }
            
            set
            {
                this._ImageUrl  = value;
            }
        }

        public bool IsDownLoad
        {
            
            get
            {
                return this._IsDownLoad ;
            }
            
            set
            {
                this._IsDownLoad  = value;
            }
        }

        public string LongDescription
        {
            
            get
            {
                return this._LongDescription ;
            }
            
            set
            {
                this._LongDescription  = value;
            }
        }

        [ValidatorComposition(CompositionType.Or, Ruleset="ValGift", MessageTemplate="市场参考价格，金额大小0.01-1000万之间"), RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValGift"), NotNullValidator(Negated=true, Ruleset="ValGift")]
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

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValGift", MessageTemplate="详细页描述长度限制在0-100个字符之间")]
        public string Meta_Description
        {
            
            get
            {
                return this._Meta_Description ;
            }
            
            set
            {
                this._Meta_Description  = value;
            }
        }

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset="ValGift", MessageTemplate="详细页关键字长度限制在0-100个字符之间")]
        public string Meta_Keywords
        {
            
            get
            {
                return this._Meta_Keywords ;
            }
            
            set
            {
                this._Meta_Keywords  = value;
            }
        }

        [StringLengthValidator(1, 60, Ruleset="ValGift", MessageTemplate="礼品名称不能为空，长度限制在1-60个字符之间"), HtmlCoding]
        public string Name
        {
            
            get
            {
                return this._Name ;
            }
            
            set
            {
                this._Name  = value;
            }
        }

        [RangeValidator(0, RangeBoundaryType.Inclusive, 0x2710, RangeBoundaryType.Inclusive, Ruleset="ValGift", MessageTemplate="兑换所需积分不能为空，大小0-10000之间")]
        public int NeedPoint
        {
            
            get
            {
                return this._NeedPoint ;
            }
            
            set
            {
                this._NeedPoint  = value;
            }
        }

        [RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset="ValGift", MessageTemplate="采购价格，金额大小0.01-1000万之间")]
        public decimal PurchasePrice
        {
            
            get
            {
                return this._PurchasePrice ;
            }
            
            set
            {
                this._PurchasePrice  = value;
            }
        }

        [StringLengthValidator(0, 300, Ruleset="ValGift", MessageTemplate="礼品简单介绍长度限制在0-300个字符之间"), HtmlCoding]
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

        [HtmlCoding, StringLengthValidator(0, 60, Ruleset="ValGift", MessageTemplate="详细页标题长度限制在0-60个字符之间")]
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

        [StringLengthValidator(0, 10, Ruleset="ValGift", MessageTemplate="计量单位长度限制在0-10个字符之间")]
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
    }
}

