using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;

namespace Hidistro.Membership.Context
{
    /// <summary>
    /// 密封对象HiContext
    /// </summary>
    public sealed class HiContext
    {

        HiConfiguration _config = null;

        IUser _currentUser = null;

        string _hostPath = "";

        HttpContext _httpContext;

        bool _isUrlReWritten = false;

        NameValueCollection _queryString = null;

        SiteSettings currentSettings;

        const string dataKey = "Hishop_ContextStore";

        string rolesCacheKey = "";

        string verifyCodeKey = "VerifyCode";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        HiContext(HttpContext context)
        {

            _httpContext = context;

            Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url, context.Request.RawUrl, GetSiteUrl());

        }

        /// <summary>
        /// 检查检验码
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public bool CheckVerifyCode(string verifyCode)
        {
            bool flag = false;

            if (Config.UseUniversalCode && verifyCode.Equals("8888"))
            {
                RemoveVerifyCookie();
                flag = true;
            }
            else
            {
                flag = string.Compare(HttpContext.Current.Request.Cookies[verifyCodeKey].Value, verifyCode, false, CultureInfo.InvariantCulture) == 0;
                RemoveVerifyCookie();
            }

            return flag;

        }

        public static HiContext Create(HttpContext context)
        {
            return Create(context, false);
        }

        public static HiContext Create(HttpContext context, UrlReWriterDelegate rewriter)
        {
            HiContext hiContext = new HiContext(context);

            SaveContextToStore(hiContext);

            if (null != rewriter)
            {
                hiContext.IsUrlReWritten = rewriter(context);
            }

            return hiContext;

        }

        public static HiContext Create(HttpContext context, bool isReWritten)
        {

            HiContext hiContext = new HiContext(context);

            hiContext.IsUrlReWritten = isReWritten;

            SaveContextToStore(hiContext);

            return hiContext;

        }

        public string CreateVerifyCode(int length)
        {
            string str = string.Empty;

            Random random = new Random();

            while (str.Length < length)
            {
                char ch;
                int num = random.Next();
                if ((num % 3) == 0)
                {
                    ch = (char)(0x61 + ((ushort)(num % 0x1a)));
                }
                else
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                if (((((ch != '0') && (ch != 'o')) && ((ch != '1') && (ch != '7'))) && ((ch != 'l') && (ch != '9'))) && (ch != 'g'))
                {
                    str = str + ch.ToString();
                }
            }

            RemoveVerifyCookie();

            HttpCookie cookie = new HttpCookie(verifyCodeKey);

            cookie.Value = str;

            HttpContext.Current.Response.Cookies.Add(cookie);

            return cookie.Value;
        }

        /// <summary>
        /// 获取站点URL
        /// </summary>
        /// <returns></returns>
        string GetSiteUrl()
        {
            return _httpContext.Request.Url.Host;
        }

        public string GetSkinPath()
        {

            if (SiteSettings.IsDistributorSettings)
            {
                return (Globals.ApplicationPath + "/Templates/sites/" + SiteSettings.UserId.Value.ToString(CultureInfo.InvariantCulture) + "/" + SiteSettings.Theme).ToLower(CultureInfo.InvariantCulture);
            }

            return (Globals.ApplicationPath + "/Templates/master/" + SiteSettings.Theme).ToLower(CultureInfo.InvariantCulture);

        }

        public string GetStoragePath()
        {

            if (SiteSettings.IsDistributorSettings)
            {
                return ("/Storage/sites/" + SiteSettings.UserId.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (Current.User.UserRole == UserRole.Distributor)
            {
                return ("/Storage/sites/" + Current.User.UserId.ToString(CultureInfo.InvariantCulture));
            }

            if (Current.ApplicationType == Hidistro.Core.Enums.ApplicationType.Distributor)
            {
                return ("/Storage/sites/" + Current.User.UserId.ToString(CultureInfo.InvariantCulture));
            }

            return "/Storage/master";

        }

        void Initialize(NameValueCollection qs, Uri uri, string rawUrl, string siteUrl)
        {

            _queryString = qs;

            _siteUrl = siteUrl.ToLower();

            if (((_queryString != null) && (_queryString.Count > 0)) && !string.IsNullOrEmpty(_queryString["ReferralUserId"]))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];

                if (cookie == null)
                {
                    cookie = new HttpCookie("Site_ReferralUser");
                }

                cookie.Value = _queryString["ReferralUserId"];

                HttpContext.Current.Response.Cookies.Add(cookie);


            }

        }

        void RemoveVerifyCookie()
        {

            HttpContext.Current.Response.Cookies[verifyCodeKey].Value = null;

            HttpContext.Current.Response.Cookies[verifyCodeKey].Expires = new DateTime(0x777, 10, 12);

        }


        static void SaveContextToStore(HiContext context)
        {
            context.Context.Items["Hishop_ContextStore"] = context;
        }

        public Hidistro.Core.Enums.ApplicationType ApplicationType
        {
            get
            {
                return Config.AppLocation.CurrentApplicationType;
            }
        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public HiConfiguration Config
        {
            get
            {
                if (_config == null)
                {
                    _config = HiConfiguration.GetConfig();
                }
                return _config;
            }
        }

        /// <summary>
        /// Context对象
        /// </summary>
        public HttpContext Context
        {
            get
            {
                return _httpContext;
            }
        }

        public static HiContext Current
        {
            get
            {

                HttpContext current = HttpContext.Current;

                HiContext context = current.Items["Hishop_ContextStore"] as HiContext;

                if (context == null)
                {

                    if (current == null)
                    {
                        throw new Exception("No HiContext exists in the Current Application. AutoCreate fails since HttpContext.Current is not accessible.");
                    }

                    context = new HiContext(current);

                    SaveContextToStore(context);

                }

                return context;

            }

        }

        /// <summary>
        /// 主机路径
        /// </summary>
        public string HostPath
        {
            get
            {

                if (_hostPath == null)
                {

                    Uri url = Context.Request.Url;

                    string str = (url.Port == 80) ? string.Empty : (":" + url.Port.ToString(CultureInfo.InvariantCulture));

                    _hostPath = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[] { url.Scheme, url.Host, str });

                }

                return _hostPath;

            }

        }

        /// <summary>
        /// 是否URL重写
        /// </summary>
        public bool IsUrlReWritten
        {
            get
            {
                return _isUrlReWritten;
            }
            set
            {
                _isUrlReWritten = value;
            }
        }


        public int ReferralUserId
        {
            get
            {
                int referralUserId = 0;

                if (string.Compare(Globals.DomainName, Current.SiteSettings.SiteUrl, true) == 0)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];

                    if (null != cookie && !string.IsNullOrEmpty(cookie.Value))
                    {
                        int.TryParse(cookie.Value, out referralUserId);
                    }

                }

                return referralUserId;


                /*
                if (string.Compare(Globals.DomainName, Current.SiteSettings.SiteUrl, true, CultureInfo.InvariantCulture) != 0)
                {
                    return 0;
                }

                HttpCookie cookie = HttpContext.Current.Request.Cookies["Site_ReferralUser"];

                if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
                {
                    return 0;
                }

                int result = 0;

                int.TryParse(cookie.Value, out result);

                return result;*/

            }
        }

        /// <summary>
        /// 角色缓存KEY
        /// </summary>
        public string RolesCacheKey
        {
            get
            {
                return rolesCacheKey;
            }
            set
            {
                rolesCacheKey = value;
            }
        }

        /// <summary>
        /// 站点设置
        /// </summary>
        public Hidistro.Membership.Context.SiteSettings SiteSettings
        {
            get
            {

                if (null == currentSettings)
                {
                    currentSettings = SettingsManager.GetSiteSettings();
                }

                return currentSettings;

            }
        }

        /// <summary>
        /// 站点URL
        /// </summary>
        string _siteUrl = "";
        public string SiteUrl
        {
            get
            {
                return _siteUrl;
            }
        }

        public IUser User
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = Users.GetUser();
                }
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }
    }
}

