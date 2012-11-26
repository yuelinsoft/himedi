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
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    /// <summary>
    /// 品牌目录
    /// </summary>
    [PrivilegeCheck(Privilege.BrandCategories)]
    public partial class AddBrandCategory : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                chlistProductTypes.DataBind();
            }
        }

        protected void btnAddBrandCategory_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = GetBrandCategoryInfo();
            if (fileUpload.HasFile)
            {
                try
                {
                    brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(fileUpload.PostedFile);
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            if (ValidationBrandCategory(brandCategoryInfo))
            {
                if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
                {
                    ShowMsg("成功添加品牌分类", true);
                }
                else
                {
                    ShowMsg("添加品牌分类失败", true);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = GetBrandCategoryInfo();
            if (fileUpload.HasFile)
            {
                try
                {
                    brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(fileUpload.PostedFile);
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            if (ValidationBrandCategory(brandCategoryInfo))
            {
                if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandCategories.aspx"), true);
                }
                else
                {
                    ShowMsg("添加品牌分类失败", true);
                }
            }
        }

        BrandCategoryInfo GetBrandCategoryInfo()
        {
            BrandCategoryInfo info2 = new BrandCategoryInfo();
            info2.BrandName = Globals.HtmlEncode(txtBrandName.Text.Trim());
            BrandCategoryInfo info = info2;
            if (!string.IsNullOrEmpty(txtCompanyUrl.Text))
            {
                info.CompanyUrl = txtCompanyUrl.Text.Trim();
            }
            else
            {
                info.CompanyUrl = null;
            }
            info.RewriteName = Globals.HtmlEncode(txtReUrl.Text.Trim());
            info.MetaKeywords = Globals.HtmlEncode(txtkeyword.Text.Trim());
            info.MetaDescription = Globals.HtmlEncode(txtMetaDescription.Text.Trim());
            IList<int> list = new List<int>();
            foreach (ListItem item in chlistProductTypes.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            info.ProductTypes = list;
            info.Description = (!string.IsNullOrEmpty(fckDescription.Text) && (fckDescription.Text.Length > 0)) ? fckDescription.Text : null;
            return info;
        }



        bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<BrandCategoryInfo>(brandCategory, new string[] { "ValBrandCategory" });
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

