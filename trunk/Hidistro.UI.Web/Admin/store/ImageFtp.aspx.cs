using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
    [AdministerCheck(true)]
    public partial class ImageFtp : AdminPage
    {
        private void btnSaveImageFtp_Click(object sender, EventArgs e)
        {
            string str = Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/gallery";
            int categoryId = Convert.ToInt32(dropImageFtp.SelectedItem.Value);
            int num2 = 0;
            int num3 = 0;
            new StringBuilder();
            HttpFileCollection files = base.Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile postedFile = files[i];
                if (postedFile.ContentLength > 0)
                {
                    num2++;
                    try
                    {
                        string str2 = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(postedFile.FileName);
                        string str3 = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1);
                        string photoName = str3.Substring(0, str3.LastIndexOf("."));
                        string str5 = DateTime.Now.ToString("yyyyMM").Substring(0, 6);
                        string virtualPath = str + "/" + str5 + "/";
                        int contentLength = postedFile.ContentLength;
                        string path = base.Request.MapPath(virtualPath);
                        string photoPath = "/Storage/master/gallery/" + str5 + "/" + str2;
                        DirectoryInfo info = new DirectoryInfo(path);
                        if (ResourcesHelper.CheckPostedFile(postedFile))
                        {
                            if (!info.Exists)
                            {
                                info.Create();
                            }
                            postedFile.SaveAs(base.Request.MapPath(virtualPath + str2));
                            if (GalleryHelper.AddPhote(categoryId, photoName, photoPath, contentLength))
                            {
                                num3++;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            if (num2 == 0)
            {
                ShowMsg("至少需要选择一个图片文件！", false);
            }
            else
            {
                ShowMsg("成功上传了" + num3.ToString() + "个文件！", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSaveImageFtp.Click += new EventHandler(btnSaveImageFtp_Click);
            if (!Page.IsPostBack)
            {
                dropImageFtp.DataBind();
            }
        }
    }
}

