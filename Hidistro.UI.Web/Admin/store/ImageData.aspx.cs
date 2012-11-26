using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class ImageData : AdminPage
    {
        int? enumOrder;
        public string GlobalsPath = Globals.ApplicationPath;
        string keyOrder;
        string keyWordIName = string.Empty;
        int pageIndex;

        private int? typeId = null;

        private void BindImageData()
        {
            pageIndex = pager.PageIndex;
            PhotoListOrder uploadTimeDesc = PhotoListOrder.UploadTimeDesc;
            if (enumOrder.HasValue)
            {
                uploadTimeDesc = (PhotoListOrder)Enum.ToObject(typeof(PhotoListOrder), enumOrder.Value);
            }
            DbQueryResult result = GalleryHelper.GetPhotoList(keyWordIName, typeId, pageIndex, uploadTimeDesc);
            photoDataList.DataSource = result.Data;
            photoDataList.DataBind();
            pager.TotalRecords = result.TotalRecords;
            lblImageData.Text = "共" + pager.TotalRecords + "张";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool flag = true;
            bool flag2 = true;
            foreach (DataListItem item in photoDataList.Controls)
            {
                CheckBox box = (CheckBox)item.FindControl("checkboxCol");
                HiddenField field = (HiddenField)item.FindControl("HiddenFieldImag");
                if ((box != null) && box.Checked)
                {
                    flag2 = false;
                    try
                    {
                        int photoId = (int)photoDataList.DataKeys[item.ItemIndex];
                        StoreHelper.DeleteImage(field.Value);
                        if (!GalleryHelper.DeletePhoto(photoId))
                        {
                            flag = false;
                        }
                    }
                    catch
                    {
                        ShowMsg("删除文件错误", false);
                        BindImageData();
                    }
                    continue;
                }
            }
            if (flag2)
            {
                ShowMsg("未选择删除的图片", false);
            }
            else
            {
                if (flag)
                {
                    ShowMsg("删除图片成功", true);
                }
                BindImageData();
            }
        }

        private void btnImagetSearch_Click(object sender, EventArgs e)
        {
            keyWordIName = txtWordName.Text;
            BindImageData();
        }

        private void btnMoveImageData_Click(object sender, EventArgs e)
        {
            List<int> pList = new List<int>();
            int pTypeId = Convert.ToInt32(dropImageFtp.SelectedItem.Value);
            foreach (DataListItem item in photoDataList.Controls)
            {
                CheckBox box = (CheckBox)item.FindControl("checkboxCol");
                if ((box != null) && box.Checked)
                {
                    int num2 = (int)photoDataList.DataKeys[item.ItemIndex];
                    pList.Add(num2);
                }
            }
            if (GalleryHelper.MovePhotoType(pList, pTypeId) > 0)
            {
                ShowMsg("图片移动成功！", true);
            }
            BindImageData();
        }

        private void btnSaveImageData_Click(object sender, EventArgs e)
        {
            string str = RePlaceImg.Value;
            int photoId = Convert.ToInt32(RePlaceId.Value);
            string photoPath = GalleryHelper.GetPhotoPath(photoId);
            string str3 = photoPath.Substring(photoPath.LastIndexOf("."));
            string extension = string.Empty;
            string str5 = string.Empty;
            try
            {
                HttpPostedFile postedFile = base.Request.Files[0];
                extension = Path.GetExtension(postedFile.FileName);
                if (str3 != extension)
                {
                    ShowMsg("上传图片类型与原文件类型不一致！", false);
                    return;
                }
                string str6 = Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/gallery";
                str5 = photoPath.Substring(photoPath.LastIndexOf("/") + 1);
                string str7 = str.Substring(str.LastIndexOf("/") - 6, 6);
                string virtualPath = str6 + "/" + str7 + "/";
                int contentLength = postedFile.ContentLength;
                string path = base.Request.MapPath(virtualPath);
                string str10 = str7 + "/" + str5;
                DirectoryInfo info = new DirectoryInfo(path);
                if (!info.Exists)
                {
                    info.Create();
                }
                if (!ResourcesHelper.CheckPostedFile(postedFile))
                {
                    ShowMsg("文件上传的类型不正确！", false);
                    return;
                }
                if (contentLength >= 0x1f4000)
                {
                    ShowMsg("图片文件已超过网站限制大小！", false);
                    return;
                }
                postedFile.SaveAs(base.Request.MapPath(virtualPath + str5));
                GalleryHelper.ReplacePhoto(photoId, contentLength);
            }
            catch
            {
                ShowMsg("替换文件错误!", false);
                return;
            }
            BindImageData();
        }

        private void btnSaveImageDataName_Click(object sender, EventArgs e)
        {
            string text = ReImageDataName.Text;
            if (string.IsNullOrEmpty(text) || (text.Length > 30))
            {
                ShowMsg("图片名称不能为空长度限制在30个字符以内！", false);
            }
            else
            {
                GalleryHelper.RenamePhoto(Convert.ToInt32(ReImageDataNameId.Value), text);
                BindImageData();
            }
        }

        public static string Html_ToClient(string Str)
        {
            if (Str == null)
            {
                return null;
            }
            if (Str != string.Empty)
            {
                return HttpContext.Current.Server.HtmlDecode(Str.Trim());
            }
            return string.Empty;
        }

        private void ImageOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            enumOrder = ImageOrder.SelectedValue;
            BindImageData();
        }

        private void LoadParameters()
        {
            if (!string.IsNullOrEmpty(Page.Request.QueryString["keyWordIName"]))
            {
                keyWordIName = Globals.UrlDecode(Page.Request.QueryString["keyWordIName"]);
            }
            if (!string.IsNullOrEmpty(Page.Request.QueryString["keyWordSel"]))
            {
                keyOrder = Globals.UrlDecode(Page.Request.QueryString["keyWordSel"]);
            }
            int result = 0;
            if (int.TryParse(Page.Request.QueryString["imageTypeId"], out result))
            {
                typeId = new int?(result);
            }
            if (enumOrder.HasValue)
            {
                ImageOrder.SelectedValue = enumOrder;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnDelete1.Click += new EventHandler(btnDelete_Click);
            btnDelete2.Click += new EventHandler(btnDelete_Click);
            btnSaveImageDataName.Click += new EventHandler(btnSaveImageDataName_Click);
            btnMoveImageData.Click += new EventHandler(btnMoveImageData_Click);
            btnSaveImageData.Click += new EventHandler(btnSaveImageData_Click);
            ImageOrder.SelectedIndexChanged += new EventHandler(ImageOrder_SelectedIndexChanged);
            btnImagetSearch.Click += new EventHandler(btnImagetSearch_Click);
            photoDataList.ItemCommand += new DataListCommandEventHandler(photoDataList_ItemCommand);
            LoadParameters();
            if (!Page.IsPostBack)
            {
                ImageOrder.DataBind();
                dropImageFtp.DataBind();
                BindImageData();
            }
        }

        private void photoDataList_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            int photoId = Convert.ToInt32(photoDataList.DataKeys[e.Item.ItemIndex]);
            string photoPath = GalleryHelper.GetPhotoPath(photoId);
            if (GalleryHelper.DeletePhoto(photoId))
            {
                StoreHelper.DeleteImage(photoPath);
                ShowMsg("删除图片成功", true);
            }
            BindImageData();
        }

        public static string TruncStr(string str, int maxSize)
        {
            str = Html_ToClient(str);
            if (!(str != string.Empty))
            {
                return string.Empty;
            }
            int num = 0;
            byte[] bytes = new ASCIIEncoding().GetBytes(str);
            for (int i = 0; i <= (bytes.Length - 1); i++)
            {
                if (bytes[i] == 0x3f)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                if (num > maxSize)
                {
                    str = str.Substring(0, i);
                    return str;
                }
            }
            return str;
        }
    }
}

