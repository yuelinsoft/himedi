namespace Hidistro.Entities
{
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Distribution;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.Entities.Store;
    using System;
    using System.Data;

    public static class DataMapper
    {
        public static CategoryInfo ConvertDataRowToProductCategory(DataRow row)
        {
            CategoryInfo info = new CategoryInfo();
            info.CategoryId = (int) row["CategoryId"];
            info.Name = (string) row["Name"];
            info.DisplaySequence = (int) row["DisplaySequence"];
            if (row["Meta_Title"] != DBNull.Value)
            {
                info.MetaTitle = (string) row["Meta_Title"];
            }
            if (row["Meta_Description"] != DBNull.Value)
            {
                info.MetaDescription = (string) row["Meta_Description"];
            }
            if (row["Meta_Keywords"] != DBNull.Value)
            {
                info.MetaKeywords = (string) row["Meta_Keywords"];
            }
            if (row["Description"] != DBNull.Value)
            {
                info.Description = (string) row["Description"];
            }
            if (row["SKUPrefix"] != DBNull.Value)
            {
                info.SKUPrefix = (string) row["SKUPrefix"];
            }
            if (row["AssociatedProductType"] != DBNull.Value)
            {
                info.AssociatedProductType = new int?((int) row["AssociatedProductType"]);
            }
            if (row["Notes1"] != DBNull.Value)
            {
                info.Notes1 = (string) row["Notes1"];
            }
            if (row["Notes2"] != DBNull.Value)
            {
                info.Notes2 = (string) row["Notes2"];
            }
            if (row["Notes3"] != DBNull.Value)
            {
                info.Notes3 = (string) row["Notes3"];
            }
            if (row["Notes4"] != DBNull.Value)
            {
                info.Notes4 = (string) row["Notes4"];
            }
            if (row["Notes5"] != DBNull.Value)
            {
                info.Notes5 = (string) row["Notes5"];
            }
            if (row["ParentCategoryId"] != DBNull.Value)
            {
                info.ParentCategoryId = new int?((int) row["ParentCategoryId"]);
            }
            info.Depth = (int) row["Depth"];
            info.Path = (string) row["Path"];
            if (row["RewriteName"] != DBNull.Value)
            {
                info.RewriteName = (string) row["RewriteName"];
            }
            if (row["Theme"] != DBNull.Value)
            {
                info.Theme = (string) row["Theme"];
            }
            info.HasChildren = (bool) row["HasChildren"];
            return info;
        }

        public static AccountSummaryInfo PopulateAccountSummary(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            AccountSummaryInfo info = new AccountSummaryInfo();
            if (reader["AccountAmount"] != DBNull.Value)
            {
                info.AccountAmount = (decimal) reader["AccountAmount"];
            }
            if (reader["FreezeBalance"] != DBNull.Value)
            {
                info.FreezeBalance = (decimal) reader["FreezeBalance"];
            }
            info.UseableBalance = info.AccountAmount - info.FreezeBalance;
            return info;
        }

        public static AfficheInfo PopulateAffiche(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            AfficheInfo info = new AfficheInfo();
            info.AfficheId = (int) reader["AfficheId"];
            if (reader["Title"] != DBNull.Value)
            {
                info.Title = (string) reader["Title"];
            }
            info.Content = (string) reader["Content"];
            info.AddedDate = (DateTime) reader["AddedDate"];
            return info;
        }

        public static ArticleInfo PopulateArticle(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ArticleInfo info = new ArticleInfo();
            info.ArticleId = (int) reader["ArticleId"];
            info.CategoryId = (int) reader["CategoryId"];
            info.Title = (string) reader["Title"];
            if (reader["Meta_Description"] != DBNull.Value)
            {
                info.MetaDescription = (string) reader["Meta_Description"];
            }
            if (reader["Meta_Keywords"] != DBNull.Value)
            {
                info.MetaKeywords = (string) reader["Meta_Keywords"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            if (reader["IconUrl"] != DBNull.Value)
            {
                info.IconUrl = (string) reader["IconUrl"];
            }
            info.Content = (string) reader["Content"];
            info.AddedDate = (DateTime) reader["AddedDate"];
            info.IsRelease = (bool) reader["IsRelease"];
            return info;
        }

        public static ArticleCategoryInfo PopulateArticleCategory(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ArticleCategoryInfo info = new ArticleCategoryInfo();
            info.CategoryId = (int) reader["CategoryId"];
            info.Name = (string) reader["Name"];
            info.DisplaySequence = (int) reader["DisplaySequence"];
            if (reader["IconUrl"] != DBNull.Value)
            {
                info.IconUrl = (string) reader["IconUrl"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            return info;
        }

        public static AttributeValueInfo PopulateAttributeValue(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            AttributeValueInfo info = new AttributeValueInfo();
            info.ValueId = (int) reader["ValueId"];
            info.AttributeId = (int) reader["AttributeId"];
            info.ValueStr = (string) reader["ValueStr"];
            return info;
        }

        public static BrandCategoryInfo PopulateBrandCategory(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            BrandCategoryInfo info = new BrandCategoryInfo();
            info.BrandId = (int) reader["BrandId"];
            info.BrandName = (string) reader["BrandName"];
            if (reader["DisplaySequence"] != DBNull.Value)
            {
                info.DisplaySequence = (int) reader["DisplaySequence"];
            }
            if (reader["Logo"] != DBNull.Value)
            {
                info.Logo = (string) reader["Logo"];
            }
            if (reader["CompanyUrl"] != DBNull.Value)
            {
                info.CompanyUrl = (string) reader["CompanyUrl"];
            }
            if (reader["RewriteName"] != DBNull.Value)
            {
                info.RewriteName = (string) reader["RewriteName"];
            }
            if (reader["MetaKeywords"] != DBNull.Value)
            {
                info.MetaKeywords = (string) reader["MetaKeywords"];
            }
            if (reader["MetaDescription"] != DBNull.Value)
            {
                info.MetaDescription = (string) reader["MetaDescription"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            if (reader["Theme"] != DBNull.Value)
            {
                info.Theme = (string) reader["Theme"];
            }
            return info;
        }

        public static ShoppingCartItemInfo PopulateCartItem(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            string skuContent = (reader["SKUContent"] != DBNull.Value) ? ((string) reader["SKUContent"]) : string.Empty;
            int purchaseGiftId = 0;
            int giveQuantity = 0;
            int wholesaleDiscountId = 0;
            int categoryId = 0;
            string purchaseGiftName = null;
            string wholesaleDiscountName = null;
            string str4 = null;
            string str5 = null;
            string str6 = null;
            decimal? discountRate = null;
            if (reader["CategoryId"] != DBNull.Value)
            {
                categoryId = (int) reader["CategoryId"];
            }
            if (reader["ThumbnailUrl40"] != DBNull.Value)
            {
                str4 = (string) reader["ThumbnailUrl40"];
            }
            if (reader["ThumbnailUrl60"] != DBNull.Value)
            {
                str5 = (string) reader["ThumbnailUrl60"];
            }
            if (reader["ThumbnailUrl100"] != DBNull.Value)
            {
                str6 = (string) reader["ThumbnailUrl100"];
            }
            return new ShoppingCartItemInfo((string) reader["SkuId"], (int) reader["ProductId"], (string) reader["SKU"], (string) reader["Name"], (decimal) reader["MemberPrice"], skuContent, (int) reader["Quantity"], (int) reader["Weight"], purchaseGiftId, purchaseGiftName, giveQuantity, wholesaleDiscountId, wholesaleDiscountName, discountRate, categoryId, str4, str5, str6);
        }

        public static CountDownInfo PopulateCountDown(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            CountDownInfo info = new CountDownInfo();
            info.CountDownId = (int) reader["CountDownId"];
            info.ProductId = (int) reader["ProductId"];
            if (DBNull.Value != reader["CountDownPrice"])
            {
                info.CountDownPrice = (decimal) reader["CountDownPrice"];
            }
            info.EndDate = (DateTime) reader["EndDate"];
            if (DBNull.Value != reader["Content"])
            {
                info.Content = (string) reader["Content"];
            }
            info.DisplaySequence = (int) reader["DisplaySequence"];
            return info;
        }

        public static CouponInfo PopulateCoupon(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            CouponInfo info = new CouponInfo();
            info.CouponId = (int) reader["CouponId"];
            info.Name = (string) reader["Name"];
            info.ClosingTime = (DateTime) reader["ClosingTime"];
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            if (reader["Amount"] != DBNull.Value)
            {
                info.Amount = new decimal?((decimal) reader["Amount"]);
            }
            info.DiscountValue = (decimal) reader["DiscountValue"];
            info.SentCount = (int) reader["SentCount"];
            info.UsedCount = (int) reader["UsedCount"];
            info.NeedPoint = (int) reader["NeedPoint"];
            return info;
        }

        public static CouponItemInfo PopulateCouponItem(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            CouponItemInfo info = new CouponItemInfo();
            info.CouponId = (int) reader["CouponId"];
            info.ClaimCode = (string) reader["ClaimCode"];
            info.GenerateTime = Convert.ToDateTime(reader["GenerateTime"]);
            if (reader["UserId"] != DBNull.Value)
            {
                info.UserId = new int?((int) reader["UserId"]);
            }
            if (reader["EmailAddress"] != DBNull.Value)
            {
                info.EmailAddress = (string) reader["EmailAddress"];
            }
            return info;
        }

        public static FriendlyLinksInfo PopulateFriendlyLink(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            FriendlyLinksInfo info = new FriendlyLinksInfo();
            info.LinkId = new int?((int) reader["LinkId"]);
            info.Visible = (bool) reader["Visible"];
            info.DisplaySequence = (int) reader["DisplaySequence"];
            if (reader["ImageUrl"] != DBNull.Value)
            {
                info.ImageUrl = (string) reader["ImageUrl"];
            }
            if (reader["Title"] != DBNull.Value)
            {
                info.Title = (string) reader["Title"];
            }
            if (reader["LinkUrl"] != DBNull.Value)
            {
                info.LinkUrl = (string) reader["LinkUrl"];
            }
            return info;
        }

        public static FullDiscountInfo PopulateFullDiscount(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            FullDiscountInfo info = new FullDiscountInfo();
            info.ActivityId = (int) reader["ActivityId"];
            info.Name = (string) reader["Name"];
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            info.Amount = (decimal) reader["Amount"];
            info.DiscountValue = (decimal) reader["DiscountValue"];
            info.ValueType = (DiscountValueType) ((int) reader["ValueType"]);
            return info;
        }

        public static FullFreeInfo PopulateFullFree(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            FullFreeInfo info = new FullFreeInfo();
            info.ActivityId = (int) reader["ActivityId"];
            info.Name = (string) reader["Name"];
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            info.Amount = (decimal) reader["Amount"];
            info.ShipChargeFree = (bool) reader["ShipChargeFree"];
            info.ServiceChargeFree = (bool) reader["ServiceChargeFree"];
            info.OptionFeeFree = (bool) reader["OptionFeeFree"];
            return info;
        }

        public static GiftInfo PopulateGift(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            GiftInfo info = new GiftInfo();
            info.GiftId = (int) reader["GiftId"];
            info.Name = (DBNull.Value == reader["Name"]) ? null : ((string) reader["Name"]);
            info.ShortDescription = (DBNull.Value == reader["ShortDescription"]) ? null : ((string) reader["ShortDescription"]);
            info.Unit = (DBNull.Value == reader["Unit"]) ? null : ((string) reader["Unit"]);
            info.LongDescription = (DBNull.Value == reader["LongDescription"]) ? null : ((string) reader["LongDescription"]);
            info.Title = (DBNull.Value == reader["Title"]) ? null : ((string) reader["Title"]);
            info.Meta_Description = (DBNull.Value == reader["Meta_Description"]) ? null : ((string) reader["Meta_Description"]);
            info.Meta_Keywords = (DBNull.Value == reader["Meta_Keywords"]) ? null : ((string) reader["Meta_Keywords"]);
            if (DBNull.Value != reader["CostPrice"])
            {
                info.CostPrice = new decimal?((decimal) reader["CostPrice"]);
            }
            if (DBNull.Value != reader["ImageUrl"])
            {
                info.ImageUrl = (string) reader["ImageUrl"];
            }
            if (DBNull.Value != reader["ThumbnailUrl40"])
            {
                info.ThumbnailUrl40 = (string) reader["ThumbnailUrl40"];
            }
            if (DBNull.Value != reader["ThumbnailUrl60"])
            {
                info.ThumbnailUrl60 = (string) reader["ThumbnailUrl60"];
            }
            if (DBNull.Value != reader["ThumbnailUrl100"])
            {
                info.ThumbnailUrl100 = (string) reader["ThumbnailUrl100"];
            }
            if (DBNull.Value != reader["ThumbnailUrl160"])
            {
                info.ThumbnailUrl160 = (string) reader["ThumbnailUrl160"];
            }
            if (DBNull.Value != reader["ThumbnailUrl180"])
            {
                info.ThumbnailUrl180 = (string) reader["ThumbnailUrl180"];
            }
            if (DBNull.Value != reader["ThumbnailUrl220"])
            {
                info.ThumbnailUrl220 = (string) reader["ThumbnailUrl220"];
            }
            if (DBNull.Value != reader["ThumbnailUrl310"])
            {
                info.ThumbnailUrl310 = (string) reader["ThumbnailUrl310"];
            }
            if (DBNull.Value != reader["ThumbnailUrl410"])
            {
                info.ThumbnailUrl410 = (string) reader["ThumbnailUrl410"];
            }
            if (DBNull.Value != reader["PurchasePrice"])
            {
                info.PurchasePrice = (decimal) reader["PurchasePrice"];
            }
            if (DBNull.Value != reader["MarketPrice"])
            {
                info.MarketPrice = new decimal?((decimal) reader["MarketPrice"]);
            }
            info.NeedPoint = (int) reader["NeedPoint"];
            info.IsDownLoad = (bool) reader["IsDownLoad"];
            return info;
        }

        public static ShoppingCartGiftInfo PopulateGiftCartItem(IDataReader reader)
        {
            ShoppingCartGiftInfo info = new ShoppingCartGiftInfo();
            info.UserId = (int) reader["UserId"];
            info.GiftId = (int) reader["GiftId"];
            info.Name = (string) reader["Name"];
            if (reader["CostPrice"] != DBNull.Value)
            {
                info.CostPrice = (decimal) reader["CostPrice"];
            }
            info.PurchasePrice = (decimal) reader["PurchasePrice"];
            info.NeedPoint = (int) reader["NeedPoint"];
            if (reader["ThumbnailUrl40"] != DBNull.Value)
            {
                info.ThumbnailUrl40 = (string) reader["ThumbnailUrl40"];
            }
            if (reader["ThumbnailUrl60"] != DBNull.Value)
            {
                info.ThumbnailUrl60 = (string) reader["ThumbnailUrl60"];
            }
            if (reader["ThumbnailUrl100"] != DBNull.Value)
            {
                info.ThumbnailUrl100 = (string) reader["ThumbnailUrl100"];
            }
            return info;
        }

        public static GroupBuyInfo PopulateGroupBuy(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            GroupBuyInfo info = new GroupBuyInfo();
            info.GroupBuyId = (int) reader["GroupBuyId"];
            info.ProductId = (int) reader["ProductId"];
            if (DBNull.Value != reader["NeedPrice"])
            {
                info.NeedPrice = (decimal) reader["NeedPrice"];
            }
            info.MaxCount = (int) reader["MaxCount"];
            info.EndDate = (DateTime) reader["EndDate"];
            if (DBNull.Value != reader["Content"])
            {
                info.Content = (string) reader["Content"];
            }
            info.Status = (GroupBuyStatus) ((int) reader["Status"]);
            return info;
        }

        public static HelpInfo PopulateHelp(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            HelpInfo info = new HelpInfo();
            info.CategoryId = (int) reader["CategoryId"];
            info.HelpId = (int) reader["HelpId"];
            info.Title = (string) reader["Title"];
            if (reader["Meta_Description"] != DBNull.Value)
            {
                info.MetaDescription = (string) reader["Meta_Description"];
            }
            if (reader["Meta_Keywords"] != DBNull.Value)
            {
                info.MetaKeywords = (string) reader["Meta_Keywords"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            info.Content = (string) reader["Content"];
            info.AddedDate = (DateTime) reader["AddedDate"];
            info.IsShowFooter = (bool) reader["IsShowFooter"];
            return info;
        }

        public static HelpCategoryInfo PopulateHelpCategory(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            HelpCategoryInfo info = new HelpCategoryInfo();
            info.CategoryId = new int?((int) reader["CategoryId"]);
            info.Name = (string) reader["Name"];
            info.DisplaySequence = (int) reader["DisplaySequence"];
            if (reader["IconUrl"] != DBNull.Value)
            {
                info.IconUrl = (string) reader["IconUrl"];
            }
            if (reader["IndexChar"] != DBNull.Value)
            {
                info.IndexChar = (string) reader["IndexChar"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            info.IsShowFooter = (bool) reader["IsShowFooter"];
            return info;
        }

        public static InpourRequestInfo PopulateInpourRequest(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            InpourRequestInfo info = new InpourRequestInfo();
            info.InpourId = (string) reader["InpourId"];
            info.TradeDate = (DateTime) reader["TradeDate"];
            info.UserId = (int) reader["UserId"];
            info.PaymentId = (int) reader["PaymentId"];
            info.InpourBlance = (decimal) reader["InpourBlance"];
            return info;
        }

        public static LeaveCommentInfo PopulateLeaveComment(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            LeaveCommentInfo info = new LeaveCommentInfo();
            info.LeaveId = (long) reader["LeaveId"];
            if (reader["UserId"] != DBNull.Value)
            {
                info.UserId = new int?((int) reader["UserId"]);
            }
            if (reader["UserName"] != DBNull.Value)
            {
                info.UserName = (string) reader["UserName"];
            }
            info.Title = (string) reader["Title"];
            info.PublishContent = (string) reader["PublishContent"];
            info.PublishDate = (DateTime) reader["PublishDate"];
            info.LastDate = (DateTime) reader["LastDate"];
            return info;
        }

        public static LeaveCommentReplyInfo PopulateLeaveCommentReply(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            LeaveCommentReplyInfo info = new LeaveCommentReplyInfo();
            info.LeaveId = (long) reader["LeaveId"];
            info.ReplyId = (long) reader["ReplyId"];
            info.UserId = (int) reader["UserId"];
            info.ReplyContent = (string) reader["ReplyContent"];
            info.ReplyDate = (DateTime) reader["ReplyDate"];
            return info;
        }

        public static LineItemInfo PopulateLineItem(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            return new LineItemInfo((string) reader["SkuId"], (int) reader["ProductId"], (reader["SKU"] != DBNull.Value) ? ((string) reader["SKU"]) : string.Empty, (int) reader["Quantity"], (int) reader["ShipmentQuantity"], (decimal) reader["CostPrice"], (decimal) reader["ItemListPrice"], (decimal) reader["ItemAdjustedPrice"], (string) reader["ItemDescription"], (reader["ThumbnailsUrl"] != DBNull.Value) ? ((string) reader["ThumbnailsUrl"]) : null, (int) reader["Weight"], (DBNull.Value != reader["PurchaseGiftId"]) ? ((int) reader["PurchaseGiftId"]) : 0, (DBNull.Value != reader["PurchaseGiftName"]) ? ((string) reader["PurchaseGiftName"]) : null, (DBNull.Value != reader["WholesaleDiscountId"]) ? ((int) reader["WholesaleDiscountId"]) : 0, (DBNull.Value != reader["WholesaleDiscountName"]) ? ((string) reader["WholesaleDiscountName"]) : null, (DBNull.Value != reader["SKUContent"]) ? ((string) reader["SKUContent"]) : null);
        }

        public static MemberGradeInfo PopulateMemberGrade(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            MemberGradeInfo info = new MemberGradeInfo();
            info.GradeId = (int) reader["GradeId"];
            info.Name = (string) reader["Name"];
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            info.Points = (int) reader["Points"];
            info.IsDefault = (bool) reader["IsDefault"];
            info.Discount = (int) reader["Discount"];
            return info;
        }

        public static OrderInfo PopulateOrder(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            OrderInfo info = new OrderInfo();
            info.OrderId = (string) reader["OrderId"];
            if (DBNull.Value != reader["GatewayOrderId"])
            {
                info.GatewayOrderId = (string) reader["GatewayOrderId"];
            }
            if (DBNull.Value != reader["Remark"])
            {
                info.Remark = (string) reader["Remark"];
            }
            if (DBNull.Value != reader["ManagerMark"])
            {
                info.ManagerMark = new OrderMark?((OrderMark) reader["ManagerMark"]);
            }
            if (DBNull.Value != reader["ManagerRemark"])
            {
                info.ManagerRemark = (string) reader["ManagerRemark"];
            }
            if (DBNull.Value != reader["AdjustedDiscount"])
            {
                info.AdjustedDiscount = (decimal) reader["AdjustedDiscount"];
            }
            if (DBNull.Value != reader["OrderStatus"])
            {
                info.OrderStatus = (OrderStatus) reader["OrderStatus"];
            }
            if (DBNull.Value != reader["CloseReason"])
            {
                info.CloseReason = (string) reader["CloseReason"];
            }
            if (DBNull.Value != reader["OrderPoint"])
            {
                info.Points = (int) reader["OrderPoint"];
            }
            info.OrderDate = (DateTime) reader["OrderDate"];
            if (DBNull.Value != reader["PayDate"])
            {
                info.PayDate = (DateTime) reader["PayDate"];
            }
            if (DBNull.Value != reader["ShippingDate"])
            {
                info.ShippingDate = (DateTime) reader["ShippingDate"];
            }
            if (DBNull.Value != reader["FinishDate"])
            {
                info.FinishDate = (DateTime) reader["FinishDate"];
            }
            info.UserId = (int) reader["UserId"];
            info.Username = (string) reader["Username"];
            if (DBNull.Value != reader["EmailAddress"])
            {
                info.EmailAddress = (string) reader["EmailAddress"];
            }
            if (DBNull.Value != reader["RealName"])
            {
                info.RealName = (string) reader["RealName"];
            }
            if (DBNull.Value != reader["QQ"])
            {
                info.QQ = (string) reader["QQ"];
            }
            if (DBNull.Value != reader["Wangwang"])
            {
                info.Wangwang = (string) reader["Wangwang"];
            }
            if (DBNull.Value != reader["MSN"])
            {
                info.MSN = (string) reader["MSN"];
            }
            if (DBNull.Value != reader["ShippingRegion"])
            {
                info.ShippingRegion = (string) reader["ShippingRegion"];
            }
            if (DBNull.Value != reader["Address"])
            {
                info.Address = (string) reader["Address"];
            }
            if (DBNull.Value != reader["ZipCode"])
            {
                info.ZipCode = (string) reader["ZipCode"];
            }
            if (DBNull.Value != reader["ShipTo"])
            {
                info.ShipTo = (string) reader["ShipTo"];
            }
            if (DBNull.Value != reader["TelPhone"])
            {
                info.TelPhone = (string) reader["TelPhone"];
            }
            if (DBNull.Value != reader["CellPhone"])
            {
                info.CellPhone = (string) reader["CellPhone"];
            }
            if (DBNull.Value != reader["ShippingModeId"])
            {
                info.ShippingModeId = (int) reader["ShippingModeId"];
            }
            if (DBNull.Value != reader["ModeName"])
            {
                info.ModeName = (string) reader["ModeName"];
            }
            if (DBNull.Value != reader["RealShippingModeId"])
            {
                info.RealShippingModeId = (int) reader["RealShippingModeId"];
            }
            if (DBNull.Value != reader["RealModeName"])
            {
                info.RealModeName = (string) reader["RealModeName"];
            }
            if (DBNull.Value != reader["RegionId"])
            {
                info.RegionId = (int) reader["RegionId"];
            }
            if (DBNull.Value != reader["Freight"])
            {
                info.Freight = (decimal) reader["Freight"];
            }
            if (DBNull.Value != reader["AdjustedFreight"])
            {
                info.AdjustedFreight = (decimal) reader["AdjustedFreight"];
            }
            if (DBNull.Value != reader["ShipOrderNumber"])
            {
                info.ShipOrderNumber = (string) reader["ShipOrderNumber"];
            }
            if (DBNull.Value != reader["ExpressCompanyName"])
            {
                info.ExpressCompanyName = (string) reader["ExpressCompanyName"];
            }
            if (DBNull.Value != reader["ExpressCompanyAbb"])
            {
                info.ExpressCompanyAbb = (string) reader["ExpressCompanyAbb"];
            }
            if (DBNull.Value != reader["PaymentTypeId"])
            {
                info.PaymentTypeId = (int) reader["PaymentTypeId"];
            }
            if (DBNull.Value != reader["PaymentType"])
            {
                info.PaymentType = (string) reader["PaymentType"];
            }
            if (DBNull.Value != reader["PayCharge"])
            {
                info.PayCharge = (decimal) reader["PayCharge"];
            }
            if (DBNull.Value != reader["AdjustedPayCharge"])
            {
                info.AdjustedPayCharge = (decimal) reader["AdjustedPayCharge"];
            }
            if (DBNull.Value != reader["RefundStatus"])
            {
                info.RefundStatus = (RefundStatus) reader["RefundStatus"];
            }
            if (DBNull.Value != reader["RefundAmount"])
            {
                info.RefundAmount = (decimal) reader["RefundAmount"];
            }
            if (DBNull.Value != reader["RefundRemark"])
            {
                info.RefundRemark = (string) reader["RefundRemark"];
            }
            if (DBNull.Value != reader["ActivityName"])
            {
                info.ActivityName = (string) reader["ActivityName"];
            }
            if (DBNull.Value != reader["ActivityId"])
            {
                info.ActivityId = (int) reader["ActivityId"];
            }
            if (DBNull.Value != reader["EightFree"])
            {
                info.EightFree = (bool) reader["EightFree"];
            }
            if (DBNull.Value != reader["ProcedureFeeFree"])
            {
                info.ProcedureFeeFree = (bool) reader["ProcedureFeeFree"];
            }
            if (DBNull.Value != reader["OrderOptionFree"])
            {
                info.OrderOptionFree = (bool) reader["OrderOptionFree"];
            }
            if (DBNull.Value != reader["DiscountName"])
            {
                info.DiscountName = (string) reader["DiscountName"];
            }
            if (DBNull.Value != reader["DiscountId"])
            {
                info.DiscountId = (int) reader["DiscountId"];
            }
            if (DBNull.Value != reader["DiscountValue"])
            {
                info.DiscountValue = (decimal) reader["DiscountValue"];
            }
            if (DBNull.Value != reader["DiscountAmount"])
            {
                info.DiscountAmount = (decimal) reader["DiscountAmount"];
            }
            if (DBNull.Value != reader["DiscountValueType"])
            {
                info.DiscountValueType = (DiscountValueType) reader["DiscountValueType"];
            }
            if (DBNull.Value != reader["CouponName"])
            {
                info.CouponName = (string) reader["CouponName"];
            }
            if (DBNull.Value != reader["CouponCode"])
            {
                info.CouponCode = (string) reader["CouponCode"];
            }
            if (DBNull.Value != reader["CouponAmount"])
            {
                info.CouponAmount = (decimal) reader["CouponAmount"];
            }
            if (DBNull.Value != reader["CouponValue"])
            {
                info.CouponValue = (decimal) reader["CouponValue"];
            }
            if (DBNull.Value != reader["GroupBuyId"])
            {
                info.GroupBuyId = (int) reader["GroupBuyId"];
            }
            if (DBNull.Value != reader["NeedPrice"])
            {
                info.NeedPrice = (decimal) reader["NeedPrice"];
            }
            if (DBNull.Value != reader["GroupBuyStatus"])
            {
                info.GroupBuyStatus = (GroupBuyStatus) reader["GroupBuyStatus"];
            }
            return info;
        }

        public static OrderGiftInfo PopulateOrderGift(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            OrderGiftInfo info2 = new OrderGiftInfo();
            info2.OrderId = (string) reader["OrderId"];
            info2.GiftId = (int) reader["GiftId"];
            info2.GiftName = (string) reader["GiftName"];
            info2.CostPrice = (reader["CostPrice"] == DBNull.Value) ? 0M : ((decimal) reader["CostPrice"]);
            info2.ThumbnailsUrl = (reader["ThumbnailsUrl"] == DBNull.Value) ? string.Empty : ((string) reader["ThumbnailsUrl"]);
            info2.Quantity = (reader["Quantity"] == DBNull.Value) ? 0 : ((int) reader["Quantity"]);
            return info2;
        }

        public static OrderLookupItemInfo PopulateOrderLookupItem(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            OrderLookupItemInfo info = new OrderLookupItemInfo();
            info.LookupListId = (int) reader["LookupListId"];
            info.LookupItemId = (int) reader["LookupItemId"];
            info.Name = (string) reader["Name"];
            info.IsUserInputRequired = (bool) reader["IsUserInputRequired"];
            if (DBNull.Value != reader["UserInputTitle"])
            {
                info.UserInputTitle = (string) reader["UserInputTitle"];
            }
            if (DBNull.Value != reader["AppendMoney"])
            {
                info.AppendMoney = new decimal?((decimal) reader["AppendMoney"]);
            }
            if (DBNull.Value != reader["CalculateMode"])
            {
                info.CalculateMode = new int?((int) reader["CalculateMode"]);
            }
            if (DBNull.Value != reader["Remark"])
            {
                info.Remark = (string) reader["Remark"];
            }
            return info;
        }

        public static OrderLookupListInfo PopulateOrderLookupList(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            OrderLookupListInfo info = new OrderLookupListInfo();
            info.Name = (string) reader["Name"];
            info.LookupListId = (int) reader["LookupListId"];
            info.SelectMode = (SelectModeTypes) reader["SelectMode"];
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            return info;
        }

        public static OrderOptionInfo PopulateOrderOption(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            OrderOptionInfo info = new OrderOptionInfo();
            info.OrderId = (string) reader["OrderId"];
            info.LookupListId = (int) reader["LookupListId"];
            info.LookupItemId = (int) reader["LookupItemId"];
            info.ListDescription = (string) reader["ListDescription"];
            info.ItemDescription = (string) reader["ItemDescription"];
            info.AdjustedPrice = (decimal) reader["AdjustedPrice"];
            if (reader["CustomerTitle"] != DBNull.Value)
            {
                info.CustomerTitle = (string) reader["CustomerTitle"];
            }
            if (reader["CustomerDescription"] != DBNull.Value)
            {
                info.CustomerDescription = (string) reader["CustomerDescription"];
            }
            return info;
        }

        public static PaymentModeInfo PopulatePayment(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            PaymentModeInfo info2 = new PaymentModeInfo();
            info2.ModeId = (int) reader["ModeId"];
            info2.Name = (string) reader["Name"];
            info2.DisplaySequence = (int) reader["DisplaySequence"];
            info2.IsUseInpour = (bool) reader["IsUseInpour"];
            info2.Charge = (decimal) reader["Charge"];
            info2.IsPercent = (bool) reader["IsPercent"];
            PaymentModeInfo info = info2;
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            if (reader["Gateway"] != DBNull.Value)
            {
                info.Gateway = (string) reader["Gateway"];
            }
            if (reader["Settings"] != DBNull.Value)
            {
                info.Settings = (string) reader["Settings"];
            }
            return info;
        }

        public static ProductInfo PopulateProduct(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            ProductInfo info = new ProductInfo();
            info.CategoryId = (int) reader["CategoryId"];
            info.ProductId = (int) reader["ProductId"];
            if (DBNull.Value != reader["TypeId"])
            {
                info.TypeId = new int?((int) reader["TypeId"]);
            }
            info.ProductName = (string) reader["ProductName"];
            if (DBNull.Value != reader["ProductCode"])
            {
                info.ProductCode = (string) reader["ProductCode"];
            }
            if (DBNull.Value != reader["ShortDescription"])
            {
                info.ShortDescription = (string) reader["ShortDescription"];
            }
            if (DBNull.Value != reader["Unit"])
            {
                info.Unit = (string) reader["Unit"];
            }
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            if (DBNull.Value != reader["Title"])
            {
                info.Title = (string) reader["Title"];
            }
            if (DBNull.Value != reader["Meta_Description"])
            {
                info.MetaDescription = (string) reader["Meta_Description"];
            }
            if (DBNull.Value != reader["Meta_Keywords"])
            {
                info.MetaKeywords = (string) reader["Meta_Keywords"];
            }
            info.SaleStatus = (ProductSaleStatus) ((int) reader["SaleStatus"]);
            info.AddedDate = (DateTime) reader["AddedDate"];
            info.VistiCounts = (int) reader["VistiCounts"];
            info.SaleCounts = (int) reader["SaleCounts"];
            info.DisplaySequence = (int) reader["DisplaySequence"];
            if (DBNull.Value != reader["ImageUrl1"])
            {
                info.ImageUrl1 = (string) reader["ImageUrl1"];
            }
            if (DBNull.Value != reader["ImageUrl2"])
            {
                info.ImageUrl2 = (string) reader["ImageUrl2"];
            }
            if (DBNull.Value != reader["ImageUrl3"])
            {
                info.ImageUrl3 = (string) reader["ImageUrl3"];
            }
            if (DBNull.Value != reader["ImageUrl4"])
            {
                info.ImageUrl4 = (string) reader["ImageUrl4"];
            }
            if (DBNull.Value != reader["ImageUrl5"])
            {
                info.ImageUrl5 = (string) reader["ImageUrl5"];
            }
            if (DBNull.Value != reader["ThumbnailUrl40"])
            {
                info.ThumbnailUrl40 = (string) reader["ThumbnailUrl40"];
            }
            if (DBNull.Value != reader["ThumbnailUrl60"])
            {
                info.ThumbnailUrl60 = (string) reader["ThumbnailUrl60"];
            }
            if (DBNull.Value != reader["ThumbnailUrl100"])
            {
                info.ThumbnailUrl100 = (string) reader["ThumbnailUrl100"];
            }
            if (DBNull.Value != reader["ThumbnailUrl160"])
            {
                info.ThumbnailUrl160 = (string) reader["ThumbnailUrl160"];
            }
            if (DBNull.Value != reader["ThumbnailUrl180"])
            {
                info.ThumbnailUrl180 = (string) reader["ThumbnailUrl180"];
            }
            if (DBNull.Value != reader["ThumbnailUrl220"])
            {
                info.ThumbnailUrl220 = (string) reader["ThumbnailUrl220"];
            }
            if (DBNull.Value != reader["ThumbnailUrl310"])
            {
                info.ThumbnailUrl310 = (string) reader["ThumbnailUrl310"];
            }
            if (DBNull.Value != reader["ThumbnailUrl410"])
            {
                info.ThumbnailUrl410 = (string) reader["ThumbnailUrl410"];
            }
            info.LineId = (int) reader["LineId"];
            if (DBNull.Value != reader["MarketPrice"])
            {
                info.MarketPrice = new decimal?((decimal) reader["MarketPrice"]);
            }
            info.LowestSalePrice = (decimal) reader["LowestSalePrice"];
            info.PenetrationStatus = (PenetrationStatus) Convert.ToInt16(reader["PenetrationStatus"]);
            if (DBNull.Value != reader["BrandId"])
            {
                info.BrandId = new int?((int) reader["BrandId"]);
            }
            if (reader["MainCategoryPath"] != DBNull.Value)
            {
                info.MainCategoryPath = (string) reader["MainCategoryPath"];
            }
            if (reader["ExtendCategoryPath"] != DBNull.Value)
            {
                info.ExtendCategoryPath = (string) reader["ExtendCategoryPath"];
            }
            info.HasSKU = (bool) reader["HasSKU"];
            return info;
        }

        public static CategoryInfo PopulateProductCategory(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            CategoryInfo info = new CategoryInfo();
            info.CategoryId = (int) reader["CategoryId"];
            info.Name = (string) reader["Name"];
            info.DisplaySequence = (int) reader["DisplaySequence"];
            if (reader["Meta_Description"] != DBNull.Value)
            {
                info.MetaDescription = (string) reader["Meta_Description"];
            }
            if (reader["Meta_Keywords"] != DBNull.Value)
            {
                info.MetaKeywords = (string) reader["Meta_Keywords"];
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            if (reader["Notes1"] != DBNull.Value)
            {
                info.Notes1 = (string) reader["Notes1"];
            }
            if (reader["Notes2"] != DBNull.Value)
            {
                info.Notes2 = (string) reader["Notes2"];
            }
            if (reader["Notes3"] != DBNull.Value)
            {
                info.Notes3 = (string) reader["Notes3"];
            }
            if (reader["Notes4"] != DBNull.Value)
            {
                info.Notes4 = (string) reader["Notes4"];
            }
            if (reader["Notes5"] != DBNull.Value)
            {
                info.Notes5 = (string) reader["Notes5"];
            }
            if (reader["ParentCategoryId"] != DBNull.Value)
            {
                info.ParentCategoryId = new int?((int) reader["ParentCategoryId"]);
            }
            info.Depth = (int) reader["Depth"];
            info.Path = (string) reader["Path"];
            if (reader["RewriteName"] != DBNull.Value)
            {
                info.RewriteName = (string) reader["RewriteName"];
            }
            if (reader["SKUPrefix"] != DBNull.Value)
            {
                info.SKUPrefix = (string) reader["SKUPrefix"];
            }
            if (reader["Theme"] != DBNull.Value)
            {
                info.Theme = (string) reader["Theme"];
            }
            info.HasChildren = (bool) reader["HasChildren"];
            return info;
        }

        public static ProductConsultationInfo PopulateProductConsultation(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ProductConsultationInfo info = new ProductConsultationInfo();
            info.ConsultationId = (int) reader["ConsultationId"];
            info.ProductId = (int) reader["ProductId"];
            info.UserId = (int) reader["UserId"];
            info.UserName = (string) reader["UserName"];
            info.ConsultationText = (string) reader["ConsultationText"];
            info.ConsultationDate = (DateTime) reader["ConsultationDate"];
            info.UserEmail = (string) reader["UserEmail"];
            if (DBNull.Value != reader["ReplyText"])
            {
                info.ReplyText = (string) reader["ReplyText"];
            }
            if (DBNull.Value != reader["ReplyDate"])
            {
                info.ReplyDate = new DateTime?((DateTime) reader["ReplyDate"]);
            }
            if (DBNull.Value != reader["ReplyUserId"])
            {
                info.ReplyUserId = new int?((int) reader["ReplyUserId"]);
            }
            return info;
        }

        public static ProductLineInfo PopulateProductLine(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ProductLineInfo info2 = new ProductLineInfo();
            info2.LineId = (int) reader["LineId"];
            info2.Name = (string) reader["Name"];
            ProductLineInfo info = info2;
            if (reader["SupplierName"] != DBNull.Value)
            {
                info.SupplierName = (string) reader["SupplierName"];
            }
            return info;
        }

        public static ProductReviewInfo PopulateProductReview(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ProductReviewInfo info = new ProductReviewInfo();
            info.ReviewId = (long) reader["ReviewId"];
            info.ProductId = (int) reader["ProductId"];
            info.UserId = (int) reader["UserId"];
            info.ReviewText = (string) reader["ReviewText"];
            info.UserName = (string) reader["UserName"];
            info.UserEmail = (string) reader["UserEmail"];
            info.ReviewDate = (DateTime) reader["ReviewDate"];
            return info;
        }

        public static PromotionInfo PopulatePromote(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            PromotionInfo info = null;
            switch (((PromoteType) reader["PromoteType"]))
            {
                case PromoteType.FullDiscount:
                    info = new FullDiscountInfo();
                    break;

                case PromoteType.FullFree:
                    info = new FullFreeInfo();
                    break;

                case PromoteType.PurchaseGift:
                    info = new PurchaseGiftInfo();
                    break;

                case PromoteType.WholesaleDiscount:
                    info = new WholesaleDiscountInfo();
                    break;
            }
            info.ActivityId = (int) reader["ActivityId"];
            info.Name = (string) reader["Name"];
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            return info;
        }

        public static PurchaseGiftInfo PopulatePurchaseGift(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            PurchaseGiftInfo info = new PurchaseGiftInfo();
            info.ActivityId = (int) reader["ActivityId"];
            info.Name = (string) reader["Name"];
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            info.BuyQuantity = (int) reader["BuyQuantity"];
            info.GiveQuantity = (int) reader["GiveQuantity"];
            return info;
        }

        public static PurchaseOrderInfo PopulatePurchaseOrder(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            PurchaseOrderInfo info = new PurchaseOrderInfo();
            info.PurchaseOrderId = (string) reader["PurchaseOrderId"];
            if (DBNull.Value != reader["OrderId"])
            {
                info.OrderId = (string) reader["OrderId"];
            }
            if (DBNull.Value != reader["ManagerMark"])
            {
                info.ManagerMark = new OrderMark?((OrderMark) reader["ManagerMark"]);
            }
            if (DBNull.Value != reader["Remark"])
            {
                info.Remark = (string) reader["Remark"];
            }
            if (DBNull.Value != reader["ManagerRemark"])
            {
                info.ManagerRemark = (string) reader["ManagerRemark"];
            }
            if (DBNull.Value != reader["AdjustedDiscount"])
            {
                info.AdjustedDiscount = (decimal) reader["AdjustedDiscount"];
            }
            if (DBNull.Value != reader["PurchaseStatus"])
            {
                info.PurchaseStatus = (OrderStatus) reader["PurchaseStatus"];
            }
            if (DBNull.Value != reader["CloseReason"])
            {
                info.CloseReason = (string) reader["CloseReason"];
            }
            info.PurchaseDate = (DateTime) reader["PurchaseDate"];
            if (DBNull.Value != reader["PayDate"])
            {
                info.PayDate = (DateTime) reader["PayDate"];
            }
            if (DBNull.Value != reader["ShippingDate"])
            {
                info.ShippingDate = (DateTime) reader["ShippingDate"];
            }
            if (DBNull.Value != reader["FinishDate"])
            {
                info.FinishDate = (DateTime) reader["FinishDate"];
            }
            info.DistributorId = (int) reader["DistributorId"];
            info.Distributorname = (string) reader["Distributorname"];
            if (DBNull.Value != reader["DistributorEmail"])
            {
                info.DistributorEmail = (string) reader["DistributorEmail"];
            }
            if (DBNull.Value != reader["DistributorRealName"])
            {
                info.DistributorRealName = (string) reader["DistributorRealName"];
            }
            if (DBNull.Value != reader["DistributorQQ"])
            {
                info.DistributorQQ = (string) reader["DistributorQQ"];
            }
            if (DBNull.Value != reader["DistributorWangwang"])
            {
                info.DistributorWangwang = (string) reader["DistributorWangwang"];
            }
            if (DBNull.Value != reader["DistributorMSN"])
            {
                info.DistributorMSN = (string) reader["DistributorMSN"];
            }
            if (DBNull.Value != reader["ShippingRegion"])
            {
                info.ShippingRegion = (string) reader["ShippingRegion"];
            }
            if (DBNull.Value != reader["Address"])
            {
                info.Address = (string) reader["Address"];
            }
            if (DBNull.Value != reader["ZipCode"])
            {
                info.ZipCode = (string) reader["ZipCode"];
            }
            if (DBNull.Value != reader["ShipTo"])
            {
                info.ShipTo = (string) reader["ShipTo"];
            }
            if (DBNull.Value != reader["TelPhone"])
            {
                info.TelPhone = (string) reader["TelPhone"];
            }
            if (DBNull.Value != reader["CellPhone"])
            {
                info.CellPhone = (string) reader["CellPhone"];
            }
            if (DBNull.Value != reader["ShippingModeId"])
            {
                info.ShippingModeId = (int) reader["ShippingModeId"];
            }
            if (DBNull.Value != reader["ModeName"])
            {
                info.ModeName = (string) reader["ModeName"];
            }
            if (DBNull.Value != reader["RealShippingModeId"])
            {
                info.RealShippingModeId = (int) reader["RealShippingModeId"];
            }
            if (DBNull.Value != reader["RealModeName"])
            {
                info.RealModeName = (string) reader["RealModeName"];
            }
            if (DBNull.Value != reader["RegionId"])
            {
                info.RegionId = (int) reader["RegionId"];
            }
            if (DBNull.Value != reader["Freight"])
            {
                info.Freight = (decimal) reader["Freight"];
            }
            if (DBNull.Value != reader["AdjustedFreight"])
            {
                info.AdjustedFreight = (decimal) reader["AdjustedFreight"];
            }
            if (DBNull.Value != reader["ShipOrderNumber"])
            {
                info.ShipOrderNumber = (string) reader["ShipOrderNumber"];
            }
            if (DBNull.Value != reader["Weight"])
            {
                info.Weight = (int) reader["Weight"];
            }
            if (DBNull.Value != reader["RefundStatus"])
            {
                info.RefundStatus = (RefundStatus) reader["RefundStatus"];
            }
            if (DBNull.Value != reader["RefundAmount"])
            {
                info.RefundAmount = (decimal) reader["RefundAmount"];
            }
            if (DBNull.Value != reader["RefundRemark"])
            {
                info.RefundRemark = (string) reader["RefundRemark"];
            }
            if (DBNull.Value != reader["OrderTotal"])
            {
                info.OrderTotal = (decimal) reader["OrderTotal"];
            }
            if (DBNull.Value != reader["ExpressCompanyName"])
            {
                info.ExpressCompanyName = (string) reader["ExpressCompanyName"];
            }
            if (DBNull.Value != reader["ExpressCompanyAbb"])
            {
                info.ExpressCompanyAbb = (string) reader["ExpressCompanyAbb"];
            }
            return info;
        }

        public static PurchaseOrderGiftInfo PopulatePurchaseOrderGift(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            PurchaseOrderGiftInfo info = new PurchaseOrderGiftInfo();
            info.PurchaseOrderId = (string) reader["PurchaseOrderId"];
            info.GiftId = (int) reader["GiftId"];
            info.GiftName = (string) reader["GiftName"];
            info.CostPrice = (reader["CostPrice"] == DBNull.Value) ? 0M : ((decimal) reader["CostPrice"]);
            info.PurchasePrice = (reader["PurchasePrice"] == DBNull.Value) ? 0M : ((decimal) reader["PurchasePrice"]);
            info.ThumbnailsUrl = (reader["ThumbnailsUrl"] == DBNull.Value) ? string.Empty : ((string) reader["ThumbnailsUrl"]);
            info.Quantity = (reader["Quantity"] == DBNull.Value) ? 0 : ((int) reader["Quantity"]);
            return info;
        }

        public static PurchaseOrderItemInfo PopulatePurchaseOrderItem(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            PurchaseOrderItemInfo info = new PurchaseOrderItemInfo();
            info.PurchaseOrderId = (string) reader["PurchaseOrderId"];
            info.SkuId = (string) reader["SkuId"];
            info.ProductId = (int) reader["ProductId"];
            if (DBNull.Value != reader["SKU"])
            {
                info.SKU = (string) reader["SKU"];
            }
            info.Quantity = (int) reader["Quantity"];
            info.ItemCostPrice = (decimal) reader["CostPrice"];
            info.ItemListPrice = (decimal) reader["ItemListPrice"];
            info.ItemPurchasePrice = (decimal) reader["ItemPurchasePrice"];
            info.ItemDescription = (string) reader["ItemDescription"];
            info.ItemHomeSiteDescription = (string) reader["ItemHomeSiteDescription"];
            if (reader["ThumbnailsUrl"] != DBNull.Value)
            {
                info.ThumbnailsUrl = (string) reader["ThumbnailsUrl"];
            }
            info.ItemWeight = (reader["Weight"] == DBNull.Value) ? 0 : ((int) reader["Weight"]);
            if (reader["SKUContent"] != DBNull.Value)
            {
                info.SKUContent = (string) reader["SKUContent"];
            }
            return info;
        }

        public static PurchaseOrderOptionInfo PopulatePurchaseOrderOption(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            PurchaseOrderOptionInfo info = new PurchaseOrderOptionInfo();
            info.PurchaseOrderId = (string) reader["PurchaseOrderId"];
            info.LookupListId = (int) reader["LookupListId"];
            info.LookupItemId = (int) reader["LookupItemId"];
            info.ListDescription = (string) reader["ListDescription"];
            info.ItemDescription = (string) reader["ItemDescription"];
            info.AdjustedPrice = (decimal) reader["AdjustedPrice"];
            if (reader["CustomerTitle"] != DBNull.Value)
            {
                info.CustomerTitle = (string) reader["CustomerTitle"];
            }
            if (reader["CustomerDescription"] != DBNull.Value)
            {
                info.CustomerDescription = (string) reader["CustomerDescription"];
            }
            return info;
        }

        public static PurchaseShoppingCartItemInfo PopulatePurchaseShoppingCartItemInfo(IDataReader reader)
        {
            if (null == reader)
            {
                return null;
            }
            PurchaseShoppingCartItemInfo info = new PurchaseShoppingCartItemInfo();
            info.SkuId = (string) reader["SkuId"];
            info.SKUContent = (reader["SKUContent"] != DBNull.Value) ? ((string) reader["SKUContent"]) : string.Empty;
            info.SKU = (reader["SKU"] != DBNull.Value) ? ((string) reader["SKU"]) : string.Empty;
            info.ProductId = (int) reader["ProductId"];
            info.ThumbnailsUrl = (reader["ThumbnailsUrl"] != DBNull.Value) ? ((string) reader["ThumbnailsUrl"]) : string.Empty;
            info.Quantity = (int) reader["Quantity"];
            info.ItemDescription = (string) reader["ItemDescription"];
            info.ItemWeight = (int) reader["Weight"];
            info.ItemListPrice = (decimal) reader["ItemListPrice"];
            info.ItemPurchasePrice = (decimal) reader["ItemPurchasePrice"];
            if (DBNull.Value != reader["CostPrice"])
            {
                info.CostPrice = (decimal) reader["CostPrice"];
            }
            return info;
        }

        public static ReceiveMessageInfo PopulateReceiveMessage(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ReceiveMessageInfo info = new ReceiveMessageInfo();
            info.ReceiveMessageId = (long) reader["ReceiveMessageId"];
            info.Addresser = (string) reader["Addresser"];
            info.Addressee = (string) reader["Addressee"];
            info.Title = (string) reader["Title"];
            info.PublishContent = (string) reader["PublishContent"];
            info.PublishDate = (DateTime) reader["PublishDate"];
            info.LastTime = (DateTime) reader["LastTime"];
            info.IsRead = (bool) reader["IsRead"];
            return info;
        }

        public static SendMessageInfo PopulateSendMessage(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            SendMessageInfo info = new SendMessageInfo();
            info.SendMessageId = (long) reader["SendMessageId"];
            info.Addressee = (string) reader["Addressee"];
            info.Addresser = (string) reader["Addresser"];
            info.PublishContent = (string) reader["PublishContent"];
            info.Title = (string) reader["Title"];
            info.PublishDate = (DateTime) reader["PublishDate"];
            if (DBNull.Value != reader["ReceiveMessageId"])
            {
                info.ReceiveMessageId = new long?((long) reader["ReceiveMessageId"]);
            }
            return info;
        }

        public static ShippersInfo PopulateShipper(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ShippersInfo info = new ShippersInfo();
            info.ShipperId = (int) reader["ShipperId"];
            info.DistributorUserId = (int) reader["DistributorUserId"];
            info.IsDefault = (bool) reader["IsDefault"];
            info.ShipperTag = (string) reader["ShipperTag"];
            info.ShipperName = (string) reader["ShipperName"];
            info.RegionId = (int) reader["RegionId"];
            info.Address = (string) reader["Address"];
            if (reader["CellPhone"] != DBNull.Value)
            {
                info.CellPhone = (string) reader["CellPhone"];
            }
            if (reader["TelPhone"] != DBNull.Value)
            {
                info.TelPhone = (string) reader["TelPhone"];
            }
            if (reader["Zipcode"] != DBNull.Value)
            {
                info.Zipcode = (string) reader["Zipcode"];
            }
            if (reader["Remark"] != DBNull.Value)
            {
                info.Remark = (string) reader["Remark"];
            }
            return info;
        }

        public static ShippingAddressInfo PopulateShippingAddress(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ShippingAddressInfo info = new ShippingAddressInfo();
            info.ShippingId = (int) reader["ShippingId"];
            info.ShipTo = (string) reader["ShipTo"];
            info.RegionId = (int) reader["RegionId"];
            info.UserId = (int) reader["UserId"];
            info.Address = (string) reader["Address"];
            info.Zipcode = (string) reader["Zipcode"];
            if (reader["TelPhone"] != DBNull.Value)
            {
                info.TelPhone = (string) reader["TelPhone"];
            }
            if (reader["CellPhone"] != DBNull.Value)
            {
                info.CellPhone = (string) reader["CellPhone"];
            }
            return info;
        }

        public static ShippingModeInfo PopulateShippingMode(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ShippingModeInfo info = new ShippingModeInfo();
            if (reader["ModeId"] != DBNull.Value)
            {
                info.ModeId = (int) reader["ModeId"];
            }
            if (reader["TemplateId"] != DBNull.Value)
            {
                info.TemplateId = (int) reader["TemplateId"];
            }
            info.Name = (string) reader["Name"];
            info.TemplateName = (string) reader["TemplateName"];
            info.Weight = (int) reader["Weight"];
            if (DBNull.Value != reader["AddWeight"])
            {
                info.AddWeight = new int?((int) reader["AddWeight"]);
            }
            info.Price = (decimal) reader["Price"];
            if (DBNull.Value != reader["AddPrice"])
            {
                info.AddPrice = new decimal?((decimal) reader["AddPrice"]);
            }
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            info.DisplaySequence = (int) reader["DisplaySequence"];
            return info;
        }

        public static ShippingModeGroupInfo PopulateShippingModeGroup(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ShippingModeGroupInfo info = new ShippingModeGroupInfo();
            info.TemplateId = (int) reader["TemplateId"];
            info.GroupId = (int) reader["GroupId"];
            info.Price = (decimal) reader["Price"];
            if (DBNull.Value != reader["AddPrice"])
            {
                info.AddPrice = (decimal) reader["AddPrice"];
            }
            return info;
        }

        public static ShippingRegionInfo PopulateShippingRegion(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ShippingRegionInfo info = new ShippingRegionInfo();
            info.TemplateId = (int) reader["TemplateId"];
            info.GroupId = (int) reader["GroupId"];
            info.RegionId = (int) reader["RegionId"];
            return info;
        }

        public static ShippingModeInfo PopulateShippingTemplate(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            ShippingModeInfo info = new ShippingModeInfo();
            if (reader["TemplateId"] != DBNull.Value)
            {
                info.TemplateId = (int) reader["TemplateId"];
            }
            info.Name = (string) reader["TemplateName"];
            info.Weight = (int) reader["Weight"];
            if (DBNull.Value != reader["AddWeight"])
            {
                info.AddWeight = new int?((int) reader["AddWeight"]);
            }
            info.Price = (decimal) reader["Price"];
            if (DBNull.Value != reader["AddPrice"])
            {
                info.AddPrice = new decimal?((decimal) reader["AddPrice"]);
            }
            return info;
        }

        public static SKUItem PopulateSKU(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            SKUItem item = new SKUItem();
            item.SkuId = (string) reader["SkuId"];
            item.ProductId = (int) reader["ProductId"];
            if (reader["SKU"] != DBNull.Value)
            {
                item.SKU = (string) reader["SKU"];
            }
            if (reader["Weight"] != DBNull.Value)
            {
                item.Weight = (int) reader["Weight"];
            }
            item.Stock = (int) reader["Stock"];
            item.AlertStock = (int) reader["AlertStock"];
            if (reader["CostPrice"] != DBNull.Value)
            {
                item.CostPrice = (decimal) reader["CostPrice"];
            }
            item.SalePrice = (decimal) reader["SalePrice"];
            item.PurchasePrice = (decimal) reader["PurchasePrice"];
            return item;
        }

        public static StatisticsInfo PopulateStatistics(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            StatisticsInfo info = new StatisticsInfo();
            info.OrderNumbWaitConsignment = (int) reader["orderNumbWaitConsignment"];
            info.ApplyRequestWaitDispose = (int) reader["ApplyRequestWaitDispose"];
            info.ProductNumStokWarning = (int) reader["ProductNumStokWarning"];
            info.PurchaseOrderNumbWaitConsignment = (int) reader["purchaseOrderNumbWaitConsignment"];
            info.LeaveComments = (int) reader["LeaveComments"];
            info.ProductConsultations = (int) reader["ProductConsultations"];
            info.Messages = (int) reader["Messages"];
            info.OrderNumbToday = (int) reader["OrderNumbToday"];
            info.OrderPriceToday = (decimal) reader["OrderPriceToday"];
            info.OrderProfitToday = (decimal) reader["OrderProfitToday"];
            info.UserNewAddToday = (int) reader["UserNewAddToday"];
            info.DistroButorsNewAddToday = (int) reader["AgentNewAddToday"];
            info.UserNumbBirthdayToday = (int) reader["userNumbBirthdayToday"];
            info.OrderNumbYesterday = (int) reader["OrderNumbYesterday"];
            info.OrderPriceYesterday = (decimal) reader["OrderPriceYesterday"];
            info.OrderProfitYesterday = (decimal) reader["OrderProfitYesterday"];
            info.UserNumb = (int) reader["UserNumb"];
            info.DistroButorsNumb = (int) reader["AgentNumb"];
            info.Balance = (decimal) reader["memberBalance"];
            info.BalanceDrawRequested = (decimal) reader["BalanceDrawRequested"];
            info.ProductNumbOnSale = (int) reader["ProductNumbOnSale"];
            info.ProductNumbInStock = (int) reader["ProductNumbInStock"];
            info.ProductAlert = (int) reader["ProductAlert"];
            if (reader["authorizeProductCount"] != DBNull.Value)
            {
                info.AuthorizeProductCount = (int) reader["authorizeProductCount"];
            }
            if (reader["arealdyPaidNum"] != DBNull.Value)
            {
                info.AlreadyPaidOrdersNum = (int) reader["arealdyPaidNum"];
            }
            if (reader["arealdyPaidTotal"] != DBNull.Value)
            {
                info.AreadyPaidOrdersAmount = (decimal) reader["arealdyPaidTotal"];
            }
            return info;
        }

        public static ProductInfo PopulateSubProduct(IDataReader reader)
        {
            if (reader == null)
            {
                return null;
            }
            ProductInfo info = new ProductInfo();
            info.CategoryId = (int) reader["CategoryId"];
            info.ProductId = (int) reader["ProductId"];
            if (DBNull.Value != reader["TypeId"])
            {
                info.TypeId = new int?((int) reader["TypeId"]);
            }
            info.ProductName = (string) reader["ProductName"];
            if (DBNull.Value != reader["ProductCode"])
            {
                info.ProductCode = (string) reader["ProductCode"];
            }
            if (DBNull.Value != reader["ShortDescription"])
            {
                info.ShortDescription = (string) reader["ShortDescription"];
            }
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            if (DBNull.Value != reader["Title"])
            {
                info.Title = (string) reader["Title"];
            }
            if (DBNull.Value != reader["Meta_Description"])
            {
                info.MetaDescription = (string) reader["Meta_Description"];
            }
            if (DBNull.Value != reader["Meta_Keywords"])
            {
                info.MetaKeywords = (string) reader["Meta_Keywords"];
            }
            if (DBNull.Value != reader["ThumbnailUrl40"])
            {
                info.ThumbnailUrl40 = (string) reader["ThumbnailUrl40"];
            }
            if (DBNull.Value != reader["ThumbnailUrl60"])
            {
                info.ThumbnailUrl60 = (string) reader["ThumbnailUrl60"];
            }
            if (DBNull.Value != reader["ThumbnailUrl100"])
            {
                info.ThumbnailUrl100 = (string) reader["ThumbnailUrl100"];
            }
            if (DBNull.Value != reader["ThumbnailUrl160"])
            {
                info.ThumbnailUrl160 = (string) reader["ThumbnailUrl160"];
            }
            if (DBNull.Value != reader["ThumbnailUrl180"])
            {
                info.ThumbnailUrl180 = (string) reader["ThumbnailUrl180"];
            }
            if (DBNull.Value != reader["ThumbnailUrl220"])
            {
                info.ThumbnailUrl220 = (string) reader["ThumbnailUrl220"];
            }
            if (DBNull.Value != reader["ThumbnailUrl310"])
            {
                info.ThumbnailUrl310 = (string) reader["ThumbnailUrl310"];
            }
            if (DBNull.Value != reader["ThumbnailUrl410"])
            {
                info.ThumbnailUrl410 = (string) reader["ThumbnailUrl410"];
            }
            if (DBNull.Value != reader["MarketPrice"])
            {
                info.MarketPrice = new decimal?((decimal) reader["MarketPrice"]);
            }
            info.LowestSalePrice = (decimal) reader["LowestSalePrice"];
            if (DBNull.Value != reader["BrandId"])
            {
                info.BrandId = new int?((int) reader["BrandId"]);
            }
            info.SaleStatus = (ProductSaleStatus) ((int) reader["SaleStatus"]);
            info.PenetrationStatus = (PenetrationStatus) Convert.ToInt16(reader["PenetrationStatus"]);
            info.VistiCounts = (int) reader["VistiCounts"];
            info.DisplaySequence = (int) reader["DisplaySequence"];
            info.LineId = (int) reader["LineId"];
            return info;
        }

        public static UserStatisticsInfo PopulateUserStatistics(IDataRecord reader)
        {
            if (null == reader)
            {
                return null;
            }
            UserStatisticsInfo info = new UserStatisticsInfo();
            if (reader["RegionId"] != DBNull.Value)
            {
                info.RegionId = (int) reader["RegionId"];
            }
            if (reader["Usercounts"] != DBNull.Value)
            {
                info.Usercounts = (int) reader["Usercounts"];
            }
            if (reader["AllUserCounts"] != DBNull.Value)
            {
                info.AllUserCounts = Convert.ToDecimal(reader["AllUserCounts"]);
            }
            return info;
        }

        public static VoteInfo PopulateVote(IDataRecord reader)
        {
            VoteInfo info = new VoteInfo();
            info.VoteId = (long) reader["VoteId"];
            info.VoteName = (string) reader["VoteName"];
            info.IsBackup = (bool) reader["IsBackup"];
            info.MaxCheck = (int) reader["MaxCheck"];
            info.VoteCounts = (int) reader["VoteCounts"];
            return info;
        }

        public static VoteItemInfo PopulateVoteItem(IDataRecord reader)
        {
            VoteItemInfo info = new VoteItemInfo();
            info.VoteId = (long) reader["VoteId"];
            info.VoteItemId = (long) reader["VoteItemId"];
            info.VoteItemName = (string) reader["VoteItemName"];
            info.ItemCount = (int) reader["ItemCount"];
            return info;
        }

        public static WholesaleDiscountInfo PopulateWholesaleDiscount(IDataRecord reader)
        {
            if (reader == null)
            {
                return null;
            }
            WholesaleDiscountInfo info = new WholesaleDiscountInfo();
            info.ActivityId = (int) reader["ActivityId"];
            info.Name = (string) reader["Name"];
            if (DBNull.Value != reader["Description"])
            {
                info.Description = (string) reader["Description"];
            }
            info.Quantity = (int) reader["Quantity"];
            info.DiscountValue = Convert.ToInt32(reader["DiscountValue"]);
            return info;
        }

        public static DistributorGradeInfo PopulDistributorGrade(IDataReader reader)
        {
            DistributorGradeInfo info = new DistributorGradeInfo();
            info.GradeId = (int) reader["GradeId"];
            info.Discount = (int) reader["Discount"];
            info.Name = (string) reader["Name"];
            if (reader["Description"] != DBNull.Value)
            {
                info.Description = (string) reader["Description"];
            }
            return info;
        }

        public static SiteRequestInfo PopulSiteRequest(IDataReader reader)
        {
            SiteRequestInfo info = new SiteRequestInfo();
            info.RequestId = (int) reader["RequestId"];
            info.UserId = (int) reader["UserId"];
            info.FirstSiteUrl = (string) reader["FirstSiteUrl"];
            info.FirstRecordCode = (string) reader["FirstRecordCode"];
            if (reader["SecondSiteUrl"] != DBNull.Value)
            {
                info.SecondSiteUrl = (string) reader["SecondSiteUrl"];
            }
            if (reader["SecondRecordCode"] != DBNull.Value)
            {
                info.SecondRecordCode = (string) reader["SecondRecordCode"];
            }
            info.RequestTime = (DateTime) reader["RequestTime"];
            info.RequestStatus = (SiteRequestStatus) reader["RequestStatus"];
            if (reader["RefuseReason"] != DBNull.Value)
            {
                info.RefuseReason = (string) reader["RefuseReason"];
            }
            return info;
        }
    }
}

