using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin
{
    public partial class UploadFile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpFileCollection files = Request.Files;
            if (files.Count > 0)
            {
                string str = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/master/flex");
                HttpPostedFile file = files[0];
                string str2 = Path.GetExtension(file.FileName).ToLower();
                if ((((str2 != ".jpg") && (str2 != ".gif")) && ((str2 != ".jpeg") && (str2 != ".png"))) && (str2 != ".bmp"))
                {
                    Response.Write("1");
                }
                else
                {
                    string s = DateTime.Now.ToString("yyyyMMdd") + new Random().Next(0x2710, 0x1869f).ToString(CultureInfo.InvariantCulture) + str2;
                    string filename = str + "/" + s;
                    try
                    {
                        file.SaveAs(filename);
                        Response.Write(s);
                    }
                    catch
                    {
                        Response.Write("0");
                    }
                }
            }
            else
            {
                Response.Write("2");
            }
        }
    }
}

