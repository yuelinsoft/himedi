using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace Hidistro.UI.SaleSystem.CodeBehind
{

    /// <summary>
    /// 订单处理器
    /// </summary>
    public class SubmmitOrderHandler : IHttpHandler
    {
        /// <summary>
        /// 计算重量
        /// </summary>
        /// <param name="context"></param>
        void CalculateFreight(HttpContext context)
        {
            decimal freight = 0M;

            if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]) && !string.IsNullOrEmpty(context.Request["RegionId"]))
            {

                int modeId = int.Parse(context.Request["ModeId"], NumberStyles.None);

                int totalWeight = int.Parse(context.Request["Weight"], NumberStyles.None);

                int regionId = int.Parse(context.Request["RegionId"], NumberStyles.None);

                ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(modeId, true);

                freight = ShoppingProcessor.CalcFreight(regionId, totalWeight, shippingMode);

            }

            StringBuilder builder = new StringBuilder();

            builder.Append("{");
            builder.Append("\"Status\":\"OK\",");
            builder.AppendFormat("\"Price\":\"{0}\", \"Price_v\":\"{1}\"", Globals.FormatMoney(freight), freight);
            builder.Append("}");
            context.Response.ContentType = "text/plain";

            context.Response.Write(builder.ToString());
        }
        
        /// <summary>
        /// 格式化货币
        /// </summary>
        /// <param name="context"></param>
        void FormatMoney(HttpContext context)
        {
            decimal money;

            context.Response.ContentType = "plain/text";

            if (decimal.TryParse(context.Request["value"], out money))
            {
                decimal point = 0M;

                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);

                if ((money / masterSettings.PointsRate) > 2147483647M)
                {
                    point = 2147483647M;
                }
                else if (masterSettings.PointsRate != 0M)
                {
                    point = Math.Round((decimal)(money / masterSettings.PointsRate), 0);
                }
                else
                {
                    point = 0M;
                }

                StringBuilder builder = new StringBuilder();

                builder.Append("{");

                builder.AppendFormat("\"Money\":\"{0}\",", Globals.FormatMoney(money));

                builder.AppendFormat("\"Money_v\":\"{0}\",", money);

                builder.AppendFormat("\"Point\":\"{0}\"", point.ToString());

                builder.Append("}");



                context.Response.Write(builder);

            }
            else
            {
                context.Response.Write("\"错误的货币格式!\"");
            }
        }

        /// <summary>
        /// 获取用户区域
        /// </summary>
        /// <param name="context"></param>
        void GetUserRegionId(HttpContext context)
        {
            string prov = context.Request["Prov"];
            string city = context.Request["City"];
            string areas = context.Request["Areas"];

            StringBuilder builder = new StringBuilder();

            builder.Append("{");

            if ((!string.IsNullOrEmpty(prov) && !string.IsNullOrEmpty(city)) && !string.IsNullOrEmpty(areas))
            {
                builder.Append("\"Status\":\"OK\",\"RegionId\":\"" + RegionHelper.GetRegionId(areas, city, prov) + "\"}");
            }
            else
            {
                builder.Append("\"Status\":\"NOK\"}");
            }

            context.Response.ContentType = "text/plain";

            context.Response.Write(builder);

        }

        /// <summary>
        /// 获取用户配送地址
        /// </summary>
        /// <param name="context"></param>
        void GetUserShippingAddress(HttpContext context)
        {
            ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(int.Parse(context.Request["ShippingId"], NumberStyles.None));

            StringBuilder builder = new StringBuilder();

            builder.Append("{");
            if (shippingAddress != null)
            {
                builder.Append("\"Status\":\"OK\",");
                builder.AppendFormat("\"ShipTo\":\"{0}\",", Globals.HtmlDecode(shippingAddress.ShipTo));
                builder.AppendFormat("\"Address\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Address));
                builder.AppendFormat("\"Zipcode\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Zipcode));
                builder.AppendFormat("\"CellPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.CellPhone));
                builder.AppendFormat("\"TelPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.TelPhone));
                builder.AppendFormat("\"RegionId\":\"{0}\"", shippingAddress.RegionId);
            }
            else
            {
                builder.Append("\"Status\":\"0\"");
            }

            builder.Append("}");

            context.Response.ContentType = "text/plain";

            context.Response.Write(builder);

        }

        /// <summary>
        /// 订单选项
        /// </summary>
        /// <param name="context"></param>
        void ProcessorOrderOption(HttpContext context)
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(context.Request.Params["LookupItemId"]))
            {
                int lookupItemId = int.Parse(context.Request["LookupItemId"], NumberStyles.None);
                decimal num2 = decimal.Parse(context.Request["carTotal"]);
                builder.Append("{");
                builder.Append("\"Status\":\"OK\",");
                OrderLookupItemInfo orderLookupItem = ShoppingProcessor.GetOrderLookupItem(lookupItemId, string.Empty);
                if (orderLookupItem != null)
                {
                    builder.AppendFormat("\"Name\":\"{0}\",", orderLookupItem.Name);
                    decimal money = 0M;
                    if (orderLookupItem.AppendMoney.HasValue)
                    {
                        if (decimal.Parse(orderLookupItem.CalculateMode.ToString()) == 2M)
                        {
                            money = (num2 * orderLookupItem.AppendMoney.Value) / 100M;
                        }
                        else
                        {
                            money = orderLookupItem.AppendMoney.Value;
                        }
                    }
                    builder.AppendFormat("\"AppendMoney\":\"{0}\",", Globals.FormatMoney(money));
                    builder.AppendFormat("\"AppendMoney_v\":\"{0}\",", money);
                    builder.AppendFormat("\"IsUserInputRequired\":\"{0}\",", orderLookupItem.IsUserInputRequired ? "true" : "false");
                    if (string.IsNullOrEmpty(orderLookupItem.UserInputTitle))
                    {
                        builder.AppendFormat("\"UserInputTitle\":\"\",", new object[0]);
                    }
                    else
                    {
                        builder.AppendFormat("\"UserInputTitle\":\"" + orderLookupItem.UserInputTitle.Replace("\"", "").Replace(@"\/", "/") + "\",", new object[0]);
                    }
                    if (string.IsNullOrEmpty(orderLookupItem.UserInputContent))
                    {
                        builder.Append("\"UserInputContent\":\"\"");
                    }
                    else
                    {
                        builder.Append("\"UserInputContent\":\"" + orderLookupItem.UserInputContent.Replace("\"", "").Replace(@"\/", "/") + "\"");
                    }
                }
                builder.Append("}");
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(builder);
        }

        /// <summary>
        /// 支付方式
        /// </summary>
        /// <param name="context"></param>
        void ProcessorPaymentMode(HttpContext context)
        {
            decimal num = 0M;
            if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]))
            {
                int modeId = int.Parse(context.Request["ModeId"], NumberStyles.None);
                decimal cartMoney = decimal.Parse(context.Request["CartTotalPrice"]);
                num = ShoppingProcessor.GetPaymentMode(modeId).CalcPayCharge(cartMoney);
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            builder.Append("\"Status\":\"OK\",");
            builder.AppendFormat("\"Charge\":\"{0}\", \"Charge_v\":\"{1}\"", Globals.FormatMoney(num), num);
            builder.Append("}");
            context.Response.ContentType = "text/plain";
            context.Response.Write(builder.ToString());
        }

        /// <summary>
        /// 活动
        /// </summary>
        /// <param name="context"></param>
        void ProcessorUseCoupon(HttpContext context)
        {
            decimal orderAmount = decimal.Parse(context.Request["CartTotal"]);
            string claimCode = context.Request["CouponCode"];
            CouponInfo info = ShoppingProcessor.UseCoupon(orderAmount, claimCode);
            StringBuilder builder = new StringBuilder();
            if (info != null)
            {
                builder.Append("{");
                builder.Append("\"Status\":\"OK\",");
                builder.AppendFormat("\"CouponName\":\"{0}\",", info.Name);
                builder.AppendFormat("\"DiscountValue\":\"{0}\", \"DiscountValue_v\":\"{1}\"", Globals.FormatMoney(info.DiscountValue), info.DiscountValue);
                builder.Append("}");
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(builder);
        }

        /// <summary>
        /// 请求入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                switch (context.Request["Action"])
                {
                    case "GetUserShippingAddress":
                        {
                            GetUserShippingAddress(context);
                            break;
                        }
                    case "CalculateFreight":
                        {
                            CalculateFreight(context);
                            break;
                        }
                    case "ProcessorOrderOption":
                        {
                            ProcessorOrderOption(context);
                            break;
                        }
                    case "ProcessorPaymentMode":
                        {
                            ProcessorPaymentMode(context);
                            break;
                        }
                    case "ProcessorUseCoupon":
                        {
                            ProcessorUseCoupon(context);
                            break;
                        }
                    case "FormatMoney":
                        {
                            FormatMoney(context);
                            break;
                        }
                    case "GetRegionId":
                        {
                            GetUserRegionId(context);
                            break;
                        }
                }
            }
            catch
            {
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

