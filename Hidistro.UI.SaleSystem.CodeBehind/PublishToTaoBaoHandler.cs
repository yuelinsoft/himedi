using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Subsites.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
    public class PublishToTaoBaoHandler : IHttpHandler
    {
        ITopClient client;
        const string Url = "http://gw.api.taobao.com/router/rest";

        public bool CheckDecimal(string decmalvalue)
        {
            Regex regex = new Regex("^(0|[1-9][0-9]*)$");
            return regex.IsMatch(decmalvalue);
        }

        public bool CheckInt(string intvalue)
        {
            Regex regex = new Regex(@"^\+?[1-9][0-9]*$");
            return regex.IsMatch(intvalue);
        }

       List<TbImage> GetProductsImgs(DataRow row, ItemAddResponse response)
        {
            List<TbImage> list = new List<TbImage>();
            if (row["ImageUrl2"] != DBNull.Value)
            {
                TbImage item = new TbImage();
                item.TbProductId = response.Item.NumIid;
                item.Imgpath = (string)row["ImageUrl2"];
                list.Add(item);
            }
            if (row["ImageUrl3"] != DBNull.Value)
            {
                TbImage image2 = new TbImage();
                image2.TbProductId = response.Item.NumIid;
                image2.Imgpath = (string)row["ImageUrl3"];
                list.Add(image2);
            }
            if (row["ImageUrl4"] != DBNull.Value)
            {
                TbImage image3 = new TbImage();
                image3.TbProductId = response.Item.NumIid;
                image3.Imgpath = (string)row["ImageUrl4"];
                list.Add(image3);
            }
            if (row["ImageUrl5"] != DBNull.Value)
            {
                TbImage image4 = new TbImage();
                image4.TbProductId = response.Item.NumIid;
                image4.Imgpath = (string)row["ImageUrl5"];
                list.Add(image4);
            }
            return list;
        }

        public void ProcessRequest(HttpContext context)
        {
            string appkey = context.Request.Form["appkey"];
            string appsecret = context.Request.Form["appsecret"];
            string sessionkey = context.Request.Form["sessionkey"];
            string productIds = context.Request.Form["productIds"];
            string approve_status = context.Request.Form["approve_status"];
            string morepic = context.Request.Form["morepic"];
            string repub = context.Request.Form["repub"];
            string chkdesc = context.Request.Form["chkdesc"];
            string chknormal = context.Request.Form["chknormal"];
            string chktitle = context.Request.Form["chktitle"];

            if (!string.IsNullOrEmpty(appkey) && !string.IsNullOrEmpty(appsecret))
            {
                this.client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", appkey, appsecret, "json");
            }

            DataTable taobaoProducts = SubSiteProducthelper.GetTaobaoProducts(productIds);

            if (null != taobaoProducts && taobaoProducts.Rows.Count > 0)
            {
                Dictionary<int, long> taobaoReturnProductIds = new Dictionary<int, long>();

                StringBuilder builder = new StringBuilder();
                string pname = "";
                int num = 0;
                string imgurl = "";
                int stock = 0;
                decimal markprice = 0M;
                string issuccess = "true";
                string msg = "";
                string imgmsg = "";
                string proTitle = "";// (string)row["ProTitle"];

                foreach (DataRow row in taobaoProducts.Rows)
                {
                    proTitle = (string)row["ProTitle"];

                    ResponseData(row, out imgurl, out stock, out markprice);

                    if ((row["taobaoproductid"] != DBNull.Value) && (repub.ToLower() == "true"))
                    {
                        ItemUpdateRequest req = new ItemUpdateRequest();

                        req.NumIid = new long?(Convert.ToInt64(row["taobaoproductid"]));

                        req.ApproveStatus = approve_status;

                        if (!string.IsNullOrEmpty(chknormal) && (chknormal.ToLower() == "true"))
                        {
                            this.SetTaoBaoUpdateData(req, row);
                        }

                        if (!string.IsNullOrEmpty(chktitle) && (chktitle.ToLower() == "true"))
                        {
                            req.Title = (row["ProTitle"] == DBNull.Value) ? "请修改商品标题" : ((string)row["ProTitle"]);
                        }

                        if (!string.IsNullOrEmpty(chkdesc) && (chkdesc.ToLower() == "true"))
                        {
                            req.Desc = (row["Description"] == DBNull.Value) ? "暂无该商品的描述信息" : ((string)row["Description"]);
                        }

                        ItemUpdateResponse response = client.Execute<ItemUpdateResponse>(req, sessionkey);

                        if (response.IsError)
                        {

                            num = (int)row["ProductId"];

                            pname = string.Format("<a href='{0}' target=_blank>{1} </a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { num }), proTitle);
                            
                            imgurl = string.Format("<a href='{0}' target=_blank><img src={1} /></a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { num }), imgurl);
                            
                            msg = string.Format("商品更新失败<br/>({0})", "：" + response.ErrMsg + "：" + response.SubErrMsg);

                            issuccess = "false";

                        }
                        else
                        {
                            imgurl = string.Format("<a href='http://item.taobao.com/item.htm?id={0}' target=_blank><img src={1} /></a>", response.Item.NumIid, imgurl);

                            pname = string.Format("<a href='http://item.taobao.com/item.htm?id={0}'>{1}</a>", response.Item.NumIid, proTitle);

                            msg = "商品更新成功";

                            issuccess = "true";

                            taobaoReturnProductIds.Add((int)row["ProductId"], response.Item.NumIid);

                        }
                    }
                    else
                    {
                        ItemAddRequest req = new ItemAddRequest();

                        req.ApproveStatus = approve_status;

                        SetTaoBaoAddData(req, row);

                        ItemAddResponse response2 = client.Execute<ItemAddResponse>(req, sessionkey);

                        if (response2.IsError)
                        {
                            num = (int)row["ProductId"];

                            pname = string.Format("<a href='{0}' target=_blank>{1} </a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { num }), proTitle);
                           
                            imgurl = string.Format("<a href='{0}' target=_blank><img src={1} /></a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[] { num }), imgurl);
                           
                            msg = string.Format("发布失败<br/>({0})", response2.ErrMsg + "：" + response2.SubErrMsg);

                            issuccess = "false";

                        }
                        else
                        {
                            imgurl = string.Format("<a href='http://item.taobao.com/item.htm?id={0}' target=_blank><img src={1} /></a>", response2.Item.NumIid, imgurl);
                           
                            pname = string.Format("<a href='http://item.taobao.com/item.htm?id={0}'>{1}</a>", response2.Item.NumIid, proTitle);
                           
                            msg = "商品发布成功";
                           
                            issuccess = "true";
                           
                            taobaoReturnProductIds.Add((int)row["ProductId"], response2.Item.NumIid);
                           
                            if (morepic == "true")
                            {
                                List<TbImage> productsImgs = this.GetProductsImgs(row, response2);

                                StringBuilder builder2 = new StringBuilder();

                                foreach (TbImage image in productsImgs)
                                {
                                    string path = Globals.ApplicationPath + image.Imgpath;

                                    if (File.Exists(Globals.MapPath(path)))
                                    {
                                        FileItem item = new FileItem(Globals.MapPath(path));

                                        ItemImgUploadRequest request = new ItemImgUploadRequest();

                                        request.Image = item;

                                        request.NumIid = new long?(image.TbProductId);

                                        request.IsMajor = false;

                                        ItemImgUploadResponse itemImgUploadResponse = this.client.Execute<ItemImgUploadResponse>(request, sessionkey);

                                        if (itemImgUploadResponse.IsError)
                                        {
                                            builder2.AppendFormat("[\"{0}发布图片错误,错误原因:{1}\"],", proTitle, itemImgUploadResponse.ErrMsg + ";" + itemImgUploadResponse.SubErrMsg);
                                        }

                                    }
                                }

                                if (builder2.Length > 0)
                                {

                                    imgmsg = builder2.ToString().Substring(0, builder2.ToString().Length - 1);

                                }

                            }
                        }
                    }

                    builder.Append(string.Concat(new object[] { "{\"pname\":\"", pname, "\",\"pimg\":\"", imgurl, "\",\"pmarkprice\":\"", markprice.ToString("F2"), "\",\"pstock\":\"", stock, "\",\"issuccess\":\"", issuccess, "\",\"msg\":\"", msg, "\",\"imgmsg\":[", imgmsg, "]}," }));

                }

                if (taobaoReturnProductIds.Count > 0)
                {
                    SubSiteProducthelper.AddTaobaoReturnProductIds(taobaoReturnProductIds, 0);
                }

                if (builder.ToString().Length > 0)
                {
                    builder.Remove(builder.Length - 1, 1);
                }

                context.Response.Write("{\"Status\":\"OK\",\"Result\":[" + builder.ToString() + "]}");
                context.Response.Flush();
                context.Response.End();
            }
            else
            {
                context.Response.Write("{\"Status\":\"Error\",\"Result\":\"发布商品到淘宝出错！\"}");
                context.Response.Flush();
                context.Response.End();
            }
        }

        public void ResponseData(DataRow row, out string imgurl, out int stock, out decimal markprice)
        {
            string str = "";
            int num = 0;
            decimal num2 = 0M;
            if (row["ImageUrl1"] != DBNull.Value)
            {
                str = row["ImageUrl1"].ToString().Replace("images/", "thumbs40/40_");
                if ((!string.IsNullOrEmpty(str) && !Utils.IsUrlAbsolute(str.ToLower())) && ((Utils.ApplicationPath.Length > 0) && !str.StartsWith(Utils.ApplicationPath)))
                {
                    str = Utils.ApplicationPath + str;
                }
            }
            if ((row["Num"] != DBNull.Value) && this.CheckInt(row["Num"].ToString()))
            {
                num = Convert.ToInt32(row["Num"].ToString());
            }
            if ((row["SalePrice"] != DBNull.Value) && this.CheckDecimal(row["SalePrice"].ToString()))
            {
                num2 = Convert.ToDecimal(row["SalePrice"].ToString());
            }
            imgurl = str;
            stock = num;
            markprice = num2;
        }

       void SetTaoBaoAddData(ItemAddRequest req, DataRow row)
        {
            req.StuffStatus = (row["StuffStatus"] == DBNull.Value) ? "" : ((string)row["StuffStatus"]);
            req.Cid = new long?((long)row["Cid"]);
            req.Price = ((decimal)row["SalePrice"]).ToString("F2");
            req.Type = "fixed";
            req.Num = new long?((long)row["Num"]);
            req.Title = (string)row["ProTitle"];
            req.OuterId = (row["ProductCode"] == DBNull.Value) ? "" : ((string)row["ProductCode"]);
            req.Desc = (row["Description"] == DBNull.Value) ? "暂无该商品的描述信息" : ((string)row["Description"]);
            req.LocationState = (string)row["LocationState"];
            req.LocationCity = (string)row["LocationCity"];
            req.HasShowcase = true;
            req.HasInvoice = new bool?((bool)row["HasInvoice"]);
            req.HasWarranty = new bool?((bool)row["HasWarranty"]);
            req.HasDiscount = new bool?((bool)row["HasDiscount"]);
            if (row["ImageUrl1"] != DBNull.Value)
            {
                string path = (string)row["ImageUrl1"];
                path = Globals.ApplicationPath + path;
                if (File.Exists(Globals.MapPath(path)))
                {
                    FileItem item = new FileItem(Globals.MapPath(path));
                    req.Image = item;
                }
            }
            if (row["ValidThru"] != DBNull.Value)
            {
                req.ValidThru = new long?((long)row["ValidThru"]);
            }
            if (row["ListTime"] != DBNull.Value)
            {
                req.ListTime = new DateTime?((DateTime)row["ListTime"]);
            }
            if (row["FreightPayer"].ToString() == "seller")
            {
                req.FreightPayer = "seller";
            }
            else
            {
                req.FreightPayer = "buyer";
                req.PostFee = (row["PostFee"] == DBNull.Value) ? "" : ((decimal)row["PostFee"]).ToString("F2");
                req.ExpressFee = (row["ExpressFee"] == DBNull.Value) ? "" : ((decimal)row["ExpressFee"]).ToString("F2");
                req.EmsFee = (row["EMSFee"] == DBNull.Value) ? "" : ((decimal)row["EMSFee"]).ToString("F2");
            }
            req.Props = (row["PropertyAlias"] == DBNull.Value) ? "" : ((string)row["PropertyAlias"]);
            req.InputPids = (row["InputPids"] == DBNull.Value) ? "" : ((string)row["InputPids"]);
            req.InputStr = (row["InputStr"] == DBNull.Value) ? "" : ((string)row["InputStr"]);
            req.SkuProperties = (row["SkuProperties"] == DBNull.Value) ? "" : ((string)row["SkuProperties"]);
            req.SkuQuantities = (row["SkuQuantities"] == DBNull.Value) ? "" : ((string)row["SkuQuantities"]);
            req.SkuPrices = (row["SkuPrices"] == DBNull.Value) ? "" : ((string)row["SkuPrices"]);
            req.SkuOuterIds = (row["SkuOuterIds"] == DBNull.Value) ? "" : ((string)row["SkuOuterIds"]);
            req.Lang = "zh_CN";
        }

       void SetTaoBaoUpdateData(ItemUpdateRequest req, DataRow row)
        {
            req.StuffStatus = (row["StuffStatus"] == DBNull.Value) ? "" : ((string)row["StuffStatus"]);
            req.Cid = new long?((long)row["Cid"]);
            req.Price = ((decimal)row["SalePrice"]).ToString("F2");
            req.Num = new long?((long)row["Num"]);
            req.OuterId = (row["ProductCode"] == DBNull.Value) ? "" : ((string)row["ProductCode"]);
            req.LocationState = (string)row["LocationState"];
            req.LocationCity = (string)row["LocationCity"];
            req.HasShowcase = true;
            req.HasInvoice = new bool?((bool)row["HasInvoice"]);
            req.HasWarranty = new bool?((bool)row["HasWarranty"]);
            req.HasDiscount = new bool?((bool)row["HasDiscount"]);
            if (row["ValidThru"] != DBNull.Value)
            {
                req.ValidThru = new long?((long)row["ValidThru"]);
            }
            if (row["ListTime"] != DBNull.Value)
            {
                req.ListTime = new DateTime?((DateTime)row["ListTime"]);
            }
            if (((string)row["FreightPayer"]) == "seller")
            {
                req.FreightPayer = "seller";
            }
            else
            {
                req.FreightPayer = "buyer";
                req.PostFee = (row["PostFee"] == DBNull.Value) ? "" : ((decimal)row["PostFee"]).ToString("F2");
                req.ExpressFee = (row["ExpressFee"] == DBNull.Value) ? "" : ((decimal)row["ExpressFee"]).ToString("F2");
                req.EmsFee = (row["EMSFee"] == DBNull.Value) ? "" : ((decimal)row["EMSFee"]).ToString("F2");
            }
            req.Props = (row["PropertyAlias"] == DBNull.Value) ? "" : ((string)row["PropertyAlias"]);
            req.InputPids = (row["InputPids"] == DBNull.Value) ? "" : ((string)row["InputPids"]);
            req.InputStr = (row["InputStr"] == DBNull.Value) ? "" : ((string)row["InputStr"]);
            req.SkuProperties = (row["SkuProperties"] == DBNull.Value) ? "" : ((string)row["SkuProperties"]);
            req.SkuQuantities = (row["SkuQuantities"] == DBNull.Value) ? "" : ((string)row["SkuQuantities"]);
            req.SkuPrices = (row["SkuPrices"] == DBNull.Value) ? "" : ((string)row["SkuPrices"]);
            req.SkuOuterIds = (row["SkuOuterIds"] == DBNull.Value) ? "" : ((string)row["SkuOuterIds"]);
            req.Lang = "zh_CN";
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

