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
    [PrivilegeCheck(Privilege.BrandCategories)]
    public partial class EditBrandCategory : AdminPage
    {
        int brandId;

        private void btnDeleteLogo_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = GetBrandCategoryInfo();
            try
            {
                ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
                brandCategoryInfo.Logo = null;
                ViewState["Logo"] = null;
                CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
            }
            catch
            {
                ShowMsg("删除失败", false);
                return;
            }
            loadData();
        }

        protected void btnUpdateBrandCategory_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = GetBrandCategoryInfo();
            if (ValidationBrandCategory(brandCategoryInfo))
            {
                if (CatalogHelper.UpdateBrandCategory(brandCategoryInfo))
                {
                    base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandCategories.aspx"), true);
                }
                else
                {
                    ShowMsg("编辑品牌分类失败", true);
                }
            }
        }

        private void btnUpoad_Click(object sender, EventArgs e)
        {
            BrandCategoryInfo brandCategoryInfo = GetBrandCategoryInfo();
            if (fileUpload.HasFile)
            {
                try
                {
                    ResourcesHelper.DeleteImage(brandCategoryInfo.Logo);
                    brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(fileUpload.PostedFile);
                    ViewState["Logo"] = brandCategoryInfo.Logo;
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
                CatalogHelper.UpdateBrandCategory(brandCategoryInfo);
            }
            loadData();
        }

        private BrandCategoryInfo GetBrandCategoryInfo()
        {
            BrandCategoryInfo info2 = new BrandCategoryInfo();
            info2.BrandId = brandId;
            BrandCategoryInfo info = info2;
            if (ViewState["Logo"] != null)
            {
                info.Logo = (string)ViewState["Logo"];
            }
            info.BrandName = Globals.HtmlEncode(txtBrandName.Text.Trim());
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
            info.Description = (!string.IsNullOrEmpty(fckDescription.Text) && (fckDescription.Text.Length > 0)) ? fckDescription.Text : null;
            IList<int> list = new List<int>();
            foreach (ListItem item in chlistProductTypes.Items)
            {
                if (item.Selected)
                {
                    list.Add(int.Parse(item.Value));
                }
            }
            info.ProductTypes = list;
            return info;
        }

        private void loadData()
        {
            BrandCategoryInfo brandCategory = CatalogHelper.GetBrandCategory(brandId);
            if (brandCategory == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
                ViewState["Logo"] = brandCategory.Logo;
                foreach (ListItem item in chlistProductTypes.Items)
                {
                    if (brandCategory.ProductTypes.Contains(int.Parse(item.Value)))
                    {
                        item.Selected = true;
                    }
                }
                txtBrandName.Text = Globals.HtmlDecode(brandCategory.BrandName);
                txtCompanyUrl.Text = brandCategory.CompanyUrl;
                txtReUrl.Text = Globals.HtmlDecode(brandCategory.RewriteName);
                txtkeyword.Text = Globals.HtmlDecode(brandCategory.MetaKeywords);
                txtMetaDescription.Text = Globals.HtmlDecode(brandCategory.MetaDescription);
                fckDescription.Text = brandCategory.Description;
                if (string.IsNullOrEmpty(brandCategory.Logo))
                {
                    btnDeleteLogo.Visible = false;
                }
                else
                {
                    btnDeleteLogo.Visible = true;
                }
                imgLogo.ImageUrl = brandCategory.Logo;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Page.Request.QueryString["brandId"], out brandId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                btnUpdateBrandCategory.Click += new EventHandler(btnUpdateBrandCategory_Click);
                btnUpoad.Click += new EventHandler(btnUpoad_Click);
                btnDeleteLogo.Click += new EventHandler(btnDeleteLogo_Click);
                if (!Page.IsPostBack)
                {
                    chlistProductTypes.DataBind();
                    loadData();
                }
            }
        }

        private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
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

