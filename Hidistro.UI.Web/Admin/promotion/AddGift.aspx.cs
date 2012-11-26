using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
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
    public partial class AddGift : AdminPage
    {
        private void btnCreate_Click(object sender, EventArgs e)
        {
            decimal? nullable;
            decimal num;
            decimal? nullable2;
            int num2;
            if (ValidateValues(out nullable, out num, out nullable2, out num2))
            {
                GiftInfo target = new GiftInfo();
                target.PurchasePrice = num;
                target.CostPrice = nullable;
                target.MarketPrice = nullable2;
                target.NeedPoint = num2;
                target.Name = txtGiftName.Text.Trim();
                target.Unit = txtUnit.Text.Trim();
                target.ShortDescription = txtShortDescription.Text.Trim();
                target.LongDescription = string.IsNullOrEmpty(fcDescription.Text) ? null : fcDescription.Text.Trim();
                target.Title = txtGiftTitle.Text.Trim();
                target.Meta_Description = txtTitleDescription.Text.Trim();
                target.Meta_Keywords = txtTitleKeywords.Text.Trim();
                target.IsDownLoad = ckdown.Checked;
                target.ImageUrl = uploader1.UploadedImageUrl;
                target.ThumbnailUrl40 = uploader1.ThumbnailUrl40;
                target.ThumbnailUrl60 = uploader1.ThumbnailUrl60;
                target.ThumbnailUrl100 = uploader1.ThumbnailUrl100;
                target.ThumbnailUrl160 = uploader1.ThumbnailUrl160;
                target.ThumbnailUrl180 = uploader1.ThumbnailUrl180;
                target.ThumbnailUrl220 = uploader1.ThumbnailUrl220;
                target.ThumbnailUrl310 = uploader1.ThumbnailUrl310;
                target.ThumbnailUrl410 = uploader1.ThumbnailUrl410;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<GiftInfo>(target, new string[] { "ValGift" });
                string str = string.Empty;
                if (target.PurchasePrice < target.CostPrice)
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
                    switch (GiftHelper.AddGift(target))
                    {
                        case GiftActionStatus.Success:
                            ShowMsg("成功的添加了一件礼品", true);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            btnCreate.Click += new EventHandler(btnCreate_Click);
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

