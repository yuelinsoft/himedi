namespace Hidistro.SaleSystem.DistributionData
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Shopping;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class CookieShoppingData : CookieShoppingSubsiteProvider
    {
       const string CartDataCookieName = "Hid_distro_ShoppingCart_Data_New";
       Database database = DatabaseFactory.CreateDatabase();

        public override bool AddGiftItem(int giftId, int quantity)
        {
            XmlDocument shoppingCartData = this.GetShoppingCartData();
            XmlNode node = shoppingCartData.SelectSingleNode("//sc/gf");
            XmlNode newChild = node.SelectSingleNode(string.Concat(new object[] { "l[@g=", giftId, " and @d='", HiContext.Current.SiteSettings.UserId.Value, "']" }));
            if (newChild == null)
            {
                newChild = CreateGiftLineItemNode(shoppingCartData, giftId, quantity);
                node.InsertBefore(newChild, node.FirstChild);
            }
            else
            {
                newChild.Attributes["q"].Value = (int.Parse(newChild.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
            }
            this.SaveShoppingCartData(shoppingCartData);
            return true;
        }

        public override AddCartItemStatus AddLineItem(int productId, string skuId, int quantity)
        {
            XmlDocument shoppingCartData = this.GetShoppingCartData();
            XmlNode node = shoppingCartData.SelectSingleNode("//sc/lis");
            XmlNode newChild = node.SelectSingleNode(string.Concat(new object[] { "l[@s='", skuId, "' and @d='", HiContext.Current.SiteSettings.UserId.Value, "']" }));
            if (newChild == null)
            {
                newChild = CreateLineItemNode(shoppingCartData, productId, skuId, quantity);
                node.InsertBefore(newChild, node.FirstChild);
            }
            else
            {
                newChild.Attributes["q"].Value = (int.Parse(newChild.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
            }
            this.SaveShoppingCartData(shoppingCartData);
            return AddCartItemStatus.Successed;
        }

        public override void ClearShoppingCart()
        {
            HiContext.Current.Context.Response.Cookies["Hid_distro_ShoppingCart_Data_New"].Value = null;
            HiContext.Current.Context.Response.Cookies["Hid_distro_ShoppingCart_Data_New"].Expires = new DateTime(0x7cf, 10, 12);
            HiContext.Current.Context.Response.Cookies["Hid_distro_ShoppingCart_Data_New"].Path = "/";
        }

       static XmlDocument CreateEmptySchema()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml("<sc><lis></lis><gf></gf></sc>");
            return document;
        }

       static XmlNode CreateGiftLineItemNode(XmlDocument doc, int giftId, int quantity)
        {
            XmlNode node = doc.CreateElement("l");
            XmlNode node2 = doc.SelectSingleNode("//gf");
            XmlAttribute attribute = doc.CreateAttribute("d");
            attribute.Value = HiContext.Current.SiteSettings.UserId.Value.ToString(CultureInfo.InvariantCulture);
            node.Attributes.Append(attribute);
            XmlAttribute attribute2 = doc.CreateAttribute("q");
            attribute2.Value = quantity.ToString(CultureInfo.InvariantCulture);
            node.Attributes.Append(attribute2);
            XmlAttribute attribute3 = doc.CreateAttribute("g");
            attribute3.Value = giftId.ToString();
            node.Attributes.Append(attribute3);
            return node;
        }

       static XmlNode CreateLineItemNode(XmlDocument doc, int productId, string skuId, int quantity)
        {
            XmlNode node = doc.CreateElement("l");
            XmlNode node2 = doc.SelectSingleNode("//lis");
            XmlAttribute attribute = doc.CreateAttribute("d");
            attribute.Value = HiContext.Current.SiteSettings.UserId.Value.ToString(CultureInfo.InvariantCulture);
            node.Attributes.Append(attribute);
            XmlAttribute attribute2 = doc.CreateAttribute("s");
            attribute2.Value = skuId;
            node.Attributes.Append(attribute2);
            XmlAttribute attribute3 = doc.CreateAttribute("q");
            attribute3.Value = quantity.ToString(CultureInfo.InvariantCulture);
            node.Attributes.Append(attribute3);
            XmlAttribute attribute4 = doc.CreateAttribute("p");
            attribute4.Value = productId.ToString();
            node.Attributes.Append(attribute4);
            return node;
        }

       static int GenerateLastItemId(XmlDocument doc)
        {
            int num;
            XmlNode node = doc.SelectSingleNode("/sc");
            XmlAttribute attribute = node.Attributes["lid"];
            if (attribute == null)
            {
                attribute = doc.CreateAttribute("lid");
                node.Attributes.Append(attribute);
                num = 1;
            }
            else
            {
                num = int.Parse(attribute.Value) + 1;
            }
            attribute.Value = num.ToString(CultureInfo.InvariantCulture);
            return num;
        }

       ShoppingCartItemInfo GetCartItemInfo(int productId, string skuId, int quantity)
        {
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, 0);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                string name = reader["ProductName"].ToString();
                int categoryId = 0;
                if (DBNull.Value != reader["CategoryId"])
                {
                    categoryId = (int) reader["CategoryId"];
                }
                int weight = 0;
                if (DBNull.Value != reader["Weight"])
                {
                    weight = (int) reader["Weight"];
                }
                decimal memberPrice = (decimal) reader["SalePrice"];
                string str2 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl40"])
                {
                    str2 = reader["ThumbnailUrl40"].ToString();
                }
                string str3 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl60"])
                {
                    str3 = reader["ThumbnailUrl60"].ToString();
                }
                string str4 = string.Empty;
                if (DBNull.Value != reader["ThumbnailUrl100"])
                {
                    str4 = reader["ThumbnailUrl100"].ToString();
                }
                int purchaseGiftId = 0;
                int giveQuantity = 0;
                int wholesaleDiscountId = 0;
                string purchaseGiftName = null;
                string wholesaleDiscountName = null;
                decimal? discountRate = null;
                string sku = (reader["SKU"] != DBNull.Value) ? ((string) reader["SKU"]) : string.Empty;
                int num7 = (int) reader["TotalQuantity"];
                string skuContent = string.Empty;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (!((((reader["AttributeName"] == DBNull.Value) || string.IsNullOrEmpty((string) reader["AttributeName"])) || (reader["ValueStr"] == DBNull.Value)) || string.IsNullOrEmpty((string) reader["ValueStr"])))
                        {
                            object obj2 = skuContent;
                            skuContent = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                        }
                    }
                }
                return new ShoppingCartItemInfo(skuId, productId, sku, name, memberPrice, skuContent, num7, weight, purchaseGiftId, purchaseGiftName, giveQuantity, wholesaleDiscountId, wholesaleDiscountName, discountRate, categoryId, str2, str3, str4);
            }
        }

        public override decimal GetCostPrice(string skuId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_GetGroupBuyProductCostPrices");
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            decimal num = 0M;
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                int num2 = 100;
                if (reader.Read())
                {
                    num2 = reader.GetInt32(0);
                }
                if (!reader.NextResult() || !reader.Read())
                {
                    return num;
                }
                if (reader["DistributorPurchasePrice"] != DBNull.Value)
                {
                    return (decimal) reader["DistributorPurchasePrice"];
                }
                return Math.Round((decimal) (((decimal) reader["PurchasePrice"]) * (num2 / 100M)), 2);
            }
        }

        public override Dictionary<string, decimal> GetCostPriceForItems(int userId)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_GetCostPrices");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(storedProcCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                while (reader.Read())
                {
                    dictionary.Add((string) reader["SkuId"], (decimal) reader["PurchasePrice"]);
                }
            }
            return dictionary;
        }

        public override void GetPromotionsWithAmount(decimal amount, out int discountActivityId, out string discountName, out decimal discountValue, out DiscountValueType discountValueType, out int feeFreeActivityId, out string feeFreeName, out bool eightFree, out bool orderOptionFree, out bool procedureFeeFree)
        {
            discountActivityId = 0;
            discountName = null;
            discountValue = 0M;
            discountValueType = DiscountValueType.Amount;
            feeFreeActivityId = 0;
            feeFreeName = null;
            eightFree = false;
            orderOptionFree = false;
            procedureFeeFree = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 p.ActivityId, p.Name, fd.DiscountValue, fd.ValueType FROM distro_Promotions p INNER JOIN distro_FullDiscounts fd ON p.ActivityId = fd.ActivityId  INNER JOIN distro_PromotionMemberGrades pm ON pm.ActivityId=fd.ActivityId WHERE @Amount >= fd.Amount  AND @GradeId IN (SELECT GradeId FROM distro_PromotionMemberGrades WHERE distro_PromotionMemberGrades.ActivityId=fd.ActivityId)  AND p.DistributorUserId = @DistributorUserId ORDER BY fd.Amount DESC");
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, amount);
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, (Users.GetUser(HiContext.Current.User.UserId) as Member).GradeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    discountActivityId = reader.GetInt32(0);
                    discountName = reader.GetString(1);
                    discountValue = reader.GetDecimal(2);
                    discountValueType = (DiscountValueType) reader.GetInt32(3);
                }
            }
            sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 1 p.ActivityId, p.Name, ff.ShipChargeFree, ff.ServiceChargeFree, ff.OptionFeeFree FROM distro_Promotions p INNER JOIN distro_FullFree ff ON p.ActivityId = ff.ActivityId INNER JOIN distro_PromotionMemberGrades pm ON pm.ActivityId=ff.ActivityId WHERE DistributorUserId = @DistributorUserId AND @SubAmount >= ff.Amount  AND @GradeId IN (SELECT GradeId FROM distro_PromotionMemberGrades WHERE distro_PromotionMemberGrades.ActivityId=ff.ActivityId) ORDER BY ff.Amount DESC");
            decimal num = 0M;
            if ((discountValueType == DiscountValueType.Percent) && (discountValue > 0M))
            {
                num = (amount * discountValue) / 100M;
            }
            else if ((discountValueType == DiscountValueType.Amount) && (discountValue > 0M))
            {
                num = amount - discountValue;
            }
            if (num > 0M)
            {
                this.database.AddInParameter(sqlStringCommand, "SubAmount", DbType.Currency, num);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "SubAmount", DbType.Currency, amount);
            }
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, (Users.GetUser(HiContext.Current.User.UserId) as Member).GradeId);
            using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader2.Read())
                {
                    feeFreeActivityId = reader2.GetInt32(0);
                    feeFreeName = reader2.GetString(1);
                    eightFree = reader2.GetBoolean(2);
                    procedureFeeFree = reader2.GetBoolean(3);
                    orderOptionFree = reader2.GetBoolean(4);
                }
            }
        }

        public override ShoppingCartInfo GetShoppingCart()
        {
            XmlDocument shoppingCartData = this.GetShoppingCartData();
            ShoppingCartInfo cartInfo = null;
            XmlNodeList list = shoppingCartData.SelectNodes("//sc/lis/l");
            XmlNodeList list2 = shoppingCartData.SelectNodes("//sc/gf/l");
            if (((list != null) && (list.Count > 0)) || ((list2 != null) && (list2.Count > 0)))
            {
                cartInfo = new ShoppingCartInfo();
            }
            if ((list != null) && (list.Count > 0))
            {
                IList<string> skuIds = new List<string>();
                Dictionary<string, int> productIdList = new Dictionary<string, int>();
                Dictionary<string, int> productQuantityList = new Dictionary<string, int>();
                foreach (XmlNode node in list)
                {
                    skuIds.Add(node.Attributes["s"].Value);
                    productQuantityList.Add(node.Attributes["s"].Value, int.Parse(node.Attributes["q"].Value));
                    productIdList.Add(node.Attributes["s"].Value, int.Parse(node.Attributes["p"].Value));
                }
                this.LoadCartProduct(cartInfo, productQuantityList, skuIds, productIdList);
            }
            if ((list2 != null) && (list2.Count > 0))
            {
                StringBuilder builder = new StringBuilder();
                Dictionary<int, int> giftIdList = new Dictionary<int, int>();
                Dictionary<int, int> giftQuantityList = new Dictionary<int, int>();
                foreach (XmlNode node2 in list2)
                {
                    builder.AppendFormat("{0},", int.Parse(node2.Attributes["g"].Value));
                    giftIdList.Add(int.Parse(node2.Attributes["g"].Value), int.Parse(node2.Attributes["g"].Value));
                    giftQuantityList.Add(int.Parse(node2.Attributes["g"].Value), int.Parse(node2.Attributes["q"].Value));
                }
                this.LoadCartGift(cartInfo, giftIdList, giftQuantityList, builder.ToString());
            }
            return cartInfo;
        }

       XmlDocument GetShoppingCartData()
        {
            XmlDocument document = new XmlDocument();
            HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Hid_distro_ShoppingCart_Data_New"];
            if ((cookie == null) || string.IsNullOrEmpty(cookie.Value))
            {
                return CreateEmptySchema();
            }
            try
            {
                document.LoadXml(Globals.UrlDecode(cookie.Value));
            }
            catch
            {
                this.ClearShoppingCart();
                document = CreateEmptySchema();
            }
            return document;
        }

        public override bool GetShoppingProductInfo(int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity)
        {
            saleStatus = ProductSaleStatus.Delete;
            stock = 0;
            totalQuantity = 0;
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock，SaleStatus,AlertStock FROM Hishop_Skus s INNER JOIN Hishop_Products p ON s.ProductId=p.ProductId WHERE s.ProductId=@ProductId AND s.SkuId=@SkuId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    saleStatus = (ProductSaleStatus) ((int) reader["SaleStatus"]);
                    stock = (int) reader["Stock"];
                    int num = (int) reader["AlertStock"];
                    if (stock <= num)
                    {
                        saleStatus = ProductSaleStatus.UnSale;
                    }
                    flag = true;
                }
                totalQuantity = this.GetShoppingProductQuantity(skuId, productId);
            }
            return flag;
        }

       int GetShoppingProductQuantity(string skuId, int ProductId)
        {
            int result = 0;
            XmlNode node = this.GetShoppingCartData().SelectSingleNode(string.Concat(new object[] { "//sc/lis/l[SkuId='", skuId, "' AND p=", ProductId, " AND d=", HiContext.Current.SiteSettings.UserId.Value, "]" }));
            if (node != null)
            {
                int.TryParse(node.Attributes["q"].Value, out result);
            }
            return result;
        }

        public override int GetSkuStock(string skuId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                return (int) obj2;
            }
            return 0;
        }

       void LoadCartGift(ShoppingCartInfo cartInfo, Dictionary<int, int> giftIdList, Dictionary<int, int> giftQuantityList, string giftIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT g.*,hg.Unit,hg.LongDescription,hg.CostPrice,hg.ImageUrl,hg.ThumbnailUrl40,hg.ThumbnailUrl60,hg.ThumbnailUrl100,hg.PurchasePrice,hg.MarketPrice,hg.IsDownLoad FROM distro_Gifts g ON gc.GiftId = g.GiftId join Hishop_Gifts hg on hg.GiftId=g.GiftId WHERE g.GiftId IN ({0}) g.DistributorUserId={1}", giftIds.TrimEnd(new char[] { ',' }), HiContext.Current.SiteSettings.UserId.Value));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ShoppingCartGiftInfo item = DataMapper.PopulateGiftCartItem(reader);
                    item.Quantity = giftQuantityList[item.GiftId];
                    cartInfo.LineGifts.Add(item);
                }
            }
        }

       void LoadCartProduct(ShoppingCartInfo cartInfo, Dictionary<string, int> productQuantityList, IList<string> skuIds, Dictionary<string, int> productIdList)
        {
            foreach (string str in skuIds)
            {
                ShoppingCartItemInfo info = this.GetCartItemInfo(productIdList[str], str, productQuantityList[str]);
                if (info != null)
                {
                    cartInfo.LineItems.Add(str, info);
                }
            }
        }

        public override void RemoveGiftItem(int giftId)
        {
            XmlDocument shoppingCartData = this.GetShoppingCartData();
            XmlNode node = shoppingCartData.SelectSingleNode("//sc/gf");
            XmlNode oldChild = node.SelectSingleNode(string.Concat(new object[] { "l[@g='", giftId, "' and @d='", HiContext.Current.SiteSettings.UserId.Value, "']" }));
            if (oldChild != null)
            {
                node.RemoveChild(oldChild);
                this.SaveShoppingCartData(shoppingCartData);
            }
        }

        public override void RemoveLineItem(string skuId)
        {
            XmlDocument shoppingCartData = this.GetShoppingCartData();
            XmlNode node = shoppingCartData.SelectSingleNode("//sc/lis");
            XmlNode oldChild = node.SelectSingleNode(string.Concat(new object[] { "l[@s='", skuId, "' and @d='", HiContext.Current.SiteSettings.UserId.Value, "']" }));
            if (oldChild != null)
            {
                node.RemoveChild(oldChild);
                this.SaveShoppingCartData(shoppingCartData);
            }
        }

       void SaveShoppingCartData(XmlDocument doc)
        {
            if (doc == null)
            {
                this.ClearShoppingCart();
            }
            else
            {
                HttpCookie cookie = HiContext.Current.Context.Request.Cookies["Hid_distro_ShoppingCart_Data_New"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("Hid_distro_ShoppingCart_Data_New");
                }
                cookie.Value = Globals.UrlEncode(doc.OuterXml);
                cookie.Expires = DateTime.Now.AddDays(3.0);
                HiContext.Current.Context.Response.Cookies.Add(cookie);
            }
        }

        public override void UpdateGiftItemQuantity(int giftId, int quantity)
        {
            if (quantity <= 0)
            {
                this.RemoveGiftItem(giftId);
            }
            else
            {
                XmlDocument shoppingCartData = this.GetShoppingCartData();
                XmlNode node2 = shoppingCartData.SelectSingleNode("//sc/gf").SelectSingleNode(string.Concat(new object[] { "l[@g='", giftId, "' and @d='", HiContext.Current.SiteSettings.UserId.Value, "']" }));
                if (node2 != null)
                {
                    node2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
                    this.SaveShoppingCartData(shoppingCartData);
                }
            }
        }

        public override void UpdateLineItemQuantity(int productId, string skuId, int quantity)
        {
            if (quantity <= 0)
            {
                this.RemoveLineItem(skuId);
            }
            else
            {
                XmlDocument shoppingCartData = this.GetShoppingCartData();
                XmlNode node2 = shoppingCartData.SelectSingleNode("//lis").SelectSingleNode(string.Concat(new object[] { "l[@s='", skuId, "' and @d='", HiContext.Current.SiteSettings.UserId.Value, "']" }));
                if (node2 != null)
                {
                    node2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
                    this.SaveShoppingCartData(shoppingCartData);
                }
            }
        }
    }
}

