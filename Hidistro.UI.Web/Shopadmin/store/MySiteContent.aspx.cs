using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class MySiteContent : DistributorPage
    {

        private void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            try
            {
                SubsiteStoreHelper.DeleteImage(siteSettings.LogoUrl);
            }
            catch
            {
            }
            siteSettings.LogoUrl = string.Empty;
            SettingsManager.Save(siteSettings);
            LoadSiteContent(siteSettings);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int num;
            if (ValidateValues(out num))
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (fileUpload.HasFile)
                {
                    try
                    {
                        siteSettings.LogoUrl = SubsiteStoreHelper.UploadLogo(fileUpload.PostedFile);
                    }
                    catch
                    {
                        ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                siteSettings.SiteName = txtSiteName.Text.Trim();
                siteSettings.SiteDescription = txtSiteDescription.Text.Trim();
                siteSettings.SearchMetaDescription = txtSearchMetaDescription.Text.Trim();
                siteSettings.SearchMetaKeywords = txtSearchMetaKeywords.Text.Trim();
                if (!string.IsNullOrEmpty(fcOnLineServer.Text))
                {
                    siteSettings.HtmlOnlineServiceCode = fcOnLineServer.Text.Trim().Replace("'", @"\");
                }
                else
                {
                    siteSettings.HtmlOnlineServiceCode = string.Empty;
                }
                siteSettings.Footer = fkFooter.Text;
                siteSettings.DecimalLength = dropBitNumber.SelectedValue;
                if (txtNamePrice.Text.Length <= 20)
                {
                    siteSettings.YourPriceName = txtNamePrice.Text;
                }
                siteSettings.IsShowGroupBuy = radiIsShowGroupBuy.SelectedValue;
                siteSettings.IsShowCountDown = radiIsShowCountDown.SelectedValue;
                siteSettings.IsShowOnlineGift = radiIsShowOnlineGift.SelectedValue;
                siteSettings.OrderShowDays = num;
                Globals.EntityCoding(siteSettings, true);
                if (ValidationSettings(siteSettings))
                {
                    SettingsManager.Save(siteSettings);
                    ShowMsg("成功修改了店铺基本信息", true);
                    LoadSiteContent(siteSettings);
                }
            }
        }

        private void LoadSiteContent(SiteSettings siteSettings)
        {
            txtSiteName.Text = siteSettings.SiteName;
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
            if (!string.IsNullOrEmpty(siteSettings.HtmlOnlineServiceCode))
            {
                fcOnLineServer.Text = siteSettings.HtmlOnlineServiceCode.Replace(@"\", "'");
            }
            fkFooter.Text = siteSettings.Footer;
            dropBitNumber.SelectedValue = siteSettings.DecimalLength;
            txtNamePrice.Text = siteSettings.YourPriceName;
            radiIsShowGroupBuy.SelectedValue = siteSettings.IsShowGroupBuy;
            radiIsShowCountDown.SelectedValue = siteSettings.IsShowCountDown;
            radiIsShowOnlineGift.SelectedValue = siteSettings.IsShowOnlineGift;
            txtShowDays.Text = siteSettings.OrderShowDays.ToString(CultureInfo.InvariantCulture);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDeleteLogo.Click += new EventHandler(btnDeleteLogo_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            if (!Page.IsPostBack)
            {
                SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
                if (siteSettings == null)
                {
                    Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/store/SiteRequest.aspx");
                }
                LoadSiteContent(siteSettings);
            }
        }

        private bool ValidateValues(out int showDays)
        {
            string str = string.Empty;
            if (!int.TryParse(txtShowDays.Text, out showDays))
            {
                str = str + Formatter.FormatErrorMessage("订单显示设置不能为空,必须为正整数,范围在1-90之间");
            }
            if (!string.IsNullOrEmpty(str))
            {
                ShowMsg(str, false);
                return false;
            }
            return true;
        }

        private bool ValidationSettings(SiteSettings setting)
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

