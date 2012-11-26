
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.Gifts)]
    public partial class EditGift : AdminPage
    {

        int giftId;


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            decimal? nullable;
            decimal num;
            decimal? nullable2;
            int num2;
            GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
            if (ValidateValues(out nullable, out num, out nullable2, out num2))
            {
                giftDetails.PurchasePrice = num;
                giftDetails.CostPrice = nullable;
                giftDetails.MarketPrice = nullable2;
                giftDetails.NeedPoint = num2;
                giftDetails.Name = txtGiftName.Text.Trim();
                giftDetails.Unit = txtUnit.Text.Trim();
                giftDetails.ShortDescription = txtShortDescription.Text.Trim();
                giftDetails.LongDescription = fcDescription.Text.Trim();
                giftDetails.Title = txtGiftTitle.Text.Trim();
                giftDetails.Meta_Description = txtTitleDescription.Text.Trim();
                giftDetails.Meta_Keywords = txtTitleKeywords.Text.Trim();
                giftDetails.IsDownLoad = ckdown.Checked;
                giftDetails.ImageUrl = uploader1.UploadedImageUrl;
                giftDetails.ThumbnailUrl40 = uploader1.ThumbnailUrl40;
                giftDetails.ThumbnailUrl60 = uploader1.ThumbnailUrl60;
                giftDetails.ThumbnailUrl100 = uploader1.ThumbnailUrl100;
                giftDetails.ThumbnailUrl160 = uploader1.ThumbnailUrl160;
                giftDetails.ThumbnailUrl180 = uploader1.ThumbnailUrl180;
                giftDetails.ThumbnailUrl220 = uploader1.ThumbnailUrl220;
                giftDetails.ThumbnailUrl310 = uploader1.ThumbnailUrl310;
                giftDetails.ThumbnailUrl410 = uploader1.ThumbnailUrl410;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<GiftInfo>(giftDetails, new string[] { "ValGift" });
                string str = string.Empty;
                if (giftDetails.PurchasePrice < giftDetails.CostPrice)
                {
                    str = str + Formatter.FormatErrorMessage("礼品采购价不能小于成本价");
                }
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        str = str + Formatter.FormatErrorMessage(result.Message);
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    ShowMsg(str, false);
                }
                else
                {
                    switch (GiftHelper.UpdateGift(giftDetails))
                    {
                        case GiftActionStatus.Success:
                            ShowMsg("成功修改了一件礼品的基本信息", true);
                            return;

                        case GiftActionStatus.DuplicateName:
                            ShowMsg("已经存在相同的礼品名称", false);
                            return;

                        case GiftActionStatus.DuplicateSKU:
                            ShowMsg("已经存在相同的商家编码", false);
                            return;

                        case GiftActionStatus.UnknowError:
                            ShowMsg("未知错误", false);
                            return;
                    }
                }
            }
        }

        private void LoadGift()
        {
            GiftInfo giftDetails = GiftHelper.GetGiftDetails(giftId);
            if (giftDetails == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                Globals.EntityCoding(giftDetails, false);
                txtGiftName.Text = giftDetails.Name;
                txtPurchasePrice.Text = string.Format("{0:F2}", giftDetails.PurchasePrice);
                txtNeedPoint.Text = giftDetails.NeedPoint.ToString();
                if (!string.IsNullOrEmpty(giftDetails.Unit))
                {
                    txtUnit.Text = giftDetails.Unit;
                }
                if (giftDetails.CostPrice.HasValue)
                {
                    txtCostPrice.Text = string.Format("{0:F2}", giftDetails.CostPrice);
                }
                if (giftDetails.MarketPrice.HasValue)
                {
                    txtMarketPrice.Text = string.Format("{0:F2}", giftDetails.MarketPrice);
                }
                if (!string.IsNullOrEmpty(giftDetails.ShortDescription))
                {
                    txtShortDescription.Text = giftDetails.ShortDescription;
                }
                if (!string.IsNullOrEmpty(giftDetails.LongDescription))
                {
                    fcDescription.Text = giftDetails.LongDescription;
                }
                if (!string.IsNullOrEmpty(giftDetails.Title))
                {
                    txtGiftTitle.Text = giftDetails.Title;
                }
                if (!string.IsNullOrEmpty(giftDetails.Meta_Description))
                {
                    txtTitleDescription.Text = giftDetails.Meta_Description;
                }
                if (!string.IsNullOrEmpty(giftDetails.Meta_Keywords))
                {
                    txtTitleKeywords.Text = giftDetails.Meta_Keywords;
                }
                if (!string.IsNullOrEmpty(giftDetails.ImageUrl))
                {
                    uploader1.UploadedImageUrl = giftDetails.ImageUrl;
                }
                ckdown.Checked = giftDetails.IsDownLoad;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["giftId"], out giftId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnUpdate.Click += new EventHandler(btnUpdate_Click);
                if (!Page.IsPostBack)
                {
                    LoadGift();
                }
            }
        }

        private bool ValidateValues(out decimal? costPrice, out decimal purchasePrice, out decimal? marketPrice, out int needPoint)
        {
            string str = string.Empty;
            costPrice = 0;
            marketPrice = 0;
            if (!string.IsNullOrEmpty(txtCostPrice.Text.Trim()))
            {
                decimal num;
                if (decimal.TryParse(txtCostPrice.Text.Trim(), out num))
                {
                    costPrice = new decimal?(num);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("成本价金额无效，大小在10000000以内");
                }
            }
            if (!decimal.TryParse(txtPurchasePrice.Text.Trim(), out purchasePrice))
            {
                str = str + Formatter.FormatErrorMessage("采购价金额无效，大小在10000000以内");
            }
            if (!string.IsNullOrEmpty(txtMarketPrice.Text.Trim()))
            {
                decimal num2;
                if (decimal.TryParse(txtMarketPrice.Text.Trim(), out num2))
                {
                    marketPrice = new decimal?(num2);
                }
                else
                {
                    str = str + Formatter.FormatErrorMessage("市场参考价金额无效，大小在10000000以内");
                }
            }
            if (!int.TryParse(txtNeedPoint.Text.Trim(), out needPoint))
            {
                str = str + Formatter.FormatErrorMessage("兑换所需积分不能为空，大小0-10000之间");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }
    }
}

