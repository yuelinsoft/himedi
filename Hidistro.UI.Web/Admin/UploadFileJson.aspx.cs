using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.ControlPanel.Utility;
using LitJson;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
    public partial class UploadFileJson : AdminPage
    {
        string savePath;
        string saveUrl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Users.GetUser(0, Users.GetLoggedOnUsername(), true, true).UserRole != UserRole.SiteManager)
            {
                showError("您没有权限执行此操作！");
            }
            else
            {
                string str = "false";
                if (Request.Form["isAdvPositions"] != null)
                {
                    str = Request.Form["isAdvPositions"].ToString().ToLower().Trim();
                }
                if (str == "false")
                {
                    savePath = "~/Storage/master/gallery/";
                    saveUrl = "/Storage/master/gallery/";
                }
                else
                {
                    savePath = string.Format("{0}/fckfiles/Files/Image/", HiContext.Current.GetSkinPath());
                    if (Request.ApplicationPath != "/")
                    {
                        saveUrl = savePath.Substring(Request.ApplicationPath.Length);
                    }
                    else
                    {
                        saveUrl = savePath;
                    }
                }
                int result = 0;
                if (Request.Form["fileCategory"] != null)
                {
                    int.TryParse(Request.Form["fileCategory"], out result);
                }
                string str2 = string.Empty;
                if (Request.Form["imgTitle"] != null)
                {
                    str2 = Request.Form["imgTitle"];
                }
                HttpPostedFile postedFile = Request.Files["imgFile"];
                if (postedFile == null)
                {
                    showError("请先选择文件！");
                }
                else if (!ResourcesHelper.CheckPostedFile(postedFile))
                {
                    showError("不能上传空文件，且必须是有效的图片文件！");
                }
                else
                {
                    string path = Server.MapPath(savePath);
                    if (!Directory.Exists(path))
                    {
                        showError("上传目录不存在。");
                    }
                    else
                    {
                        if (str == "false")
                        {
                            path = path + string.Format("{0}/", DateTime.Now.ToString("yyyyMM"));
                            saveUrl = saveUrl + string.Format("{0}/", DateTime.Now.ToString("yyyyMM"));
                        }
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = postedFile.FileName;
                        if (str2.Length == 0)
                        {
                            str2 = fileName;
                        }
                        string str5 = Path.GetExtension(fileName).ToLower();
                        string str6 = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + str5;
                        string filename = path + str6;
                        string str8 = saveUrl + str6;
                        try
                        {
                            postedFile.SaveAs(filename);
                            if (str == "false")
                            {
                                Database database = DatabaseFactory.CreateDatabase();
                                DbCommand sqlStringCommand = database.GetSqlStringCommand("insert into Hishop_PhotoGallery(CategoryId,PhotoName,PhotoPath,FileSize,UploadTime,LastUpdateTime)values(@cid,@name,@path,@size,@time,@time1)");
                                database.AddInParameter(sqlStringCommand, "cid", DbType.Int32, result);
                                database.AddInParameter(sqlStringCommand, "name", DbType.String, str2);
                                database.AddInParameter(sqlStringCommand, "path", DbType.String, str8);
                                database.AddInParameter(sqlStringCommand, "size", DbType.Int32, postedFile.ContentLength);
                                database.AddInParameter(sqlStringCommand, "time", DbType.DateTime, DateTime.Now);
                                database.AddInParameter(sqlStringCommand, "time1", DbType.DateTime, DateTime.Now);
                                database.ExecuteNonQuery(sqlStringCommand);
                            }
                        }
                        catch
                        {
                            showError("保存文件出错！");
                        }
                        string str9 = Request.Url.ToString();
                        str9 = str9.Substring(0, str9.IndexOf("/", 7));
                        if (Request.ApplicationPath != "/")
                        {
                            str9 = str9 + Request.ApplicationPath;
                        }
                        Hashtable hashtable = new Hashtable();
                        hashtable["error"] = 0;
                        hashtable["url"] = str9 + str8;
                        Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                        Response.Write(JsonMapper.ToJson(hashtable));
                        Response.End();
                    }
                }
            }
        }

        private void showError(string message)
        {
            Hashtable hashtable = new Hashtable();
            hashtable["error"] = 1;
            hashtable["message"] = message;
            Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            Response.Write(JsonMapper.ToJson(hashtable));
            Response.End();
        }
    }
}

