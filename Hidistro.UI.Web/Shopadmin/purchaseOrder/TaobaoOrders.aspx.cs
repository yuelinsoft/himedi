using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Cryptography;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 淘宝订单同步
    /// </summary>
    public partial class TaobaoOrders : ToTaobaoBasePage
    {
        string buyerNick;
        ITopClient client;
        HttpCookie cookie;
        DateTime? endDate;
        DateTime? startDate;

        private void btnSearch_Click(object sender, EventArgs e)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("buyerNick", Globals.UrlEncode(txtBuyerName.Text.Trim()));
            if (calendarStartDate.SelectedDate.HasValue)
            {
                queryStrings.Add("startDate", calendarStartDate.SelectedDate.Value.ToString());
            }
            if (calendarEndDate.SelectedDate.HasValue)
            {
                queryStrings.Add("endDate", calendarEndDate.SelectedDate.Value.ToString());
            }
            base.ReloadPage(queryStrings);
        }

        private string GetShippingType(string shippingType)
        {
            string shippingTypeName = string.Empty;

            switch (shippingType)
            {
                case "free":
                    {
                        shippingTypeName = "卖家承担运费";
                        break;
                    }

                case "ems":
                    {
                        shippingTypeName = "EMS";
                        break;
                    }

                case "post":
                    {
                        shippingTypeName = "平邮";
                        break;
                    }
                case "express":
                    {
                        shippingTypeName = "快递";
                        break;
                    }
                case "virtual":
                    {
                        shippingTypeName = "虚拟交易";
                        break;
                    }
            }
            return shippingTypeName;

        }

        private string GetTradeStatus(string status)
        {
            string statusText = "";
            switch (status)
            {
                case "WAIT_BUYER_PAY":
                    {
                        statusText = "等待买家付款";
                        break;
                    }
                case "WAIT_SELLER_SEND_GOODS":
                    {
                        statusText = "已付款";
                        break;
                    }
                case "WAIT_BUYER_CONFIRM_GOODS":
                    {
                        statusText = "等待买家确认收货";
                        break;
                    }
                case "TRADE_BUYER_SIGNED":
                    {
                        statusText = "货到付款，买家已签收";
                        break;
                    }
                case "TRADE_FINISHED":
                    {
                        statusText = "交易成功";
                        break;
                    }
                case "TRADE_NO_CREATE_PAY":
                    {
                        statusText = "非支付宝交易";
                        break;
                    }
                case "TRADE_CLOSED":
                    {
                        statusText = "交易已被关闭";
                        break;
                    }
                case "TRADE_CLOSED_BY_TAOBAO":
                    {
                        statusText = "交易被淘宝关闭";
                        break;
                    }
            }
            return statusText;
        }

        private void LoadParameters()
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["buyerNick"]))
                {
                    buyerNick = Globals.UrlDecode(Page.Request.QueryString["buyerNick"]);
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["startDate"]))
                {
                    startDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["startDate"]));
                }
                if (!string.IsNullOrEmpty(Page.Request.QueryString["endDate"]))
                {
                    endDate = new DateTime?(DateTime.Parse(Page.Request.QueryString["endDate"]));
                }
                txtBuyerName.Text = buyerNick;
                calendarStartDate.SelectedDate = startDate;
                calendarEndDate.SelectedDate = endDate;
            }
            else
            {
                buyerNick = txtBuyerName.Text;
                startDate = calendarStartDate.SelectedDate;
                endDate = calendarEndDate.SelectedDate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            cookie = HiContext.Current.Context.Request.Cookies["TopSession_" + HiContext.Current.User.UserId.ToString()];

            //if (!Page.IsPostBack && ((cookie == null) || string.IsNullOrEmpty(cookie.Value)))
            //{
            //    string str = "0";
            //    try
            //    {
            //        str = base.SendHttpRequest();
            //    }
            //    catch (Exception)
            //    {
            //        content1.Visible = false;
            //        content12.Visible = true;
            //        txtMsg.Text = "SAAS分销平台请求失败，可能是网络原因，请刷新重试";
            //        return;
            //    }
            //    string str5 = str;
            //    if (str5 != null)
            //    {
            //        if (!(str5 == "-1"))
            //        {
            //            if (str5 == "-2")
            //            {
            //                content1.Visible = false;
            //                content12.Visible = true;
            //                txtMsg.Text = "更新分销商信息出错";
            //                return;
            //            }
            //            if (str5 == "-3")
            //            {
            //                content1.Visible = false;
            //                content12.Visible = true;
            //                txtMsg.Text = "添加分销商记录出错";
            //                return;
            //            }
            //            if (str5 == "-99")
            //            {
            //                content1.Visible = false;
            //                content12.Visible = true;
            //                txtMsg.Text = "未知错误";
            //                return;
            //            }
            //            if (str5 == "0")
            //            {
            //                content1.Visible = false;
            //                content12.Visible = true;
            //                txtMsg.Text = "您提交的参数有误";
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            content1.Visible = false;
            //            content12.Visible = true;
            //            txtMsg.Text = "您的供货商(即主站管理员)并没有注册开通同步淘宝";
            //            return;
            //        }
            //    }
            //}
            LoadParameters();
            shippingModeDropDownList.DataBind();
            btnSearch.Click += new EventHandler(btnSearch_Click);
            //SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            SiteSettings masterSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                if (null != masterSettings)
                {
                    if (!(string.IsNullOrEmpty(masterSettings.Topkey) || string.IsNullOrEmpty(masterSettings.TopSecret)))
                    {
                        HttpCookie TaobaoSessionReturnUrlCookies = new HttpCookie("TaobaoSessionReturnUrl_" + HiContext.Current.User.UserId.ToString());
                        TaobaoSessionReturnUrlCookies.Value = HttpContext.Current.Request.Url.AbsoluteUri;
                        HttpContext.Current.Response.Cookies.Add(TaobaoSessionReturnUrlCookies);
                        Response.Redirect("http://container.api.taobao.com/container?appkey=" + Cryptographer.Decrypt(masterSettings.Topkey), true);
                    }
                }
                else
                {
                    this.ShowMsg("请先申请开店铺，设置淘宝同步信息！", false);
                }
            }
            else
            {
                string serverUrl = "http://gw.api.taobao.com/router/rest";
                string appKey = Cryptographer.Decrypt(masterSettings.Topkey);
                string appSecret = Cryptographer.Decrypt(masterSettings.TopSecret);
                client = new DefaultTopClient(serverUrl, appKey, appSecret, "json");
                TradesSoldGetRequest request2 = new TradesSoldGetRequest();
                request2.Fields = "tid,created,buyer_nick,receiver_name,price,num,payment,shipping_type,post_fee,status,seller_rate,orders";
                TradesSoldGetRequest request = request2;
                if (startDate.HasValue)
                {
                    request.StartCreated = new DateTime?(startDate.Value);
                }
                if (endDate.HasValue)
                {
                    request.EndCreated = new DateTime?(endDate.Value);
                }
                request.Status = "WAIT_SELLER_SEND_GOODS";
                if (!string.IsNullOrEmpty(buyerNick))
                {
                    request.BuyerNick = buyerNick;
                }
                request.PageNo = new long?((long)pager.PageIndex);
                request.PageSize = 40;
                TradesSoldGetResponse response = client.Execute<TradesSoldGetResponse>(request, cookie.Value);
                if (!response.IsError)
                {
                    List<Trade> trades = response.Trades;
                    if (trades != null)
                    {
                        foreach (Trade trade in trades)
                        {
                            trade.ShippingType = GetShippingType(trade.ShippingType);
                            trade.Status = GetTradeStatus(trade.Status);
                            foreach (Order order in trade.Orders)
                            {
                                if (!string.IsNullOrEmpty(order.OuterSkuId))
                                {
                                    order.OuterSkuId = "商家编码：" + order.OuterSkuId;
                                }
                            }
                        }
                        rptTrades.DataSource = trades;
                        rptTrades.DataBind();
                        pager.TotalRecords = int.Parse(response.TotalResults.ToString());
                        pager1.TotalRecords = int.Parse(response.TotalResults.ToString());
                    }
                }
            }
        }
    }
}

