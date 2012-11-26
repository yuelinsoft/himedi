using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.HOP;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.API
{
    /// <summary>
    /// 制作淘宝数据回调页
    /// </summary>
    public partial class MakeTaobaoProductData_url : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            TaobaoProductInfo taobaoProduct = new TaobaoProductInfo();

            string productid = Request.Form["productid"];

            string stuffstatus = Request.Form["stuffstatus"];

            string title = Request.Form["title"];

            long num = long.Parse(Request.Form["num"]);

            string locationcity = Request.Form["locationcity"];

            string LocationState = Request.Form["LocationState"];

            string Cid = Request.Form["Cid"];

            string hasinvoice = Request.Form["hasinvoice"];

            string HasWarranty = Request.Form["HasWarranty"];

            string props = Request.Form["props"];

            string inputpids = Request.Form["inputpids"];

            string inputstr = Request.Form["inputstr"];

            string skuproperties = Request.Form["skuproperties"];

            string skuquantities = Request.Form["skuquantities"];

            string skuprices = Request.Form["skuprices"];

            string skuouterids = Request.Form["skuouterids"];

            string freightpayer = Request.Form["freightpayer"];


            if (freightpayer == "buyer")
            {
                string postfee = Request.Form["postfee"];

                string expressfee = Request.Form["expressfee"];

                string emsfee = Request.Form["emsfee"];

                taobaoProduct.PostFee = decimal.Parse(postfee);

                taobaoProduct.EMSFee = decimal.Parse(expressfee);

                taobaoProduct.ExpressFee = decimal.Parse(emsfee);

            }

            taobaoProduct.ProductId = int.Parse(productid);

            taobaoProduct.StuffStatus = stuffstatus;

            taobaoProduct.PropertyAlias = props;

            taobaoProduct.ProTitle = title;

            taobaoProduct.Num = num;

            taobaoProduct.LocationState = LocationState;

            taobaoProduct.LocationCity = locationcity;

            taobaoProduct.FreightPayer = freightpayer;

            taobaoProduct.Cid = Convert.ToInt32(Cid);

            if (!string.IsNullOrEmpty(inputpids))
            {
                taobaoProduct.InputPids = inputpids;
            }

            if (!string.IsNullOrEmpty(inputstr))
            {
                taobaoProduct.InputStr = inputstr;
            }

            if (!string.IsNullOrEmpty(skuproperties))
            {
                taobaoProduct.SkuProperties = skuproperties;
            }

            if (!string.IsNullOrEmpty(skuprices))
            {
                taobaoProduct.SkuPrices = skuprices;
            }

            if (!string.IsNullOrEmpty(skuouterids))
            {
                taobaoProduct.SkuOuterIds = skuouterids;
            }

            if (!string.IsNullOrEmpty(skuquantities))
            {
                taobaoProduct.SkuQuantities = skuquantities;
            }

            taobaoProduct.HasInvoice = Convert.ToBoolean(hasinvoice);

            taobaoProduct.HasWarranty = Convert.ToBoolean(HasWarranty);

            taobaoProduct.HasDiscount = false;

            taobaoProduct.ListTime = DateTime.Now;

            if (ProductHelper.UpdateToaobProduct(taobaoProduct))
            {
                litmsg.Text = "制作淘宝格式的商品数据成功";
            }
            else
            {
                litmsg.Text = "制作淘宝格式的商品数据失败";
            }

        }

    }

}

