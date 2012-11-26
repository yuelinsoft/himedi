using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class ImageType : AdminPage
    {
        private void btnSaveImageType_Click(object sender, EventArgs e)
        {
            string text = AddImageTypeName.Text;
            if (text.Length > 20)
            {
                ShowMsg("分类长度限在20个字符以内", false);
            }
            else if (GalleryHelper.AddPhotoCategory(Globals.HtmlEncode(text)))
            {
                ShowMsg("添加成功！", true);
                GetImageType();
            }
            else
            {
                ShowMsg("添加失败", false);
            }
        }

        private void GetImageType()
        {
            ImageTypeList.DataSource = GalleryHelper.GetPhotoCategories();
            ImageTypeList.DataBind();
        }

        private void ImageTypeEdit_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> photoCategorys = new Dictionary<int, string>();
            for (int i = 0; i < ImageTypeList.Rows.Count; i++)
            {
                GridViewRow row = ImageTypeList.Rows[i];
                string text = ((TextBox)row.Cells[1].FindControl("ImageTypeName")).Text;
                if (text.Length > 20)
                {
                    ShowMsg("分类长度限在20个字符以内", false);
                    return;
                }
                int key = Convert.ToInt32(ImageTypeList.DataKeys[i].Value);
                photoCategorys.Add(key, Globals.HtmlEncode(text.ToString()));
            }
            try
            {
                if (GalleryHelper.UpdatePhotoCategories(photoCategorys) > 0)
                {
                    ShowMsg("修改成功！", true);
                }
                else
                {
                    ShowMsg("修改失败！", false);
                }
            }
            catch
            {
                ShowMsg("修改失败！", false);
            }
        }

        protected void ImageTypeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow)((Control)e.CommandSource).NamingContainer).RowIndex;
            int num2 = (int)ImageTypeList.DataKeys[rowIndex].Value;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (ImageTypeList.Rows.Count - 1))
                {
                    GalleryHelper.SwapSequence(num2, (int)ImageTypeList.DataKeys[rowIndex + 1].Value);
                    GetImageType();
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                GalleryHelper.SwapSequence(num2, (int)ImageTypeList.DataKeys[rowIndex - 1].Value);
                GetImageType();
            }
        }

        protected void ImageTypeList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int categoryId = (int)ImageTypeList.DataKeys[e.RowIndex].Value;
            if (GalleryHelper.DeletePhotoCategory(categoryId))
            {
                ShowMsg("删除成功!", true);
                GetImageType();
            }
            else
            {
                ShowMsg("删除分类失败", false);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            ImageTypeList.RowDeleting += new GridViewDeleteEventHandler(ImageTypeList_RowDeleting);
            ImageTypeList.RowCommand += new GridViewCommandEventHandler(ImageTypeList_RowCommand);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveImageType.Click += new EventHandler(btnSaveImageType_Click);
            ImageTypeEdit.Click += new EventHandler(ImageTypeEdit_Click);
            if (!Page.IsPostBack)
            {
                GetImageType();
            }
        }
    }
}

