
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.HelpCategories)]
    public partial class EditHelpCategory : AdminPage
    {
        int categoryId;


        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(categoryId);
            try
            {
                ResourcesHelper.DeleteImage(helpCategory.IconUrl);
            }
            catch
            {
            }
            helpCategory.IconUrl = imgPic.ImageUrl = string.Empty;
            if (ArticleHelper.UpdateHelpCategory(helpCategory))
            {
                btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
            }
        }

        private void btnSubmitHelpCategory_Click(object sender, EventArgs e)
        {
            string iconUrl = string.Empty;
            iconUrl = ArticleHelper.GetHelpCategory(categoryId).IconUrl;
            if (fileUpload.HasFile)
            {
                try
                {
                    if (!string.IsNullOrEmpty(iconUrl))
                    {
                        ResourcesHelper.DeleteImage(iconUrl);
                    }
                    iconUrl = ArticleHelper.UploadHelpImage(fileUpload.PostedFile);
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            HelpCategoryInfo info2 = new HelpCategoryInfo();
            info2.CategoryId = new int?(categoryId);
            info2.Name = txtHelpCategoryName.Text.Trim();
            info2.IconUrl = iconUrl;
            info2.Description = txtHelpCategoryDesc.Text.Trim();
            info2.IsShowFooter = radioShowFooter.SelectedValue;
            HelpCategoryInfo target = info2;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<HelpCategoryInfo>(target, new string[] { "ValHelpCategoryInfo" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                UpdateCategory(target);
            }
            else
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                ShowMsg(msg, false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitHelpCategory.Click += new EventHandler(btnSubmitHelpCategory_Click);
            btnPicDelete.Click += new EventHandler(btnPicDelete_Click);
            if (!int.TryParse(base.Request.QueryString["CategoryId"], out categoryId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(categoryId);
                if (helpCategory == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(helpCategory, false);
                    txtHelpCategoryName.Text = helpCategory.Name;
                    txtHelpCategoryDesc.Text = helpCategory.Description;
                    radioShowFooter.SelectedValue = helpCategory.IsShowFooter;
                    imgPic.ImageUrl = helpCategory.IconUrl;
                    imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                    btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                }
            }
        }

        private void UpdateCategory(HelpCategoryInfo category)
        {
            if (ArticleHelper.UpdateHelpCategory(category))
            {
                imgPic.ImageUrl = null;
                imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                ShowMsg("成功修改了帮助分类", true);
            }
            else
            {
                ShowMsg("操作失败,未知错误", false);
            }
        }
    }
}

