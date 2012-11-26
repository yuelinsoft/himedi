using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
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
    [PrivilegeCheck(Privilege.EditProductType)]
    public partial class EditProductType : AdminPage
    {

        int typeId;

        private void btnEditProductType_Click(object sender, EventArgs e)
        {
            ProductTypeInfo productType = new ProductTypeInfo();
            productType.TypeId = typeId;
            productType.TypeName = txtTypeName.Text;
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
            if (ValidationProductType(productType) && ProductTypeHelper.UpdateProductType(productType))
            {
                ShowMsg("修改成功", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["typeId"]))
            {
                int.TryParse(Page.Request.QueryString["typeId"], out typeId);
            }
            btnEditProductType.Click += new EventHandler(btnEditProductType_Click);
            if (!Page.IsPostBack)
            {
                chlistBrand.DataBind();
                ProductTypeInfo productType = ProductTypeHelper.GetProductType(typeId);
                if (productType == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    txtTypeName.Text = productType.TypeName;
                    txtRemark.Text = productType.Remark;
                    foreach (ListItem item in chlistBrand.Items)
                    {
                        if (productType.Brands.Contains(int.Parse(item.Value)))
                        {
                            item.Selected = true;
                        }
                    }
                }
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

