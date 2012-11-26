using Hidistro.Core;
using Hidistro.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Xml;

namespace Hidistro.Core.Configuration
{
    public class HiConfiguration
    {
        string adminFolder = "admin";
        Hidistro.Core.Configuration.AppLocation app;
        const string ConfigCacheKey = "FileCache-Configuragion";
        string emailEncoding = "utf-8";
        string emailRegex = @"([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]{2,3,4}){1,2})";
        const string filesPath = "/";
        readonly Dictionary<string, string> integratedApplications = new Dictionary<string, string>();
        int passwordMaxLength = 0x10;
        Hidistro.Core.Configuration.RolesConfiguration roleConfiguration;
        int shippingAddressQuantity = 5;
        short smtpServerConnectionLimit = -1;
        SSLSettings ssl = SSLSettings.Ignore;
        readonly Dictionary<string, string> supportedLanguages = new Dictionary<string, string>();
        public static readonly int[,] ThumbnailSizes = new int[,] { { 10, 10 }, { 22, 22 }, { 40, 40 }, { 100, 100 }, { 160, 160 }, { 310, 310 } };
        int usernameMaxLength = 20;
        int usernameMinLength = 3;
        string usernameRegex = "[一-龥a-zA-Z]+[一-龥_a-zA-Z0-9]*";
        bool useUniversalCode = false;
        readonly XmlDocument xmlDoc;

        public HiConfiguration(XmlDocument doc)
        {
            xmlDoc = doc;
            LoadValuesFromConfigurationXml();
        }

        internal void GetAppLocation(XmlNode node)
        {
            app = Hidistro.Core.Configuration.AppLocation.Create(node);
        }

        internal void GetAttributes(XmlAttributeCollection attributeCollection)
        {
            XmlAttribute attribute = attributeCollection["smtpServerConnectionLimit"];
            if (attribute != null)
            {
                smtpServerConnectionLimit = short.Parse(attribute.Value, CultureInfo.InvariantCulture);
            }
            else
            {
                smtpServerConnectionLimit = -1;
            }
            attribute = attributeCollection["ssl"];
            if (attribute != null)
            {
                ssl = (SSLSettings)Enum.Parse(typeof(SSLSettings), attribute.Value, true);
            }
            attribute = attributeCollection["usernameMinLength"];
            if (attribute != null)
            {
                usernameMinLength = int.Parse(attribute.Value);
            }
            attribute = attributeCollection["usernameMaxLength"];
            if (attribute != null)
            {
                usernameMaxLength = int.Parse(attribute.Value);
            }
            attribute = attributeCollection["usernameRegex"];
            if (attribute != null)
            {
                usernameRegex = attribute.Value;
            }
            attribute = attributeCollection["emailEncoding"];
            if (attribute != null)
            {
                emailEncoding = attribute.Value;
            }
            attribute = attributeCollection["shippingAddressQuantity"];
            if (attribute != null)
            {
                shippingAddressQuantity = int.Parse(attribute.Value);
            }
            attribute = attributeCollection["passwordMaxLength"];
            if (attribute != null)
            {
                passwordMaxLength = int.Parse(attribute.Value);
            }
            if (passwordMaxLength < Membership.Provider.MinRequiredPasswordLength)
            {
                passwordMaxLength = 0x10;
            }
            attribute = attributeCollection["emailRegex"];
            if (attribute != null)
            {
                emailRegex = attribute.Value;
            }
            attribute = attributeCollection["adminFolder"];
            if (attribute != null)
            {
                adminFolder = attribute.Value;
            }
            attribute = attributeCollection["useUniversalCode"];
            if ((attribute != null) && attribute.Value.Equals("true"))
            {
                useUniversalCode = true;
            }
        }

        public static HiConfiguration GetConfig()
        {
            HiConfiguration configuration = HiCache.Get("FileCache-Configuragion") as HiConfiguration;
            if (configuration == null)
            {
                string filename = null;
                HttpContext current = HttpContext.Current;
                if (current != null)
                {
                    filename = current.Request.MapPath("~/config/Hishop.config");
                }
                else
                {
                    filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config\Hishop.config");
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                configuration = new HiConfiguration(doc);
                HiCache.Max("FileCache-Configuragion", configuration, new CacheDependency(filename));
            }
            return configuration;
        }

        public XmlNode GetConfigSection(string nodePath)
        {
            return xmlDoc.SelectSingleNode(nodePath);
        }

        internal void GetIntegratedApplications(XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (!integratedApplications.ContainsKey(item.Attributes["applicationName"].Value))
                {
                    integratedApplications.Add(item.Attributes["applicationName"].Value, item.Attributes["implement"].Value);
                }
            }
        }

        internal void GetLanguages(XmlNode node)
        {
            foreach (XmlNode item in node.ChildNodes)
            {
                if (!((string.Compare(item.Attributes["enabled"].Value, "true", false, CultureInfo.InvariantCulture) != 0) || supportedLanguages.ContainsKey(item.Attributes["key"].Value)))
                {
                    supportedLanguages.Add(item.Attributes["key"].Value, item.Attributes["name"].Value);
                }
            }
        }

        internal void LoadValuesFromConfigurationXml()
        {
            XmlNode configSection = GetConfigSection("Hishop/Core");
            XmlAttributeCollection attributeCollection = configSection.Attributes;
            GetAttributes(attributeCollection);

            foreach (XmlNode node in configSection.ChildNodes)
            {
                if (node.Name == "Languages")
                {
                    GetLanguages(node);
                }
                if (node.Name == "appLocation")
                {
                    GetAppLocation(node);
                }
                if (node.Name == "IntegratedApplications")
                {
                    GetIntegratedApplications(node);
                }
            }
            if (app == null)
            {
                app = Hidistro.Core.Configuration.AppLocation.Default();
            }
            if (roleConfiguration == null)
            {
                roleConfiguration = new Hidistro.Core.Configuration.RolesConfiguration();
            }
        }

        public string AdminFolder
        {
            get
            {
                return adminFolder;
            }
        }

        public Hidistro.Core.Configuration.AppLocation AppLocation
        {
            get
            {
                return app;
            }
        }

        public string EmailEncoding
        {
            get
            {
                return emailEncoding;
            }
        }

        public string EmailRegex
        {
            get
            {
                return emailRegex;
            }
        }

        public string FilesPath
        {
            get
            {
                return "/";
            }
        }

        public Dictionary<string, string> IntegratedApplications
        {
            get
            {
                return integratedApplications;
            }
        }

        public int PasswordMaxLength
        {
            get
            {
                return passwordMaxLength;
            }
        }

        public int QueuedThreads
        {
            get
            {
                return 2;
            }
        }

        public Hidistro.Core.Configuration.RolesConfiguration RolesConfiguration
        {
            get
            {
                return roleConfiguration;
            }
        }

        public int ShippingAddressQuantity
        {
            get
            {
                return shippingAddressQuantity;
            }
        }

        public short SmtpServerConnectionLimit
        {
            get
            {
                return smtpServerConnectionLimit;
            }
        }

        public SSLSettings SSL
        {
            get
            {
                return ssl;
            }
        }

        public Dictionary<string, string> SupportedLanguages
        {
            get
            {
                return supportedLanguages;
            }
        }

        public int UsernameMaxLength
        {
            get
            {
                return usernameMaxLength;
            }
        }

        public int UsernameMinLength
        {
            get
            {
                return usernameMinLength;
            }
        }

        public string UsernameRegex
        {
            get
            {
                return usernameRegex;
            }
        }

        public bool UseUniversalCode
        {
            get
            {
                return useUniversalCode;
            }
        }
    }
}

