using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.Membership.Data
{
    /// <summary>
    /// 保存设置数据
    /// </summary>
    public class SettingsData : SiteSettingsProvider
    {
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        Database database = DatabaseFactory.CreateDatabase();


        /// <summary>
        /// 获取分销站点设置
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public override SiteSettings GetDistributorSettings(string siteUrl)
        {

            SiteSettings siteConfig = null;

            DbCommand sqlStringCommand = database.GetSqlStringCommand(@"select * from distro_Settings where lower(SiteUrl)=@SiteUrl or lower(SiteUrl2)=@SiteUrl");

            database.AddInParameter(sqlStringCommand, "SiteUrl", DbType.String, siteUrl.ToLower());

            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                if (null != reader && reader.Read())
                {
                    siteConfig = SiteSettingsProvider.PopulateDistributorSettings(reader);
                }
            }

            return siteConfig;

        }

        /// <summary>
        /// 获取分销站点设置
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public override SiteSettings GetDistributorSettings(int userId)
        {
            SiteSettings settings = null;

            DbCommand sqlStringCommand = database.GetSqlStringCommand("select * from distro_Settings where @UserId=UserId");

            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);

            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {

                if (null != reader && reader.Read())
                {
                    settings = SiteSettingsProvider.PopulateDistributorSettings(reader);
                }

            }

            return settings;

        }

        /// <summary>
        /// 保存分销站点设置
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <returns></returns>
        public override void SaveDistributorSettings(SiteSettings settings)
        {

            DbCommand sqlStringCommand = database.GetSqlStringCommand("UPDATE distro_Settings SET SiteUrl=@SiteUrl, RecordCode=@RecordCode, LogoUrl=@LogoUrl, SiteDescription=@SiteDescription, SiteName=@SiteName, Theme=@Theme, Footer=@Footer, SearchMetaKeywords=@SearchMetaKeywords, SearchMetaDescription=@SearchMetaDescription,DecimalLength=@DecimalLength, YourPriceName=@YourPriceName, Disabled=@Disabled, ReferralDeduct = @ReferralDeduct, SiteUrl2=@SiteUrl2, RecordCode2=@RecordCode2, DefaultProductImage=@DefaultProductImage, PointsRate=@PointsRate,IsShowGroupBuy=@IsShowGroupBuy,IsShowCountDown=@IsShowCountDown, IsShowOnlineGift=@IsShowOnlineGift, OrderShowDays=@OrderShowDays, HtmlOnlineServiceCode=@HtmlOnlineServiceCode,EmailSender=@EmailSender, EmailSettings=@EmailSettings, SMSSender=@SMSSender, SMSSettings=@SMSSettings, Topkey=@Topkey,TopSecret=@TopSecret WHERE UserId=@UserId");
            
            #region 存储过程参数
            database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, settings.UserId);
            database.AddInParameter(sqlStringCommand, "SiteUrl", DbType.String, settings.SiteUrl);
            database.AddInParameter(sqlStringCommand, "RecordCode", DbType.String, settings.RecordCode);
            database.AddInParameter(sqlStringCommand, "SiteUrl2", DbType.String, settings.SiteUrl2);
            database.AddInParameter(sqlStringCommand, "RecordCode2", DbType.String, settings.RecordCode2);
            database.AddInParameter(sqlStringCommand, "LogoUrl", DbType.String, settings.LogoUrl);
            database.AddInParameter(sqlStringCommand, "SiteDescription", DbType.String, settings.SiteDescription);
            database.AddInParameter(sqlStringCommand, "SiteName", DbType.String, settings.SiteName);
            database.AddInParameter(sqlStringCommand, "Theme", DbType.String, settings.Theme);
            database.AddInParameter(sqlStringCommand, "Footer", DbType.String, settings.Footer);
            database.AddInParameter(sqlStringCommand, "SearchMetaKeywords", DbType.String, settings.SearchMetaKeywords);
            database.AddInParameter(sqlStringCommand, "SearchMetaDescription", DbType.String, settings.SearchMetaDescription);
            database.AddInParameter(sqlStringCommand, "DecimalLength", DbType.Int32, settings.DecimalLength);
            database.AddInParameter(sqlStringCommand, "YourPriceName", DbType.String, settings.YourPriceName);
            database.AddInParameter(sqlStringCommand, "Disabled", DbType.Boolean, settings.Disabled);
            database.AddInParameter(sqlStringCommand, "ReferralDeduct", DbType.Int32, settings.ReferralDeduct);
            database.AddInParameter(sqlStringCommand, "DefaultProductImage", DbType.String, settings.DefaultProductImage);
            database.AddInParameter(sqlStringCommand, "PointsRate", DbType.Decimal, settings.PointsRate);
            database.AddInParameter(sqlStringCommand, "IsShowGroupBuy", DbType.Boolean, settings.IsShowGroupBuy);
            database.AddInParameter(sqlStringCommand, "IsShowCountDown", DbType.Boolean, settings.IsShowCountDown);
            database.AddInParameter(sqlStringCommand, "IsShowOnlineGift", DbType.Boolean, settings.IsShowOnlineGift);
            database.AddInParameter(sqlStringCommand, "OrderShowDays", DbType.Int32, settings.OrderShowDays);
            database.AddInParameter(sqlStringCommand, "HtmlOnlineServiceCode", DbType.String, settings.HtmlOnlineServiceCode);
            database.AddInParameter(sqlStringCommand, "EmailSender", DbType.String, settings.EmailSender);
            database.AddInParameter(sqlStringCommand, "EmailSettings", DbType.String, settings.EmailSettings);
            database.AddInParameter(sqlStringCommand, "SMSSender", DbType.String, settings.SMSSender);
            database.AddInParameter(sqlStringCommand, "SMSSettings", DbType.String, settings.SMSSettings);
            database.AddInParameter(sqlStringCommand, "Topkey", DbType.String, settings.Topkey);
            database.AddInParameter(sqlStringCommand, "TopSecret", DbType.String, settings.TopSecret);
            #endregion

            database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

