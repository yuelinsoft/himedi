namespace Hidistro.SaleSystem.DistributionData
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Membership.Context;
    using Hidistro.SaleSystem.Catalog;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class CategoryData : CategorySubsiteProvider
    {
       Database database = DatabaseFactory.CreateDatabase();

        public override DataTable GetBrandCategories(int maxNum)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT Top {0} * FROM Hishop_BrandCategories WHERE BrandId IN (SELECT BrandId FROM distro_Products WHERE DistributorUserId  = {1}) ORDER BY DisplaySequence DESC", maxNum, HiContext.Current.SiteSettings.UserId.Value));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetBrandCategories(int categoryId, int maxNum)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" select top {0} p.brandId,b.logo,b.brandname from distro_Products p left join Hishop_BrandCategories b on p.brandId=b.brandId ", maxNum);
            builder.AppendFormat(" WHERE (CategoryId = {0} OR CategoryId IN (SELECT CategoryId FROM distro_Categories ", categoryId);
            builder.AppendFormat(" WHERE Path LIKE (SELECT Path FROM distro_Categories WHERE CategoryId = {0} AND DistributorUserId={1}) + '|%' AND DistributorUserId={2}))", categoryId, HiContext.Current.SiteSettings.UserId.Value, HiContext.Current.SiteSettings.UserId.Value);
            builder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
            builder.Append(" GROUP BY p.brandId,b.logo,b.brandname,b.displaysequence order by b.displaysequence desc ");
            Database database = DatabaseFactory.CreateDatabase();
            DbCommand sqlStringCommand = database.GetSqlStringCommand(builder.ToString());
            using (IDataReader reader = database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override BrandCategoryInfo GetBrandCategory(int brandId)
        {
            BrandCategoryInfo info = new BrandCategoryInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId ");
            this.database.AddInParameter(sqlStringCommand, "BrandId", DbType.Int32, brandId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateBrandCategory(reader);
                }
            }
            return info;
        }

        public override DataTable GetCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,NULL AS SKUPrefix FROM distro_Categories WHERE DistributorUserId=@DistributorUserId ORDER BY DisplaySequence");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataSet GetThreeLayerCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name,CategoryId,RewriteName FROM distro_Categories WHERE ParentCategoryId=0 AND Depth = 1 AND DistributorUserId=@DistributorUserId ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,CategoryId,RewriteName FROM distro_Categories WHERE  Depth = 2 AND DistributorUserId=@DistributorUserId ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,CategoryId,RewriteName FROM distro_Categories WHERE  Depth = 3 AND DistributorUserId=@DistributorUserId ORDER BY DisplaySequence;");
            this.database.AddInParameter(sqlStringCommand, "DistributorUserId", DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            set.Relations.Add("One", set.Tables[0].Columns["CategoryId"], set.Tables[1].Columns["ParentCategoryId"], false);
            set.Relations.Add("Two", set.Tables[1].Columns["CategoryId"], set.Tables[2].Columns["ParentCategoryId"], false);
            return set;
        }
    }
}

