namespace Hidistro.Entities.Sales
{
    using Hidistro.Entities.Promotions;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ShoppingCartInfo
    {
        
       int _DiscountActivityId ;
        
       string _DiscountName ;
        
       decimal _DiscountValue ;
        
       Hidistro.Entities.Promotions.DiscountValueType _DiscountValueType ;
        
       bool _EightFree ;
        
       int _FeeFreeActivityId ;
        
       string _FeeFreeName ;
        
       bool _OrderOptionFree ;
        
       bool _ProcedureFeeFree ;
       IList<ShoppingCartGiftInfo> lineGifts;
       Dictionary<string, ShoppingCartItemInfo> lineItems;

        public decimal GetAmount()
        {
            decimal num = 0M;
            foreach (ShoppingCartItemInfo info in this.lineItems.Values)
            {
                num += info.SubTotal;
            }
            return num;
        }

        public decimal GetTotal()
        {
            decimal amount = this.GetAmount();
            if ((string.IsNullOrEmpty(this.DiscountName) || (this.DiscountValue <=0M)) || !Enum.IsDefined(typeof(Hidistro.Entities.Promotions.DiscountValueType), this.DiscountValueType))
            {
                return amount;
            }
            if (this.DiscountValueType == Hidistro.Entities.Promotions.DiscountValueType.Amount)
            {
                return (amount - this.DiscountValue);
            }
            return (amount * (this.DiscountValue / 100M));
        }

        public int GetTotalNeedPoint()
        {
            int num = 0;
            foreach (ShoppingCartGiftInfo info in this.LineGifts)
            {
                num += info.SubPointTotal;
            }
            return num;
        }

        public int DiscountActivityId
        {
            
            get
            {
                return this._DiscountActivityId ;
            }
            
            set
            {
                this._DiscountActivityId  = value;
            }
        }

        public string DiscountName
        {
            
            get
            {
                return this._DiscountName ;
            }
            
            set
            {
                this._DiscountName  = value;
            }
        }

        public decimal DiscountValue
        {
            
            get
            {
                return this._DiscountValue ;
            }
            
            set
            {
                this._DiscountValue  = value;
            }
        }

        public Hidistro.Entities.Promotions.DiscountValueType DiscountValueType
        {
            
            get
            {
                return this._DiscountValueType ;
            }
            
            set
            {
                this._DiscountValueType  = value;
            }
        }

        public bool EightFree
        {
            
            get
            {
                return this._EightFree ;
            }
            
            set
            {
                this._EightFree  = value;
            }
        }

        public int FeeFreeActivityId
        {
            
            get
            {
                return this._FeeFreeActivityId ;
            }
            
            set
            {
                this._FeeFreeActivityId  = value;
            }
        }

        public string FeeFreeName
        {
            
            get
            {
                return this._FeeFreeName ;
            }
            
            set
            {
                this._FeeFreeName  = value;
            }
        }

        public IList<ShoppingCartGiftInfo> LineGifts
        {
            get
            {
                if (this.lineGifts == null)
                {
                    this.lineGifts = new List<ShoppingCartGiftInfo>();
                }
                return this.lineGifts;
            }
        }

        public Dictionary<string, ShoppingCartItemInfo> LineItems
        {
            get
            {
                if (this.lineItems == null)
                {
                    this.lineItems = new Dictionary<string, ShoppingCartItemInfo>();
                }
                return this.lineItems;
            }
        }

        public bool OrderOptionFree
        {
            
            get
            {
                return this._OrderOptionFree ;
            }
            
            set
            {
                this._OrderOptionFree  = value;
            }
        }

        public bool ProcedureFeeFree
        {
            
            get
            {
                return this._ProcedureFeeFree ;
            }
            
            set
            {
                this._ProcedureFeeFree  = value;
            }
        }

        public int Quantity
        {
            get
            {
                int num = 0;
                foreach (ShoppingCartItemInfo info in this.lineItems.Values)
                {
                    num += info.Quantity;
                }
                return num;
            }
        }

        public int Weight
        {
            get
            {
                int num = 0;
                foreach (ShoppingCartItemInfo info in this.lineItems.Values)
                {
                    num += info.GetSubWeight();
                }
                return num;
            }
        }
    }
}

