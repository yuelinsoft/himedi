using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    /// <summary>
    /// 购物处理器
    /// </summary>
    public class ShoppingHandler : IHttpHandler
    {
        void ClearBrowsedProduct(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            BrowsedProductQueue.ClearQueue();
            context.Response.Write("{\"Status\":\"Succes\"}");
        }

        public void ProcessAddToCart(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int quantity = int.Parse(context.Request["quantity"], NumberStyles.None);
            string skuId = context.Request["productSkuId"];
            DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(skuId);
            if (((productInfoBySku == null) || (productInfoBySku.Rows.Count <= 0)) || (((int)productInfoBySku.Rows[0]["SaleStatus"]) == 0))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else if ((((int)productInfoBySku.Rows[0]["Stock"]) <= 0) || (((int)productInfoBySku.Rows[0]["Stock"]) < quantity))
            {
                context.Response.Write("{\"Status\":\"1\"}");
            }
            else
            {
                string skuContent = string.Empty;
                foreach (DataRow row in productInfoBySku.Rows)
                {
                    if (((row["AttributeName"] != DBNull.Value) && !string.IsNullOrEmpty((string)row["AttributeName"])) && ((row["ValueStr"] != DBNull.Value) && !string.IsNullOrEmpty((string)row["ValueStr"])))
                    {
                        object obj2 = skuContent;
                        skuContent = string.Concat(new object[] { obj2, row["AttributeName"], "：", row["ValueStr"], "; " });
                    }
                }
                ShoppingCartProcessor.AddLineItem((int)productInfoBySku.Rows[0]["ProductId"], skuId, skuContent, quantity);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                context.Response.Write(string.Concat(new object[] { "{\"Status\":\"OK\",\"TotalMoney\":\"", shoppingCart.GetTotal(), "\",\"Quantity\":\"", shoppingCart.Quantity.ToString(), "\"}" }));
            }
        }

        void ProcessAddToCartBySkus(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int quantity = int.Parse(context.Request["quantity"], NumberStyles.None);
            string skuId = context.Request["productSkuId"];
            DataTable productInfoBySku = ShoppingProcessor.GetProductInfoBySku(skuId);
            if ((productInfoBySku == null) || (productInfoBySku.Rows.Count <= 0))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else if ((((int)productInfoBySku.Rows[0]["Stock"]) <= 0) || (((int)productInfoBySku.Rows[0]["Stock"]) < quantity))
            {
                context.Response.Write("{\"Status\":\"1\"}");
            }
            else
            {
                string skuContent = string.Empty;
                foreach (DataRow row in productInfoBySku.Rows)
                {
                    if (((row["AttributeName"] != DBNull.Value) && !string.IsNullOrEmpty((string)row["AttributeName"])) && ((row["ValueStr"] != DBNull.Value) && !string.IsNullOrEmpty((string)row["ValueStr"])))
                    {
                        object obj2 = skuContent;
                        skuContent = string.Concat(new object[] { obj2, row["AttributeName"], "：", row["ValueStr"], "; " });
                    }
                }
                ShoppingCartProcessor.AddLineItem((int)productInfoBySku.Rows[0]["ProductId"], skuId, skuContent, quantity);
                ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
                context.Response.Write("{\"Status\":\"OK\",\"TotalMoney\":\"" + shoppingCart.GetTotal().ToString(".00") + "\",\"Quantity\":\"" + shoppingCart.Quantity.ToString() + "\"}");
            }
        }

        void ProcessGetSkuByOptions(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            int productId = int.Parse(context.Request["productId"], NumberStyles.None);
            string str = context.Request["options"];
            if (string.IsNullOrEmpty(str))
            {
                context.Response.Write("{\"Status\":\"0\"}");
            }
            else
            {
                if (str.EndsWith(","))
                {
                    str = str.Substring(0, str.Length - 1);
                }
                SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(productId, str);
                if (productAndSku == null)
                {
                    context.Response.Write("{\"Status\":\"1\"}");
                }
                else if (productAndSku.Stock <= 0)
                {
                    context.Response.Write("{\"Status\":\"3\"}");
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("{");
                    builder.Append("\"Status\":\"OK\",");
                    builder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
                    builder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
                    builder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight);
                    builder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
                    builder.AppendFormat("\"AlertStock\":\"{0}\",", productAndSku.AlertStock);
                    builder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.ToString("F2"));
                    builder.Append("}");
                    context.Response.ContentType = "application/json";
                    context.Response.Write(builder.ToString());
                }
            }
        }

        /// <summary>
        /// AJAX请求入口
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string action = context.Request["action"];
                switch (action)
                {
                    case "AddToCartBySkus":
                        {
                            ProcessAddToCartBySkus(context);
                            break;
                        }
                    case "GetSkuByOptions":
                        {
                            ProcessGetSkuByOptions(context);
                            break;
                        }
                    case "ClearBrowsed":
                        {
                            ClearBrowsedProduct(context);
                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write("{\"Status\":\"" + exception.Message.Replace("\"", "'") + "\"}");
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

