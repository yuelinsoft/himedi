using Hidistro.AccountCenter.Profile;
using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;

namespace Hidistro.UI.Web.OpenID
{
    public partial class LogisticsAddress_url : Page
    {
        //获取请求数据
        private SortedDictionary<string, string> GetRequestPost()
        {

            int index = 0;

            SortedDictionary<string, string> dictionary = new SortedDictionary<string, string>();

            string[] allKeys = Request.Form.AllKeys;

            for (index = 0; index < allKeys.Length; index++)
            {
                dictionary.Add(allKeys[index], Request.Form[allKeys[index]]);
            }

            return dictionary;

        }

        //加载事件
        protected void Page_Load(object sender, EventArgs e)
        {
            int num = 0;

            SortedDictionary<string, string> requestPost = GetRequestPost();

            if (requestPost.Count > 0)
            {

                string openIdType = "hishop.plugins.openid.alipay.alipayservice";

                OpenIdSettingsInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(openIdType);

                if (openIdSettings == null)
                {

                    Response.Write("登录失败，没有找到对应的插件配置信息。");

                    return;

                }

                XmlDocument document = new XmlDocument();

                document.LoadXml(Cryptographer.Decrypt(openIdSettings.Settings));

                AliPayNotify notify = new AliPayNotify(requestPost, Request.Form["notify_id"], document.FirstChild.SelectSingleNode("Partner").InnerText, document.FirstChild.SelectSingleNode("Key").InnerText);
                string responseTxt = notify.ResponseTxt;

                string sign = Request.Form["sign"];

                string mysign = notify.Mysign;

                if ((responseTxt == "true") && (sign == mysign))
                {
                    string receive_address = Request.Form["receive_address"];

                    if (!string.IsNullOrEmpty(receive_address))
                    {

                        XmlDocument xmlDoc = new XmlDocument();

                        xmlDoc.LoadXml(receive_address);

                        ShippingAddressInfo shippingAddrInfo = new ShippingAddressInfo();

                        shippingAddrInfo.UserId = HiContext.Current.User.UserId;

                        ShippingAddressInfo shippingAddress = shippingAddrInfo;

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/address") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/address").InnerText)))
                        {
                            shippingAddress.Address = Globals.HtmlEncode(xmlDoc.SelectSingleNode("/receiveAddress/address").InnerText);
                        }

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/fullname") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/fullname").InnerText)))
                        {
                            shippingAddress.ShipTo = Globals.HtmlEncode(xmlDoc.SelectSingleNode("/receiveAddress/fullname").InnerText);
                        }

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/post") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/post").InnerText)))
                        {
                            shippingAddress.Zipcode = xmlDoc.SelectSingleNode("/receiveAddress/post").InnerText;
                        }

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/mobile_phone") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/mobile_phone").InnerText)))
                        {
                            shippingAddress.CellPhone = xmlDoc.SelectSingleNode("/receiveAddress/mobile_phone").InnerText;
                        }

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/phone") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/phone").InnerText)))
                        {
                            shippingAddress.TelPhone = xmlDoc.SelectSingleNode("/receiveAddress/phone").InnerText;
                        }

                        string innerText = string.Empty;

                        string city = string.Empty;

                        string prov = string.Empty;

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/area") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/area").InnerText)))
                        {
                            innerText = xmlDoc.SelectSingleNode("/receiveAddress/area").InnerText;
                        }

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/city") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/city").InnerText)))
                        {
                            city = xmlDoc.SelectSingleNode("/receiveAddress/city").InnerText;
                        }

                        if (!((xmlDoc.SelectSingleNode("/receiveAddress/prov") == null) || string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/receiveAddress/prov").InnerText)))
                        {
                            prov = xmlDoc.SelectSingleNode("/receiveAddress/prov").InnerText;
                        }

                        if ((string.IsNullOrEmpty(innerText) && string.IsNullOrEmpty(city)) && string.IsNullOrEmpty(prov))
                        {
                            shippingAddress.RegionId = 0;
                        }
                        else
                        {
                            shippingAddress.RegionId = RegionHelper.GetRegionId(innerText, city, prov);
                        }

                        SiteSettings siteSettings = HiContext.Current.SiteSettings;

                        if (PersonalHelper.GetShippingAddressCount(HiContext.Current.User.UserId) < HiContext.Current.Config.ShippingAddressQuantity)
                        {
                            num = PersonalHelper.AddShippingAddress(shippingAddress);
                        }

                    }

                }

            }

            Page.Response.Redirect(Globals.ApplicationPath + "/SubmmitOrder.aspx?shippingId=" + num);

        }
    }
}

