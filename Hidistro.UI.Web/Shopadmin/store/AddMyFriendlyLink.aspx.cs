using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Shopadmin
{
    public partial class AddMyFriendlyLink : DistributorPage
    {


        private void AddNewFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (SubsiteStoreHelper.CreateFriendlyLink(friendlyLink))
            {
                ShowMsg("成功添加了一个友情链接", true);
            }
            else
            {
                ShowMsg("未知错误", false);
            }
        }

        private void btnSubmitLinks_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (!(uploadImageUrl.HasFile || !string.IsNullOrEmpty(txtaddTitle.Text.Trim())))
            {
                ShowMsg("友情链接Logo和网站名称不能同时为空", false);
            }
            else
            {
                FriendlyLinksInfo target = new FriendlyLinksInfo();
                if (uploadImageUrl.HasFile)
                {
                    try
                    {
                        str = SubsiteStoreHelper.UploadLinkImage(uploadImageUrl.PostedFile);
                    }
                    catch
                    {
                        ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                        return;
                    }
                }
                target.ImageUrl = str;
                target.LinkUrl = txtaddLinkUrl.Text;
                target.Title = Globals.HtmlEncode(txtaddTitle.Text.Trim());
                target.Visible = radioShowLinks.SelectedValue;
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<FriendlyLinksInfo>(target, new string[] { "ValFriendlyLinksInfo" });
                string msg = string.Empty;
                if (results.IsValid)
                {
                    AddNewFriendlyLink(target);
                    Reset();
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitLinks.Click += new EventHandler(btnSubmitLinks_Click);
        }

        private void Reset()
        {
            txtaddTitle.Text = string.Empty;
            radioShowLinks.SelectedValue = true;
            txtaddLinkUrl.Text = string.Empty;
        }
    }
}

