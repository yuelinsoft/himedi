using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Data;
using System.Text;

namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
    public partial class ReturnSearchPurchaseProduct : DistributorPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            string sarachName = "";
            string searchSku = "";
            string page = "1";
            if (Request.Params["serachName"] != null)
            {
                sarachName = Request.Params["serachName"].Trim();
            }
            if (Request.Params["serachSKU"] != null)
            {
                searchSku = Request.Params["serachSKU"].Trim();
            }
            if (base.Request.Params["page"] != null)
            {
                page = Request.Params["page"].Trim();
            }
            ProductQuery query = new ProductQuery();
            query.PageSize = 8;
            query.PageIndex = Convert.ToInt32(page);
            query.Keywords = sarachName;
            query.ProductCode = searchSku;

            int count = 0;
            builder.Append("{data:[");
            DataTable puchaseProducts = SubSiteProducthelper.GetPuchaseProducts(query, out count);
            for (int i = 0; i < puchaseProducts.Rows.Count; i++)
            {
                builder.Append("{'skuId':'");
                builder.Append(puchaseProducts.Rows[i]["SkuId"].ToString());
                builder.Append("','sku':'");
                builder.Append(puchaseProducts.Rows[i]["SKU"].ToString());
                builder.Append("','productId':'");
                builder.Append(puchaseProducts.Rows[i]["ProductId"].ToString().Trim());
                builder.Append("','Name':'");
                builder.Append(puchaseProducts.Rows[i]["ProductName"].ToString());
                builder.Append("','Price':'");
                builder.Append(puchaseProducts.Rows[i]["PurchasePrice"].ToString());
                builder.Append("','Stock':'");
                builder.Append(puchaseProducts.Rows[i]["Stock"].ToString());
                builder.Append("'},");
            }
            builder.Append("],recCount:'");
            builder.Append(count);
            builder.Append("'}");
            Response.ContentType = "text/plain";
            Response.Write(builder.ToString());
        }
    }
}

