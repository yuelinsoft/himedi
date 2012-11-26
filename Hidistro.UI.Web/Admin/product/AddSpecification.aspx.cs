using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.product.ascx;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.AddProductType)]
    public partial class AddSpecification : AdminPage
    {

        private void btnFilish_Click(object server, EventArgs e)
        {
            Response.Redirect(Globals.GetAdminAbsolutePath("/product/AddProductTypeFinish.aspx"), true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int num;
            if ((!string.IsNullOrEmpty(Request["isCallback"]) && (Request["isCallback"] == "true")) && int.TryParse(Request["ValueId"], out num))
            {
                if (ProductTypeHelper.DeleteAttributeValue(num))
                {
                    if (!string.IsNullOrEmpty(Request["ImageUrl"]))
                    {
                        StoreHelper.DeleteImage(Request["ImageUrl"]);
                        Response.Clear();
                        Response.ContentType = "application/json";
                        Response.Write("{\"Status\":\"true\"}");
                        Response.End();
                    }
                    else
                    {
                        Response.Clear();
                        Response.ContentType = "application/json";
                        Response.Write("{\"Status\":\"true\"}");
                        Response.End();
                    }
                }
                else
                {
                    Response.Clear();
                    Response.ContentType = "application/json";
                    Response.Write("{\"Status\":\"false\"}");
                    Response.End();
                }
            }
            btnFilish.Click += new EventHandler(btnFilish_Click);
        }
    }
}

