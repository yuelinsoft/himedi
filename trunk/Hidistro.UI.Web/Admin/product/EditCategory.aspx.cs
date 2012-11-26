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
    [PrivilegeCheck(Privilege.EditProductCategory)]
    public partial class EditCategory : AdminPage
    {
        int categoryId;

        private void BindCategoryInfo(CategoryInfo categoryInfo)
        {
            if (categoryInfo != null)
            {
                txtCategoryName.Text = categoryInfo.Name;
                dropProductTypes.SelectedValue = categoryInfo.AssociatedProductType;
                txtSKUPrefix.Text = categoryInfo.SKUPrefix;
                txtRewriteName.Text = categoryInfo.RewriteName;
                txtPageKeyTitle.Text = categoryInfo.MetaTitle;
                txtPageKeyWords.Text = categoryInfo.MetaKeywords;
                txtPageDesc.Text = categoryInfo.MetaDescription;
                fckNotes1.Text = categoryInfo.Notes1;
                fckNotes2.Text = categoryInfo.Notes2;
                fckNotes3.Text = categoryInfo.Notes3;
            }
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = CatalogHelper.GetCategory(categoryId);
            if (category == null)
            {
                ShowMsg("编缉店铺分类错误,未知", false);
            }
            else
            {
                category.Name = txtCategoryName.Text;
                category.SKUPrefix = txtSKUPrefix.Text;
                category.RewriteName = txtRewriteName.Text;
                category.MetaTitle = txtPageKeyTitle.Text;
                category.MetaKeywords = txtPageKeyWords.Text;
                category.MetaDescription = txtPageDesc.Text;
                category.AssociatedProductType = dropProductTypes.SelectedValue;
                category.Notes1 = fckNotes1.Text;
                category.Notes2 = fckNotes2.Text;
                category.Notes3 = fckNotes3.Text;
                if (category.Depth > 1)
                {
                    CategoryInfo info2 = CatalogHelper.GetCategory(category.ParentCategoryId.Value);
                    if (string.IsNullOrEmpty(category.Notes1))
                    {
                        category.Notes1 = info2.Notes1;
                    }
                    if (string.IsNullOrEmpty(category.Notes2))
                    {
                        category.Notes2 = info2.Notes2;
                    }
                    if (string.IsNullOrEmpty(category.Notes3))
                    {
                        category.Notes3 = info2.Notes3;
                    }
                }
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<CategoryInfo>(category, new string[] { "ValCategory" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    ShowMsg(msg, false);
                }
                else
                {
                    CategoryActionStatus status = CatalogHelper.UpdateCategory(category);
                    if (status == CategoryActionStatus.Success)
                    {
                        base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ManageCategories.aspx"), true);
                    }
                    else if (status == CategoryActionStatus.UpdateParentError)
                    {
                        ShowMsg("不能自己成为自己的上级分类", false);
                    }
                    else
                    {
                        ShowMsg("编缉店铺分类错误,未知", false);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["categoryId"], out categoryId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnSaveCategory.Click += new EventHandler(btnSaveCategory_Click);
                if (!Page.IsPostBack)
                {
                    CategoryInfo category = CatalogHelper.GetCategory(categoryId);
                    dropProductTypes.DataBind();
                    if (category == null)
                    {
                        base.GotoResourceNotFound();
                    }
                    else
                    {
                        Globals.EntityCoding(category, false);
                        BindCategoryInfo(category);
                        if (category.Depth > 1)
                        {
                            liRewriteName.Style.Add("display", "none");
                        }
                    }
                }
            }
        }
    }
}

