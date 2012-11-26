namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Core;
    using Hidistro.Membership.Context;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;

    public class UploadHandler : IHttpHandler
    {
       string action;
       string uploaderId;
       string uploadType;

       void DoDelete(HttpContext context)
        {
            string originalPath = context.Request.MapPath(Globals.ApplicationPath + context.Request.Form[uploaderId + "_uploadedImageUrl"]);
            string thumbnailUrl40 = originalPath.Replace(@"\images\", @"\thumbs40\40_");
            string thumbnailUrl60 = originalPath.Replace(@"\images\", @"\thumbs60\60_");
            string thumbnailUrl100 = originalPath.Replace(@"\images\", @"\thumbs100\100_");
            string thumbnailUrl160 = originalPath.Replace(@"\images\", @"\thumbs160\160_");
            string thumbnailUrl180 = originalPath.Replace(@"\images\", @"\thumbs180\180_");
            string thumbnailUrl220 = originalPath.Replace(@"\images\", @"\thumbs220\220_");
            string thumbnailUrl310 = originalPath.Replace(@"\images\", @"\thumbs310\310_");
            string thumbnailUrl410 = originalPath.Replace(@"\images\", @"\thumbs410\410_");
            if (File.Exists(originalPath))
            {
                File.Delete(originalPath);
            }
            if (File.Exists(thumbnailUrl40))
            {
                File.Delete(thumbnailUrl40);
            }
            if (File.Exists(thumbnailUrl60))
            {
                File.Delete(thumbnailUrl60);
            }
            if (File.Exists(thumbnailUrl100))
            {
                File.Delete(thumbnailUrl100);
            }
            if (File.Exists(thumbnailUrl160))
            {
                File.Delete(thumbnailUrl160);
            }
            if (File.Exists(thumbnailUrl180))
            {
                File.Delete(thumbnailUrl180);
            }
            if (File.Exists(thumbnailUrl220))
            {
                File.Delete(thumbnailUrl220);
            }
            if (File.Exists(thumbnailUrl310))
            {
                File.Delete(thumbnailUrl310);
            }
            if (File.Exists(thumbnailUrl410))
            {
                File.Delete(thumbnailUrl410);
            }
            context.Response.Write("<script type=\"text/javascript\">window.parent.DeleteCallback('" + uploaderId + "');</script>");
        }

       void DoUpload(HttpContext context)
        {
            if (context.Request.Files.Count == 0)
            {
                WriteBackError(context, "没有检测到任何文件");
            }
            else
            {
                HttpPostedFile file = context.Request.Files[0];
                for (int index = 1; (file.ContentLength == 0) && (index < context.Request.Files.Count); index++)
                {
                    file = context.Request.Files[index];
                }
                if (file.ContentLength == 0)
                {
                    WriteBackError(context, "当前文件没有任何内容");
                }
                else if (!file.ContentType.ToLower().StartsWith("image/") || !Regex.IsMatch(Path.GetExtension(file.FileName.ToLower()), @"\.(jpg|gif|png|bmp|jpeg)$", RegexOptions.Compiled))
                {
                    WriteBackError(context, "文件类型错误，请选择有效的图片文件");
                }
                else
                {
                    UploadImage(context, file);
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            uploaderId = context.Request.QueryString["uploaderId"];
            uploadType = context.Request.QueryString["uploadType"];
            action = context.Request.QueryString["action"];
            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Expires = -1;
            try
            {
                if (action.Equals("upload"))
                {
                    DoUpload(context);
                }
                else if (action.Equals("delete"))
                {
                    DoDelete(context);
                }
            }
            catch (Exception e)
            {
                WriteBackError(context, e.Message);
            }
        }

       void UploadImage(HttpContext context, HttpPostedFile file)
        {
            string uploadPath = HiContext.Current.GetStoragePath() + "/" + uploadType;
            string newFilename = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture) + Path.GetExtension(file.FileName);
            string originalSavePath = uploadPath + "/images/" + newFilename;
            string thumbnail40SavePath = uploadPath + "/thumbs40/40_" + newFilename;
            string thumbnail60SavePath = uploadPath + "/thumbs60/60_" + newFilename;
            string thumbnail100SavePath = uploadPath + "/thumbs100/100_" + newFilename;
            string thumbnail160SavePath = uploadPath + "/thumbs160/160_" + newFilename;
            string thumbnail180SavePath = uploadPath + "/thumbs180/180_" + newFilename;
            string thumbnail220SavePath = uploadPath + "/thumbs220/220_" + newFilename;
            string thumbnail310SavePath = uploadPath + "/thumbs310/310_" + newFilename;
            string thumbnail410SavePath = uploadPath + "/thumbs410/410_" + newFilename;
            file.SaveAs(context.Request.MapPath(Globals.ApplicationPath + originalSavePath));
            string originalFullPath = context.Request.MapPath(Globals.ApplicationPath + originalSavePath);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail40SavePath), 40, 40);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail60SavePath), 60, 60);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail100SavePath), 100, 100);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail160SavePath), 160, 160);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail180SavePath), 180, 180);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail220SavePath), 220, 220);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail310SavePath), 310, 310);
            ResourcesHelper.CreateThumbnail(originalFullPath, context.Request.MapPath(Globals.ApplicationPath + thumbnail410SavePath), 410, 410);
            string[] parameters = new string[] { "'" + uploadType + "'", "'" + uploaderId + "'", "'" + originalSavePath + "'", "'" + thumbnail40SavePath + "'", "'" + thumbnail60SavePath + "'", "'" + thumbnail100SavePath + "'", "'" + thumbnail160SavePath + "'", "'" + thumbnail180SavePath + "'", "'" + thumbnail220SavePath + "'", "'" + thumbnail310SavePath + "'", "'" + thumbnail410SavePath + "'" };
            context.Response.Write("<script type=\"text/javascript\">window.parent.UploadCallback(" + string.Join(",", parameters) + ");</script>");
        }

       void WriteBackError(HttpContext context, string error)
        {
            string[] parameters = new string[] { "'" + uploadType + "'", "'" + uploaderId + "'", "'" + error + "'" };
            context.Response.Write("<script type=\"text/javascript\">window.parent.ErrorCallback(" + string.Join(",", parameters) + ");</script>");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

