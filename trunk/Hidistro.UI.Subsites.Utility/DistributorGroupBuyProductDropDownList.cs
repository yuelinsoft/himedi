using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Subsites.Utility
{
    public class DistributorGroupBuyProductDropDownList : DropDownList
    {
        int? categoryId;
        string productCode;
        string productName;

        public override void DataBind()
        {
            Items.Clear();
            ProductQuery query = new ProductQuery();
            query.Keywords = productName;
            query.ProductCode = productCode;
            query.CategoryId = categoryId;
            query.SaleStatus = ProductSaleStatus.OnSale;
            if (categoryId.HasValue)
            {
                query.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(categoryId.Value).Path;
            }
            DataTable groupBuyProducts = SubSiteProducthelper.GetGroupBuyProducts(query);
            base.Items.Add(new ListItem("--请选择--", string.Empty));
            foreach (DataRow row in groupBuyProducts.Rows)
            {
                base.Items.Add(new ListItem(row["ProductName"].ToString(), row["ProductId"].ToString()));
            }
        }

        public int? CategoryId
        {
            get
            {
                return categoryId;
            }
            set
            {
                categoryId = value;
            }
        }

        public string ProductCode
        {
            get
            {
                return productCode;
            }
            set
            {
                productCode = value;
            }
        }

        public string ProductName
        {
            get
            {
                return productName;
            }
            set
            {
                productName = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
                else
                {
                    base.SelectedIndex = -1;
                }
            }
        }
    }
}

