using Hidistro.Membership.Core;
using System;
using System.Text.RegularExpressions;
using System.Web;


namespace Hidistro.Membership.Context
{
    public class UserCookie : IUserCookie
    {
        readonly HttpContext context = null;
        readonly HiContext hiContext = HiContext.Current;

        public UserCookie(IUser user)
        {
            if (!((user == null) || user.IsAnonymous))
            {
                this.context = this.hiContext.Context;
            }
        }

        public void DeleteCookie(HttpCookie cookie)
        {
            if ((cookie != null) && (this.context != null))
            {
                this.SetCookieDomain(cookie);
                cookie.Expires = new DateTime(0x777, 10, 12);
                this.context.Response.Cookies.Add(cookie);
            }
        }

        void SetCookieDomain(HttpCookie cookie)
        {
            Regex regex = new Regex(@"[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (regex.IsMatch(this.context.Request.Url.Host) && this.context.Request.Url.Host.ToLower().EndsWith(this.hiContext.SiteSettings.SiteUrl.ToLower()))
            {
                cookie.Path = "/";
                cookie.Domain = this.hiContext.SiteSettings.SiteUrl;
            }
        }

        public void WriteCookie(HttpCookie cookie, int days, bool autoLogin)
        {
            if ((cookie != null) && (this.context != null))
            {
                this.SetCookieDomain(cookie);
                if (autoLogin)
                {
                    cookie.Expires = DateTime.Now.AddDays((double)days);
                }
                this.context.Response.Cookies.Add(cookie);
            }
        }
    }
}

