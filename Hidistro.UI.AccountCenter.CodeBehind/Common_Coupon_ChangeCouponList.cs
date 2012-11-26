namespace Hidistro.UI.AccountCenter.CodeBehind
{
    using Hidistro.UI.Common.Controls;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class Common_Coupon_ChangeCouponList : AscxTemplatedWebControl
    {
       DataList dataListCoupon;
        public const string TagID = "Common_Coupon_ChangeCouponList";

        public event CommandEventHandler ItemCommand;

        public Common_Coupon_ChangeCouponList()
        {
            base.ID = "Common_Coupon_ChangeCouponList";
        }

        protected override void AttachChildControls()
        {
            this.dataListCoupon = (DataList) this.FindControl("dataListCoupon");
            this.dataListCoupon.ItemCommand += new DataListCommandEventHandler(this.dataListCoupon_ItemCommand);
        }

        public override void DataBind()
        {
            this.EnsureChildControls();
            if (this.dataListCoupon.DataSource != null)
            {
                this.dataListCoupon.DataBind();
            }
        }

       void dataListCoupon_ItemCommand(object source, DataListCommandEventArgs e)
        {
            this.ItemCommand(source, e);
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_UserCenter/Skin-Common_Coupon_ChangeCouponList.ascx";
            }
            base.OnInit(e);
        }

        public DataKeyCollection DataKeys
        {
            get
            {
                return this.dataListCoupon.DataKeys;
            }
        }

        [Browsable(false)]
        public object DataSource
        {
            get
            {
                return this.dataListCoupon.DataSource;
            }
            set
            {
                this.EnsureChildControls();
                this.dataListCoupon.DataSource = value;
            }
        }

        public override string ID
        {
            get
            {
                return base.ID;
            }
            set
            {
            }
        }

        public delegate void CommandEventHandler(object sender, DataListCommandEventArgs e);
    }
}

