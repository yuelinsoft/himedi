using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    /// <summary>
    /// 主站基本设置
    /// </summary>
    [PrivilegeCheck(Privilege.SiteSettings)]
    public partial class SiteContent : AdminPage
    {
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

                LoadSiteContent(masterSettings);

                radEnableHtmRewrite.SelectedValue = SiteUrls.GetEnableHtmRewrite();


            }

        }

        protected void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            try
            {
                StoreHelper.DeleteImage(masterSettings.LogoUrl);
            }
            catch
            {
            }
            masterSettings.LogoUrl = string.Empty;
            if (ValidationSettings(masterSettings))
            {
                SettingsManager.Save(masterSettings);
                LoadSiteContent(masterSettings);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            decimal productPointSet = 0m;// num;
            int showDays = 0;// num2;
            int closeOrderDays = 0;// num3;
            int closePurchaseOrderDays = 0;// num4;
            int finishOrderDays = 0;// num5;
            int finishPurchaseOrderDays = 0;// num6;

            if (ValidateValues(out productPointSet, out showDays, out closeOrderDays, out closePurchaseOrderDays, out finishOrderDays, out finishPurchaseOrderDays))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                masterSettings.SiteName = txtSiteName.Text.Trim();
                masterSettings.SiteUrl = txtDomainName.Text.Trim();
                if (!Regex.IsMatch(masterSettings.SiteUrl, @"^[0-9a-zA-Z]([0-9a-zA-Z-\.]+)$"))
                {
                    ShowMsg("您输入的域名格式不对！", false);
                }
                else
                {
                    masterSettings.SiteDescription = txtSiteDescription.Text.Trim();
                    masterSettings.SearchMetaDescription = txtSearchMetaDescription.Text.Trim();
                    masterSettings.SearchMetaKeywords = txtSearchMetaKeywords.Text.Trim();
                    masterSettings.HtmlOnlineServiceCode = fcOnLineServer.Text;
                    masterSettings.Footer = fkFooter.Text;
                    masterSettings.DecimalLength = dropBitNumber.SelectedValue;
                    masterSettings.PointsRate = productPointSet;

                    if (txtNamePrice.Text.Length <= 20)
                    {
                        masterSettings.YourPriceName = txtNamePrice.Text;
                    }

                    masterSettings.DefaultProductImage = uploader1.UploadedImageUrl;
                    masterSettings.DefaultProductThumbnail1 = uploader1.ThumbnailUrl40;
                    masterSettings.DefaultProductThumbnail2 = uploader1.ThumbnailUrl60;
                    masterSettings.DefaultProductThumbnail3 = uploader1.ThumbnailUrl100;
                    masterSettings.DefaultProductThumbnail4 = uploader1.ThumbnailUrl160;
                    masterSettings.DefaultProductThumbnail5 = uploader1.ThumbnailUrl180;
                    masterSettings.DefaultProductThumbnail6 = uploader1.ThumbnailUrl220;
                    masterSettings.DefaultProductThumbnail7 = uploader1.ThumbnailUrl310;
                    masterSettings.DefaultProductThumbnail8 = uploader1.ThumbnailUrl410;
                    masterSettings.IsOpenSiteSale = radiIsOpenSiteSale.SelectedValue;
                    masterSettings.IsShowGroupBuy = radiIsShowGroupBuy.SelectedValue;
                    masterSettings.IsShowCountDown = radiIsShowCountDown.SelectedValue;
                    masterSettings.IsShowOnlineGift = radiIsShowOnlineGift.SelectedValue;
                    masterSettings.OrderShowDays = showDays;
                    masterSettings.CloseOrderDays = closeOrderDays;
                    masterSettings.ClosePurchaseOrderDays = closePurchaseOrderDays;
                    masterSettings.FinishOrderDays = finishOrderDays;
                    masterSettings.FinishPurchaseOrderDays = finishPurchaseOrderDays;

                    masterSettings.Author = txtAuthor.Text.Trim();
                    masterSettings.Generator = txtGenerator.Text.Trim();

                    if (ValidationSettings(masterSettings))
                    {

                        Globals.EntityCoding(masterSettings, true);

                        SettingsManager.Save(masterSettings);

                        if (radEnableHtmRewrite.SelectedValue != SiteUrls.GetEnableHtmRewrite())
                        {

                            if (radEnableHtmRewrite.SelectedValue)
                            {
                                SiteUrls.EnableHtmRewrite();
                            }
                            else
                            {
                                SiteUrls.CloseHtmRewrite();
                            }

                        }

                        ShowMsg("成功修改了店铺基本信息", true);

                        LoadSiteContent(masterSettings);

                    }

                }

            }

        }

        protected void btnUpoad_Click(object sender, EventArgs e)
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            if (fileUpload.HasFile)
            {
                try
                {
                    masterSettings.LogoUrl = StoreHelper.UploadLogo(fileUpload.PostedFile);
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            SettingsManager.Save(masterSettings);
            LoadSiteContent(masterSettings);
        }

        void LoadSiteContent(SiteSettings siteSettings)
        {
            txtSiteName.Text = siteSettings.SiteName;
            txtDomainName.Text = siteSettings.SiteUrl;
            imgLogo.ImageUrl = siteSettings.LogoUrl;
            if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
            {
                btnDeleteLogo.Visible = true;
            }
            else
            {
                btnDeleteLogo.Visible = false;
            }
            txtSiteDescription.Text = siteSettings.SiteDescription;
            txtSearchMetaDescription.Text = siteSettings.SearchMetaDescription;
            txtSearchMetaKeywords.Text = siteSettings.SearchMetaKeywords;
            fcOnLineServer.Text = siteSettings.HtmlOnlineServiceCode;
            fkFooter.Text = siteSettings.Footer;
            dropBitNumber.SelectedValue = siteSettings.DecimalLength;
            txtProductPointSet.Text = siteSettings.PointsRate.ToString();
            txtNamePrice.Text = siteSettings.YourPriceName;
            uploader1.UploadedImageUrl = siteSettings.DefaultProductImage;
            radiIsOpenSiteSale.SelectedValue = siteSettings.IsOpenSiteSale;
            radiIsShowGroupBuy.SelectedValue = siteSettings.IsShowGroupBuy;
            radiIsShowCountDown.SelectedValue = siteSettings.IsShowCountDown;
            radiIsShowOnlineGift.SelectedValue = siteSettings.IsShowOnlineGift;
            txtShowDays.Text = siteSettings.OrderShowDays.ToString();
            txtCloseOrderDays.Text = siteSettings.CloseOrderDays.ToString();
            txtClosePurchaseOrderDays.Text = siteSettings.ClosePurchaseOrderDays.ToString();
            txtFinishOrderDays.Text = siteSettings.FinishOrderDays.ToString();
            txtFinishPurchaseOrderDays.Text = siteSettings.FinishPurchaseOrderDays.ToString();

            //网站版权信息
            txtAuthor.Text = siteSettings.Author;
            txtGenerator.Text = siteSettings.Generator;

        }



        bool ValidateValues(out decimal productPointSet, out int showDays, out int closeOrderDays, out int closePurchaseOrderDays, out int finishOrderDays, out int finishPurchaseOrderDays)
        {
            string str = string.Empty;
            if (!(decimal.TryParse(txtProductPointSet.Text.Trim(), out productPointSet) && (!txtProductPointSet.Text.Trim().Contains(".") || (txtProductPointSet.Text.Trim().Substring(txtProductPointSet.Text.Trim().IndexOf(".") + 1).Length <= 2))))
            {
                str = str + Formatter.FormatErrorMessage("几元一积分不能为空,为非负数字,范围在0.1-10000000之间");
            }
            if (!int.TryParse(txtShowDays.Text, out showDays))
            {
                str = str + Formatter.FormatErrorMessage("订单显示设置不能为空,必须为正整数,范围在1-90之间");
            }
            if (!int.TryParse(txtCloseOrderDays.Text, out closeOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("过期几天自动关闭订单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!int.TryParse(txtClosePurchaseOrderDays.Text, out closePurchaseOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("过期几天自动关闭采购单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!int.TryParse(txtFinishOrderDays.Text, out finishOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("发货几天自动完成订单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!int.TryParse(txtFinishPurchaseOrderDays.Text, out finishPurchaseOrderDays))
            {
                str = str + Formatter.FormatErrorMessage("发货几天自动完成采购单不能为空,必须为正整数,范围在1-90之间");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }

        bool ValidationSettings(SiteSettings setting)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<SiteSettings>(setting, new string[] { "ValMasterSettings" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

