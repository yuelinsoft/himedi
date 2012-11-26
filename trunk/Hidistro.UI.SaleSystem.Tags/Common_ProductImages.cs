namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    [ParseChildren(true)]
    public class Common_ProductImages : AscxTemplatedWebControl
    {
        
       bool _Is410Image;
        
       bool _Is60Image;
       ProductInfo imageInfo;
       HiImage imgBig;
       HiImage imgSmall1;
       HiImage imgSmall2;
       HiImage imgSmall3;
       HiImage imgSmall4;
       HiImage imgSmall5;
       HtmlInputHidden iptPicUrl1;
       HtmlInputHidden iptPicUrl2;
       HtmlInputHidden iptPicUrl3;
       HtmlInputHidden iptPicUrl4;
       HtmlInputHidden iptPicUrl5;
        public const string TagID = "common_ProductImages";

        public Common_ProductImages()
        {
            base.ID = "common_ProductImages";
        }

        protected override void AttachChildControls()
        {
            this.imgBig = (HiImage) this.FindControl("imgBig");
            this.imgSmall1 = (HiImage) this.FindControl("imgSmall1");
            this.imgSmall2 = (HiImage) this.FindControl("imgSmall2");
            this.imgSmall3 = (HiImage) this.FindControl("imgSmall3");
            this.imgSmall4 = (HiImage) this.FindControl("imgSmall4");
            this.imgSmall5 = (HiImage) this.FindControl("imgSmall5");
            this.iptPicUrl1 = (HtmlInputHidden) this.FindControl("iptPicUrl1");
            this.iptPicUrl2 = (HtmlInputHidden) this.FindControl("iptPicUrl2");
            this.iptPicUrl3 = (HtmlInputHidden) this.FindControl("iptPicUrl3");
            this.iptPicUrl4 = (HtmlInputHidden) this.FindControl("iptPicUrl4");
            this.iptPicUrl5 = (HtmlInputHidden) this.FindControl("iptPicUrl5");
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

       void BindData()
        {
            if (this.imageInfo != null)
            {
                string oldValue = "/Storage/master/product/images/";
                string newValue = "/Storage/master/product/thumbs310/310_";
                if (this.Is410Image)
                {
                    newValue = "/Storage/master/product/thumbs410/410_";
                }
                string str3 = "/Storage/master/product/thumbs40/40_";
                if (this.Is60Image)
                {
                    str3 = "/Storage/master/product/thumbs60/60_";
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl1))
                {
                    this.imgBig.ImageUrl = this.iptPicUrl1.Value = this.imageInfo.ImageUrl1.Replace(oldValue, newValue);
                    this.imgSmall1.ImageUrl = this.imageInfo.ImageUrl1.Replace(oldValue, str3);
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl2))
                {
                    this.iptPicUrl2.Value = this.imageInfo.ImageUrl2.Replace(oldValue, newValue);
                    this.imgSmall2.ImageUrl = this.imageInfo.ImageUrl2.Replace(oldValue, str3);
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl3))
                {
                    this.iptPicUrl3.Value = this.imageInfo.ImageUrl3.Replace(oldValue, newValue);
                    this.imgSmall3.ImageUrl = this.imageInfo.ImageUrl3.Replace(oldValue, str3);
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl4))
                {
                    this.iptPicUrl4.Value = this.imageInfo.ImageUrl4.Replace(oldValue, newValue);
                    this.imgSmall4.ImageUrl = this.imageInfo.ImageUrl4.Replace(oldValue, str3);
                }
                if (!string.IsNullOrEmpty(this.imageInfo.ImageUrl5))
                {
                    this.iptPicUrl5.Value = this.imageInfo.ImageUrl5.Replace(oldValue, newValue);
                    this.imgSmall5.ImageUrl = this.imageInfo.ImageUrl5.Replace(oldValue, str3);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "/ascx/tags/Common_ViewProduct/Skin-Common_ProductImages.ascx";
            }
            base.OnInit(e);
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

        public ProductInfo ImageInfo
        {
            get
            {
                return this.imageInfo;
            }
            set
            {
                this.imageInfo = value;
            }
        }

        public bool Is410Image
        {
            
            get
            {
                return this._Is410Image;
            }
            
            set
            {
                this._Is410Image = value;
            }
        }

        public bool Is60Image
        {
            
            get
            {
                return this._Is60Image;
            }
            
            set
            {
                this._Is60Image = value;
            }
        }
    }
}

