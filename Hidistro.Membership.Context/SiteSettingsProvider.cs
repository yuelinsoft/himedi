using System;
using System.Data;
using Hidistro.Core;

namespace Hidistro.Membership.Context
{

    /// <summary>
    /// 站点设置提供者
    /// </summary>
    public abstract class SiteSettingsProvider
    {
        /// <summary>
        /// 静态成功
        /// </summary>
        static readonly SiteSettingsProvider _defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.SettingsData,Hidistro.Membership.Data") as SiteSettingsProvider);

        /// <summary>
        /// 受保护构造函数
        /// </summary>
        protected SiteSettingsProvider()
        {
        }

        /// <summary>
        /// 获取分销商站点设置
        /// </summary>
        /// <param name="userId">分销商ID</param>
        /// <returns></returns>
        public abstract SiteSettings GetDistributorSettings(int userId);

        /// <summary>
        /// 获取分销商站点设置，重载
        /// </summary>
        /// <param name="siteUrl">分销域名</param>
        /// <returns></returns>
        public abstract SiteSettings GetDistributorSettings(string siteUrl);

        /// <summary>
        /// 静态方法，返回站点设置提供者
        /// </summary>
        /// <returns></returns>
        public static SiteSettingsProvider Instance()
        {
            if (null != _defaultInstance)
            {
                return _defaultInstance;
            }
            else
            {
                throw new Exception("反射对象：Hidistro.Membership.Data.SettingsData,Hidistro.Membership.Data 失败！");
            }
        }

        /// <summary>
        /// 构造站点设置
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <returns>站点设置对象</returns>
        public static SiteSettings PopulateDistributorSettings(IDataReader reader)
        {
            SiteSettings settings = new SiteSettings(reader["SiteUrl"].ToString().ToLower(), new int?((int)reader["UserId"]));

            if (reader["RecordCode"] != DBNull.Value)
            {
                settings.RecordCode = reader["RecordCode"].ToString();
            }
            if (reader["LogoUrl"] != DBNull.Value)
            {
                settings.LogoUrl = reader["LogoUrl"].ToString();
            }
            if (reader["RecordCode2"] != DBNull.Value)
            {
                settings.RecordCode2 = reader["RecordCode2"].ToString();
            }
            if (reader["SiteUrl2"] != DBNull.Value)
            {
                settings.SiteUrl2 = reader["SiteUrl2"].ToString();
            }
            if (reader["RequestDate"] != DBNull.Value)
            {
                settings.RequestDate = new DateTime?((DateTime)reader["RequestDate"]);
            }
            if (reader["CreateDate"] != DBNull.Value)
            {
                settings.CreateDate = new DateTime?((DateTime)reader["CreateDate"]);
            }
            if (reader["SiteDescription"] != DBNull.Value)
            {
                settings.SiteDescription = reader["SiteDescription"].ToString();
            }
            if (reader["SiteName"] != DBNull.Value)
            {
                settings.SiteName = reader["SiteName"].ToString();
            }
            if (reader["Theme"] != DBNull.Value)
            {
                settings.Theme = reader["Theme"].ToString();
            }
            if (reader["Footer"] != DBNull.Value)
            {
                settings.Footer = reader["Footer"].ToString();
            }
            if (reader["SearchMetaKeywords"] != DBNull.Value)
            {
                settings.SearchMetaKeywords = reader["SearchMetaKeywords"].ToString();
            }
            if (reader["SearchMetaDescription"] != DBNull.Value)
            {
                settings.SearchMetaDescription = reader["SearchMetaDescription"].ToString();
            }
            if (reader["DecimalLength"] != DBNull.Value)
            {
                settings.DecimalLength = (int)reader["DecimalLength"];
            }
            if (reader["YourPriceName"] != DBNull.Value)
            {
                settings.YourPriceName = reader["YourPriceName"].ToString();
            }
            if (reader["Disabled"] != DBNull.Value)
            {
                settings.Disabled = (bool)reader["Disabled"];
            }
            if (reader["ReferralDeduct"] != DBNull.Value)
            {
                settings.ReferralDeduct = (int)reader["ReferralDeduct"];
            }
            if (reader["IsShowGroupBuy"] != DBNull.Value)
            {
                settings.IsShowGroupBuy = (bool)reader["IsShowGroupBuy"];
            }
            if (reader["IsShowCountDown"] != DBNull.Value)
            {
                settings.IsShowCountDown = (bool)reader["IsShowCountDown"];
            }
            if (reader["IsShowOnlineGift"] != DBNull.Value)
            {
                settings.IsShowOnlineGift = (bool)reader["IsShowOnlineGift"];
            }
            if (reader["DefaultProductImage"] != DBNull.Value)
            {
                settings.DefaultProductImage = reader["DefaultProductImage"].ToString();
            }
            if (reader["PointsRate"] != DBNull.Value)
            {
                settings.PointsRate = (decimal)reader["PointsRate"];
            }
            if (reader["OrderShowDays"] != DBNull.Value)
            {
                settings.OrderShowDays = (int)reader["OrderShowDays"];
            }
            if (reader["HtmlOnlineServiceCode"] != DBNull.Value)
            {
                settings.HtmlOnlineServiceCode = reader["HtmlOnlineServiceCode"].ToString();
            }
            if (reader["EmailSender"] != DBNull.Value)
            {
                settings.EmailSender = reader["EmailSender"].ToString();
            }
            if (reader["EmailSettings"] != DBNull.Value)
            {
                settings.EmailSettings = reader["EmailSettings"].ToString();
            }
            if (reader["SMSSender"] != DBNull.Value)
            {
                settings.SMSSender = reader["SMSSender"].ToString();
            }
            if (reader["SMSSettings"] != DBNull.Value)
            {
                settings.SMSSettings = reader["SMSSettings"].ToString();
            }
            if (reader["Topkey"] != DBNull.Value)
            {
                settings.Topkey = reader["Topkey"].ToString();
            }
            if (reader["TopSecret"] != DBNull.Value)
            {
                settings.TopSecret = reader["TopSecret"].ToString();
            }
            return settings;
        }

        /// <summary>
        /// 保存站点设置
        /// </summary>
        /// <param name="settings">站点设置对象</param>
        public abstract void SaveDistributorSettings(SiteSettings settings);
    }
}

