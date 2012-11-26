namespace Hidistro.Entities.Promotions
{
    using System;
    using System.Runtime.CompilerServices;

    public class CouponItemInfo
    {
        
       string _ClaimCode ;
        
       int _CouponId ;
        
       string _EmailAddress ;
        
       DateTime _GenerateTime ;
        
       int? _UserId ;

        public CouponItemInfo()
        {
        }

        public CouponItemInfo(int couponId, string claimCode, int? userId, string emailAddress, DateTime generateTime)
        {
            this.CouponId = couponId;
            this.ClaimCode = claimCode;
            this.UserId = userId;
            this.EmailAddress = emailAddress;
            this.GenerateTime = generateTime;
        }

        public string ClaimCode
        {
            
            get
            {
                return this._ClaimCode ;
            }
            
            set
            {
                this._ClaimCode  = value;
            }
        }

        public int CouponId
        {
            
            get
            {
                return this._CouponId ;
            }
            
            set
            {
                this._CouponId  = value;
            }
        }

        public string EmailAddress
        {
            
            get
            {
                return this._EmailAddress ;
            }
            
            set
            {
                this._EmailAddress  = value;
            }
        }

        public DateTime GenerateTime
        {
            
            get
            {
                return this._GenerateTime ;
            }
            
            set
            {
                this._GenerateTime  = value;
            }
        }

        public int? UserId
        {
            
            get
            {
                return this._UserId ;
            }
            
            set
            {
                this._UserId  = value;
            }
        }
    }
}

