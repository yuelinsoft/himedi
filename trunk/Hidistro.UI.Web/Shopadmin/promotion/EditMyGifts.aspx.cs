using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class EditMyGifts : DistributorPage
    {

        int Id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["GiftId"], out Id))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    LoadGift();
                }
            }
        }


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int num;
            GiftInfo entity = new GiftInfo();
            if (!int.TryParse(txtNeedPoint.Text.Trim(), out num))
            {
                ShowMsg("兑换所需积分不能为空，大小0-10000之间", false);
            }
            else
            {
                entity.GiftId = Id;
                entity.NeedPoint = num;
                entity.Name = txtGiftName.Text.Trim();
                entity.Title = txtGiftTitle.Text.Trim();
                entity.Meta_Description = txtTitleDescription.Text.Trim();
                entity.Meta_Keywords = txtTitleKeywords.Text.Trim();
                Globals.EntityCoding(entity, false);
                if (SubsiteGiftHelper.UpdateMyGifts(entity))
                {
                    ShowMsg("成功修改了一件礼品的基本信息", true);
                }
                else
                {
                    ShowMsg("修改件礼品的基本信息失败", true);
                }
            }
        }

        private void LoadGift()
        {
            GiftInfo myGiftsDetails = SubsiteGiftHelper.GetMyGiftsDetails(Id);
            txtGiftName.Text = myGiftsDetails.Name;
            lblPurchasePrice.Text = string.Format("{0:F2}", myGiftsDetails.PurchasePrice);
            txtNeedPoint.Text = myGiftsDetails.NeedPoint.ToString();
            if (!string.IsNullOrEmpty(myGiftsDetails.Unit))
            {
                lblUnit.Text = myGiftsDetails.Unit;
            }
            if (myGiftsDetails.MarketPrice.HasValue)
            {
                lblMarketPrice.Text = string.Format("{0:F2}", myGiftsDetails.MarketPrice);
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.ShortDescription))
            {
                lblShortDescription.Text = Globals.HtmlEncode(myGiftsDetails.ShortDescription);
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.LongDescription))
            {
                fcDescription.Text = myGiftsDetails.LongDescription;
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.Title))
            {
                txtGiftTitle.Text = myGiftsDetails.Title;
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.Meta_Description))
            {
                txtTitleDescription.Text = myGiftsDetails.Meta_Description;
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.Meta_Keywords))
            {
                txtTitleKeywords.Text = myGiftsDetails.Meta_Keywords;
            }
            if (!string.IsNullOrEmpty(myGiftsDetails.ImageUrl))
            {
                uploader1.UploadedImageUrl = myGiftsDetails.ImageUrl;
            }
        }


    }
}

