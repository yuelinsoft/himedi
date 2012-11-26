using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Hidistro.UI.Subsites.Utility
{
    /// <summary>
    /// 发送请求创建分销
    /// </summary>
    public class ToTaobaoBasePage : DistributorPage
    {
        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <returns></returns>
        protected string SendHttpRequest()
        {

            string responseStr;

            Distributor distributor = SubsiteStoreHelper.GetDistributor();

            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);

            WebRequest request = WebRequest.Create("http://saas.92hi.com/CreateDistributors.aspx");

            request.Method = "POST";

            request.ContentType = "application/x-www-form-urlencoded";

            string args = "Host=" + masterSettings.SiteUrl + "&DistributorUserId=" + distributor.UserId + "&Email=" + Page.Server.UrlEncode(distributor.Email) + "&RealName=" + Page.Server.UrlEncode(distributor.RealName) + "&CompanyName=" + Page.Server.UrlEncode(distributor.CompanyName) + "&Address=" + Page.Server.UrlEncode(distributor.Address) + "&TelPhone=" + Page.Server.UrlEncode(distributor.TelPhone) + "&QQ=" + Page.Server.UrlEncode(distributor.QQ) + "&Wangwang=" + Page.Server.UrlEncode(distributor.Wangwang);

            byte[] bytes = new ASCIIEncoding().GetBytes(args);

            request.ContentLength = bytes.Length;

            //写数据
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            //读数据
            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.Default))
            {
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }
    }
}

