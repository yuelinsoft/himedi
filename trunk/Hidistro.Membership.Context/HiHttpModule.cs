
///
///页面请求管道
///

using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Core.Jobs;
using Hidistro.Core.Urls;
using System;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace Hidistro.Membership.Context
{

    public class HiHttpModule : IHttpModule
    {

        bool applicationInstalled = false;

        ApplicationType currentApplicationType = ApplicationType.Unknown;

        static readonly Regex urlReg = new Regex("(loginentry.aspx|login.aspx|logout.aspx|resourcenotfound.aspx|verifycodeimage.aspx|SendPayment.aspx|PaymentReturn_url|PaymentNotify_url|InpourReturn_url|InpourNotify_url)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);


        void Application_AuthorizeRequest(object source, EventArgs e)
        {
            if (currentApplicationType != ApplicationType.Installer)
            {

                HttpApplication application = (HttpApplication)source;

                HttpContext context = application.Context;

                HiContext current = HiContext.Current;

                if (context.Request.IsAuthenticated)
                {
                    string name = context.User.Identity.Name;

                    if (!string.IsNullOrEmpty(name))
                    {
                        string[] rolesForUser = Roles.GetRolesForUser(name);

                        if ((rolesForUser != null) && (rolesForUser.Length > 0))
                        {
                            current.RolesCacheKey = string.Join(",", rolesForUser);
                        }

                    }

                }

            }

        }

        /// <summary>
        /// 网站模块入口
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void Application_BeginRequest(object source, EventArgs e)
        {
            currentApplicationType = HiConfiguration.GetConfig().AppLocation.CurrentApplicationType;

            HttpApplication application = (HttpApplication)source;

            HttpContext context = application.Context;

            if (context.Request.RawUrl.IndexOfAny(new char[] { '<', '>', '\'', '"' }) != -1)
            {
                context.Response.Redirect(context.Request.RawUrl.Replace("<", "%3c").Replace(">", "%3e").Replace("'", "%27").Replace("\"", "%22"), false);
            }
            else
            {
                //检查是否已经安装了网站
                CheckInstall(context);

                if (currentApplicationType != ApplicationType.Installer)
                {
                    //后台域名验证
                    if (currentApplicationType == ApplicationType.Admin && string.Compare(Globals.DomainName, HiContext.Current.SiteSettings.SiteUrl, true) != 0)
                    {
                        context.Response.Redirect(Globals.GetSiteUrls().Home, true);
                    }
                    else
                    {

                        CheckSSL(HiConfiguration.GetConfig().SSL, context);

                        HiContext.Create(context, new UrlReWriterDelegate(HiHttpModule.ReWriteUrl));

                        if (HiContext.Current.SiteSettings.IsDistributorSettings)
                        {
                            if (!((!HiContext.Current.SiteSettings.Disabled || (currentApplicationType != ApplicationType.Common)) || urlReg.IsMatch(context.Request.Url.AbsolutePath)))
                            {
                                context.Response.Write("站点维护中，暂停访问！");
                                context.Response.End();
                            }
                            else if (currentApplicationType == ApplicationType.Admin)
                            {
                                context.Response.Redirect(Globals.GetSiteUrls().Home, false);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 是否已经安装
        /// </summary>
        /// <param name="context"></param>
        void CheckInstall(HttpContext context)
        {
            if ((currentApplicationType == ApplicationType.Installer) && applicationInstalled)
            {
                //如果已经安装这了，跳转到首页
                context.Response.Redirect(Globals.GetSiteUrls().Home, true);
            }
            else if (!(applicationInstalled || (currentApplicationType == ApplicationType.Installer)))
            {
                //如果没有安装，跳过安装向导,并终止当前请求
                context.Response.Redirect(Globals.ApplicationPath + "/installer/default.aspx", true);// false);
            }
        }

        /// <summary>
        /// 检查SSL
        /// </summary>
        /// <param name="ssl"></param>
        /// <param name="context"></param>
        static void CheckSSL(SSLSettings ssl, HttpContext context)
        {
            if (ssl == SSLSettings.All)
            {
                Globals.RedirectToSSL(context);
            }
        }


        #region IHttpModule接口实现

        /// <summary>
        /// 资源清理
        /// </summary>
        public void Dispose()
        {
            if (currentApplicationType != ApplicationType.Installer)
            {
                Hidistro.Core.Jobs.Jobs.Instance().Stop();
            }
        }

        /// <summary>
        /// 入口程序
        /// </summary>
        /// <param name="application"></param>
        public void Init(HttpApplication application)
        {
            if (null != application)
            {

                //关联处理事件
                application.BeginRequest += new EventHandler(Application_BeginRequest);
                application.AuthorizeRequest += new EventHandler(Application_AuthorizeRequest);

                applicationInstalled = ConfigurationManager.AppSettings["Installer"] == null;

                currentApplicationType = HiConfiguration.GetConfig().AppLocation.CurrentApplicationType;

                //检查是否安装
                CheckInstall(application.Context);

                if (currentApplicationType != ApplicationType.Installer)
                {
                    //启动工作线程
                    Hidistro.Core.Jobs.Jobs.Instance().Start();
                    ExtensionContainer.LoadExtensions();
                }
            }
        }

        #endregion

        //url重定向
        static bool ReWriteUrl(HttpContext context)
        {
            string path = context.Request.Path;

            string filePath = UrlReWriteProvider.Instance().RewriteUrl(path, context.Request.Url.Query);

            if (filePath != null)
            {
                string queryString = null;

                int index = filePath.IndexOf('?');

                if (index >= 0)
                {
                    queryString = (index < (filePath.Length - 1)) ? filePath.Substring(index + 1) : string.Empty;

                    filePath = filePath.Substring(0, index);

                }

                context.RewritePath(filePath, null, queryString);

            }

            return (filePath != null);

        }

        //模块名称
        public string ModuleName
        {
            get
            {
                return "HiHttpModule";
            }
        }
    }
}

