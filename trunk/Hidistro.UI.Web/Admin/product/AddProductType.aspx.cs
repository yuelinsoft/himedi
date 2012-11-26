using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.product
{
    [PrivilegeCheck(Privilege.AddProductType)]
    public partial class AddProductType : AdminPage
    {
        private void btnAddProductType_Click(object sender, EventArgs e)
        {
            ProductTypeInfo productType = new ProductTypeInfo();
            productType.TypeName = txtTypeName.Text.Trim();
            productType.Remark = txtRemark.Text;
            IList<int> list = new List<int>();
            foreach (ListItem item in chlistBrand.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            productType.Brands = list;
            if (ValidationProductType(productType))
            {
                int num = ProductTypeHelper.AddProductType(productType);
                if (num > 0)
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/AddAttribute.aspx?typeId=" + num), true);
                }
                else
                {
                    ShowMsg("添加商品类型失败", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnAddProductType.Click += new EventHandler(btnAddProductType_Click);
            if (!base.IsPostBack)
            {
                chlistBrand.DataBind();
            }
        }

        private bool ValidationProductType(ProductTypeInfo productType)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ProductTypeInfo>(productType, new string[] { "ValProductType" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

