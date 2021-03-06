﻿using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Text;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Core.Enums;

namespace Hidistro.UI.Common.Data
{
    public class SqlCommonDataProvider : ControlProvider
    {
        Database database = DatabaseFactory.CreateDatabase();

        public override DataTable GetBrandCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence DESC");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetBrandCategoriesByTypeId(int typeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE ProductTypeId=@ProductTypeId ORDER BY DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "ProductTypeId", DbType.Int32, typeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override void GetMemberExpandInfo(int gradeId, string userName, out string gradeName, out int messageNum)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name FROM aspnet_MemberGrades WHERE GradeId = @GradeId;SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_ReceivedMessages WHERE Addressee = @Addressee AND IsRead=0");
            if (HiContext.Current.SiteSettings.IsDistributorSettings)
            {
                sqlStringCommand.CommandText = "SELECT Name FROM distro_MemberGrades WHERE GradeId = @GradeId;SELECT COUNT(*) AS NoReadMessageNum FROM distro_ReceivedMessages WHERE Addressee = @Addressee AND IsRead=0";
            }
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeId);
            this.database.AddInParameter(sqlStringCommand, "Addressee", DbType.String, userName);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    gradeName = (string)reader["Name"];
                }
                else
                {
                    gradeName = string.Empty;
                }
                if (reader.NextResult() && reader.Read())
                {
                    messageNum = (int)reader["NoReadMessageNum"];
                }
                else
                {
                    messageNum = 0;
                }
            }
        }

        public override IList<ProductLineInfo> GetProductLineList()
        {
            IList<ProductLineInfo> list = new List<ProductLineInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines");
            if (HiContext.Current.User.UserRole == UserRole.Distributor)
            {
                sqlStringCommand.CommandText = sqlStringCommand.CommandText + " WHERE LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = @UserId)";
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, HiContext.Current.User.UserId);
            }
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (null != reader && reader.Read())
                {
                    ProductLineInfo productLineInfo = new ProductLineInfo();
                    productLineInfo.LineId = (int)reader["LineId"];
                    productLineInfo.Name = (string)reader["Name"];
                    ProductLineInfo item = productLineInfo;
                    list.Add(item);
                }
            }
            return list;
        }

        public override IList<ProductTypeInfo> GetProductTypes()
        {
            IList<ProductTypeInfo> list = new List<ProductTypeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (null != reader && reader.Read())
                {
                    ProductTypeInfo productTypeInfo = new ProductTypeInfo();
                    productTypeInfo.TypeId = (int)reader["TypeId"];
                    productTypeInfo.TypeName = (string)reader["TypeName"];
                    productTypeInfo.Remark = (string)reader["Remark"];
                    list.Add(productTypeInfo);
                }
            }
            return list;
        }

        public override IList<ShippingModeInfo> GetShippingModes()
        {
            IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (null != reader && reader.Read())
                {
                    list.Add(DataMapper.PopulateShippingMode(reader));
                }
            }
            return list;
        }

        public override DataTable GetSkuContentBySku(string skuId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT AttributeName, ValueStr");
            builder.Append(" FROM Hishop_SKUs s join Hishop_SKUItems si on s.SkuId = si.SkuId");
            builder.Append(" join Hishop_Attributes a on si.AttributeId = a.AttributeId join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }
    }
}

