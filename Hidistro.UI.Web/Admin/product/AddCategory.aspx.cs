
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.AddProductCategory)]
    public partial class AddCategory : AdminPage
    {

        private void btnSaveAddCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = GetCategory();
            if (category != null)
            {
                if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    ShowMsg("成功添加了店铺分类", true);
                    dropCategories.DataBind();
                    dropProductTypes.DataBind();
                    txtCategoryName.Text = string.Empty;
                    txtSKUPrefix.Text = string.Empty;
                    txtRewriteName.Text = string.Empty;
                    txtPageKeyTitle.Text = string.Empty;
                    txtPageKeyWords.Text = string.Empty;
                    txtPageDesc.Text = string.Empty;
                    fckNotes1.Text = string.Empty;
                    fckNotes2.Text = string.Empty;
                    fckNotes3.Text = string.Empty;
                }
                else
                {
                    ShowMsg("添加店铺分类失败,未知错误", false);
                }
            }
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = GetCategory();
            if (category != null)
            {
                if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ManageCategories.aspx"), true);
                }
                else
                {
                    ShowMsg("添加店铺分类失败,未知错误", false);
                }
            }
        }

        private CategoryInfo GetCategory()
        {
            CategoryInfo info3 = new CategoryInfo();
            info3.Name = txtCategoryName.Text.Trim();
            info3.ParentCategoryId = dropCategories.SelectedValue;
            info3.SKUPrefix = txtSKUPrefix.Text.Trim();
            info3.AssociatedProductType = dropProductTypes.SelectedValue;
            CategoryInfo target = info3;
            if (!string.IsNullOrEmpty(txtRewriteName.Text.Trim()))
            {
                target.RewriteName = txtRewriteName.Text.Trim();
            }
            else
            {
                target.RewriteName = null;
            }
            target.MetaTitle = txtPageKeyTitle.Text.Trim();
            target.MetaKeywords = txtPageKeyWords.Text.Trim();
            target.MetaDescription = txtPageDesc.Text.Trim();
            target.Notes1 = fckNotes1.Text;
            target.Notes2 = fckNotes2.Text;
            target.Notes3 = fckNotes3.Text;
            target.DisplaySequence = 1;
            if (target.ParentCategoryId.HasValue)
            {
                CategoryInfo category = CatalogHelper.GetCategory(target.ParentCategoryId.Value);
                if ((category == null) || (category.Depth >= 5))
                {
                    ShowMsg(string.Format("您选择的上级分类有误，店铺分类最多只支持{0}级分类", 5), false);
                    return null;
                }
                if (string.IsNullOrEmpty(target.Notes1))
                {
                    target.Notes1 = category.Notes1;
                }
                if (string.IsNullOrEmpty(target.Notes2))
                {
                    target.Notes2 = category.Notes2;
                }
                if (string.IsNullOrEmpty(target.Notes3))
                {
                    target.Notes3 = category.Notes3;
                }
                if (string.IsNullOrEmpty(target.RewriteName))
                {
                    target.RewriteName = category.RewriteName;
                }
            }
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<CategoryInfo>(target, new string[] { "ValCategory" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                return target;
            }
            foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
            {
                msg = msg + Formatter.FormatErrorMessage(result.Message);
            }
            ShowMsg(msg, false);
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveCategory.Click += new EventHandler(btnSaveCategory_Click);
            btnSaveAddCategory.Click += new EventHandler(btnSaveAddCategory_Click);
            if (!string.IsNullOrEmpty(base.Request["isCallback"]) && (base.Request["isCallback"] == "true"))
            {
                int result = 0;
                int.TryParse(base.Request["parentCategoryId"], out result);
                CategoryInfo category = CatalogHelper.GetCategory(result);
                if (category != null)
                {
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    base.Response.Write("{ ");
                    base.Response.Write(string.Format("\"SKUPrefix\":\"{0}\"", category.SKUPrefix));
                    base.Response.Write("}");
                    base.Response.End();
                }
            }
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropProductTypes.DataBind();
            }
        }
    }
}

