namespace Hidistro.Entities.Promotions
{
    using Hidistro.Core;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class GroupBuyInfo
    {
        
       string _Content ;
        
       DateTime _EndDate ;
        
       int _GroupBuyId ;
        
       int _MaxCount ;
        
       decimal _NeedPrice ;
        
       int _ProductId ;
        
       GroupBuyStatus _Status ;
       IList<GropBuyConditionInfo> groupBuyConditions;

        [HtmlCoding]
        public string Content
        {
            
            get
            {
                return this._Content ;
            }
            
            set
            {
                this._Content  = value;
            }
        }

        public DateTime EndDate
        {
            
            get
            {
                return this._EndDate ;
            }
            
            set
            {
                this._EndDate  = value;
            }
        }

        public IList<GropBuyConditionInfo> GroupBuyConditions
        {
            get
            {
                if (this.groupBuyConditions == null)
                {
                    this.groupBuyConditions = new List<GropBuyConditionInfo>();
                }
                return this.groupBuyConditions;
            }
        }

        public int GroupBuyId
        {
            
            get
            {
                return this._GroupBuyId ;
            }
            
            set
            {
                this._GroupBuyId  = value;
            }
        }

        public int MaxCount
        {
            
            get
            {
                return this._MaxCount ;
            }
            
            set
            {
                this._MaxCount  = value;
            }
        }

        public decimal NeedPrice
        {
            
            get
            {
                return this._NeedPrice ;
            }
            
            set
            {
                this._NeedPrice  = value;
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

        public GroupBuyStatus Status
        {
            
            get
            {
                return this._Status ;
            }
            
            set
            {
                this._Status  = value;
            }
        }
    }
}

