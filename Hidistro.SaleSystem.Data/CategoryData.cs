namespace Hidistro.SaleSystem.Data
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Catalog;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class CategoryData : CategoryMasterProvider
    {
       Database database = DatabaseFactory.CreateDatabase();

        public override DataTable GetBrandCategories(int maxNum)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT Top {0} * FROM Hishop_BrandCategories ORDER BY DisplaySequence DESC", maxNum));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataTable GetBrandCategories(int categoryId, int maxNum)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(" select top {0} p.brandId,b.logo,b.brandname from Hishop_Products p left join Hishop_BrandCategories b on p.brandId=b.brandId ", maxNum);
            builder.AppendFormat(" WHERE (CategoryId = {0} OR CategoryId IN (SELECT CategoryId FROM Hishop_Categories ", categoryId);
            builder.AppendFormat(" WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '|%')) ", categoryId);
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId");
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
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Categories ORDER BY DisplaySequence");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public override DataSet GetThreeLayerCategories()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Name,CategoryId,RewriteName FROM Hishop_Categories WHERE ParentCategoryId=0 AND Depth = 1 ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,CategoryId,RewriteName FROM Hishop_Categories WHERE Depth = 2 ORDER BY DisplaySequence; SELECT ParentCategoryId,Name,CategoryId,RewriteName FROM Hishop_Categories WHERE Depth = 3 ORDER BY DisplaySequence;");
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            set.Relations.Add("One", set.Tables[0].Columns["CategoryId"], set.Tables[1].Columns["ParentCategoryId"], false);
            set.Relations.Add("Two", set.Tables[1].Columns["CategoryId"], set.Tables[2].Columns["ParentCategoryId"], false);
            return set;
        }
    }
}

