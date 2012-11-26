namespace Hidistro.UI.Web.Shopadmin
{
    using Hidistro.Entities.Comments;
    using Hidistro.Subsites.Comments;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.Subsites.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public partial class AddMyHelpCategory : DistributorPage
    {

        private void AddNewCategory(HelpCategoryInfo category)
        {
            if (SubsiteCommentsHelper.CreateHelpCategory(category))
            {
                this.ShowMsg("成功添加了一个帮助分类", true);
            }
            else
            {
                this.ShowMsg("操作失败,未知错误", false);
            }
        }

        private void btnSubmitHelpCategory_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (this.fileUpload.HasFile)
            {
                try
                {
                    str = SubsiteCommentsHelper.UploadHelpImage(this.fileUpload.PostedFile);
                }
                catch
                {
                    this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            HelpCategoryInfo info2 = new HelpCategoryInfo();
            info2.Name = this.txtHelpCategoryName.Text.Trim();
            info2.IconUrl = str;
            info2.Description = this.txtHelpCategoryDesc.Text.Trim();
            info2.IsShowFooter = this.radioShowFooter.SelectedValue;
            HelpCategoryInfo target = info2;
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<HelpCategoryInfo>(target, new string[] { "ValHelpCategoryInfo" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                this.AddNewCategory(target);
                this.Reset();
            }
            else
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSubmitHelpCategory.Click += new EventHandler(this.btnSubmitHelpCategory_Click);
        }

        private void Reset()
        {
            this.txtHelpCategoryName.Text = string.Empty;
            this.txtHelpCategoryDesc.Text = string.Empty;
        }
    }
}

