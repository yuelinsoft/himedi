using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.API
{
    //制作淘宝数据包
    public partial class MakeTaobaoProductData : Page
    {

        int productId;

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取产品ID号
            int.TryParse(base.Request.QueryString["productId"], out productId);

            //获取记录集
            DataSet taobaoProductDetails = ProductHelper.GetTaobaoProductDetails(productId);

            //第一个表
            DataTable table = taobaoProductDetails.Tables[0];

            SortedDictionary<string, string> dicArrayPre = new SortedDictionary<string, string>();

            dicArrayPre.Add("SiteUrl", "demo2.shopefx.com");//"shopefx.china220.92hidc.net");//"localhost");//"www.92hi.com");//"shopefx.china220.92hidc.net");// HiContext.Current.SiteUrl);localhost//原来是这里在作怪
            dicArrayPre.Add("_input_charset", "utf-8");
            dicArrayPre.Add("return_url", Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("MakeTaobaoProductData_url")));
            dicArrayPre.Add("ProductId", productId.ToString());
            dicArrayPre.Add("HasSKU", table.Rows[0]["HasSKU"].ToString());
            dicArrayPre.Add("ProductName", (string)table.Rows[0]["ProductName"]);
            dicArrayPre.Add("ProductCode", (string)table.Rows[0]["ProductCode"]);
            dicArrayPre.Add("CategoryName", (table.Rows[0]["CategoryName"] == DBNull.Value) ? "" : ((string)table.Rows[0]["CategoryName"]));
            dicArrayPre.Add("ProductLineName", (table.Rows[0]["ProductLineName"] == DBNull.Value) ? "" : ((string)table.Rows[0]["ProductLineName"]));
            dicArrayPre.Add("BrandName", (table.Rows[0]["BrandName"] == DBNull.Value) ? "" : ((string)table.Rows[0]["BrandName"]));
            dicArrayPre.Add("SalePrice", Convert.ToDecimal(table.Rows[0]["SalePrice"]).ToString("F2"));
            dicArrayPre.Add("MarketPrice", (table.Rows[0]["MarketPrice"] == DBNull.Value) ? "0" : Convert.ToDecimal(table.Rows[0]["MarketPrice"]).ToString("F2"));
            dicArrayPre.Add("CostPrice", Convert.ToDecimal(table.Rows[0]["CostPrice"]).ToString("F2"));
            dicArrayPre.Add("PurchasePrice", Convert.ToDecimal(table.Rows[0]["PurchasePrice"]).ToString("F2"));
            dicArrayPre.Add("Stock", table.Rows[0]["Stock"].ToString());

            //第二个表
            DataTable taobaoProductTB1 = taobaoProductDetails.Tables[1];
            if (taobaoProductTB1.Rows.Count > 0)
            {
                string str = string.Empty;

                foreach (DataRow row in taobaoProductTB1.Rows)
                {
                    //object obj2 = str;
                    str = string.Concat(new object[] { str, row["AttributeName"], ":", row["ValueStr"], ";" });//(new object[] { obj2, row["AttributeName"], ":", row["ValueStr"], ";" });
                }

                str = str.Remove(str.Length - 1);

                dicArrayPre.Add("Attributes", str);
            }

            //第三个表
            DataTable taobaoProductTB2 = taobaoProductDetails.Tables[2];
            if (taobaoProductTB2.Rows.Count > 0)
            {

                StringBuilder builder = new StringBuilder();

                StringBuilder builder2 = new StringBuilder();

                for (int i = taobaoProductTB2.Columns.Count - 1; i >= 0; i--)
                {
                    builder2.Append(taobaoProductTB2.Columns[i].ColumnName).Append(";");
                }

                foreach (DataRow row2 in taobaoProductTB2.Rows)
                {
                    for (int j = taobaoProductTB2.Columns.Count - 1; j >= 0; j--)
                    {
                        builder.Append(row2[taobaoProductTB2.Columns[j].ColumnName]).Append(";");
                    }

                    builder.Remove(builder.Length - 1, 1).Append(",");
                }

                builder2.Remove(builder2.Length - 1, 1).Append(",").Append(builder.Remove(builder.Length - 1, 1));

                dicArrayPre.Add("skus", builder2.ToString());

            }

            //第四个表
            DataTable taobaoProductTB3 = taobaoProductDetails.Tables[3];

            if (taobaoProductTB3.Rows.Count > 0)
            {

                dicArrayPre.Add("Cid", taobaoProductTB3.Rows[0]["Cid"].ToString());

                if (taobaoProductTB3.Rows[0]["StuffStatus"] != DBNull.Value)
                {
                    dicArrayPre.Add("StuffStatus", (string)taobaoProductTB3.Rows[0]["StuffStatus"]);
                }

                dicArrayPre.Add("ProTitle", (string)taobaoProductTB3.Rows[0]["ProTitle"]);
                dicArrayPre.Add("Num", taobaoProductTB3.Rows[0]["Num"].ToString());
                dicArrayPre.Add("LocationState", (string)taobaoProductTB3.Rows[0]["LocationState"]);
                dicArrayPre.Add("LocationCity", (string)taobaoProductTB3.Rows[0]["LocationCity"]);
                dicArrayPre.Add("FreightPayer", (string)taobaoProductTB3.Rows[0]["FreightPayer"]);

                if (taobaoProductTB3.Rows[0]["PostFee"] != DBNull.Value)
                {
                    dicArrayPre.Add("PostFee", taobaoProductTB3.Rows[0]["PostFee"].ToString());
                }
                if (taobaoProductTB3.Rows[0]["ExpressFee"] != DBNull.Value)
                {
                    dicArrayPre.Add("ExpressFee", taobaoProductTB3.Rows[0]["ExpressFee"].ToString());
                }
                if (taobaoProductTB3.Rows[0]["EMSFee"] != DBNull.Value)
                {
                    dicArrayPre.Add("EMSFee", taobaoProductTB3.Rows[0]["EMSFee"].ToString());
                }

                dicArrayPre.Add("HasInvoice", taobaoProductTB3.Rows[0]["HasInvoice"].ToString());

                dicArrayPre.Add("HasWarranty", taobaoProductTB3.Rows[0]["HasWarranty"].ToString());

                dicArrayPre.Add("HasDiscount", taobaoProductTB3.Rows[0]["HasDiscount"].ToString());

                if (taobaoProductTB3.Rows[0]["PropertyAlias"] != DBNull.Value)
                {
                    dicArrayPre.Add("PropertyAlias", (string)taobaoProductTB3.Rows[0]["PropertyAlias"]);
                }
                if (taobaoProductTB3.Rows[0]["SkuProperties"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuProperties", (string)taobaoProductTB3.Rows[0]["SkuProperties"]);
                }
                if (taobaoProductTB3.Rows[0]["SkuQuantities"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuQuantities", (string)taobaoProductTB3.Rows[0]["SkuQuantities"]);
                }
                if (taobaoProductTB3.Rows[0]["SkuPrices"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuPrices", (string)taobaoProductTB3.Rows[0]["SkuPrices"]);
                }
                if (taobaoProductTB3.Rows[0]["SkuOuterIds"] != DBNull.Value)
                {
                    dicArrayPre.Add("SkuOuterIds", (string)taobaoProductTB3.Rows[0]["SkuOuterIds"]);
                }
                if (taobaoProductTB3.Rows[0]["inputpids"] != DBNull.Value)
                {
                    dicArrayPre.Add("inputpids", (string)taobaoProductTB3.Rows[0]["inputpids"]);
                }
                if (taobaoProductTB3.Rows[0]["inputstr"] != DBNull.Value)
                {
                    dicArrayPre.Add("inputstr", (string)taobaoProductTB3.Rows[0]["inputstr"]);
                }
            }

            Dictionary<string, string> dictionary2 = OpenIdFunction.FilterPara(dicArrayPre);

            StringBuilder builder3 = new StringBuilder();

            foreach (KeyValuePair<string, string> pair in dictionary2)
            {
                builder3.Append(OpenIdFunction.CreateField(pair.Key, pair.Value));
            }

            dicArrayPre.Clear();

            dictionary2.Clear();

            string action = "http://saas.92hi.com/MakeTaoBaoData.aspx";

            if (taobaoProductTB3.Rows.Count > 0)
            {
                action = "http://saas.92hi.com/UpdateTaoBaoData.aspx";
            }

            OpenIdFunction.Submit(OpenIdFunction.CreateForm(builder3.ToString(), action));

        }
    }
}

