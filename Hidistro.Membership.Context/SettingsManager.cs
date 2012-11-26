using Hidistro.Core;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Hidistro.Membership.Context
{

    /// <summary>
    /// 站点管理
    /// </summary>
    public static class SettingsManager
    {

        const string MasterSettingsCacheKey = "FileCache-MasterSettings";

        const string SettingsCacheKey = "DataCache-Settings:{0}";

        public static SiteSettings GetMasterSettings(bool cacheable)
        {
            if (!cacheable)
            {
                HiCache.Remove("FileCache-MasterSettings");
            }

            XmlDocument document = HiCache.Get("FileCache-MasterSettings") as XmlDocument;

            if (document == null)
            {
                string masterSettingsFilename = GetMasterSettingsFilename();

                if (!File.Exists(masterSettingsFilename))
                {
                    return null;
                }

                document = new XmlDocument();

                document.Load(masterSettingsFilename);

                if (cacheable)
                {
                    HiCache.Max("FileCache-MasterSettings", document, new CacheDependency(masterSettingsFilename));
                }

            }

            return SiteSettings.FromXml(document);

        }

       static string GetMasterSettingsFilename()
        {
            HttpContext current = HttpContext.Current;
            return ((current != null) ? current.Request.MapPath("~/config/SiteSettings.config") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\SiteSettings.config"));
        }

        public static SiteSettings GetSiteSettings()
        {
            string siteUrl = HiContext.Current.SiteUrl;

            string key = string.Format("DataCache-Settings:{0}", siteUrl);

            SiteSettings distributorSettings = HiCache.Get(key) as SiteSettings;

            if (distributorSettings != null)
            {
                return distributorSettings;
            }

            distributorSettings = SiteSettingsProvider.Instance().GetDistributorSettings(siteUrl);

            if (distributorSettings != null)
            {

                HiCache.Insert(key, distributorSettings, null, 180);

                return distributorSettings;

            }

            return GetMasterSettings(true);

        }

        public static SiteSettings GetSiteSettings(int distributorUserId)
        {
            return SiteSettingsProvider.Instance().GetDistributorSettings(distributorUserId);
        }

        public static void Save(SiteSettings settings)
        {

            if (settings.IsDistributorSettings)
            {

                SiteSettingsProvider.Instance().SaveDistributorSettings(settings);

                HiCache.Remove(string.Format("DataCache-Settings:{0}", settings.SiteUrl));

            }
            else
            {

                SaveMasterSettings(settings);

                HiCache.Remove("FileCache-MasterSettings");

            }

        }

       static void SaveMasterSettings(SiteSettings settings)
        {
            string masterSettingsFilename = GetMasterSettingsFilename();

            XmlDocument doc = new XmlDocument();

            if (File.Exists(masterSettingsFilename))
            {
                doc.Load(masterSettingsFilename);
            }

            settings.WriteToXml(doc);

            doc.Save(masterSettingsFilename);
        }

    }

}

