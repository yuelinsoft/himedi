using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Hidistro.Membership.Context
{
    /// <summary>
    /// 站点设置
    /// </summary>
    public class SiteSettings
    {
        public SiteSettings(string siteUrl, int? distributorUserId)
        {
            SiteUrl = siteUrl;
            UserId = distributorUserId;
            Disabled = false;
            SiteDescription = "最安全，最专业的网上商店系统";
            Theme = "default";
            SiteName = "Hishop";
            LogoUrl = "/utility/pics/logo.jpg";
            DefaultProductImage = "/utility/pics/none.gif";
            DefaultProductThumbnail1 = "/utility/pics/none.gif";
            DefaultProductThumbnail2 = "/utility/pics/none.gif";
            DefaultProductThumbnail3 = "/utility/pics/none.gif";
            DefaultProductThumbnail4 = "/utility/pics/none.gif";
            DecimalLength = 2;
            IsOpenSiteSale = true;
            IsShowGroupBuy = true;
            IsShowCountDown = true;
            IsShowOnlineGift = true;
            PointsRate = 1M;
            OrderShowDays = 7;
            CloseOrderDays = 3;
            ClosePurchaseOrderDays = 5;
            FinishOrderDays = 7;
            FinishPurchaseOrderDays = 10;
            Author = "Hishop development team";
            Generator = "H易分销 2.0";
            Topkey = "";
            TopSecret = "";
        }

        public static SiteSettings FromXml(XmlDocument doc)
        {

            XmlNode node = doc.SelectSingleNode("Settings");

            return new SiteSettings(node.SelectSingleNode("SiteUrl").InnerText, null)
            {
                AssistantIv = node.SelectSingleNode("AssistantIv").InnerText,
                AssistantKey = node.SelectSingleNode("AssistantKey").InnerText,
                Topkey = node.SelectSingleNode("Topkey").InnerText,
                TopSecret = node.SelectSingleNode("TopSecret").InnerText,
                DecimalLength = int.Parse(node.SelectSingleNode("DecimalLength").InnerText),
                DefaultProductImage = node.SelectSingleNode("DefaultProductImage").InnerText,
                DefaultProductThumbnail1 = node.SelectSingleNode("DefaultProductThumbnail1").InnerText,
                DefaultProductThumbnail2 = node.SelectSingleNode("DefaultProductThumbnail2").InnerText,
                DefaultProductThumbnail3 = node.SelectSingleNode("DefaultProductThumbnail3").InnerText,
                DefaultProductThumbnail4 = node.SelectSingleNode("DefaultProductThumbnail4").InnerText,
                DefaultProductThumbnail5 = node.SelectSingleNode("DefaultProductThumbnail5").InnerText,
                DefaultProductThumbnail6 = node.SelectSingleNode("DefaultProductThumbnail6").InnerText,
                DefaultProductThumbnail7 = node.SelectSingleNode("DefaultProductThumbnail7").InnerText,
                DefaultProductThumbnail8 = node.SelectSingleNode("DefaultProductThumbnail8").InnerText,
                IsOpenSiteSale = bool.Parse(node.SelectSingleNode("IsOpenSiteSale").InnerText),
                IsShowGroupBuy = bool.Parse(node.SelectSingleNode("IsShowGroupBuy").InnerText),
                IsShowCountDown = bool.Parse(node.SelectSingleNode("IsShowCountDown").InnerText),
                IsShowOnlineGift = bool.Parse(node.SelectSingleNode("IsShowOnlineGift").InnerText),
                Disabled = bool.Parse(node.SelectSingleNode("Disabled").InnerText),
                ReferralDeduct = int.Parse(node.SelectSingleNode("ReferralDeduct").InnerText),
                Footer = node.SelectSingleNode("Footer").InnerText,
                HtmlOnlineServiceCode = node.SelectSingleNode("HtmlOnlineServiceCode").InnerText,
                LogoUrl = node.SelectSingleNode("LogoUrl").InnerText,
                OrderShowDays = int.Parse(node.SelectSingleNode("OrderShowDays").InnerText),
                CloseOrderDays = int.Parse(node.SelectSingleNode("CloseOrderDays").InnerText),
                ClosePurchaseOrderDays = int.Parse(node.SelectSingleNode("ClosePurchaseOrderDays").InnerText),
                FinishOrderDays = int.Parse(node.SelectSingleNode("FinishOrderDays").InnerText),
                FinishPurchaseOrderDays = int.Parse(node.SelectSingleNode("FinishPurchaseOrderDays").InnerText),
                PointsRate = decimal.Parse(node.SelectSingleNode("PointsRate").InnerText),
                SearchMetaDescription = node.SelectSingleNode("SearchMetaDescription").InnerText,
                SearchMetaKeywords = node.SelectSingleNode("SearchMetaKeywords").InnerText,
                SiteDescription = node.SelectSingleNode("SiteDescription").InnerText,
                SiteName = node.SelectSingleNode("SiteName").InnerText,
                SiteUrl = node.SelectSingleNode("SiteUrl").InnerText,
                UserId = null,
                Theme = node.SelectSingleNode("Theme").InnerText,
                YourPriceName = node.SelectSingleNode("YourPriceName").InnerText,
                DistributorRequestInstruction = node.SelectSingleNode("DistributorRequestInstruction").InnerText,
                DistributorRequestProtocols = node.SelectSingleNode("DistributorRequestProtocols").InnerText,
                EmailSender = node.SelectSingleNode("EmailSender").InnerText,
                EmailSettings = node.SelectSingleNode("EmailSettings").InnerText,
                SMSSender = node.SelectSingleNode("SMSSender").InnerText,
                SMSSettings = node.SelectSingleNode("SMSSettings").InnerText,
                Author = node.SelectSingleNode("Author").InnerText,
                Generator = node.SelectSingleNode("Generator").InnerText
            };
        }

       static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
        {

            XmlNode newChild = root.SelectSingleNode(nodeName);

            if (newChild == null)
            {
                newChild = doc.CreateElement(nodeName);
                root.AppendChild(newChild);
            }

            newChild.InnerText = nodeValue;

        }

        /// <summary>
        /// 更新主站基本信息
        /// </summary>
        /// <param name="doc"></param>
        public void WriteToXml(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("Settings");
            SetNodeValue(doc, root, "SiteUrl", SiteUrl);
            SetNodeValue(doc, root, "AssistantIv", AssistantIv);
            SetNodeValue(doc, root, "AssistantKey", AssistantKey);
            SetNodeValue(doc, root, "Topkey", Topkey);
            SetNodeValue(doc, root, "TopSecret", TopSecret);
            SetNodeValue(doc, root, "DecimalLength", DecimalLength.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "DefaultProductImage", DefaultProductImage);
            SetNodeValue(doc, root, "DefaultProductThumbnail1", DefaultProductThumbnail1);
            SetNodeValue(doc, root, "DefaultProductThumbnail2", DefaultProductThumbnail2);
            SetNodeValue(doc, root, "DefaultProductThumbnail3", DefaultProductThumbnail3);
            SetNodeValue(doc, root, "DefaultProductThumbnail4", DefaultProductThumbnail4);
            SetNodeValue(doc, root, "DefaultProductThumbnail5", DefaultProductThumbnail5);
            SetNodeValue(doc, root, "DefaultProductThumbnail6", DefaultProductThumbnail6);
            SetNodeValue(doc, root, "DefaultProductThumbnail7", DefaultProductThumbnail7);
            SetNodeValue(doc, root, "DefaultProductThumbnail8", DefaultProductThumbnail8);
            SetNodeValue(doc, root, "IsOpenSiteSale", IsOpenSiteSale ? "true" : "false");
            SetNodeValue(doc, root, "IsShowGroupBuy", IsShowGroupBuy ? "true" : "false");
            SetNodeValue(doc, root, "IsShowCountDown", IsShowCountDown ? "true" : "false");
            SetNodeValue(doc, root, "IsShowOnlineGift", IsShowOnlineGift ? "true" : "false");
            SetNodeValue(doc, root, "Disabled", Disabled ? "true" : "false");
            SetNodeValue(doc, root, "ReferralDeduct", ReferralDeduct.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "Footer", Footer);
            SetNodeValue(doc, root, "HtmlOnlineServiceCode", HtmlOnlineServiceCode);
            SetNodeValue(doc, root, "LogoUrl", LogoUrl);
            SetNodeValue(doc, root, "OrderShowDays", OrderShowDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "CloseOrderDays", CloseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "ClosePurchaseOrderDays", ClosePurchaseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishOrderDays", FinishOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishPurchaseOrderDays", FinishPurchaseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "PointsRate", PointsRate.ToString("F"));
            SetNodeValue(doc, root, "SearchMetaDescription", SearchMetaDescription);
            SetNodeValue(doc, root, "SearchMetaKeywords", SearchMetaKeywords);
            SetNodeValue(doc, root, "SiteDescription", SiteDescription);
            SetNodeValue(doc, root, "SiteName", SiteName);
            SetNodeValue(doc, root, "Theme", Theme);
            SetNodeValue(doc, root, "YourPriceName", YourPriceName);
            SetNodeValue(doc, root, "DistributorRequestInstruction", DistributorRequestInstruction);
            SetNodeValue(doc, root, "DistributorRequestProtocols", DistributorRequestProtocols);
            SetNodeValue(doc, root, "EmailSender", EmailSender);
            SetNodeValue(doc, root, "EmailSettings", EmailSettings);
            SetNodeValue(doc, root, "SMSSender", SMSSender);
            SetNodeValue(doc, root, "SMSSettings", SMSSettings);
            SetNodeValue(doc, root, "Author", Author);
            SetNodeValue(doc, root, "Generator", Generator);


        }

        public string AssistantIv { get; set; }

        public string AssistantKey { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "过期几天自动关闭订单的天数必须在1-90之间")]
        public int CloseOrderDays { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "过期几天自动关闭采购单的天数必须在1-90之间")]
        public int ClosePurchaseOrderDays { get; set; }

        public DateTime? CreateDate { get; set; }

        public int DecimalLength { get; set; }

        public string DefaultProductImage { get; set; }

        public string DefaultProductThumbnail1 { get; set; }

        public string DefaultProductThumbnail2 { get; set; }

        public string DefaultProductThumbnail3 { get; set; }

        public string DefaultProductThumbnail4 { get; set; }

        public string DefaultProductThumbnail5 { get; set; }

        public string DefaultProductThumbnail6 { get; set; }

        public string DefaultProductThumbnail7 { get; set; }

        public string DefaultProductThumbnail8 { get; set; }

        public bool Disabled { get; set; }

        public bool IsOpenSiteSale { get; set; }

        public string DistributorRequestInstruction { get; set; }

        public string DistributorRequestProtocols { get; set; }

        public bool EmailEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(EmailSender) && !string.IsNullOrEmpty(EmailSettings)) && (EmailSender.Trim().Length > 0)) && (EmailSettings.Trim().Length > 0));
            }
        }

        public string EmailSender { get; set; }

        public string EmailSettings { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "发货几天自动完成订单的天数必须在1-90之间")]
        public int FinishOrderDays { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "发货几天自动完成采购单的天数必须在1-90之间")]
        public int FinishPurchaseOrderDays { get; set; }

        public string Footer { get; set; }

        [StringLengthValidator(0, 4000, Ruleset = "ValMasterSettings", MessageTemplate = "网页客服代码长度限制在4000个字符以内")]
        public string HtmlOnlineServiceCode { get; set; }

        public bool IsDistributorSettings
        {
            get
            {
                return UserId.HasValue;
            }
        }

        public bool IsShowCountDown { get; set; }

        public bool IsShowGroupBuy { get; set; }

        public bool IsShowOnlineGift { get; set; }

        public string Topkey { get; set; }

        public string TopSecret { get; set; }

        public string LogoUrl { get; set; }

        [RangeValidator(1, RangeBoundaryType.Inclusive, 90, RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "最近几天内订单的天数必须在1-90之间")]
        public int OrderShowDays { get; set; }

        [RangeValidator(typeof(decimal), "0.1", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValMasterSettings", MessageTemplate = "几元一积分必须在0.1-10000000之间")]
        public decimal PointsRate { get; set; }

        public string RecordCode { get; set; }

        public string RecordCode2 { get; set; }

        public int ReferralDeduct { get; set; }

        public DateTime? RequestDate { get; set; }

        [HtmlCoding, StringLengthValidator(0, 100, Ruleset = "ValMasterSettings", MessageTemplate = "店铺描述META_DESCRIPTION的长度限制在100字符以内")]
        public string SearchMetaDescription { get; set; }

        [StringLengthValidator(0, 100, Ruleset = "ValMasterSettings", MessageTemplate = "搜索关键字META_KEYWORDS的长度限制在100字符以内")]
        public string SearchMetaKeywords { get; set; }

        [StringLengthValidator(0, 60, Ruleset = "ValMasterSettings", MessageTemplate = "简单介绍TITLE的长度限制在60字符以内")]
        public string SiteDescription { get; set; }

        [StringLengthValidator(1, 60, Ruleset = "ValMasterSettings", MessageTemplate = "店铺名称为必填项，长度限制在60字符以内")]
        public string SiteName { get; set; }

        [StringLengthValidator(1, 128, Ruleset = "ValMasterSettings", MessageTemplate = "店铺域名必须控制在128个字符以内")]
        public string SiteUrl { get; set; }

        public string SiteUrl2 { get; set; }

        public bool SMSEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(SMSSender) && !string.IsNullOrEmpty(SMSSettings)) && (SMSSender.Trim().Length > 0)) && (SMSSettings.Trim().Length > 0));
            }
        }

        public string SMSSender { get; set; }

        public string SMSSettings { get; set; }

        public string Theme { get; set; }

        public int? UserId { get;set; }

        [StringLengthValidator(0, 10, Ruleset = "ValMasterSettings", MessageTemplate = "“您的价”重命名的长度限制在10字符以内")]
        public string YourPriceName { get; set; }

        string author = "Hishop development team";
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        string generator = "H易分销 2.0";
        public string Generator
        {
            get { return generator; }
            set { generator = value; }
        }
    }
}

