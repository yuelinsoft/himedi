using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [PrivilegeCheck(Privilege.FriendlyLinks)]
    public partial class EditFriendlyLink : AdminPage
    {
        int linkId;

        private void btnPicDelete_Click(object sender, EventArgs e)
        {
            FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(linkId);
            try
            {
                StoreHelper.DeleteImage(friendlyLink.ImageUrl);
            }
            catch
            {
            }
            friendlyLink.ImageUrl = imgPic.ImageUrl = string.Empty;
            if (StoreHelper.UpdateFriendlyLink(friendlyLink))
            {
                btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
            }
        }

        private void btnSubmitLinks_Click(object sender, EventArgs e)
        {
            string str = string.Empty;
            if (uploadImageUrl.HasFile)
            {
                try
                {
                    str = StoreHelper.UploadLinkImage(uploadImageUrl.PostedFile);
                }
                catch
                {
                    ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
                    return;
                }
            }
            FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(linkId);
            friendlyLink.ImageUrl = uploadImageUrl.HasFile ? str : friendlyLink.ImageUrl;
            friendlyLink.LinkUrl = txtaddLinkUrl.Text;
            friendlyLink.Title = Globals.HtmlEncode(txtaddTitle.Text.Trim());
            friendlyLink.Visible = radioShowLinks.SelectedValue;
            if (!string.IsNullOrEmpty(friendlyLink.ImageUrl) || !string.IsNullOrEmpty(friendlyLink.Title))
            {
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<FriendlyLinksInfo>(friendlyLink, new string[] { "ValFriendlyLinksInfo" });
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
                    UpdateFriendlyLink(friendlyLink);
                }
            }
            else
            {
                ShowMsg("友情链接Logo和网站名称不能同时为空", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSubmitLinks.Click += new EventHandler(btnSubmitLinks_Click);
            btnPicDelete.Click += new EventHandler(btnPicDelete_Click);
            if (!int.TryParse(base.Request.QueryString["linkId"], out linkId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(linkId);
                if (friendlyLink == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    txtaddTitle.Text = Globals.HtmlDecode(friendlyLink.Title);
                    txtaddLinkUrl.Text = friendlyLink.LinkUrl;
                    radioShowLinks.SelectedValue = friendlyLink.Visible;
                    imgPic.ImageUrl = friendlyLink.ImageUrl;
                    btnPicDelete.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                    imgPic.Visible = !string.IsNullOrEmpty(imgPic.ImageUrl);
                }
            }
        }

        private void UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
        {
            if (StoreHelper.UpdateFriendlyLink(friendlyLink))
            {
                imgPic.ImageUrl = string.Empty;
                ShowMsg("修改友情链接信息成功", true);
            }
            else
            {
                ShowMsg("修改友情链接信息失败", false);
            }
        }
    }
}

