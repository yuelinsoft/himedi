using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;
using System.Xml;

namespace Hidistro.Membership.Context
{
    /// <summary>
    /// 授权检查帮助类
    /// </summary>
    public static class LicenseChecker
    {

        const string CacheCommercialKey = "FileCache_CommercialLicenser";

        public static void Check(out bool isValid, out bool expired, out int siteQty)
        {

            isValid = true;
            expired = false;
            siteQty = 100000;

            /*
            siteQty = 0;

            isValid = false;

            expired = true;

            HiContext current = HiContext.Current;

            XmlDocument document = HiCache.Get("FileCache_CommercialLicenser") as XmlDocument;

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

            if (document == null)
            {

                string path = (current.Context != null) ? current.Context.Request.MapPath("~/config/Certificates.cer") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\Certificates.cer");

                if (!File.Exists(path))
                {
                    return;
                }

                document = new XmlDocument();

                document.LoadXml(File.ReadAllText(path));

                HiCache.Max("FileCache_CommercialLicenser", document, new CacheDependency(path));

            }

            XmlNode node = document.DocumentElement.SelectSingleNode("//Host");
            XmlNode node2 = document.DocumentElement.SelectSingleNode("//LicenseDate");
            XmlNode node3 = document.DocumentElement.SelectSingleNode("//Expires");
            XmlNode node4 = document.DocumentElement.SelectSingleNode("//SiteQty");
            XmlNode node5 = document.DocumentElement.SelectSingleNode("//Signature");

            if (string.Compare(node.InnerText, masterSettings.SiteUrl, true, CultureInfo.InvariantCulture) == 0)
            {
                string s = string.Format(CultureInfo.InvariantCulture, "Host={0}&Expires={1}&SiteQty={2}&LicenseDate={3}", new object[] { masterSettings.SiteUrl, node3.InnerText, node4.InnerText, node2.InnerText });

                using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
                {
                    provider.FromXmlString(LicenseHelper.GetPublicKey());

                    RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(provider);

                    deformatter.SetHashAlgorithm("SHA1");

                    byte[] rgbSignature = Convert.FromBase64String(node5.InnerText);

                    byte[] rgbHash = new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(s));

                    isValid = deformatter.VerifySignature(rgbHash, rgbSignature);

                }

                expired = DateTime.Now > DateTime.Parse(node3.InnerText);

                if (!(!isValid ? true : expired))
                {

                    int.TryParse(node4.InnerText, out siteQty);

                }

            }
            */
        }

    }


}

