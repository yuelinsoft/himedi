using Hidistro.Core;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web
{
    public partial class PicRar : Page
    {
        string label_html = string.Empty;

        public static bool IsNumeric(string strNumeric)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.Match(strNumeric).Success;
        }

        public static bool IsQueryString(string strQuery)
        {
            return IsQueryString(strQuery, "N");
        }

        public static bool IsQueryString(string strQuery, string Q)
        {
            bool flag = false;
            string str = Q;
            switch (str)
            {
                case null:
                    return flag;

                case "N":
                    if ((HttpContext.Current.Request.QueryString[strQuery] != null) && IsNumeric(HttpContext.Current.Request.QueryString[strQuery].ToString()))
                    {
                        flag = true;
                    }
                    return flag;
            }
            if ((str == "S") && ((HttpContext.Current.Request.QueryString[strQuery] != null) && (HttpContext.Current.Request.QueryString[strQuery].ToString() != string.Empty)))
            {
                flag = true;
            }
            return flag;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string virtualPath = string.Empty;
                int maxWidth = 0;
                int maxHeight = 0;
                if (IsQueryString("P", "S"))
                {
                    virtualPath = Request.QueryString["P"];
                }
                if (IsQueryString("W"))
                {
                    maxWidth = int.Parse(Request.QueryString["W"]);
                }
                if (IsQueryString("H"))
                {
                    maxHeight = int.Parse(Request.QueryString["H"]);
                }
                if (virtualPath != string.Empty)
                {
                    PIC pic = new PIC();
                    if (!virtualPath.StartsWith("/"))
                    {
                        virtualPath = "/" + virtualPath;
                    }
                    virtualPath = Globals.ApplicationPath + virtualPath;
                    pic.SendSmallImage(Request.MapPath(virtualPath), maxHeight, maxWidth);
                    string watermarkFilename = Request.MapPath(Globals.ApplicationPath + "/Admin/images/watermark.gif");
                    MemoryStream stream = pic.AddImageSignPic(pic.OutBMP, watermarkFilename, 9, 60, 4);
                    pic.Dispose();
                    Response.ClearContent();
                    Response.ContentType = "image/gif";
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                    stream.Dispose();
                }
            }
            catch (Exception exception)
            {
                label_html = exception.Message;
            }
        }
    }
}

